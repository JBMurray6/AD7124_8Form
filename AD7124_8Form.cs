using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Threading;

using System.Collections.Concurrent;

namespace AD7128_8NameSp
{
    public partial class AD7124_8Form : Form
    {
        #region constructors
        public AD7124_8Form()
        {
            InitializeComponent();

            AutosaveLocation = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\Autosave.csv";
        }

        //Start the form with it initialized
        public AD7124_8Form(bool initialize)
        {
            InitializeComponent();
            AutosaveLocation = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\Autosave.csv";
            if (initialize)
            {
                InitializeButton_Click(this, new EventArgs());
            }
        }

        //Set the serial number and initialize
        public AD7124_8Form(string initializestring)
        {
            InitializeComponent();
            AutosaveLocation = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\Autosave.csv";
            if (initializestring != null)
            {
                NameTextBox.Text = initializestring;
                InitializeButton_Click(this, new EventArgs());
            }

        }

        //Mostly for remote starts and designed to sync up with other programs via the TriggerNotify and TimeStepNotify lists.
        public AD7124_8Form(AD7128_8NameSp.AD7124_8StartClass ksc)
        {
            InitializeComponent();
            AutosaveLocation = ksc.AutoSaveLocation;

            NumReads = 4;

            //If we dont have a SN just start with the default
            if (ksc.SN != null)
            {
                NameTextBox.Text = ksc.SN;
                InitializeButton_Click(this, new EventArgs());
            }

            //
            AreWeSyncing = ksc.HowToStep;
            TimeStepNotify = ksc.TimeStepNotify;
            if (AreWeSyncing == SyncType.Synchronous)
            {
                if (TimeStepNotify == null)//needs a way to time step
                {
                    throw new Exception("Synchronous operation requires a TimeStepNotify");
                }
                PeriodNumericUpDown.Enabled = false;
                PeriodLabel.Enabled = false;
            }
            TriggerNotify = ksc.TriggerNotify;

            OffsetSec = ksc.OffsetTimeInSec;
            if ((ksc.StartFiles != null) && (ksc.StartFiles.Count != 0))
            {

                StartFileNotify = ksc.StartFiles;
                string S;
                while (!StartFileNotify.TryTake(out S)) ;

                StartStopButton_Click(S, new EventArgs());
            }
            else
            {
                throw new Exception("Not an actual start file or no file");
            }

        }

        #endregion

        #region form variables
        //Communication interface with the board
        private SerialDevices.IMeasurementDevice AD7124_8;

        //Holds the current values and ties them to a list on the GUI 
        private BindingList<NameValueCombo> CurrentValsList = new BindingList<NameValueCombo>();

        //What is the value of current right now
        private double CurrentNow_uA = 500;

        //holds all the data from the device
        private DataTable Results = new DataTable("AllData");

        //When the readings were started
        private DateTime StartTime;
        //Extra time relative to some desired reference
        private double OffsetSec = 0;

        //Last reading time
        private DateTime MostRecent = new DateTime();

        //Used if not all the data is to be saved
        private int LastSaveIndex = 0;

        //Is this the first time through the reading loop
        private bool FristStart = true;
        private bool FirstMeas = true;

        //SN for devce. The communication iterface will use this to find the correct device
        private string SN;

        //Where should the data be saved
        private string AutosaveLocation;

        private int NumReads = 4;//used by sweep for num of steps. used by multi for number of connected devices.

        //Available currents on the AD7124-8
        private static readonly double[] CurrentRanges_uA = {
                                                              0,
                                                              50,
                                                              100,
                                                              250,
                                                              500,
                                                              750,
                                                              1000,
                                                            };


        public enum SyncType//need values in order so we can used drop down
        {
            Synchronous = 0,
            ASynchronous = 1
        }

        //Synchronous means data is only taken when the TimeStepNotify has a new item
        public SyncType AreWeSyncing = SyncType.ASynchronous;

