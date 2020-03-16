namespace WeatherGrabber.App
{
    partial class MainForm
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
            this.cbCity = new System.Windows.Forms.ComboBox();
            this.lCity = new System.Windows.Forms.Label();
            this.bGet = new System.Windows.Forms.Button();
            this.bRefresh = new System.Windows.Forms.Button();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.tbOutput = new System.Windows.Forms.TextBox();
            this.cbTemperatureType = new System.Windows.Forms.ComboBox();
            this.gbMeasurement = new System.Windows.Forms.GroupBox();
            this.lTemperatureType = new System.Windows.Forms.Label();
            this.lWindType = new System.Windows.Forms.Label();
            this.cbWindType = new System.Windows.Forms.ComboBox();
            this.lPressureType = new System.Windows.Forms.Label();
            this.cbPressureType = new System.Windows.Forms.ComboBox();
            this.gbMeasurement.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbCity
            // 
            this.cbCity.FormattingEnabled = true;
            this.cbCity.Location = new System.Drawing.Point(73, 15);
            this.cbCity.Margin = new System.Windows.Forms.Padding(4);
            this.cbCity.Name = "cbCity";
            this.cbCity.Size = new System.Drawing.Size(223, 24);
            this.cbCity.TabIndex = 0;
            // 
            // lCity
            // 
            this.lCity.AutoSize = true;
            this.lCity.Location = new System.Drawing.Point(16, 18);
            this.lCity.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lCity.Name = "lCity";
            this.lCity.Size = new System.Drawing.Size(47, 16);
            this.lCity.TabIndex = 1;
            this.lCity.Text = "Город";
            // 
            // bGet
            // 
            this.bGet.Location = new System.Drawing.Point(473, 12);
            this.bGet.Margin = new System.Windows.Forms.Padding(4);
            this.bGet.Name = "bGet";
            this.bGet.Size = new System.Drawing.Size(100, 28);
            this.bGet.TabIndex = 2;
            this.bGet.Text = "Узнать";
            this.bGet.UseVisualStyleBackColor = true;
            this.bGet.Click += new System.EventHandler(this.bGet_Click);
            // 
            // bRefresh
            // 
            this.bRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bRefresh.Location = new System.Drawing.Point(645, 12);
            this.bRefresh.Margin = new System.Windows.Forms.Padding(4);
            this.bRefresh.Name = "bRefresh";
            this.bRefresh.Size = new System.Drawing.Size(100, 28);
            this.bRefresh.TabIndex = 6;
            this.bRefresh.Text = "Обновить";
            this.bRefresh.UseVisualStyleBackColor = true;
            this.bRefresh.Click += new System.EventHandler(this.bRefresh_Click);
            // 
            // dtpDate
            // 
            this.dtpDate.Location = new System.Drawing.Point(305, 15);
            this.dtpDate.Margin = new System.Windows.Forms.Padding(4);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(160, 22);
            this.dtpDate.TabIndex = 1;
            // 
            // tbOutput
            // 
            this.tbOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbOutput.Location = new System.Drawing.Point(19, 66);
            this.tbOutput.Multiline = true;
            this.tbOutput.Name = "tbOutput";
            this.tbOutput.Size = new System.Drawing.Size(448, 288);
            this.tbOutput.TabIndex = 99;
            this.tbOutput.TabStop = false;
            // 
            // cbTemperatureType
            // 
            this.cbTemperatureType.FormattingEnabled = true;
            this.cbTemperatureType.Location = new System.Drawing.Point(6, 41);
            this.cbTemperatureType.Name = "cbTemperatureType";
            this.cbTemperatureType.Size = new System.Drawing.Size(260, 24);
            this.cbTemperatureType.TabIndex = 3;
            this.cbTemperatureType.SelectedIndexChanged += new System.EventHandler(this.cbTemperatureType_SelectedIndexChanged);
            // 
            // gbMeasurement
            // 
            this.gbMeasurement.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gbMeasurement.Controls.Add(this.cbPressureType);
            this.gbMeasurement.Controls.Add(this.lPressureType);
            this.gbMeasurement.Controls.Add(this.cbWindType);
            this.gbMeasurement.Controls.Add(this.lWindType);
            this.gbMeasurement.Controls.Add(this.lTemperatureType);
            this.gbMeasurement.Controls.Add(this.cbTemperatureType);
            this.gbMeasurement.Location = new System.Drawing.Point(473, 66);
            this.gbMeasurement.Name = "gbMeasurement";
            this.gbMeasurement.Size = new System.Drawing.Size(272, 171);
            this.gbMeasurement.TabIndex = 7;
            this.gbMeasurement.TabStop = false;
            this.gbMeasurement.Text = "Единицы измерения";
            // 
            // lTemperatureType
            // 
            this.lTemperatureType.AutoSize = true;
            this.lTemperatureType.Location = new System.Drawing.Point(7, 22);
            this.lTemperatureType.Name = "lTemperatureType";
            this.lTemperatureType.Size = new System.Drawing.Size(97, 16);
            this.lTemperatureType.TabIndex = 7;
            this.lTemperatureType.Text = "Температура";
            // 
            // lWindType
            // 
            this.lWindType.AutoSize = true;
            this.lWindType.Location = new System.Drawing.Point(7, 68);
            this.lWindType.Name = "lWindType";
            this.lWindType.Size = new System.Drawing.Size(111, 16);
            this.lWindType.TabIndex = 8;
            this.lWindType.Text = "Скорость ветра";
            // 
            // cbWindType
            // 
            this.cbWindType.FormattingEnabled = true;
            this.cbWindType.Location = new System.Drawing.Point(6, 87);
            this.cbWindType.Name = "cbWindType";
            this.cbWindType.Size = new System.Drawing.Size(260, 24);
            this.cbWindType.TabIndex = 4;
            this.cbWindType.SelectedIndexChanged += new System.EventHandler(this.cbWindType_SelectedIndexChanged);
            // 
            // lPressureType
            // 
            this.lPressureType.AutoSize = true;
            this.lPressureType.Location = new System.Drawing.Point(10, 118);
            this.lPressureType.Name = "lPressureType";
            this.lPressureType.Size = new System.Drawing.Size(73, 16);
            this.lPressureType.TabIndex = 10;
            this.lPressureType.Text = "Давление";
            // 
            // cbPressureType
            // 
            this.cbPressureType.FormattingEnabled = true;
            this.cbPressureType.Location = new System.Drawing.Point(6, 137);
            this.cbPressureType.Name = "cbPressureType";
            this.cbPressureType.Size = new System.Drawing.Size(260, 24);
            this.cbPressureType.TabIndex = 5;
            this.cbPressureType.SelectedIndexChanged += new System.EventHandler(this.cbPressureType_SelectedIndexChanged);
            // 
            // MainForm
            // 
            this.AcceptButton = this.bGet;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(753, 366);
            this.Controls.Add(this.gbMeasurement);
            this.Controls.Add(this.tbOutput);
            this.Controls.Add(this.dtpDate);
            this.Controls.Add(this.bRefresh);
            this.Controls.Add(this.bGet);
            this.Controls.Add(this.lCity);
            this.Controls.Add(this.cbCity);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(703, 405);
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.Text = "Погода";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.gbMeasurement.ResumeLayout(false);
            this.gbMeasurement.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbCity;
        private System.Windows.Forms.Label lCity;
        private System.Windows.Forms.Button bGet;
        private System.Windows.Forms.Button bRefresh;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.TextBox tbOutput;
        private System.Windows.Forms.ComboBox cbTemperatureType;
        private System.Windows.Forms.GroupBox gbMeasurement;
        private System.Windows.Forms.ComboBox cbPressureType;
        private System.Windows.Forms.Label lPressureType;
        private System.Windows.Forms.ComboBox cbWindType;
        private System.Windows.Forms.Label lWindType;
        private System.Windows.Forms.Label lTemperatureType;
    }
}

