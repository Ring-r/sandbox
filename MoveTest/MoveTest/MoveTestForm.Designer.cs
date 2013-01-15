namespace MoveTest
{
    partial class MoveTestForm
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
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.tableLayoutPanelSettings = new System.Windows.Forms.TableLayoutPanel();
            this.labelWidth = new System.Windows.Forms.Label();
            this.textBoxWidth = new System.Windows.Forms.TextBox();
            this.labelHeight = new System.Windows.Forms.Label();
            this.textBoxHeight = new System.Windows.Forms.TextBox();
            this.labelSpeedT = new System.Windows.Forms.Label();
            this.textBoxSpeedT = new System.Windows.Forms.TextBox();
            this.checkBoxIsReverse = new System.Windows.Forms.CheckBox();
            this.labelOffsetT = new System.Windows.Forms.Label();
            this.textBoxOffsetT = new System.Windows.Forms.TextBox();
            this.labelXP = new System.Windows.Forms.Label();
            this.textBoxXP = new System.Windows.Forms.TextBox();
            this.labelYP = new System.Windows.Forms.Label();
            this.textBoxYP = new System.Windows.Forms.TextBox();
            this.labelPointsCount = new System.Windows.Forms.Label();
            this.buttonStartStopTimer = new System.Windows.Forms.Button();
            this.checkBoxIsShowDots = new System.Windows.Forms.CheckBox();
            this.checkBoxIsShowPoints = new System.Windows.Forms.CheckBox();
            this.tableLayoutPanelAddDelEntity = new System.Windows.Forms.TableLayoutPanel();
            this.buttonDelEntity = new System.Windows.Forms.Button();
            this.buttonAddEntity = new System.Windows.Forms.Button();
            this.tableLayoutPanelAddDelPoints = new System.Windows.Forms.TableLayoutPanel();
            this.buttonDelPoint = new System.Windows.Forms.Button();
            this.buttonAddPoint = new System.Windows.Forms.Button();
            this.tableLayoutPanelLoadSave = new System.Windows.Forms.TableLayoutPanel();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonLoad = new System.Windows.Forms.Button();
            this.trackBar = new System.Windows.Forms.TrackBar();
            this.labelMinT = new System.Windows.Forms.Label();
            this.labelMaxT = new System.Windows.Forms.Label();
            this.textBoxMinT = new System.Windows.Forms.TextBox();
            this.textBoxMaxT = new System.Windows.Forms.TextBox();
            this.labelNormalMaxTime = new System.Windows.Forms.Label();
            this.labelNormalSpeedTime = new System.Windows.Forms.Label();
            this.labelUnnormalMaxTime = new System.Windows.Forms.Label();
            this.labelUnnormalSpeedTime = new System.Windows.Forms.Label();
            this.labelProperties = new System.Windows.Forms.Label();
            this.textBoxNormalMaxTime = new System.Windows.Forms.TextBox();
            this.textBoxNormalSpeedTime = new System.Windows.Forms.TextBox();
            this.textBoxUnnormalMaxTime = new System.Windows.Forms.TextBox();
            this.textBoxUnnormalSpeedTime = new System.Windows.Forms.TextBox();
            this.textBoxProperties = new System.Windows.Forms.TextBox();
            this.labelSize = new System.Windows.Forms.Label();
            this.textBoxSize = new System.Windows.Forms.TextBox();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.tableLayoutPanelSettings.SuspendLayout();
            this.tableLayoutPanelAddDelEntity.SuspendLayout();
            this.tableLayoutPanelAddDelPoints.SuspendLayout();
            this.tableLayoutPanelLoadSave.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Interval = 10;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // tableLayoutPanelSettings
            // 
            this.tableLayoutPanelSettings.AutoSize = true;
            this.tableLayoutPanelSettings.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanelSettings.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Inset;
            this.tableLayoutPanelSettings.ColumnCount = 2;
            this.tableLayoutPanelSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelSettings.Controls.Add(this.labelWidth, 0, 0);
            this.tableLayoutPanelSettings.Controls.Add(this.textBoxWidth, 1, 0);
            this.tableLayoutPanelSettings.Controls.Add(this.labelHeight, 0, 1);
            this.tableLayoutPanelSettings.Controls.Add(this.textBoxHeight, 1, 1);
            this.tableLayoutPanelSettings.Controls.Add(this.labelSpeedT, 0, 8);
            this.tableLayoutPanelSettings.Controls.Add(this.textBoxSpeedT, 1, 8);
            this.tableLayoutPanelSettings.Controls.Add(this.checkBoxIsReverse, 0, 9);
            this.tableLayoutPanelSettings.Controls.Add(this.labelOffsetT, 0, 7);
            this.tableLayoutPanelSettings.Controls.Add(this.textBoxOffsetT, 1, 7);
            this.tableLayoutPanelSettings.Controls.Add(this.labelXP, 0, 19);
            this.tableLayoutPanelSettings.Controls.Add(this.textBoxXP, 1, 19);
            this.tableLayoutPanelSettings.Controls.Add(this.labelYP, 0, 20);
            this.tableLayoutPanelSettings.Controls.Add(this.textBoxYP, 1, 20);
            this.tableLayoutPanelSettings.Controls.Add(this.labelPointsCount, 0, 17);
            this.tableLayoutPanelSettings.Controls.Add(this.buttonStartStopTimer, 0, 24);
            this.tableLayoutPanelSettings.Controls.Add(this.checkBoxIsShowDots, 0, 22);
            this.tableLayoutPanelSettings.Controls.Add(this.checkBoxIsShowPoints, 0, 23);
            this.tableLayoutPanelSettings.Controls.Add(this.tableLayoutPanelAddDelEntity, 0, 3);
            this.tableLayoutPanelSettings.Controls.Add(this.tableLayoutPanelAddDelPoints, 0, 18);
            this.tableLayoutPanelSettings.Controls.Add(this.tableLayoutPanelLoadSave, 0, 26);
            this.tableLayoutPanelSettings.Controls.Add(this.trackBar, 0, 25);
            this.tableLayoutPanelSettings.Controls.Add(this.labelMinT, 0, 5);
            this.tableLayoutPanelSettings.Controls.Add(this.labelMaxT, 0, 6);
            this.tableLayoutPanelSettings.Controls.Add(this.textBoxMinT, 1, 5);
            this.tableLayoutPanelSettings.Controls.Add(this.textBoxMaxT, 1, 6);
            this.tableLayoutPanelSettings.Controls.Add(this.labelNormalMaxTime, 0, 11);
            this.tableLayoutPanelSettings.Controls.Add(this.labelNormalSpeedTime, 0, 12);
            this.tableLayoutPanelSettings.Controls.Add(this.labelUnnormalMaxTime, 0, 13);
            this.tableLayoutPanelSettings.Controls.Add(this.labelUnnormalSpeedTime, 0, 14);
            this.tableLayoutPanelSettings.Controls.Add(this.labelProperties, 0, 15);
            this.tableLayoutPanelSettings.Controls.Add(this.textBoxNormalMaxTime, 1, 11);
            this.tableLayoutPanelSettings.Controls.Add(this.textBoxNormalSpeedTime, 1, 12);
            this.tableLayoutPanelSettings.Controls.Add(this.textBoxUnnormalMaxTime, 1, 13);
            this.tableLayoutPanelSettings.Controls.Add(this.textBoxUnnormalSpeedTime, 1, 14);
            this.tableLayoutPanelSettings.Controls.Add(this.textBoxProperties, 1, 15);
            this.tableLayoutPanelSettings.Controls.Add(this.labelSize, 0, 4);
            this.tableLayoutPanelSettings.Controls.Add(this.textBoxSize, 1, 4);
            this.tableLayoutPanelSettings.Dock = System.Windows.Forms.DockStyle.Left;
            this.tableLayoutPanelSettings.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelSettings.Name = "tableLayoutPanelSettings";
            this.tableLayoutPanelSettings.RowCount = 27;
            this.tableLayoutPanelSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.tableLayoutPanelSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.tableLayoutPanelSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
            this.tableLayoutPanelSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelSettings.Size = new System.Drawing.Size(222, 673);
            this.tableLayoutPanelSettings.TabIndex = 1;
            // 
            // labelWidth
            // 
            this.labelWidth.AutoSize = true;
            this.labelWidth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelWidth.Location = new System.Drawing.Point(5, 5);
            this.labelWidth.Margin = new System.Windows.Forms.Padding(3);
            this.labelWidth.Name = "labelWidth";
            this.labelWidth.Size = new System.Drawing.Size(57, 20);
            this.labelWidth.TabIndex = 0;
            this.labelWidth.Text = "Width:";
            this.labelWidth.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxWidth
            // 
            this.textBoxWidth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxWidth.Location = new System.Drawing.Point(70, 5);
            this.textBoxWidth.Name = "textBoxWidth";
            this.textBoxWidth.Size = new System.Drawing.Size(147, 20);
            this.textBoxWidth.TabIndex = 1;
            this.textBoxWidth.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // labelHeight
            // 
            this.labelHeight.AutoSize = true;
            this.labelHeight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelHeight.Location = new System.Drawing.Point(5, 33);
            this.labelHeight.Margin = new System.Windows.Forms.Padding(3);
            this.labelHeight.Name = "labelHeight";
            this.labelHeight.Size = new System.Drawing.Size(57, 20);
            this.labelHeight.TabIndex = 2;
            this.labelHeight.Text = "Height:";
            this.labelHeight.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxHeight
            // 
            this.textBoxHeight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxHeight.Location = new System.Drawing.Point(70, 33);
            this.textBoxHeight.Name = "textBoxHeight";
            this.textBoxHeight.Size = new System.Drawing.Size(147, 20);
            this.textBoxHeight.TabIndex = 3;
            this.textBoxHeight.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // labelSpeedT
            // 
            this.labelSpeedT.AutoSize = true;
            this.labelSpeedT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSpeedT.Location = new System.Drawing.Point(5, 217);
            this.labelSpeedT.Margin = new System.Windows.Forms.Padding(3);
            this.labelSpeedT.Name = "labelSpeedT";
            this.labelSpeedT.Size = new System.Drawing.Size(57, 20);
            this.labelSpeedT.TabIndex = 6;
            this.labelSpeedT.Text = "Speed:";
            this.labelSpeedT.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxSpeedT
            // 
            this.textBoxSpeedT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxSpeedT.Location = new System.Drawing.Point(70, 217);
            this.textBoxSpeedT.Name = "textBoxSpeedT";
            this.textBoxSpeedT.Size = new System.Drawing.Size(147, 20);
            this.textBoxSpeedT.TabIndex = 7;
            this.textBoxSpeedT.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // checkBoxIsReverse
            // 
            this.checkBoxIsReverse.AutoSize = true;
            this.tableLayoutPanelSettings.SetColumnSpan(this.checkBoxIsReverse, 2);
            this.checkBoxIsReverse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxIsReverse.Location = new System.Drawing.Point(5, 245);
            this.checkBoxIsReverse.Name = "checkBoxIsReverse";
            this.checkBoxIsReverse.Size = new System.Drawing.Size(212, 17);
            this.checkBoxIsReverse.TabIndex = 8;
            this.checkBoxIsReverse.Text = "Is reverse";
            this.checkBoxIsReverse.UseVisualStyleBackColor = true;
            this.checkBoxIsReverse.CheckedChanged += new System.EventHandler(this.checkBox_CheckedChanged);
            this.checkBoxIsReverse.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form_KeyDown);
            // 
            // labelOffsetT
            // 
            this.labelOffsetT.AutoSize = true;
            this.labelOffsetT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelOffsetT.Location = new System.Drawing.Point(5, 189);
            this.labelOffsetT.Margin = new System.Windows.Forms.Padding(3);
            this.labelOffsetT.Name = "labelOffsetT";
            this.labelOffsetT.Size = new System.Drawing.Size(57, 20);
            this.labelOffsetT.TabIndex = 9;
            this.labelOffsetT.Text = "Offset:";
            this.labelOffsetT.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxOffsetT
            // 
            this.textBoxOffsetT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxOffsetT.Location = new System.Drawing.Point(70, 189);
            this.textBoxOffsetT.Name = "textBoxOffsetT";
            this.textBoxOffsetT.Size = new System.Drawing.Size(147, 20);
            this.textBoxOffsetT.TabIndex = 10;
            this.textBoxOffsetT.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // labelXP
            // 
            this.labelXP.AutoSize = true;
            this.labelXP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelXP.Location = new System.Drawing.Point(5, 482);
            this.labelXP.Margin = new System.Windows.Forms.Padding(3);
            this.labelXP.Name = "labelXP";
            this.labelXP.Size = new System.Drawing.Size(57, 20);
            this.labelXP.TabIndex = 13;
            this.labelXP.Text = "X (%):";
            this.labelXP.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxXP
            // 
            this.textBoxXP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxXP.Location = new System.Drawing.Point(70, 482);
            this.textBoxXP.Name = "textBoxXP";
            this.textBoxXP.Size = new System.Drawing.Size(147, 20);
            this.textBoxXP.TabIndex = 14;
            this.textBoxXP.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // labelYP
            // 
            this.labelYP.AutoSize = true;
            this.labelYP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelYP.Location = new System.Drawing.Point(5, 510);
            this.labelYP.Margin = new System.Windows.Forms.Padding(3);
            this.labelYP.Name = "labelYP";
            this.labelYP.Size = new System.Drawing.Size(57, 20);
            this.labelYP.TabIndex = 15;
            this.labelYP.Text = "Y (%):";
            this.labelYP.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxYP
            // 
            this.textBoxYP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxYP.Location = new System.Drawing.Point(70, 510);
            this.textBoxYP.Name = "textBoxYP";
            this.textBoxYP.Size = new System.Drawing.Size(147, 20);
            this.textBoxYP.TabIndex = 16;
            this.textBoxYP.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // labelPointsCount
            // 
            this.labelPointsCount.AutoSize = true;
            this.tableLayoutPanelSettings.SetColumnSpan(this.labelPointsCount, 2);
            this.labelPointsCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelPointsCount.Location = new System.Drawing.Point(5, 424);
            this.labelPointsCount.Margin = new System.Windows.Forms.Padding(3);
            this.labelPointsCount.Name = "labelPointsCount";
            this.labelPointsCount.Size = new System.Drawing.Size(212, 13);
            this.labelPointsCount.TabIndex = 17;
            this.labelPointsCount.Text = "Points count:";
            this.labelPointsCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonStartStopTimer
            // 
            this.buttonStartStopTimer.AutoSize = true;
            this.buttonStartStopTimer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanelSettings.SetColumnSpan(this.buttonStartStopTimer, 2);
            this.buttonStartStopTimer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonStartStopTimer.Location = new System.Drawing.Point(5, 555);
            this.buttonStartStopTimer.Name = "buttonStartStopTimer";
            this.buttonStartStopTimer.Size = new System.Drawing.Size(212, 23);
            this.buttonStartStopTimer.TabIndex = 18;
            this.buttonStartStopTimer.Text = "Stop timer";
            this.buttonStartStopTimer.UseVisualStyleBackColor = true;
            this.buttonStartStopTimer.Click += new System.EventHandler(this.buttonStartStopTimer_Click);
            // 
            // checkBoxIsShowDots
            // 
            this.checkBoxIsShowDots.AutoSize = true;
            this.checkBoxIsShowDots.Checked = true;
            this.checkBoxIsShowDots.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tableLayoutPanelSettings.SetColumnSpan(this.checkBoxIsShowDots, 2);
            this.checkBoxIsShowDots.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxIsShowDots.Location = new System.Drawing.Point(5, 505);
            this.checkBoxIsShowDots.Name = "checkBoxIsShowDots";
            this.checkBoxIsShowDots.Size = new System.Drawing.Size(212, 17);
            this.checkBoxIsShowDots.TabIndex = 19;
            this.checkBoxIsShowDots.Text = "Is show dots";
            this.checkBoxIsShowDots.UseVisualStyleBackColor = true;
            // 
            // checkBoxIsShowPoints
            // 
            this.checkBoxIsShowPoints.AutoSize = true;
            this.checkBoxIsShowPoints.Checked = true;
            this.checkBoxIsShowPoints.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tableLayoutPanelSettings.SetColumnSpan(this.checkBoxIsShowPoints, 2);
            this.checkBoxIsShowPoints.Location = new System.Drawing.Point(5, 530);
            this.checkBoxIsShowPoints.Name = "checkBoxIsShowPoints";
            this.checkBoxIsShowPoints.Size = new System.Drawing.Size(93, 17);
            this.checkBoxIsShowPoints.TabIndex = 20;
            this.checkBoxIsShowPoints.Text = "Is show points";
            this.checkBoxIsShowPoints.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanelAddDelEntity
            // 
            this.tableLayoutPanelAddDelEntity.AutoSize = true;
            this.tableLayoutPanelAddDelEntity.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanelAddDelEntity.ColumnCount = 2;
            this.tableLayoutPanelSettings.SetColumnSpan(this.tableLayoutPanelAddDelEntity, 2);
            this.tableLayoutPanelAddDelEntity.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelAddDelEntity.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelAddDelEntity.Controls.Add(this.buttonDelEntity, 1, 0);
            this.tableLayoutPanelAddDelEntity.Controls.Add(this.buttonAddEntity, 0, 0);
            this.tableLayoutPanelAddDelEntity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelAddDelEntity.Location = new System.Drawing.Point(5, 68);
            this.tableLayoutPanelAddDelEntity.Name = "tableLayoutPanelAddDelEntity";
            this.tableLayoutPanelAddDelEntity.RowCount = 1;
            this.tableLayoutPanelAddDelEntity.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelAddDelEntity.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanelAddDelEntity.Size = new System.Drawing.Size(212, 29);
            this.tableLayoutPanelAddDelEntity.TabIndex = 23;
            // 
            // buttonDelEntity
            // 
            this.buttonDelEntity.AutoSize = true;
            this.buttonDelEntity.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonDelEntity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonDelEntity.Location = new System.Drawing.Point(109, 3);
            this.buttonDelEntity.Name = "buttonDelEntity";
            this.buttonDelEntity.Size = new System.Drawing.Size(100, 23);
            this.buttonDelEntity.TabIndex = 5;
            this.buttonDelEntity.Text = "Del entity";
            this.buttonDelEntity.UseVisualStyleBackColor = true;
            this.buttonDelEntity.Click += new System.EventHandler(this.buttonDelEntity_Click);
            // 
            // buttonAddEntity
            // 
            this.buttonAddEntity.AutoSize = true;
            this.buttonAddEntity.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonAddEntity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonAddEntity.Location = new System.Drawing.Point(3, 3);
            this.buttonAddEntity.Name = "buttonAddEntity";
            this.buttonAddEntity.Size = new System.Drawing.Size(100, 23);
            this.buttonAddEntity.TabIndex = 4;
            this.buttonAddEntity.Text = "Add entity";
            this.buttonAddEntity.UseVisualStyleBackColor = true;
            this.buttonAddEntity.Click += new System.EventHandler(this.buttonAddEntity_Click);
            // 
            // tableLayoutPanelAddDelPoints
            // 
            this.tableLayoutPanelAddDelPoints.AutoSize = true;
            this.tableLayoutPanelAddDelPoints.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanelAddDelPoints.ColumnCount = 2;
            this.tableLayoutPanelSettings.SetColumnSpan(this.tableLayoutPanelAddDelPoints, 2);
            this.tableLayoutPanelAddDelPoints.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelAddDelPoints.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelAddDelPoints.Controls.Add(this.buttonDelPoint, 1, 0);
            this.tableLayoutPanelAddDelPoints.Controls.Add(this.buttonAddPoint, 0, 0);
            this.tableLayoutPanelAddDelPoints.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelAddDelPoints.Location = new System.Drawing.Point(5, 445);
            this.tableLayoutPanelAddDelPoints.Name = "tableLayoutPanelAddDelPoints";
            this.tableLayoutPanelAddDelPoints.RowCount = 1;
            this.tableLayoutPanelAddDelPoints.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelAddDelPoints.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanelAddDelPoints.Size = new System.Drawing.Size(212, 29);
            this.tableLayoutPanelAddDelPoints.TabIndex = 24;
            // 
            // buttonDelPoint
            // 
            this.buttonDelPoint.AutoSize = true;
            this.buttonDelPoint.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonDelPoint.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonDelPoint.Location = new System.Drawing.Point(109, 3);
            this.buttonDelPoint.Name = "buttonDelPoint";
            this.buttonDelPoint.Size = new System.Drawing.Size(100, 23);
            this.buttonDelPoint.TabIndex = 12;
            this.buttonDelPoint.Text = "Del point";
            this.buttonDelPoint.UseVisualStyleBackColor = true;
            this.buttonDelPoint.Click += new System.EventHandler(this.buttonDelPoint_Click);
            // 
            // buttonAddPoint
            // 
            this.buttonAddPoint.AutoSize = true;
            this.buttonAddPoint.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonAddPoint.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonAddPoint.Location = new System.Drawing.Point(3, 3);
            this.buttonAddPoint.Name = "buttonAddPoint";
            this.buttonAddPoint.Size = new System.Drawing.Size(100, 23);
            this.buttonAddPoint.TabIndex = 11;
            this.buttonAddPoint.Text = "Add point";
            this.buttonAddPoint.UseVisualStyleBackColor = true;
            this.buttonAddPoint.Click += new System.EventHandler(this.buttonAddPoint_Click);
            // 
            // tableLayoutPanelLoadSave
            // 
            this.tableLayoutPanelLoadSave.AutoSize = true;
            this.tableLayoutPanelLoadSave.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanelLoadSave.ColumnCount = 2;
            this.tableLayoutPanelSettings.SetColumnSpan(this.tableLayoutPanelLoadSave, 2);
            this.tableLayoutPanelLoadSave.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelLoadSave.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelLoadSave.Controls.Add(this.buttonSave, 1, 0);
            this.tableLayoutPanelLoadSave.Controls.Add(this.buttonLoad, 0, 0);
            this.tableLayoutPanelLoadSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelLoadSave.Location = new System.Drawing.Point(5, 639);
            this.tableLayoutPanelLoadSave.Name = "tableLayoutPanelLoadSave";
            this.tableLayoutPanelLoadSave.RowCount = 1;
            this.tableLayoutPanelLoadSave.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelLoadSave.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            this.tableLayoutPanelLoadSave.Size = new System.Drawing.Size(212, 29);
            this.tableLayoutPanelLoadSave.TabIndex = 25;
            // 
            // buttonSave
            // 
            this.buttonSave.AutoSize = true;
            this.buttonSave.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSave.Location = new System.Drawing.Point(109, 3);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(100, 23);
            this.buttonSave.TabIndex = 22;
            this.buttonSave.Text = "Save...";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonLoad
            // 
            this.buttonLoad.AutoSize = true;
            this.buttonLoad.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonLoad.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonLoad.Location = new System.Drawing.Point(3, 3);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(100, 23);
            this.buttonLoad.TabIndex = 21;
            this.buttonLoad.Text = "Load...";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new System.EventHandler(this.buttonLoad_Click);
            // 
            // trackBar
            // 
            this.tableLayoutPanelSettings.SetColumnSpan(this.trackBar, 2);
            this.trackBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackBar.Location = new System.Drawing.Point(5, 586);
            this.trackBar.Maximum = 5000;
            this.trackBar.Name = "trackBar";
            this.trackBar.Size = new System.Drawing.Size(212, 45);
            this.trackBar.TabIndex = 26;
            this.trackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBar.Value = 1000;
            this.trackBar.Scroll += new System.EventHandler(this.trackBar_Scroll);
            // 
            // labelMinT
            // 
            this.labelMinT.AutoSize = true;
            this.labelMinT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMinT.Location = new System.Drawing.Point(5, 133);
            this.labelMinT.Margin = new System.Windows.Forms.Padding(3);
            this.labelMinT.Name = "labelMinT";
            this.labelMinT.Size = new System.Drawing.Size(57, 20);
            this.labelMinT.TabIndex = 27;
            this.labelMinT.Text = "Min:";
            this.labelMinT.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelMaxT
            // 
            this.labelMaxT.AutoSize = true;
            this.labelMaxT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelMaxT.Location = new System.Drawing.Point(5, 161);
            this.labelMaxT.Margin = new System.Windows.Forms.Padding(3);
            this.labelMaxT.Name = "labelMaxT";
            this.labelMaxT.Size = new System.Drawing.Size(57, 20);
            this.labelMaxT.TabIndex = 28;
            this.labelMaxT.Text = "Max:";
            this.labelMaxT.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxMinT
            // 
            this.textBoxMinT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxMinT.Location = new System.Drawing.Point(70, 133);
            this.textBoxMinT.Name = "textBoxMinT";
            this.textBoxMinT.Size = new System.Drawing.Size(147, 20);
            this.textBoxMinT.TabIndex = 29;
            this.textBoxMinT.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // textBoxMaxT
            // 
            this.textBoxMaxT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxMaxT.Location = new System.Drawing.Point(70, 161);
            this.textBoxMaxT.Name = "textBoxMaxT";
            this.textBoxMaxT.Size = new System.Drawing.Size(147, 20);
            this.textBoxMaxT.TabIndex = 30;
            this.textBoxMaxT.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // labelNormalMaxTime
            // 
            this.labelNormalMaxTime.AutoSize = true;
            this.labelNormalMaxTime.Location = new System.Drawing.Point(5, 274);
            this.labelNormalMaxTime.Name = "labelNormalMaxTime";
            this.labelNormalMaxTime.Size = new System.Drawing.Size(41, 13);
            this.labelNormalMaxTime.TabIndex = 31;
            this.labelNormalMaxTime.Text = "NTime:";
            // 
            // labelNormalSpeedTime
            // 
            this.labelNormalSpeedTime.AutoSize = true;
            this.labelNormalSpeedTime.Location = new System.Drawing.Point(5, 302);
            this.labelNormalSpeedTime.Name = "labelNormalSpeedTime";
            this.labelNormalSpeedTime.Size = new System.Drawing.Size(49, 13);
            this.labelNormalSpeedTime.TabIndex = 32;
            this.labelNormalSpeedTime.Text = "NSpeed:";
            // 
            // labelUnnormalMaxTime
            // 
            this.labelUnnormalMaxTime.AutoSize = true;
            this.labelUnnormalMaxTime.Location = new System.Drawing.Point(5, 330);
            this.labelUnnormalMaxTime.Name = "labelUnnormalMaxTime";
            this.labelUnnormalMaxTime.Size = new System.Drawing.Size(41, 13);
            this.labelUnnormalMaxTime.TabIndex = 33;
            this.labelUnnormalMaxTime.Text = "UTime:";
            // 
            // labelUnnormalSpeedTime
            // 
            this.labelUnnormalSpeedTime.AutoSize = true;
            this.labelUnnormalSpeedTime.Location = new System.Drawing.Point(5, 358);
            this.labelUnnormalSpeedTime.Name = "labelUnnormalSpeedTime";
            this.labelUnnormalSpeedTime.Size = new System.Drawing.Size(49, 13);
            this.labelUnnormalSpeedTime.TabIndex = 34;
            this.labelUnnormalSpeedTime.Text = "USpeed:";
            // 
            // labelProperties
            // 
            this.labelProperties.AutoSize = true;
            this.labelProperties.Location = new System.Drawing.Point(5, 386);
            this.labelProperties.Name = "labelProperties";
            this.labelProperties.Size = new System.Drawing.Size(57, 13);
            this.labelProperties.TabIndex = 35;
            this.labelProperties.Text = "Properties:";
            // 
            // textBoxNormalMaxTime
            // 
            this.textBoxNormalMaxTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxNormalMaxTime.Location = new System.Drawing.Point(70, 277);
            this.textBoxNormalMaxTime.Name = "textBoxNormalMaxTime";
            this.textBoxNormalMaxTime.Size = new System.Drawing.Size(147, 20);
            this.textBoxNormalMaxTime.TabIndex = 36;
            this.textBoxNormalMaxTime.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // textBoxNormalSpeedTime
            // 
            this.textBoxNormalSpeedTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxNormalSpeedTime.Location = new System.Drawing.Point(70, 305);
            this.textBoxNormalSpeedTime.Name = "textBoxNormalSpeedTime";
            this.textBoxNormalSpeedTime.Size = new System.Drawing.Size(147, 20);
            this.textBoxNormalSpeedTime.TabIndex = 37;
            this.textBoxNormalSpeedTime.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // textBoxUnnormalMaxTime
            // 
            this.textBoxUnnormalMaxTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxUnnormalMaxTime.Location = new System.Drawing.Point(70, 333);
            this.textBoxUnnormalMaxTime.Name = "textBoxUnnormalMaxTime";
            this.textBoxUnnormalMaxTime.Size = new System.Drawing.Size(147, 20);
            this.textBoxUnnormalMaxTime.TabIndex = 38;
            this.textBoxUnnormalMaxTime.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // textBoxUnnormalSpeedTime
            // 
            this.textBoxUnnormalSpeedTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxUnnormalSpeedTime.Location = new System.Drawing.Point(70, 361);
            this.textBoxUnnormalSpeedTime.Name = "textBoxUnnormalSpeedTime";
            this.textBoxUnnormalSpeedTime.Size = new System.Drawing.Size(147, 20);
            this.textBoxUnnormalSpeedTime.TabIndex = 39;
            this.textBoxUnnormalSpeedTime.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // textBoxProperties
            // 
            this.textBoxProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxProperties.Location = new System.Drawing.Point(70, 389);
            this.textBoxProperties.Name = "textBoxProperties";
            this.textBoxProperties.Size = new System.Drawing.Size(147, 20);
            this.textBoxProperties.TabIndex = 40;
            this.textBoxProperties.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // labelSize
            // 
            this.labelSize.AutoSize = true;
            this.labelSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelSize.Location = new System.Drawing.Point(5, 102);
            this.labelSize.Name = "labelSize";
            this.labelSize.Size = new System.Drawing.Size(57, 26);
            this.labelSize.TabIndex = 41;
            this.labelSize.Text = "Size:";
            this.labelSize.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // textBoxSize
            // 
            this.textBoxSize.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxSize.Location = new System.Drawing.Point(70, 105);
            this.textBoxSize.Name = "textBoxSize";
            this.textBoxSize.Size = new System.Drawing.Size(147, 20);
            this.textBoxSize.TabIndex = 42;
            this.textBoxSize.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "enteties.txml";
            // 
            // MoveTestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(492, 673);
            this.Controls.Add(this.tableLayoutPanelSettings);
            this.DoubleBuffered = true;
            this.Name = "MoveTestForm";
            this.Text = "MoveTestForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form_KeyDown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MoveTestForm_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MoveTestForm_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MoveTestForm_MouseUp);
            this.Resize += new System.EventHandler(this.Form_Resize);
            this.tableLayoutPanelSettings.ResumeLayout(false);
            this.tableLayoutPanelSettings.PerformLayout();
            this.tableLayoutPanelAddDelEntity.ResumeLayout(false);
            this.tableLayoutPanelAddDelEntity.PerformLayout();
            this.tableLayoutPanelAddDelPoints.ResumeLayout(false);
            this.tableLayoutPanelAddDelPoints.PerformLayout();
            this.tableLayoutPanelLoadSave.ResumeLayout(false);
            this.tableLayoutPanelLoadSave.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelSettings;
        private System.Windows.Forms.Label labelWidth;
        private System.Windows.Forms.TextBox textBoxWidth;
        private System.Windows.Forms.Label labelHeight;
        private System.Windows.Forms.TextBox textBoxHeight;
        private System.Windows.Forms.Button buttonAddEntity;
        private System.Windows.Forms.Button buttonDelEntity;
        private System.Windows.Forms.Label labelSpeedT;
        private System.Windows.Forms.TextBox textBoxSpeedT;
        private System.Windows.Forms.CheckBox checkBoxIsReverse;
        private System.Windows.Forms.Label labelOffsetT;
        private System.Windows.Forms.TextBox textBoxOffsetT;
        private System.Windows.Forms.Button buttonAddPoint;
        private System.Windows.Forms.Button buttonDelPoint;
        private System.Windows.Forms.Label labelXP;
        private System.Windows.Forms.TextBox textBoxXP;
        private System.Windows.Forms.Label labelYP;
        private System.Windows.Forms.TextBox textBoxYP;
        private System.Windows.Forms.Label labelPointsCount;
        private System.Windows.Forms.Button buttonStartStopTimer;
        private System.Windows.Forms.CheckBox checkBoxIsShowDots;
        private System.Windows.Forms.CheckBox checkBoxIsShowPoints;
        private System.Windows.Forms.Button buttonLoad;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelAddDelEntity;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelAddDelPoints;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelLoadSave;
        private System.Windows.Forms.TrackBar trackBar;
        private System.Windows.Forms.Label labelMinT;
        private System.Windows.Forms.Label labelMaxT;
        private System.Windows.Forms.TextBox textBoxMinT;
        private System.Windows.Forms.TextBox textBoxMaxT;
        private System.Windows.Forms.Label labelNormalMaxTime;
        private System.Windows.Forms.Label labelNormalSpeedTime;
        private System.Windows.Forms.Label labelUnnormalMaxTime;
        private System.Windows.Forms.Label labelUnnormalSpeedTime;
        private System.Windows.Forms.Label labelProperties;
        private System.Windows.Forms.TextBox textBoxNormalMaxTime;
        private System.Windows.Forms.TextBox textBoxNormalSpeedTime;
        private System.Windows.Forms.TextBox textBoxUnnormalMaxTime;
        private System.Windows.Forms.TextBox textBoxUnnormalSpeedTime;
        private System.Windows.Forms.TextBox textBoxProperties;
        private System.Windows.Forms.Label labelSize;
        private System.Windows.Forms.TextBox textBoxSize;
    }
}