        //Setpoints for currents and when to change them
        public class Setpoints
        {
            public double Sec;
            public double Val;

            public Setpoints(double s, double v)
            {
                Sec = s;
                Val = v;
            }
        }

        //If the system is set to run continuously, the system will repeat the setpoints file but will add a bit extra to the end.
        public double SecsOnEnd = 60;
        //holds all the setpoints
        private List<Setpoints> SetpointList = new List<Setpoints>();
        //A
        public bool Triggered = true;
        public DateTime TriggerTime = DateTime.Now;

        //Adding a new time to these notifies the system that we should trigger, use a new startfile, or take a reading. 
        public BlockingCollection<int> TriggerNotify;
        public BlockingCollection<string> StartFileNotify;
        public BlockingCollection<int> TimeStepNotify;


        #endregion

        #region form events
        private void InitializeButton_Click(object sender, EventArgs e)
        {

            ResultsChart.Enabled = true;

            try
            {

                AD7124_8 = new AD7128_8Serial();
                //prep this device
                AD7124_8.DeviceID = SN;
                AD7124_8.DeviceName = "AD7124_8";

                //This order matters
                AD7124_8.ReadDataTemplate.Add(new SerialDevices.GenericReadResult(SerialDevices.ResultType.Temperature));
                AD7124_8.ReadDataTemplate.Add(new SerialDevices.GenericReadResult(SerialDevices.ResultType.Temperature));
                AD7124_8.ReadDataTemplate.Add(new SerialDevices.GenericReadResult(SerialDevices.ResultType.Temperature));
                AD7124_8.ReadDataTemplate.Add(new SerialDevices.GenericReadResult(SerialDevices.ResultType.Current));
                //The device wants you to make sure it is initialized
                AD7124_8.Initialized = true;
                AD7124_8.Start();
                if (AD7124_8.Port.FoundPort == false) this.Close();//Quit if start failed
                Thread.Sleep(100);
            }
            catch (Exception ex1)
            {
                MessageBox.Show("Not a Valid Instrument Resource Name OR Something went wrong", "CSharpApplication", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            //Prep the results table
            Results.Columns.Add("Time (s)");
            Results.Columns.Add("Time of Day");
            Results.Columns["Time of Day"].DataType = typeof(DateTime);//need to do this so that we can format output properly
            Results.Columns.Add("Current");
            //Prep the chart
            ResultsChart.Series.Add(Results.Columns["Current"].ColumnName);
            ResultsChart.Series[Results.Columns["Current"].ColumnName].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
            ResultsChart.Series[Results.Columns["Current"].ColumnName].YAxisType = System.Windows.Forms.DataVisualization.Charting.AxisType.Secondary;

            //Used to display the results right now
            CurrentValsList.Add(new NameValueCombo("Current", 0.1));
            //Set up a trace for each of the results
            for (int count = 0; count < (AD7124_8.ReadDataTemplate.Count - 1); count++)
            {
                Results.Columns.Add("Ch" + count + " Res");
                ResultsChart.Series.Add(Results.Columns["Ch" + count + " Res"].ColumnName);
                ResultsChart.Series[Results.Columns["Ch" + count + " Res"].ColumnName].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine;
                ResultsChart.ChartAreas[0].AxisY.MajorGrid.LineWidth = 0;
                ResultsChart.ChartAreas[0].AxisY.IsStartedFromZero = false;
                //count++;
                //Used to display the results right now
                CurrentValsList.Add(new NameValueCombo("Ch" + count + " Res", 150));
            }
            //Attach the the values right now to the display table
            CurrentDataGridView.DataSource = CurrentValsList;
            System.IO.File.WriteAllText(AutosaveLocation, string.Empty);//Clears file. We will just be appending to save write time

        }


        /// <summary>
        /// Handles the Click event of the StartStopButton control. Starts reading the AD7124-8 device and plots it.
        /// Initializes the autosave file. Reads the script file.
        /// Note that if the form is being run asychronously, it wont actually start it b/c it is waiting for pings from the notifies.
        /// It will check for notifies every 10 ms.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        /// <exception cref="Exception">Something wrong with button</exception>
        private void StartStopButton_Click(object sender, EventArgs e)
        {

            if (FristStart)//Setup some stuff if running for the first time
            {
                double time;
                double.TryParse(ExtraTimeTextBox.Text, out time);
                SecsOnEnd = time;

                StartTime = DateTime.Now;

                FristStart = false;
                string ImportFilename;
                if (typeof(string) == sender.GetType())
                {
                    ImportFilename = (string)sender;
                }
                else
                {
                    OpenFileDialog FileDialog = new OpenFileDialog();

                    // Set filter options and filter index.
                    FileDialog.Filter = "CSV Files (.csv)|*.csv";
                    FileDialog.FilterIndex = 1;
                    FileDialog.Multiselect = false;

                    //// Process input if the user clicked OK.
                    if (FileDialog.ShowDialog() == DialogResult.OK)
                    {
                        ImportFilename = FileDialog.FileName;
                    }
                    else
                    {
                        MessageBox.Show("Not a valid file");
                        return;
                    }
                }
                PopulateStartFile(ImportFilename);
                timer_autosave.Start();
            }
            if (StartStopButton.Text == "Start")
            {
                StartStopButton.Text = "Stop";//Toggle label
                if (AreWeSyncing == SyncType.ASynchronous)
                {
                    MainTimer.Interval = (int)PeriodNumericUpDown.Value;
                }
                else
                {
                    //just use default
                }
                int throwaway;
                while (TimeStepNotify.TryTake(out throwaway, 25)) { }//jetison all built up time steps 


                MainTimer.Start();
                timer_autosave.Start();
            }
            else if (StartStopButton.Text == "Stop")
            {
                StartStopButton.Text = "Start";//Toggle label
                MainTimer.Stop();
                timer_autosave.Stop();
            }
            else
            {
                throw new Exception("Something wrong with button");
            }
        }

        /// <summary>
        /// Handles the Tick event of the timer1 control. Each step either checks the notify arrays if in Synchronous mode. If in Async...
        /// it simply steps. At each step, the function checks for triggers (repeat the script), changes the ouput if commanded by script, stores and plots the data.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void MainTimer_Tick(object sender, EventArgs e)
        {
            if ((StartFileNotify.Count != 0))
            {
                string S;
                while (!StartFileNotify.TryTake(out S)) ;
                PopulateStartFile(S);
            }
            if ((TimeStepNotify.Count != 0) || (AreWeSyncing == SyncType.ASynchronous))
            {
                int a;
                TimeStepNotify.TryTake(out a);
                double SecsNow = 0;

                Setpoints Current = new Setpoints(0, 0);

                if (TriggerNotify.Count > 0)
                {
                    Triggered = true;
                    TriggerTime = DateTime.Now;
                    TriggerNotify.Take();
                }

                if (ContRepeatRadioButton.Checked)//todo you stopped here.  need to add triggering
                {
                    SecsNow = ((DateTime.Now - StartTime).TotalSeconds);
                    Current = SetpointList.Last(x => (SecsNow % (SetpointList[SetpointList.Count - 1].Sec + SecsOnEnd)) >= x.Sec);
                    if (Current == null)
                    {
                        Current = SetpointList[SetpointList.Count - 1];
                    }
                }
                else if (TrigRepeatRadioButton.Checked)
                {
                    SecsNow = ((DateTime.Now - TriggerTime).TotalSeconds);

                    Current = SetpointList.Last(x => (SecsNow) >= x.Sec);
                    if (Current == null)
                    {
                        Current = SetpointList[SetpointList.Count - 1];
                    }
                }
                else
                {

                }
                if (!FirstMeas)
                {
                    LoadDataToTable();
                    if (((Results.Rows.Count % 5) == 0))
                    {


                        ResultsChart.Series["Current"].Points.AddXY(
    Results.Rows[Results.Rows.Count - 1]["Time (s)"],
    Results.Rows[Results.Rows.Count - 1]["Current"]
    );
                        CurrentValsList[0].Value = Convert.ToDouble(Results.Rows[Results.Rows.Count - 1]["Current"]);

                        for (int count = 0; count < (AD7124_8.ReadDataTemplate.Count - 1); count++)
                        {
                            ResultsChart.Series["Ch" + count + " Res"].Points.AddXY(
    Results.Rows[Results.Rows.Count - 1]["Time (s)"],
    Convert.ToDouble(Results.Rows[Results.Rows.Count - 1]["Ch" + count + " Res"]) / Convert.ToDouble(Results.Rows[1]["Ch" + count + " Res"])
    );
                            CurrentValsList[count + 1].Value = Convert.ToDouble(Results.Rows[Results.Rows.Count - 1]["Ch" + count + " Res"]);
                        }

                        CurrentDataGridView.Refresh();
                    }

                }
                else
                {
                    FirstMeas = false;

                }

                if (Current.Val != CurrentNow_uA)
                {
                    AD7124_8.SendVal(Current.Val);
                    CurrentNow_uA = Current.Val;
                    Thread.Sleep(10);
                }

                //Asks for a result bur doesn't wait for it. Otherwise most of the program's time would be just waiting.
                AD7124_8.KickOffRead();

                MostRecent = DateTime.Now;
                //Thread.Sleep(20);



            }
        }

        /// <summary>
        /// Handles the Click event of the SaveButton control. Gives file dialog to save
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (saveFileDialog1.FileName != null)
                    {

                        StringBuilder sb = new StringBuilder();

                        IEnumerable<string> columnNames = Results.Columns.Cast<DataColumn>().
                                                          Select(column => column.ColumnName);
                        sb.AppendLine(string.Join(",", columnNames));

                        foreach (DataRow row in Results.Rows)
                        {
                            IEnumerable<string> fields = row.ItemArray.Select(field => ((field.GetType() == typeof(DateTime)) ? ((DateTime)field).ToString("MM/dd/yyyy HH:mm:ss.fff") : field.ToString()));
                            sb.AppendLine(string.Join(",", fields));
                        }
                        System.IO.File.WriteAllText(saveFileDialog1.FileName, sb.ToString());
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Handles the Tick event of the timer_autosave control. Autosave every so often (1 min)
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void timer_autosave_Tick(object sender, EventArgs e)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                if (LastSaveIndex == 0)
                {

                    IEnumerable<string> columnNames = Results.Columns.Cast<DataColumn>().
                                                      Select(column => column.ColumnName);
                    sb.AppendLine(string.Join(",", columnNames));
                }


                for (int i = LastSaveIndex; i < Results.Rows.Count; i++)
                {
                    IEnumerable<string> fields = Results.Rows[i].ItemArray.Select(field => ((field.GetType() == typeof(DateTime)) ? ((DateTime)field).ToString("MM/dd/yyyy HH:mm:ss.fff") : field.ToString()));
                    sb.AppendLine(string.Join(",", fields));
                }
                LastSaveIndex = Results.Rows.Count;

                System.IO.File.AppendAllText(AutosaveLocation, sb.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: Could not autosave. Original error: " + ex.Message);
            }
        }

        /// <summary>
        /// Handles the ValueChanged event of the numericUpDown1 control. Chnages the MainTimer interval
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (AreWeSyncing == SyncType.ASynchronous)
            {
                MainTimer.Interval = (int)PeriodNumericUpDown.Value;
            }
        }

        /// <summary>
        /// Handles the Click event of the TriggerButton control. Sets the system to trigger on next round (repeat script)
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void TriggerButton_Click(object sender, EventArgs e)
        {
            Triggered = true;
            TriggerTime = DateTime.Now;
        }



        #endregion

        #region form functions


        /// <summary>
        /// Loads the data to table. Grabs data from the device and loads it into the table as well as the 
        /// </summary>
        /// <returns></returns>
        private bool LoadDataToTable()
        {
            DataRow Line = Results.NewRow();
            Line = Results.NewRow();

            Line["Time (S)"] = ((MostRecent - StartTime).TotalSeconds + OffsetSec);
            Line["Time of Day"] = MostRecent;


            AD7124_8.ReadReadyData(AD7124_8.ParseReadVal_SetAll);

            //See the AD7124-8 datasheet for the origin of the formula beelow
            for (int count = 0; count < (AD7124_8.ReadDataTemplate.Count - 1); count++)
            {
                Line["Ch" + count + " Res"] = -1 * ((AD7124_8.ReadDataTemplate[count].Result / Math.Pow(2, 23) - 1)) * 250;//Assumes a 250 Ohm reference resistor
            }
            Line["Current"] = -1 * ((AD7124_8.ReadDataTemplate.Last().Result / Math.Pow(2, 23) - 1)) * (2.5 / 250);//Assumes a 250 Ohm reference resistor

            Results.Rows.Add(Line);

            return true;

        }

        /// <summary>
        /// Populates the settings array. Takes a 2d csv file. with columns of Secs and Current
        /// </summary>
        /// <param name="ImportFilename">The script filename.</param>
        private void PopulateStartFile(string ImportFilename)
        {
            SetpointList = new List<Setpoints>();
            using (var fs = System.IO.File.OpenRead(ImportFilename))
            using (var reader = new System.IO.StreamReader(fs))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    SetpointList.Add(new Setpoints(Convert.ToDouble(values[0]), 1e6 * Convert.ToDouble(values[1])));

                }
            }
        }
        #endregion




