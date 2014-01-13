namespace uxPlayer
{
    partial class MainForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menu_file = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_open = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.menu_export = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.menu_exit = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_playing = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_playFirst = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_play = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_stop = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.menu_allNoteOff = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_allReset = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_tool = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_connect = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_refresh = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.menu_masterControl = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_help = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_versionInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStrip_open = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStrip_playFirst = new System.Windows.Forms.ToolStripButton();
            this.toolStrip_play = new System.Windows.Forms.ToolStripButton();
            this.toolStrip_stop = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStrip_connect = new System.Windows.Forms.ToolStripButton();
            this.toolStrip_refresh = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStrip_allNoteOff = new System.Windows.Forms.ToolStripButton();
            this.toolStrip_allReset = new System.Windows.Forms.ToolStripButton();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.monitorBox = new System.Windows.Forms.PictureBox();
            this.monitorMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menu_noMonitor = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_waveform = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_spectrum = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_history = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.hScrollBar = new System.Windows.Forms.HScrollBar();
            this.panel = new System.Windows.Forms.Panel();
            this.volumeMonitorBox = new System.Windows.Forms.PictureBox();
            this.label_static_title = new System.Windows.Forms.Label();
            this.label_static_tone = new System.Windows.Forms.Label();
            this.label_static_tick = new System.Windows.Forms.Label();
            this.label_static_tempo = new System.Windows.Forms.Label();
            this.label_static_message = new System.Windows.Forms.Label();
            this.label_static_resolution = new System.Windows.Forms.Label();
            this.label_static_play = new System.Windows.Forms.Label();
            this.label_static_connect = new System.Windows.Forms.Label();
            this.label_title = new System.Windows.Forms.Label();
            this.label_tone_left = new System.Windows.Forms.Label();
            this.label_tone_right = new System.Windows.Forms.Label();
            this.label_sep1 = new System.Windows.Forms.Label();
            this.label_tick_left = new System.Windows.Forms.Label();
            this.label_tick_right = new System.Windows.Forms.Label();
            this.label_sep2 = new System.Windows.Forms.Label();
            this.label_tempo = new System.Windows.Forms.Label();
            this.label_message = new System.Windows.Forms.Label();
            this.label_resolution = new System.Windows.Forms.Label();
            this.monitorTimer = new System.Windows.Forms.Timer(this.components);
            this.displayTimer = new System.Windows.Forms.Timer(this.components);
            this.fastTimer = new System.Windows.Forms.Timer(this.components);
            this.smfFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.slowTimer = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.monitorBox)).BeginInit();
            this.monitorMenu.SuspendLayout();
            this.tableLayoutPanel.SuspendLayout();
            this.panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.volumeMonitorBox)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menu_file,
            this.menu_playing,
            this.menu_tool,
            this.menu_help});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(472, 26);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menu_file
            // 
            this.menu_file.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menu_open,
            this.toolStripSeparator5,
            this.menu_export,
            this.toolStripSeparator6,
            this.menu_exit});
            this.menu_file.Name = "menu_file";
            this.menu_file.Size = new System.Drawing.Size(85, 22);
            this.menu_file.Text = "ファイル(&F)";
            // 
            // menu_open
            // 
            this.menu_open.Image = global::uxPlayer.Properties.Resources.folder_horizontal_open;
            this.menu_open.Name = "menu_open";
            this.menu_open.Size = new System.Drawing.Size(244, 22);
            this.menu_open.Text = "MIDIファイルの読み込み(&O)...";
            this.menu_open.Click += new System.EventHandler(this.menu_open_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(241, 6);
            // 
            // menu_export
            // 
            this.menu_export.Name = "menu_export";
            this.menu_export.Size = new System.Drawing.Size(244, 22);
            this.menu_export.Text = "WAVEファイルへの出力(&E)...";
            this.menu_export.Click += new System.EventHandler(this.menu_export_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(241, 6);
            // 
            // menu_exit
            // 
            this.menu_exit.Name = "menu_exit";
            this.menu_exit.Size = new System.Drawing.Size(244, 22);
            this.menu_exit.Text = "終了(&X)";
            this.menu_exit.Click += new System.EventHandler(this.menu_exit_Click);
            // 
            // menu_playing
            // 
            this.menu_playing.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menu_playFirst,
            this.menu_play,
            this.menu_stop,
            this.toolStripSeparator4,
            this.menu_allNoteOff,
            this.menu_allReset});
            this.menu_playing.Name = "menu_playing";
            this.menu_playing.Size = new System.Drawing.Size(61, 22);
            this.menu_playing.Text = "演奏(&P)";
            // 
            // menu_playFirst
            // 
            this.menu_playFirst.Image = global::uxPlayer.Properties.Resources.control_stop_180;
            this.menu_playFirst.Name = "menu_playFirst";
            this.menu_playFirst.Size = new System.Drawing.Size(191, 22);
            this.menu_playFirst.Text = "最初から再生(&F)";
            this.menu_playFirst.Click += new System.EventHandler(this.menu_playFirst_Click);
            // 
            // menu_play
            // 
            this.menu_play.Image = global::uxPlayer.Properties.Resources.control;
            this.menu_play.Name = "menu_play";
            this.menu_play.Size = new System.Drawing.Size(191, 22);
            this.menu_play.Text = "再生(&P)";
            this.menu_play.Click += new System.EventHandler(this.menu_play_Click);
            // 
            // menu_stop
            // 
            this.menu_stop.Image = global::uxPlayer.Properties.Resources.control_stop_square;
            this.menu_stop.Name = "menu_stop";
            this.menu_stop.Size = new System.Drawing.Size(191, 22);
            this.menu_stop.Text = "停止(&S)";
            this.menu_stop.Click += new System.EventHandler(this.menu_stop_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(188, 6);
            // 
            // menu_allNoteOff
            // 
            this.menu_allNoteOff.Image = global::uxPlayer.Properties.Resources.cross_white;
            this.menu_allNoteOff.Name = "menu_allNoteOff";
            this.menu_allNoteOff.Size = new System.Drawing.Size(191, 22);
            this.menu_allNoteOff.Text = "オールノートオフ(&O)";
            this.menu_allNoteOff.Click += new System.EventHandler(this.menu_allNoteOff_Click);
            // 
            // menu_allReset
            // 
            this.menu_allReset.Image = global::uxPlayer.Properties.Resources.exclamation_white;
            this.menu_allReset.Name = "menu_allReset";
            this.menu_allReset.Size = new System.Drawing.Size(191, 22);
            this.menu_allReset.Text = "オールリセット(&R)";
            this.menu_allReset.Click += new System.EventHandler(this.menu_allReset_Click);
            // 
            // menu_tool
            // 
            this.menu_tool.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menu_connect,
            this.menu_refresh,
            this.toolStripSeparator7,
            this.menu_masterControl});
            this.menu_tool.Name = "menu_tool";
            this.menu_tool.Size = new System.Drawing.Size(74, 22);
            this.menu_tool.Text = "ツール(&T)";
            // 
            // menu_connect
            // 
            this.menu_connect.Image = global::uxPlayer.Properties.Resources.plug_disconnect;
            this.menu_connect.Name = "menu_connect";
            this.menu_connect.Size = new System.Drawing.Size(226, 22);
            this.menu_connect.Text = "MIDI-INへの接続(&M)";
            this.menu_connect.Click += new System.EventHandler(this.menu_connect_Click);
            // 
            // menu_refresh
            // 
            this.menu_refresh.Image = global::uxPlayer.Properties.Resources.arrow_circle;
            this.menu_refresh.Name = "menu_refresh";
            this.menu_refresh.Size = new System.Drawing.Size(226, 22);
            this.menu_refresh.Text = "プリセットリフレッシュ(&R)";
            this.menu_refresh.Click += new System.EventHandler(this.menu_refresh_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(223, 6);
            // 
            // menu_masterControl
            // 
            this.menu_masterControl.Name = "menu_masterControl";
            this.menu_masterControl.Size = new System.Drawing.Size(226, 22);
            this.menu_masterControl.Text = "マスター調整(&F)";
            this.menu_masterControl.Click += new System.EventHandler(this.menu_masterControl_Click);
            // 
            // menu_help
            // 
            this.menu_help.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menu_versionInfo});
            this.menu_help.Name = "menu_help";
            this.menu_help.Size = new System.Drawing.Size(75, 22);
            this.menu_help.Text = "ヘルプ(&H)";
            // 
            // menu_versionInfo
            // 
            this.menu_versionInfo.Name = "menu_versionInfo";
            this.menu_versionInfo.Size = new System.Drawing.Size(175, 22);
            this.menu_versionInfo.Text = "バージョン情報(&I)";
            this.menu_versionInfo.Click += new System.EventHandler(this.menu_versionInfo_Click);
            // 
            // toolStrip
            // 
            this.toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStrip_open,
            this.toolStripSeparator1,
            this.toolStrip_playFirst,
            this.toolStrip_play,
            this.toolStrip_stop,
            this.toolStripSeparator2,
            this.toolStrip_connect,
            this.toolStrip_refresh,
            this.toolStripSeparator3,
            this.toolStrip_allNoteOff,
            this.toolStrip_allReset});
            this.toolStrip.Location = new System.Drawing.Point(0, 26);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(472, 25);
            this.toolStrip.TabIndex = 1;
            this.toolStrip.Text = "toolStrip1";
            // 
            // toolStrip_open
            // 
            this.toolStrip_open.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStrip_open.Image = global::uxPlayer.Properties.Resources.folder_horizontal_open;
            this.toolStrip_open.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStrip_open.Name = "toolStrip_open";
            this.toolStrip_open.Size = new System.Drawing.Size(23, 22);
            this.toolStrip_open.Text = "MIDIファイルの読み込み";
            this.toolStrip_open.Click += new System.EventHandler(this.toolStrip_open_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStrip_playFirst
            // 
            this.toolStrip_playFirst.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStrip_playFirst.Image = global::uxPlayer.Properties.Resources.control_stop_180;
            this.toolStrip_playFirst.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStrip_playFirst.Name = "toolStrip_playFirst";
            this.toolStrip_playFirst.Size = new System.Drawing.Size(23, 22);
            this.toolStrip_playFirst.Text = "最初から再生";
            this.toolStrip_playFirst.Click += new System.EventHandler(this.toolStrip_playFirst_Click);
            // 
            // toolStrip_play
            // 
            this.toolStrip_play.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStrip_play.Image = global::uxPlayer.Properties.Resources.control;
            this.toolStrip_play.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStrip_play.Name = "toolStrip_play";
            this.toolStrip_play.Size = new System.Drawing.Size(23, 22);
            this.toolStrip_play.Text = "再生";
            this.toolStrip_play.Click += new System.EventHandler(this.toolStrip_play_Click);
            // 
            // toolStrip_stop
            // 
            this.toolStrip_stop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStrip_stop.Image = global::uxPlayer.Properties.Resources.control_stop_square;
            this.toolStrip_stop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStrip_stop.Name = "toolStrip_stop";
            this.toolStrip_stop.Size = new System.Drawing.Size(23, 22);
            this.toolStrip_stop.Text = "停止";
            this.toolStrip_stop.Click += new System.EventHandler(this.toolStrip_stop_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStrip_connect
            // 
            this.toolStrip_connect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStrip_connect.Image = global::uxPlayer.Properties.Resources.plug_disconnect;
            this.toolStrip_connect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStrip_connect.Name = "toolStrip_connect";
            this.toolStrip_connect.Size = new System.Drawing.Size(23, 22);
            this.toolStrip_connect.Text = "MIDI-INへの接続";
            this.toolStrip_connect.Click += new System.EventHandler(this.toolStrip_connect_Click);
            // 
            // toolStrip_refresh
            // 
            this.toolStrip_refresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStrip_refresh.Image = global::uxPlayer.Properties.Resources.arrow_circle;
            this.toolStrip_refresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStrip_refresh.Name = "toolStrip_refresh";
            this.toolStrip_refresh.Size = new System.Drawing.Size(23, 22);
            this.toolStrip_refresh.Text = "プリセットリフレッシュ";
            this.toolStrip_refresh.Click += new System.EventHandler(this.toolStrip_refresh_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStrip_allNoteOff
            // 
            this.toolStrip_allNoteOff.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStrip_allNoteOff.Image = global::uxPlayer.Properties.Resources.cross_white;
            this.toolStrip_allNoteOff.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStrip_allNoteOff.Name = "toolStrip_allNoteOff";
            this.toolStrip_allNoteOff.Size = new System.Drawing.Size(23, 22);
            this.toolStrip_allNoteOff.Text = "オールノートオフ";
            this.toolStrip_allNoteOff.Click += new System.EventHandler(this.toolStrip_allNoteOff_Click);
            // 
            // toolStrip_allReset
            // 
            this.toolStrip_allReset.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStrip_allReset.Image = global::uxPlayer.Properties.Resources.exclamation_white;
            this.toolStrip_allReset.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStrip_allReset.Name = "toolStrip_allReset";
            this.toolStrip_allReset.Size = new System.Drawing.Size(23, 22);
            this.toolStrip_allReset.Text = "オールリセット";
            this.toolStrip_allReset.Click += new System.EventHandler(this.toolStrip_allReset_Click);
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer.IsSplitterFixed = true;
            this.splitContainer.Location = new System.Drawing.Point(0, 51);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.monitorBox);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.tableLayoutPanel);
            this.splitContainer.Size = new System.Drawing.Size(472, 101);
            this.splitContainer.SplitterDistance = 192;
            this.splitContainer.SplitterWidth = 1;
            this.splitContainer.TabIndex = 2;
            // 
            // monitorBox
            // 
            this.monitorBox.BackColor = System.Drawing.Color.PaleGreen;
            this.monitorBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.monitorBox.ContextMenuStrip = this.monitorMenu;
            this.monitorBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.monitorBox.Location = new System.Drawing.Point(0, 0);
            this.monitorBox.Name = "monitorBox";
            this.monitorBox.Size = new System.Drawing.Size(192, 101);
            this.monitorBox.TabIndex = 0;
            this.monitorBox.TabStop = false;
            // 
            // monitorMenu
            // 
            this.monitorMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menu_noMonitor,
            this.menu_waveform,
            this.menu_spectrum,
            this.menu_history});
            this.monitorMenu.Name = "contextMenuStrip1";
            this.monitorMenu.Size = new System.Drawing.Size(243, 92);
            // 
            // menu_noMonitor
            // 
            this.menu_noMonitor.Name = "menu_noMonitor";
            this.menu_noMonitor.Size = new System.Drawing.Size(242, 22);
            this.menu_noMonitor.Text = "表示なし";
            this.menu_noMonitor.Click += new System.EventHandler(this.menu_noMonitor_Click);
            // 
            // menu_waveform
            // 
            this.menu_waveform.Name = "menu_waveform";
            this.menu_waveform.Size = new System.Drawing.Size(242, 22);
            this.menu_waveform.Text = "波形表示";
            this.menu_waveform.Click += new System.EventHandler(this.menu_waveform_Click);
            // 
            // menu_spectrum
            // 
            this.menu_spectrum.Name = "menu_spectrum";
            this.menu_spectrum.Size = new System.Drawing.Size(242, 22);
            this.menu_spectrum.Text = "周波数スペクトラム";
            this.menu_spectrum.Click += new System.EventHandler(this.menu_spectrum_Click);
            // 
            // menu_history
            // 
            this.menu_history.Name = "menu_history";
            this.menu_history.Size = new System.Drawing.Size(242, 22);
            this.menu_history.Text = "周波数スペクトラム(履歴表示)";
            this.menu_history.Click += new System.EventHandler(this.menu_historyClick);
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.BackColor = System.Drawing.Color.PaleGreen;
            this.tableLayoutPanel.ColumnCount = 1;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.Controls.Add(this.hScrollBar, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.panel, 0, 0);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Font = new System.Drawing.Font("Segoe UI Symbol", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 18F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(279, 101);
            this.tableLayoutPanel.TabIndex = 1;
            // 
            // hScrollBar
            // 
            this.hScrollBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hScrollBar.Location = new System.Drawing.Point(0, 83);
            this.hScrollBar.Name = "hScrollBar";
            this.hScrollBar.Size = new System.Drawing.Size(279, 18);
            this.hScrollBar.TabIndex = 0;
            this.hScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar_Scroll);
            // 
            // panel
            // 
            this.panel.Controls.Add(this.volumeMonitorBox);
            this.panel.Controls.Add(this.label_static_title);
            this.panel.Controls.Add(this.label_static_tone);
            this.panel.Controls.Add(this.label_static_tick);
            this.panel.Controls.Add(this.label_static_tempo);
            this.panel.Controls.Add(this.label_static_message);
            this.panel.Controls.Add(this.label_static_resolution);
            this.panel.Controls.Add(this.label_static_play);
            this.panel.Controls.Add(this.label_static_connect);
            this.panel.Controls.Add(this.label_title);
            this.panel.Controls.Add(this.label_tone_left);
            this.panel.Controls.Add(this.label_tone_right);
            this.panel.Controls.Add(this.label_sep1);
            this.panel.Controls.Add(this.label_tick_left);
            this.panel.Controls.Add(this.label_tick_right);
            this.panel.Controls.Add(this.label_sep2);
            this.panel.Controls.Add(this.label_tempo);
            this.panel.Controls.Add(this.label_message);
            this.panel.Controls.Add(this.label_resolution);
            this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel.Location = new System.Drawing.Point(3, 3);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(273, 77);
            this.panel.TabIndex = 1;
            // 
            // volumeMonitorBox
            // 
            this.volumeMonitorBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.volumeMonitorBox.Location = new System.Drawing.Point(63, 60);
            this.volumeMonitorBox.Name = "volumeMonitorBox";
            this.volumeMonitorBox.Size = new System.Drawing.Size(147, 17);
            this.volumeMonitorBox.TabIndex = 19;
            this.volumeMonitorBox.TabStop = false;
            // 
            // label_static_title
            // 
            this.label_static_title.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label_static_title.ForeColor = System.Drawing.Color.PaleGreen;
            this.label_static_title.Location = new System.Drawing.Point(-1, 0);
            this.label_static_title.Name = "label_static_title";
            this.label_static_title.Size = new System.Drawing.Size(41, 17);
            this.label_static_title.TabIndex = 16;
            this.label_static_title.Text = "TITLE";
            this.label_static_title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_static_tone
            // 
            this.label_static_tone.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label_static_tone.ForeColor = System.Drawing.Color.PaleGreen;
            this.label_static_tone.Location = new System.Drawing.Point(-1, 20);
            this.label_static_tone.Name = "label_static_tone";
            this.label_static_tone.Size = new System.Drawing.Size(41, 17);
            this.label_static_tone.TabIndex = 11;
            this.label_static_tone.Text = "TONE";
            this.label_static_tone.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_static_tick
            // 
            this.label_static_tick.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label_static_tick.ForeColor = System.Drawing.Color.PaleGreen;
            this.label_static_tick.Location = new System.Drawing.Point(115, 20);
            this.label_static_tick.Name = "label_static_tick";
            this.label_static_tick.Size = new System.Drawing.Size(37, 17);
            this.label_static_tick.TabIndex = 3;
            this.label_static_tick.Text = "TICK";
            this.label_static_tick.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_static_tempo
            // 
            this.label_static_tempo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label_static_tempo.ForeColor = System.Drawing.Color.PaleGreen;
            this.label_static_tempo.Location = new System.Drawing.Point(-1, 40);
            this.label_static_tempo.Name = "label_static_tempo";
            this.label_static_tempo.Size = new System.Drawing.Size(41, 17);
            this.label_static_tempo.TabIndex = 7;
            this.label_static_tempo.Text = "TEMP";
            this.label_static_tempo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_static_message
            // 
            this.label_static_message.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label_static_message.ForeColor = System.Drawing.Color.PaleGreen;
            this.label_static_message.Location = new System.Drawing.Point(115, 40);
            this.label_static_message.Name = "label_static_message";
            this.label_static_message.Size = new System.Drawing.Size(36, 17);
            this.label_static_message.TabIndex = 9;
            this.label_static_message.Text = "MSG";
            this.label_static_message.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_static_resolution
            // 
            this.label_static_resolution.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label_static_resolution.ForeColor = System.Drawing.Color.PaleGreen;
            this.label_static_resolution.Location = new System.Drawing.Point(202, 40);
            this.label_static_resolution.Name = "label_static_resolution";
            this.label_static_resolution.Size = new System.Drawing.Size(31, 17);
            this.label_static_resolution.TabIndex = 1;
            this.label_static_resolution.Text = "RES";
            this.label_static_resolution.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_static_play
            // 
            this.label_static_play.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label_static_play.ForeColor = System.Drawing.Color.PaleGreen;
            this.label_static_play.Location = new System.Drawing.Point(0, 60);
            this.label_static_play.Name = "label_static_play";
            this.label_static_play.Size = new System.Drawing.Size(60, 17);
            this.label_static_play.TabIndex = 18;
            this.label_static_play.Text = "STOP";
            this.label_static_play.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_static_connect
            // 
            this.label_static_connect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label_static_connect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label_static_connect.ForeColor = System.Drawing.Color.PaleGreen;
            this.label_static_connect.Location = new System.Drawing.Point(213, 60);
            this.label_static_connect.Name = "label_static_connect";
            this.label_static_connect.Size = new System.Drawing.Size(60, 17);
            this.label_static_connect.TabIndex = 0;
            this.label_static_connect.Text = "MIDI-IN";
            this.label_static_connect.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_title
            // 
            this.label_title.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_title.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(229)))), ((int)(((byte)(139)))));
            this.label_title.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label_title.Location = new System.Drawing.Point(40, 0);
            this.label_title.Name = "label_title";
            this.label_title.Size = new System.Drawing.Size(233, 17);
            this.label_title.TabIndex = 17;
            this.label_title.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_tone_left
            // 
            this.label_tone_left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(229)))), ((int)(((byte)(139)))));
            this.label_tone_left.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label_tone_left.Location = new System.Drawing.Point(40, 20);
            this.label_tone_left.Name = "label_tone_left";
            this.label_tone_left.Size = new System.Drawing.Size(32, 17);
            this.label_tone_left.TabIndex = 12;
            this.label_tone_left.Text = "0";
            this.label_tone_left.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label_tone_right
            // 
            this.label_tone_right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(229)))), ((int)(((byte)(139)))));
            this.label_tone_right.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label_tone_right.Location = new System.Drawing.Point(80, 20);
            this.label_tone_right.Name = "label_tone_right";
            this.label_tone_right.Size = new System.Drawing.Size(32, 17);
            this.label_tone_right.TabIndex = 14;
            this.label_tone_right.Text = "0";
            this.label_tone_right.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_sep1
            // 
            this.label_sep1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(229)))), ((int)(((byte)(139)))));
            this.label_sep1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label_sep1.Location = new System.Drawing.Point(70, 20);
            this.label_sep1.Name = "label_sep1";
            this.label_sep1.Size = new System.Drawing.Size(10, 17);
            this.label_sep1.TabIndex = 15;
            this.label_sep1.Text = "/";
            this.label_sep1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_tick_left
            // 
            this.label_tick_left.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(229)))), ((int)(((byte)(139)))));
            this.label_tick_left.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label_tick_left.Location = new System.Drawing.Point(151, 20);
            this.label_tick_left.Name = "label_tick_left";
            this.label_tick_left.Size = new System.Drawing.Size(56, 17);
            this.label_tick_left.TabIndex = 4;
            this.label_tick_left.Text = "0";
            this.label_tick_left.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label_tick_right
            // 
            this.label_tick_right.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_tick_right.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(229)))), ((int)(((byte)(139)))));
            this.label_tick_right.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label_tick_right.Location = new System.Drawing.Point(217, 20);
            this.label_tick_right.Name = "label_tick_right";
            this.label_tick_right.Size = new System.Drawing.Size(56, 17);
            this.label_tick_right.TabIndex = 5;
            this.label_tick_right.Text = "0";
            this.label_tick_right.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_sep2
            // 
            this.label_sep2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(229)))), ((int)(((byte)(139)))));
            this.label_sep2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label_sep2.Location = new System.Drawing.Point(207, 20);
            this.label_sep2.Name = "label_sep2";
            this.label_sep2.Size = new System.Drawing.Size(10, 17);
            this.label_sep2.TabIndex = 6;
            this.label_sep2.Text = "/";
            this.label_sep2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label_tempo
            // 
            this.label_tempo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(229)))), ((int)(((byte)(139)))));
            this.label_tempo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label_tempo.Location = new System.Drawing.Point(40, 40);
            this.label_tempo.Name = "label_tempo";
            this.label_tempo.Size = new System.Drawing.Size(72, 17);
            this.label_tempo.TabIndex = 8;
            this.label_tempo.Text = "120.0";
            this.label_tempo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_message
            // 
            this.label_message.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(229)))), ((int)(((byte)(139)))));
            this.label_message.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label_message.Location = new System.Drawing.Point(152, 40);
            this.label_message.Name = "label_message";
            this.label_message.Size = new System.Drawing.Size(47, 17);
            this.label_message.TabIndex = 10;
            this.label_message.Text = "0";
            this.label_message.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label_resolution
            // 
            this.label_resolution.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label_resolution.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(139)))), ((int)(((byte)(229)))), ((int)(((byte)(139)))));
            this.label_resolution.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label_resolution.Location = new System.Drawing.Point(233, 40);
            this.label_resolution.Name = "label_resolution";
            this.label_resolution.Size = new System.Drawing.Size(40, 17);
            this.label_resolution.TabIndex = 2;
            this.label_resolution.Text = "480";
            this.label_resolution.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // monitorTimer
            // 
            this.monitorTimer.Enabled = true;
            this.monitorTimer.Interval = 33;
            this.monitorTimer.Tick += new System.EventHandler(this.monitorTimer_Tick);
            // 
            // displayTimer
            // 
            this.displayTimer.Enabled = true;
            this.displayTimer.Interval = 500;
            this.displayTimer.Tick += new System.EventHandler(this.displayTimer_Tick);
            // 
            // fastTimer
            // 
            this.fastTimer.Enabled = true;
            this.fastTimer.Interval = 50;
            this.fastTimer.Tick += new System.EventHandler(this.fastTimer_Tick);
            // 
            // smfFileDialog
            // 
            this.smfFileDialog.Filter = "サポートするファイル|*.mid;*.midi;*.rmi|MIDI ファイル(*.mid, *.midi)|*.mid;*.midi|RMI ファイル(*.rm" +
    "i)|*.rmi";
            this.smfFileDialog.Title = "MIDIファイルの読み込み";
            // 
            // slowTimer
            // 
            this.slowTimer.Enabled = true;
            this.slowTimer.Interval = 250;
            this.slowTimer.Tick += new System.EventHandler(this.slowTimer_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(472, 152);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "uxPlayer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.monitorBox)).EndInit();
            this.monitorMenu.ResumeLayout(false);
            this.tableLayoutPanel.ResumeLayout(false);
            this.panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.volumeMonitorBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menu_file;
        private System.Windows.Forms.ToolStripMenuItem menu_tool;
        private System.Windows.Forms.ToolStripMenuItem menu_help;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.PictureBox monitorBox;
        private System.Windows.Forms.HScrollBar hScrollBar;
        private System.Windows.Forms.Timer monitorTimer;
        private System.Windows.Forms.ContextMenuStrip monitorMenu;
        private System.Windows.Forms.ToolStripMenuItem menu_noMonitor;
        private System.Windows.Forms.ToolStripMenuItem menu_waveform;
        private System.Windows.Forms.ToolStripMenuItem menu_spectrum;
        private System.Windows.Forms.ToolStripMenuItem menu_history;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.Label label_static_connect;
        private System.Windows.Forms.PictureBox volumeMonitorBox;
        private System.Windows.Forms.Label label_static_play;
        private System.Windows.Forms.Label label_title;
        private System.Windows.Forms.Label label_static_title;
        private System.Windows.Forms.Label label_sep1;
        private System.Windows.Forms.Label label_tone_right;
        private System.Windows.Forms.Label label_tone_left;
        private System.Windows.Forms.Label label_static_tone;
        private System.Windows.Forms.Label label_message;
        private System.Windows.Forms.Label label_static_message;
        private System.Windows.Forms.Label label_tempo;
        private System.Windows.Forms.Label label_static_tempo;
        private System.Windows.Forms.Label label_sep2;
        private System.Windows.Forms.Label label_tick_right;
        private System.Windows.Forms.Label label_tick_left;
        private System.Windows.Forms.Label label_static_tick;
        private System.Windows.Forms.Label label_resolution;
        private System.Windows.Forms.Label label_static_resolution;
        private System.Windows.Forms.ToolStripButton toolStrip_open;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStrip_play;
        private System.Windows.Forms.ToolStripButton toolStrip_stop;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStrip_refresh;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolStrip_allReset;
        private System.Windows.Forms.ToolStripButton toolStrip_allNoteOff;
        private System.Windows.Forms.ToolStripMenuItem menu_exit;
        private System.Windows.Forms.ToolStripMenuItem menu_playing;
        private System.Windows.Forms.ToolStripMenuItem menu_play;
        private System.Windows.Forms.ToolStripMenuItem menu_stop;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem menu_allNoteOff;
        private System.Windows.Forms.ToolStripMenuItem menu_allReset;
        private System.Windows.Forms.ToolStripMenuItem menu_connect;
        private System.Windows.Forms.ToolStripMenuItem menu_refresh;
        private System.Windows.Forms.ToolStripMenuItem menu_versionInfo;
        private System.Windows.Forms.ToolStripButton toolStrip_connect;
        private System.Windows.Forms.Timer displayTimer;
        private System.Windows.Forms.Timer fastTimer;
        private System.Windows.Forms.OpenFileDialog smfFileDialog;
        private System.Windows.Forms.Timer slowTimer;
        private System.Windows.Forms.ToolStripMenuItem menu_open;
        private System.Windows.Forms.ToolStripMenuItem menu_playFirst;
        private System.Windows.Forms.ToolStripButton toolStrip_playFirst;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem menu_export;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem menu_masterControl;
    }
}

