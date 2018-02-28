namespace AD7128_8NameSp
{
    partial class AD7124_8Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AD7124_8Form));
            this.NameTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.InitializeButton = new System.Windows.Forms.Button();
            this.ResultsChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.StartStopButton = new System.Windows.Forms.Button();
            this.MainTimer = new System.Windows.Forms.Timer(this.components);
            this.SaveButton = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.timer_autosave = new System.Windows.Forms.Timer(this.components);
            this.PeriodNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.PeriodLabel = new System.Windows.Forms.Label();
            this.ExtraTimeTextBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.TriggerGroupBox = new System.Windows.Forms.GroupBox();
            this.TriggerButton = new System.Windows.Forms.Button();
            this.TrigRepeatRadioButton = new System.Windows.Forms.RadioButton();
            this.ContRepeatRadioButton = new System.Windows.Forms.RadioButton();
            this.CurrentDataGridView = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.ResultsChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PeriodNumericUpDown)).BeginInit();
            this.TriggerGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CurrentDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // NameTextBox
            // 
            this.NameTextBox.Location = new System.Drawing.Point(16, 26);
            this.NameTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.NameTextBox.Name = "NameTextBox";
            this.NameTextBox.Size = new System.Drawing.Size(433, 22);
            this.NameTextBox.TabIndex = 0;
            this.NameTextBox.Text = "Serial Number";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 6);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(208, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Resource Name / Logical Name";
            // 
            // InitializeButton
            // 
            this.InitializeButton.Location = new System.Drawing.Point(16, 58);
            this.InitializeButton.Margin = new System.Windows.Forms.Padding(4);
            this.InitializeButton.Name = "InitializeButton";
            this.InitializeButton.Size = new System.Drawing.Size(255, 31);
            this.InitializeButton.TabIndex = 3;
            this.InitializeButton.Text = "Initialize and Enable Remote Control";
            this.InitializeButton.UseVisualStyleBackColor = true;
            this.InitializeButton.Click += new System.EventHandler(this.InitializeButton_Click);
            // 
            // ResultsChart
            // 
            chartArea1.AxisX2.IsStartedFromZero = false;
            chartArea1.Name = "ChartArea1";
            this.ResultsChart.ChartAreas.Add(chartArea1);
            this.ResultsChart.Enabled = false;
            legend1.Name = "Legend1";
            this.ResultsChart.Legends.Add(legend1);
            this.ResultsChart.Location = new System.Drawing.Point(11, 287);
            this.ResultsChart.Margin = new System.Windows.Forms.Padding(4);
            this.ResultsChart.Name = "ResultsChart";
            this.ResultsChart.Size = new System.Drawing.Size(809, 405);
            this.ResultsChart.TabIndex = 18;
            this.ResultsChart.Text = "chart1";
            // 
            // StartStopButton
            // 
            this.StartStopButton.Location = new System.Drawing.Point(9, 251);
            this.StartStopButton.Margin = new System.Windows.Forms.Padding(4);
            this.StartStopButton.Name = "StartStopButton";
            this.StartStopButton.Size = new System.Drawing.Size(100, 28);
            this.StartStopButton.TabIndex = 19;
            this.StartStopButton.Text = "Start";
            this.StartStopButton.UseVisualStyleBackColor = true;
            this.StartStopButton.Click += new System.EventHandler(this.StartStopButton_Click);
            // 
            // MainTimer
            // 
            this.MainTimer.Interval = 10;
            this.MainTimer.Tick += new System.EventHandler(this.MainTimer_Tick);
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(125, 251);
            this.SaveButton.Margin = new System.Windows.Forms.Padding(4);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(100, 28);
            this.SaveButton.TabIndex = 20;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // timer_autosave
            // 
            this.timer_autosave.Interval = 60000;
            this.timer_autosave.Tick += new System.EventHandler(this.timer_autosave_Tick);
            // 
            // PeriodNumericUpDown
            // 
            this.PeriodNumericUpDown.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.PeriodNumericUpDown.Location = new System.Drawing.Point(329, 118);
            this.PeriodNumericUpDown.Margin = new System.Windows.Forms.Padding(4);
            this.PeriodNumericUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.PeriodNumericUpDown.Minimum = new decimal(new int[] {
            240,
            0,
            0,
            0});
            this.PeriodNumericUpDown.Name = "PeriodNumericUpDown";
            this.PeriodNumericUpDown.Size = new System.Drawing.Size(76, 22);
            this.PeriodNumericUpDown.TabIndex = 27;
            this.PeriodNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.PeriodNumericUpDown.Value = new decimal(new int[] {
            350,
            0,
            0,
            0});
            this.PeriodNumericUpDown.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // PeriodLabel
            // 
            this.PeriodLabel.AutoSize = true;
            this.PeriodLabel.Location = new System.Drawing.Point(325, 98);
            this.PeriodLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.PeriodLabel.Name = "PeriodLabel";
            this.PeriodLabel.Size = new System.Drawing.Size(169, 17);
            this.PeriodLabel.TabIndex = 28;
            this.PeriodLabel.Text = "Period in msec (min. 240)";
            // 
            // ExtraTimeTextBox
            // 
            this.ExtraTimeTextBox.Location = new System.Drawing.Point(129, 42);
            this.ExtraTimeTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ExtraTimeTextBox.Name = "ExtraTimeTextBox";
            this.ExtraTimeTextBox.Size = new System.Drawing.Size(100, 22);
            this.ExtraTimeTextBox.TabIndex = 34;
            this.ExtraTimeTextBox.Text = "60";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(125, 26);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(171, 17);
            this.label7.TabIndex = 35;
            this.label7.Text = "Extra time per cycle (sec):";
            // 
            // TriggerGroupBox
            // 
            this.TriggerGroupBox.Controls.Add(this.TriggerButton);
            this.TriggerGroupBox.Controls.Add(this.TrigRepeatRadioButton);
            this.TriggerGroupBox.Controls.Add(this.ExtraTimeTextBox);
            this.TriggerGroupBox.Controls.Add(this.label7);
            this.TriggerGroupBox.Controls.Add(this.ContRepeatRadioButton);
            this.TriggerGroupBox.Location = new System.Drawing.Point(11, 95);
            this.TriggerGroupBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TriggerGroupBox.Name = "TriggerGroupBox";
            this.TriggerGroupBox.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TriggerGroupBox.Size = new System.Drawing.Size(312, 138);
            this.TriggerGroupBox.TabIndex = 31;
            this.TriggerGroupBox.TabStop = false;
            this.TriggerGroupBox.Text = "How to repeat trace";
            // 
            // TriggerButton
            // 
            this.TriggerButton.Location = new System.Drawing.Point(129, 92);
            this.TriggerButton.Margin = new System.Windows.Forms.Padding(4);
            this.TriggerButton.Name = "TriggerButton";
            this.TriggerButton.Size = new System.Drawing.Size(100, 28);
            this.TriggerButton.TabIndex = 36;
            this.TriggerButton.Text = "Trigger";
            this.TriggerButton.UseVisualStyleBackColor = true;
            this.TriggerButton.Click += new System.EventHandler(this.TriggerButton_Click);
            // 
            // TrigRepeatRadioButton
            // 
            this.TrigRepeatRadioButton.AutoSize = true;
            this.TrigRepeatRadioButton.Checked = true;
            this.TrigRepeatRadioButton.Location = new System.Drawing.Point(5, 96);
            this.TrigRepeatRadioButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.TrigRepeatRadioButton.Name = "TrigRepeatRadioButton";
            this.TrigRepeatRadioButton.Size = new System.Drawing.Size(75, 21);
            this.TrigRepeatRadioButton.TabIndex = 1;
            this.TrigRepeatRadioButton.TabStop = true;
            this.TrigRepeatRadioButton.Text = "Trigger";
            this.TrigRepeatRadioButton.UseVisualStyleBackColor = true;
            // 
            // ContRepeatRadioButton
            // 
            this.ContRepeatRadioButton.AutoSize = true;
            this.ContRepeatRadioButton.Location = new System.Drawing.Point(7, 22);
            this.ContRepeatRadioButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ContRepeatRadioButton.Name = "ContRepeatRadioButton";
            this.ContRepeatRadioButton.Size = new System.Drawing.Size(110, 21);
            this.ContRepeatRadioButton.TabIndex = 0;
            this.ContRepeatRadioButton.Text = "Continuously";
            this.ContRepeatRadioButton.UseVisualStyleBackColor = true;
            // 
            // CurrentDataGridView
            // 
            this.CurrentDataGridView.AllowUserToAddRows = false;
            this.CurrentDataGridView.AllowUserToDeleteRows = false;
            this.CurrentDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.CurrentDataGridView.Location = new System.Drawing.Point(500, 95);
            this.CurrentDataGridView.Margin = new System.Windows.Forms.Padding(4);
            this.CurrentDataGridView.Name = "CurrentDataGridView";
            this.CurrentDataGridView.Size = new System.Drawing.Size(320, 185);
            this.CurrentDataGridView.TabIndex = 32;
            // 
            // AD7124_8Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(836, 727);
            this.Controls.Add(this.CurrentDataGridView);
            this.Controls.Add(this.TriggerGroupBox);
            this.Controls.Add(this.PeriodLabel);
            this.Controls.Add(this.PeriodNumericUpDown);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.StartStopButton);
            this.Controls.Add(this.ResultsChart);
            this.Controls.Add(this.InitializeButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.NameTextBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "AD7124_8Form";
            this.Text = "AD7124_8Form";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AD7124_8Form_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.ResultsChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PeriodNumericUpDown)).EndInit();
            this.TriggerGroupBox.ResumeLayout(false);
            this.TriggerGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CurrentDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button InitializeButton;
        private System.Windows.Forms.DataVisualization.Charting.Chart ResultsChart;
        private System.Windows.Forms.Button StartStopButton;
        private System.Windows.Forms.Timer MainTimer;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Timer timer_autosave;
        private System.Windows.Forms.NumericUpDown PeriodNumericUpDown;
        private System.Windows.Forms.Label PeriodLabel;
        private System.Windows.Forms.TextBox ExtraTimeTextBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox TriggerGroupBox;
        private System.Windows.Forms.RadioButton TrigRepeatRadioButton;
        private System.Windows.Forms.RadioButton ContRepeatRadioButton;
        public System.Windows.Forms.TextBox NameTextBox;
        public System.Windows.Forms.Button TriggerButton;
        private System.Windows.Forms.DataGridView CurrentDataGridView;
    }
}

