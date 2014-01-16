namespace uxPlayer
{
    partial class MasterControlDialog
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
            this.trackBar_mastervolume = new System.Windows.Forms.TrackBar();
            this.groupBox_mastervolume = new System.Windows.Forms.GroupBox();
            this.label_mastervolume = new System.Windows.Forms.Label();
            this.groupBox_compressor_ratio = new System.Windows.Forms.GroupBox();
            this.label_compressor_ratio = new System.Windows.Forms.Label();
            this.trackBar_compressor_ratio = new System.Windows.Forms.TrackBar();
            this.groupBox_compressor_threshold = new System.Windows.Forms.GroupBox();
            this.label_compressor_threshold = new System.Windows.Forms.Label();
            this.trackBar_compressor_threshold = new System.Windows.Forms.TrackBar();
            this.groupBox_tempo = new System.Windows.Forms.GroupBox();
            this.label_tempo = new System.Windows.Forms.Label();
            this.trackBar_tempo = new System.Windows.Forms.TrackBar();
            this.button_reset = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_mastervolume)).BeginInit();
            this.groupBox_mastervolume.SuspendLayout();
            this.groupBox_compressor_ratio.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_compressor_ratio)).BeginInit();
            this.groupBox_compressor_threshold.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_compressor_threshold)).BeginInit();
            this.groupBox_tempo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_tempo)).BeginInit();
            this.SuspendLayout();
            // 
            // trackBar_mastervolume
            // 
            this.trackBar_mastervolume.Location = new System.Drawing.Point(47, 15);
            this.trackBar_mastervolume.Maximum = 200;
            this.trackBar_mastervolume.Name = "trackBar_mastervolume";
            this.trackBar_mastervolume.Size = new System.Drawing.Size(163, 45);
            this.trackBar_mastervolume.TabIndex = 0;
            this.trackBar_mastervolume.TickFrequency = 20;
            this.trackBar_mastervolume.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBar_mastervolume.Value = 100;
            this.trackBar_mastervolume.ValueChanged += new System.EventHandler(this.trackBar1_ValueChanged);
            // 
            // groupBox_mastervolume
            // 
            this.groupBox_mastervolume.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_mastervolume.Controls.Add(this.label_mastervolume);
            this.groupBox_mastervolume.Controls.Add(this.trackBar_mastervolume);
            this.groupBox_mastervolume.Location = new System.Drawing.Point(12, 12);
            this.groupBox_mastervolume.Name = "groupBox_mastervolume";
            this.groupBox_mastervolume.Size = new System.Drawing.Size(213, 63);
            this.groupBox_mastervolume.TabIndex = 1;
            this.groupBox_mastervolume.TabStop = false;
            this.groupBox_mastervolume.Text = "マスターボリューム";
            // 
            // label_mastervolume
            // 
            this.label_mastervolume.AutoSize = true;
            this.label_mastervolume.Location = new System.Drawing.Point(8, 26);
            this.label_mastervolume.Name = "label_mastervolume";
            this.label_mastervolume.Size = new System.Drawing.Size(29, 12);
            this.label_mastervolume.TabIndex = 1;
            this.label_mastervolume.Text = "100%";
            // 
            // groupBox_compressor_ratio
            // 
            this.groupBox_compressor_ratio.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_compressor_ratio.Controls.Add(this.label_compressor_ratio);
            this.groupBox_compressor_ratio.Controls.Add(this.trackBar_compressor_ratio);
            this.groupBox_compressor_ratio.Location = new System.Drawing.Point(12, 81);
            this.groupBox_compressor_ratio.Name = "groupBox_compressor_ratio";
            this.groupBox_compressor_ratio.Size = new System.Drawing.Size(213, 63);
            this.groupBox_compressor_ratio.TabIndex = 2;
            this.groupBox_compressor_ratio.TabStop = false;
            this.groupBox_compressor_ratio.Text = "コンプレッサ - レシオ";
            // 
            // label_compressor_ratio
            // 
            this.label_compressor_ratio.AutoSize = true;
            this.label_compressor_ratio.Location = new System.Drawing.Point(8, 29);
            this.label_compressor_ratio.Name = "label_compressor_ratio";
            this.label_compressor_ratio.Size = new System.Drawing.Size(33, 12);
            this.label_compressor_ratio.TabIndex = 2;
            this.label_compressor_ratio.Text = "1:2.00";
            // 
            // trackBar_compressor_ratio
            // 
            this.trackBar_compressor_ratio.LargeChange = 100;
            this.trackBar_compressor_ratio.Location = new System.Drawing.Point(47, 15);
            this.trackBar_compressor_ratio.Maximum = 1000;
            this.trackBar_compressor_ratio.Minimum = 100;
            this.trackBar_compressor_ratio.Name = "trackBar_compressor_ratio";
            this.trackBar_compressor_ratio.Size = new System.Drawing.Size(163, 45);
            this.trackBar_compressor_ratio.SmallChange = 50;
            this.trackBar_compressor_ratio.TabIndex = 0;
            this.trackBar_compressor_ratio.TickFrequency = 100;
            this.trackBar_compressor_ratio.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBar_compressor_ratio.Value = 200;
            this.trackBar_compressor_ratio.ValueChanged += new System.EventHandler(this.trackBar2_ValueChanged);
            // 
            // groupBox_compressor_threshold
            // 
            this.groupBox_compressor_threshold.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_compressor_threshold.Controls.Add(this.label_compressor_threshold);
            this.groupBox_compressor_threshold.Controls.Add(this.trackBar_compressor_threshold);
            this.groupBox_compressor_threshold.Location = new System.Drawing.Point(12, 150);
            this.groupBox_compressor_threshold.Name = "groupBox_compressor_threshold";
            this.groupBox_compressor_threshold.Size = new System.Drawing.Size(213, 63);
            this.groupBox_compressor_threshold.TabIndex = 3;
            this.groupBox_compressor_threshold.TabStop = false;
            this.groupBox_compressor_threshold.Text = "コンプレッサ - スレッショルド";
            // 
            // label_compressor_threshold
            // 
            this.label_compressor_threshold.AutoSize = true;
            this.label_compressor_threshold.Location = new System.Drawing.Point(8, 29);
            this.label_compressor_threshold.Name = "label_compressor_threshold";
            this.label_compressor_threshold.Size = new System.Drawing.Size(25, 12);
            this.label_compressor_threshold.TabIndex = 3;
            this.label_compressor_threshold.Text = "0.80";
            // 
            // trackBar_compressor_threshold
            // 
            this.trackBar_compressor_threshold.LargeChange = 10;
            this.trackBar_compressor_threshold.Location = new System.Drawing.Point(47, 15);
            this.trackBar_compressor_threshold.Maximum = 100;
            this.trackBar_compressor_threshold.Name = "trackBar_compressor_threshold";
            this.trackBar_compressor_threshold.Size = new System.Drawing.Size(163, 45);
            this.trackBar_compressor_threshold.TabIndex = 0;
            this.trackBar_compressor_threshold.TickFrequency = 10;
            this.trackBar_compressor_threshold.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBar_compressor_threshold.Value = 80;
            this.trackBar_compressor_threshold.ValueChanged += new System.EventHandler(this.trackBar3_ValueChanged);
            // 
            // groupBox_tempo
            // 
            this.groupBox_tempo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox_tempo.Controls.Add(this.label_tempo);
            this.groupBox_tempo.Controls.Add(this.trackBar_tempo);
            this.groupBox_tempo.Location = new System.Drawing.Point(12, 219);
            this.groupBox_tempo.Name = "groupBox_tempo";
            this.groupBox_tempo.Size = new System.Drawing.Size(213, 63);
            this.groupBox_tempo.TabIndex = 4;
            this.groupBox_tempo.TabStop = false;
            this.groupBox_tempo.Text = "テンポ";
            // 
            // label_tempo
            // 
            this.label_tempo.AutoSize = true;
            this.label_tempo.Location = new System.Drawing.Point(8, 32);
            this.label_tempo.Name = "label_tempo";
            this.label_tempo.Size = new System.Drawing.Size(29, 12);
            this.label_tempo.TabIndex = 4;
            this.label_tempo.Text = "100%";
            // 
            // trackBar_tempo
            // 
            this.trackBar_tempo.LargeChange = 10;
            this.trackBar_tempo.Location = new System.Drawing.Point(47, 15);
            this.trackBar_tempo.Maximum = 200;
            this.trackBar_tempo.Minimum = 50;
            this.trackBar_tempo.Name = "trackBar_tempo";
            this.trackBar_tempo.Size = new System.Drawing.Size(163, 45);
            this.trackBar_tempo.TabIndex = 0;
            this.trackBar_tempo.TickFrequency = 10;
            this.trackBar_tempo.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.trackBar_tempo.Value = 100;
            this.trackBar_tempo.ValueChanged += new System.EventHandler(this.trackBar4_ValueChanged);
            // 
            // button_reset
            // 
            this.button_reset.Location = new System.Drawing.Point(12, 285);
            this.button_reset.Name = "button_reset";
            this.button_reset.Size = new System.Drawing.Size(75, 23);
            this.button_reset.TabIndex = 5;
            this.button_reset.Text = "リセット";
            this.button_reset.UseVisualStyleBackColor = true;
            this.button_reset.Click += new System.EventHandler(this.button1_Click);
            // 
            // MasterControlDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(237, 320);
            this.Controls.Add(this.button_reset);
            this.Controls.Add(this.groupBox_mastervolume);
            this.Controls.Add(this.groupBox_compressor_ratio);
            this.Controls.Add(this.groupBox_compressor_threshold);
            this.Controls.Add(this.groupBox_tempo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "MasterControlDialog";
            this.Text = "マスター調整";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MasterControlDialog_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_mastervolume)).EndInit();
            this.groupBox_mastervolume.ResumeLayout(false);
            this.groupBox_mastervolume.PerformLayout();
            this.groupBox_compressor_ratio.ResumeLayout(false);
            this.groupBox_compressor_ratio.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_compressor_ratio)).EndInit();
            this.groupBox_compressor_threshold.ResumeLayout(false);
            this.groupBox_compressor_threshold.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_compressor_threshold)).EndInit();
            this.groupBox_tempo.ResumeLayout(false);
            this.groupBox_tempo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_tempo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TrackBar trackBar_mastervolume;
        private System.Windows.Forms.GroupBox groupBox_mastervolume;
        private System.Windows.Forms.GroupBox groupBox_compressor_ratio;
        private System.Windows.Forms.TrackBar trackBar_compressor_ratio;
        private System.Windows.Forms.GroupBox groupBox_compressor_threshold;
        private System.Windows.Forms.TrackBar trackBar_compressor_threshold;
        private System.Windows.Forms.GroupBox groupBox_tempo;
        private System.Windows.Forms.TrackBar trackBar_tempo;
        private System.Windows.Forms.Button button_reset;
        private System.Windows.Forms.Label label_mastervolume;
        private System.Windows.Forms.Label label_compressor_ratio;
        private System.Windows.Forms.Label label_compressor_threshold;
        private System.Windows.Forms.Label label_tempo;
    }
}