        /// <summary>
        /// Handles the FormClosing event of the AD7124_8Form control. Makes sure the data is saved
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FormClosingEventArgs"/> instance containing the event data.</param>
        private void AD7124_8Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer_autosave_Tick(this, new EventArgs());
            System.Threading.Thread.Sleep(50);
            AD7124_8.Port.Close();
        }

    }

    /// <summary>
    /// Serial interface for the AD7124-8. 
    /// </summary>
    /// <seealso cref="SerialDevices.IMeasurementDevice" />
    [Serializable]
    public class AD7128_8Serial : SerialDevices.IMeasurementDevice
    {
        private const string DeviceTypeName = "AD7128_8Serial";
        private const string CallString = "ID ?";
        private readonly string ResponseString = "20170915_AD7124_8_JBM\r\n";
        //public string SNString;
        public const string WritePrefixString = "Current";
        private readonly string TerminatorString = ";";
        private const string ReadString = "Read;";
        private const string ChannelString = "Channel ";


        private String[] SplitString = { "Ch ", ": ", "\r\n" };

        public List<DateTime> errtimes = new List<DateTime>();
        public List<string> errstr = new List<string>();

        /// <summary>
        /// Parses the read value. Splits up the values returned and puts them into the ReadDataTemplate.
        /// </summary>
        /// <param name="ReadWord">The read word.</param>
        /// <returns></returns>
        public override double ParseReadVal_SetAll(string ReadWord)
        {

            List<SerialDevices.GenericReadResult> OldResult2 = new List<SerialDevices.GenericReadResult>();
            OldResult2 = ReadDataTemplate;
            int channel;
            try
            {
                /*expected format
                 * Ch 0: Val1
                 * Ch 1: Val2
                 * ...
                 * Ch N: ValN
                 */
                for (int i = 0; i < ReadDataTemplate.Count; i++)
                {
                    channel = Convert.ToUInt16(ReadWord.Split(SplitString, StringSplitOptions.RemoveEmptyEntries)[i * 2]);

                    ReadDataTemplate[channel].Result = Convert.ToDouble(ReadWord.Split(SplitString, StringSplitOptions.RemoveEmptyEntries)[i * 2 + 1]);
                }

                return 1;
            }
            catch
            {

                errtimes.Add(DateTime.Now);
                errstr.Add(ReadWord);
                ReadDataTemplate = OldResult2;
                return 0;
            }

        }

        public double ParseReadVal(string ReadWord)
        {
            return Convert.ToDouble(ReadWord);
        }


        /// <summary>
        /// Sends a current value.
        /// </summary>
        /// <param name="Val">The value.</param>
        public override void SendVal(double Val)
        {
            Port.WriteLine(WritePrefixString + ((int)Val).ToString() + TerminatorString);
        }

        /// <summary>
        /// Adds (or changes if the channel already is used) a read channel to the AD7124-8. Always enables the channel.
        /// </summary>
        /// <param name="ChNum">The ch number.</param>
        /// <param name="PosPinNum">The positive pin number.</param>
        /// <param name="NegPinNum">The negitive pin number.</param>
        /// <param name="SetupNum">The setup number which determines operation of the channel (see datasheet).</param>
        public void AddChannel(int ChNum, int PosPinNum, int NegPinNum, int SetupNum = 0)
        {
            Port.WriteLine(ChannelString + ChNum + ',' + '1' + ',' + PosPinNum + ',' + NegPinNum + ',' + SetupNum + TerminatorString);
        }

        /// <summary>
        /// Starts this instance, finds port, adds the channels based on the ReadDataTemplate (must be setup before running this) and zeros the current.
        /// </summary>
        /// <exception cref="Exception">
        /// Trying to add too many channels to AD7124_8
        /// or
        /// Template can only be set once
        /// </exception>
        public override void Start()
        {
            if (Initialized)
            {
                string ResponseString_Build = ResponseString;
                Port = new SerialDevices.DeviceSerial(DeviceName, CallString + TerminatorString, ResponseString_Build, ReadString, 115200);

                if (Port.FoundPort)
                {
                    Port.WriteLine("Reset;");//Resets the system. Will reset the speed and filters and returns offsets back to 0;
                    Thread.Sleep(100);
                    for (int i = 0; i < (ReadDataTemplate.Count - 1); i++)//Assumes the last one is the current channel
                    {
                        if ((i * 2) >= 14)
                        {
                            throw new Exception("Trying to add too many channels to AD7124_8");
                        }
                        if ((i * 2) < 8)//current comes from pin 8 current sense comes from 9 and 10
                        {
                            AddChannel(i, i * 2 + 3, i * 2 + 2);
                        }
                        else
                        {
                            AddChannel(i, i * 2 + 4, i * 2 + 3);
                        }
                        Thread.Sleep(100); //avoids sending too fast
                    }
                    Port.WriteLine("Addr 0x03 0x000008;");//sets the current output to pin 8 and sets the current to zero
                    Thread.Sleep(50); //avoids sending too fast
                    AddChannel(ReadDataTemplate.Count - 1, 9, 10, 1);//setup one uses the internal reference 
                    Thread.Sleep(50); //avoids sending too fast
                }
                Port.NewLine = "\r\n";
            }
            else
            {
                throw new Exception("Template can only be set once");
            }
        }
    }

    [Serializable]
    public class NameValueCombo
    {
        public string Name { get; set; }
        public double Value { get; set; }
        public NameValueCombo()
        {
            Name = "";
            Value = 0;
        }
        public NameValueCombo(string k, double v)
        {
            Name = k;
            Value = v;
        }
    }

    /// <summary>
    /// Holds the data needed to startup form and run it remotely
    /// </summary>
    public class AD7124_8StartClass
    {
        public string SN;
        public BlockingCollection<string> StartFiles;
        public string AutoSaveLocation = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\Autosave.csv";
        public BlockingCollection<int> TriggerNotify;
        public BlockingCollection<int> TimeStepNotify;
        public double OffsetTimeInSec = 0;
        public AD7124_8Form.SyncType HowToStep = AD7124_8Form.SyncType.Synchronous;
    }
}
