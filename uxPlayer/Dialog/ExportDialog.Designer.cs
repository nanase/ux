namespace uxPlayer
{
	partial class ExportDialog
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
            this.groupBox_type = new System.Windows.Forms.GroupBox();
            this.radioButton_16bit = new System.Windows.Forms.RadioButton();
            this.radioButton_8bit = new System.Windows.Forms.RadioButton();
            this.comboBox_type = new System.Windows.Forms.ComboBox();
            this.groupBox_oversampling = new System.Windows.Forms.GroupBox();
            this.label_oversampling = new System.Windows.Forms.Label();
            this.trackBar_oversampling = new System.Windows.Forms.TrackBar();
            this.groupBox_samplingRate = new System.Windows.Forms.GroupBox();
            this.comboBox_samplingRate = new System.Windows.Forms.ComboBox();
            this.groupBox_size = new System.Windows.Forms.GroupBox();
            this.radioButton_unlimit = new System.Windows.Forms.RadioButton();
            this.label_sec = new System.Windows.Forms.Label();
            this.label_min = new System.Windows.Forms.Label();
            this.numericUpDown_sec = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown_min = new System.Windows.Forms.NumericUpDown();
            this.label_mb = new System.Windows.Forms.Label();
            this.numericUpDown_filesize = new System.Windows.Forms.NumericUpDown();
            this.radioButton_filesize = new System.Windows.Forms.RadioButton();
            this.radioButton_time = new System.Windows.Forms.RadioButton();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.textBox_saveto = new System.Windows.Forms.TextBox();
            this.button_open = new System.Windows.Forms.Button();
            this.button_close = new System.Windows.Forms.Button();
            this.groupBox_progress = new System.Windows.Forms.GroupBox();
            this.label_time = new System.Windows.Forms.Label();
            this.label_tick = new System.Windows.Forms.Label();
            this.label_filesize = new System.Windows.Forms.Label();
            this.label_progress = new System.Windows.Forms.Label();
            this.label_saveto = new System.Windows.Forms.Label();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.button_start = new System.Windows.Forms.Button();
            this.button_stop = new System.Windows.Forms.Button();
            this.groupBox_type.SuspendLayout();
            this.groupBox_oversampling.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_oversampling)).BeginInit();
            this.groupBox_samplingRate.SuspendLayout();
            this.groupBox_size.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_sec)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_min)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_filesize)).BeginInit();
            this.groupBox_progress.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox_type
            // 
            this.groupBox_type.Controls.Add(this.comboBox_type);
            this.groupBox_type.Controls.Add(this.radioButton_8bit);
            this.groupBox_type.Controls.Add(this.radioButton_16bit);
            this.groupBox_type.Location = new System.Drawing.Point(12, 12);
            this.groupBox_type.Name = "groupBox_type";
            this.groupBox_type.Size = new System.Drawing.Size(186, 82);
            this.groupBox_type.TabIndex = 0;
            this.groupBox_type.TabStop = false;
            this.groupBox_type.Text = "ファイル形式";
            // 
            // radioButton_16bit
            // 
            this.radioButton_16bit.AutoSize = true;
            this.radioButton_16bit.Checked = true;
            this.radioButton_16bit.Location = new System.Drawing.Point(85, 51);
            this.radioButton_16bit.Name = "radioButton_16bit";
            this.radioButton_16bit.Size = new System.Drawing.Size(51, 19);
            this.radioButton_16bit.TabIndex = 2;
            this.radioButton_16bit.TabStop = true;
            this.radioButton_16bit.Text = "16bit";
            this.radioButton_16bit.UseVisualStyleBackColor = true;
            // 
            // radioButton_8bit
            // 
            this.radioButton_8bit.AutoSize = true;
            this.radioButton_8bit.Location = new System.Drawing.Point(12, 51);
            this.radioButton_8bit.Name = "radioButton_8bit";
            this.radioButton_8bit.Size = new System.Drawing.Size(45, 19);
            this.radioButton_8bit.TabIndex = 1;
            this.radioButton_8bit.TabStop = true;
            this.radioButton_8bit.Text = "8bit";
            this.radioButton_8bit.UseVisualStyleBackColor = true;
            // 
            // comboBox_type
            // 
            this.comboBox_type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_type.FormattingEnabled = true;
            this.comboBox_type.Items.AddRange(new object[] {
            "WAVE PCM (無圧縮)"});
            this.comboBox_type.Location = new System.Drawing.Point(12, 22);
            this.comboBox_type.Name = "comboBox_type";
            this.comboBox_type.Size = new System.Drawing.Size(161, 23);
            this.comboBox_type.TabIndex = 0;
            // 
            // groupBox_oversampling
            // 
            this.groupBox_oversampling.Controls.Add(this.label_oversampling);
            this.groupBox_oversampling.Controls.Add(this.trackBar_oversampling);
            this.groupBox_oversampling.Location = new System.Drawing.Point(204, 76);
            this.groupBox_oversampling.Name = "groupBox_oversampling";
            this.groupBox_oversampling.Size = new System.Drawing.Size(186, 91);
            this.groupBox_oversampling.TabIndex = 1;
            this.groupBox_oversampling.TabStop = false;
            this.groupBox_oversampling.Text = "オーバーサンプリング";
            // 
            // label_oversampling
            // 
            this.label_oversampling.Location = new System.Drawing.Point(12, 70);
            this.label_oversampling.Name = "label_oversampling";
            this.label_oversampling.Size = new System.Drawing.Size(161, 19);
            this.label_oversampling.TabIndex = 1;
            this.label_oversampling.Text = "x1 (無効)";
            this.label_oversampling.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // trackBar_oversampling
            // 
            this.trackBar_oversampling.LargeChange = 1;
            this.trackBar_oversampling.Location = new System.Drawing.Point(12, 22);
            this.trackBar_oversampling.Maximum = 5;
            this.trackBar_oversampling.Name = "trackBar_oversampling";
            this.trackBar_oversampling.Size = new System.Drawing.Size(161, 45);
            this.trackBar_oversampling.TabIndex = 0;
            this.trackBar_oversampling.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBar_oversampling.ValueChanged += new System.EventHandler(this.trackBar1_ValueChanged);
            // 
            // groupBox_samplingRate
            // 
            this.groupBox_samplingRate.Controls.Add(this.comboBox_samplingRate);
            this.groupBox_samplingRate.Location = new System.Drawing.Point(204, 12);
            this.groupBox_samplingRate.Name = "groupBox_samplingRate";
            this.groupBox_samplingRate.Size = new System.Drawing.Size(186, 58);
            this.groupBox_samplingRate.TabIndex = 2;
            this.groupBox_samplingRate.TabStop = false;
            this.groupBox_samplingRate.Text = "サンプリング周波数";
            // 
            // comboBox_samplingRate
            // 
            this.comboBox_samplingRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_samplingRate.Items.AddRange(new object[] {
            "4000 Hz",
            "8000 Hz",
            "11025 Hz",
            "16000 Hz",
            "22050 Hz",
            "24000 Hz",
            "32000 Hz",
            "44100 Hz",
            "48000 Hz",
            "88200 Hz",
            "96000 Hz",
            "176400 Hz",
            "192000 Hz"});
            this.comboBox_samplingRate.Location = new System.Drawing.Point(12, 22);
            this.comboBox_samplingRate.Name = "comboBox_samplingRate";
            this.comboBox_samplingRate.Size = new System.Drawing.Size(161, 23);
            this.comboBox_samplingRate.TabIndex = 0;
            // 
            // groupBox_size
            // 
            this.groupBox_size.Controls.Add(this.radioButton_unlimit);
            this.groupBox_size.Controls.Add(this.radioButton_time);
            this.groupBox_size.Controls.Add(this.numericUpDown_min);
            this.groupBox_size.Controls.Add(this.numericUpDown_sec);
            this.groupBox_size.Controls.Add(this.label_sec);
            this.groupBox_size.Controls.Add(this.label_min);
            this.groupBox_size.Controls.Add(this.radioButton_filesize);
            this.groupBox_size.Controls.Add(this.numericUpDown_filesize);
            this.groupBox_size.Controls.Add(this.label_mb);
            this.groupBox_size.Location = new System.Drawing.Point(396, 12);
            this.groupBox_size.Name = "groupBox_size";
            this.groupBox_size.Size = new System.Drawing.Size(200, 155);
            this.groupBox_size.TabIndex = 3;
            this.groupBox_size.TabStop = false;
            this.groupBox_size.Text = "出力サイズ";
            // 
            // radioButton_unlimit
            // 
            this.radioButton_unlimit.AutoSize = true;
            this.radioButton_unlimit.Checked = true;
            this.radioButton_unlimit.Location = new System.Drawing.Point(12, 22);
            this.radioButton_unlimit.Name = "radioButton_unlimit";
            this.radioButton_unlimit.Size = new System.Drawing.Size(61, 19);
            this.radioButton_unlimit.TabIndex = 8;
            this.radioButton_unlimit.TabStop = true;
            this.radioButton_unlimit.Text = "無制限";
            this.radioButton_unlimit.UseVisualStyleBackColor = true;
            this.radioButton_unlimit.CheckedChanged += new System.EventHandler(this.radioButton3_CheckedChanged);
            // 
            // label_sec
            // 
            this.label_sec.AutoSize = true;
            this.label_sec.Location = new System.Drawing.Point(161, 73);
            this.label_sec.Name = "label_sec";
            this.label_sec.Size = new System.Drawing.Size(19, 15);
            this.label_sec.TabIndex = 7;
            this.label_sec.Text = "秒";
            // 
            // label_min
            // 
            this.label_min.AutoSize = true;
            this.label_min.Location = new System.Drawing.Point(80, 73);
            this.label_min.Name = "label_min";
            this.label_min.Size = new System.Drawing.Size(19, 15);
            this.label_min.TabIndex = 6;
            this.label_min.Text = "分";
            // 
            // numericUpDown_sec
            // 
            this.numericUpDown_sec.Location = new System.Drawing.Point(105, 71);
            this.numericUpDown_sec.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.numericUpDown_sec.Name = "numericUpDown_sec";
            this.numericUpDown_sec.Size = new System.Drawing.Size(50, 23);
            this.numericUpDown_sec.TabIndex = 5;
            this.numericUpDown_sec.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // numericUpDown_min
            // 
            this.numericUpDown_min.Location = new System.Drawing.Point(24, 71);
            this.numericUpDown_min.Name = "numericUpDown_min";
            this.numericUpDown_min.Size = new System.Drawing.Size(50, 23);
            this.numericUpDown_min.TabIndex = 4;
            this.numericUpDown_min.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDown_min.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label_mb
            // 
            this.label_mb.AutoSize = true;
            this.label_mb.Location = new System.Drawing.Point(111, 127);
            this.label_mb.Name = "label_mb";
            this.label_mb.Size = new System.Drawing.Size(25, 15);
            this.label_mb.TabIndex = 3;
            this.label_mb.Text = "MB";
            // 
            // numericUpDown_filesize
            // 
            this.numericUpDown_filesize.DecimalPlaces = 1;
            this.numericUpDown_filesize.Location = new System.Drawing.Point(24, 125);
            this.numericUpDown_filesize.Maximum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.numericUpDown_filesize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numericUpDown_filesize.Name = "numericUpDown_filesize";
            this.numericUpDown_filesize.Size = new System.Drawing.Size(81, 23);
            this.numericUpDown_filesize.TabIndex = 2;
            this.numericUpDown_filesize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDown_filesize.ThousandsSeparator = true;
            this.numericUpDown_filesize.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // radioButton_filesize
            // 
            this.radioButton_filesize.AutoSize = true;
            this.radioButton_filesize.Location = new System.Drawing.Point(12, 100);
            this.radioButton_filesize.Name = "radioButton_filesize";
            this.radioButton_filesize.Size = new System.Drawing.Size(133, 19);
            this.radioButton_filesize.TabIndex = 1;
            this.radioButton_filesize.Text = "ファイルサイズ指定";
            this.radioButton_filesize.UseVisualStyleBackColor = true;
            this.radioButton_filesize.CheckedChanged += new System.EventHandler(this.radioButton3_CheckedChanged);
            // 
            // radioButton_time
            // 
            this.radioButton_time.AutoSize = true;
            this.radioButton_time.Location = new System.Drawing.Point(12, 47);
            this.radioButton_time.Name = "radioButton_time";
            this.radioButton_time.Size = new System.Drawing.Size(73, 19);
            this.radioButton_time.TabIndex = 0;
            this.radioButton_time.Text = "時間指定";
            this.radioButton_time.UseVisualStyleBackColor = true;
            this.radioButton_time.CheckedChanged += new System.EventHandler(this.radioButton3_CheckedChanged);
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(186, 203);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(330, 23);
            this.progressBar.TabIndex = 4;
            // 
            // textBox_saveto
            // 
            this.textBox_saveto.Location = new System.Drawing.Point(186, 174);
            this.textBox_saveto.Name = "textBox_saveto";
            this.textBox_saveto.Size = new System.Drawing.Size(330, 23);
            this.textBox_saveto.TabIndex = 5;
            // 
            // button_open
            // 
            this.button_open.Location = new System.Drawing.Point(522, 174);
            this.button_open.Name = "button_open";
            this.button_open.Size = new System.Drawing.Size(75, 23);
            this.button_open.TabIndex = 6;
            this.button_open.Text = "参照...";
            this.button_open.UseVisualStyleBackColor = true;
            this.button_open.Click += new System.EventHandler(this.button1_Click);
            // 
            // button_close
            // 
            this.button_close.Location = new System.Drawing.Point(521, 203);
            this.button_close.Name = "button_close";
            this.button_close.Size = new System.Drawing.Size(75, 23);
            this.button_close.TabIndex = 7;
            this.button_close.Text = "閉じる";
            this.button_close.UseVisualStyleBackColor = true;
            this.button_close.Click += new System.EventHandler(this.button2_Click);
            // 
            // groupBox_progress
            // 
            this.groupBox_progress.Controls.Add(this.label_filesize);
            this.groupBox_progress.Controls.Add(this.label_tick);
            this.groupBox_progress.Controls.Add(this.label_time);
            this.groupBox_progress.Location = new System.Drawing.Point(12, 164);
            this.groupBox_progress.Name = "groupBox_progress";
            this.groupBox_progress.Size = new System.Drawing.Size(105, 65);
            this.groupBox_progress.TabIndex = 8;
            this.groupBox_progress.TabStop = false;
            // 
            // label_time
            // 
            this.label_time.AutoSize = true;
            this.label_time.Location = new System.Drawing.Point(9, 46);
            this.label_time.Name = "label_time";
            this.label_time.Size = new System.Drawing.Size(58, 15);
            this.label_time.TabIndex = 2;
            this.label_time.Text = "時間: 0:00";
            // 
            // label_tick
            // 
            this.label_tick.AutoSize = true;
            this.label_tick.Location = new System.Drawing.Point(9, 30);
            this.label_tick.Name = "label_tick";
            this.label_tick.Size = new System.Drawing.Size(43, 15);
            this.label_tick.TabIndex = 1;
            this.label_tick.Text = "位置: 0";
            // 
            // label_filesize
            // 
            this.label_filesize.AutoSize = true;
            this.label_filesize.Location = new System.Drawing.Point(9, 14);
            this.label_filesize.Name = "label_filesize";
            this.label_filesize.Size = new System.Drawing.Size(60, 15);
            this.label_filesize.TabIndex = 0;
            this.label_filesize.Text = "出力: 0 KB";
            // 
            // label_progress
            // 
            this.label_progress.Location = new System.Drawing.Point(123, 203);
            this.label_progress.Name = "label_progress";
            this.label_progress.Size = new System.Drawing.Size(57, 23);
            this.label_progress.TabIndex = 2;
            this.label_progress.Text = "0%";
            this.label_progress.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label_saveto
            // 
            this.label_saveto.Location = new System.Drawing.Point(123, 174);
            this.label_saveto.Name = "label_saveto";
            this.label_saveto.Size = new System.Drawing.Size(57, 23);
            this.label_saveto.TabIndex = 9;
            this.label_saveto.Text = "保存先";
            this.label_saveto.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "WAVE ファイル(*.wav)|*.wav";
            this.saveFileDialog.Title = "外部ファイルへ書き出し";
            // 
            // button_start
            // 
            this.button_start.Location = new System.Drawing.Point(13, 113);
            this.button_start.Name = "button_start";
            this.button_start.Size = new System.Drawing.Size(75, 23);
            this.button_start.TabIndex = 10;
            this.button_start.Text = "開始";
            this.button_start.UseVisualStyleBackColor = true;
            this.button_start.Click += new System.EventHandler(this.button3_Click);
            // 
            // button_stop
            // 
            this.button_stop.Enabled = false;
            this.button_stop.Location = new System.Drawing.Point(13, 142);
            this.button_stop.Name = "button_stop";
            this.button_stop.Size = new System.Drawing.Size(75, 23);
            this.button_stop.TabIndex = 11;
            this.button_stop.Text = "中止";
            this.button_stop.UseVisualStyleBackColor = true;
            this.button_stop.Click += new System.EventHandler(this.button4_Click);
            // 
            // ExportDialog
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(609, 237);
            this.ControlBox = false;
            this.Controls.Add(this.button_start);
            this.Controls.Add(this.button_stop);
            this.Controls.Add(this.label_saveto);
            this.Controls.Add(this.label_progress);
            this.Controls.Add(this.groupBox_progress);
            this.Controls.Add(this.button_close);
            this.Controls.Add(this.button_open);
            this.Controls.Add(this.textBox_saveto);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.groupBox_size);
            this.Controls.Add(this.groupBox_samplingRate);
            this.Controls.Add(this.groupBox_oversampling);
            this.Controls.Add(this.groupBox_type);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExportDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "外部ファイルへ書き出し";
            this.Load += new System.EventHandler(this.ExportDialog_Load);
            this.groupBox_type.ResumeLayout(false);
            this.groupBox_type.PerformLayout();
            this.groupBox_oversampling.ResumeLayout(false);
            this.groupBox_oversampling.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_oversampling)).EndInit();
            this.groupBox_samplingRate.ResumeLayout(false);
            this.groupBox_size.ResumeLayout(false);
            this.groupBox_size.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_sec)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_min)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_filesize)).EndInit();
            this.groupBox_progress.ResumeLayout(false);
            this.groupBox_progress.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox_type;
		private System.Windows.Forms.RadioButton radioButton_16bit;
		private System.Windows.Forms.RadioButton radioButton_8bit;
		private System.Windows.Forms.ComboBox comboBox_type;
		private System.Windows.Forms.GroupBox groupBox_oversampling;
		private System.Windows.Forms.Label label_oversampling;
		private System.Windows.Forms.TrackBar trackBar_oversampling;
		private System.Windows.Forms.GroupBox groupBox_samplingRate;
		private System.Windows.Forms.ComboBox comboBox_samplingRate;
		private System.Windows.Forms.GroupBox groupBox_size;
		private System.Windows.Forms.Label label_sec;
		private System.Windows.Forms.Label label_min;
		private System.Windows.Forms.NumericUpDown numericUpDown_sec;
		private System.Windows.Forms.NumericUpDown numericUpDown_min;
		private System.Windows.Forms.Label label_mb;
		private System.Windows.Forms.NumericUpDown numericUpDown_filesize;
		private System.Windows.Forms.RadioButton radioButton_filesize;
		private System.Windows.Forms.RadioButton radioButton_time;
		private System.Windows.Forms.ProgressBar progressBar;
		private System.Windows.Forms.TextBox textBox_saveto;
		private System.Windows.Forms.Button button_open;
		private System.Windows.Forms.Button button_close;
		private System.Windows.Forms.GroupBox groupBox_progress;
		private System.Windows.Forms.Label label_tick;
		private System.Windows.Forms.Label label_filesize;
		private System.Windows.Forms.Label label_progress;
		private System.Windows.Forms.Label label_saveto;
		private System.Windows.Forms.SaveFileDialog saveFileDialog;
		private System.Windows.Forms.Button button_start;
		private System.Windows.Forms.Button button_stop;
		private System.Windows.Forms.Label label_time;
		private System.Windows.Forms.RadioButton radioButton_unlimit;
	}
}