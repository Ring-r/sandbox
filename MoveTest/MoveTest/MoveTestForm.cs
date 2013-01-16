using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace MoveTest
{
    public partial class MoveTestForm : Form
    {
        private readonly Random rand = new Random();

        #region Данные визуализации.

        private readonly int dotRadius = 2;
        private readonly int pointRadius = 5;

        private readonly int lineCount = 20;
        private readonly Brush unsableAreaBrush = new SolidBrush(Color.FromArgb(100, 0, 0, 0));

        #endregion Данные визуализации.

        private readonly Stopwatch stopwatch = new Stopwatch();

        private readonly List<Chiky> entityList = new List<Chiky>();

        private int entIndex = 0;
        private int pointIndex = 0;

        public MoveTestForm()
        {
            InitializeComponent();

            this.entityList.Add(new Chiky() { mSpeedTime = 1 });

            this.entityList[this.entIndex].Add(50, 50);

            this.stopwatch.Start();
        }

        private void UpdateData()
        {
            if (!this.isUpdateForm)
            {
                try
                {
                    Options.CameraWidth = float.Parse(this.textBoxWidth.Text);
                    Options.CameraHeight = float.Parse(this.textBoxHeight.Text);
                    Options.CameraX = this.ClientSize.Width / 2 - Options.CameraWidth / 2;
                    Options.CameraY = this.ClientSize.Height / 2 - Options.CameraHeight / 2;

                    this.entityList[this.entIndex].setWidth(float.Parse(this.textBoxSize.Text));
                    this.entityList[this.entIndex].setHeight(float.Parse(this.textBoxSize.Text));

                    this.entityList[this.entIndex].mMinTime = float.Parse(this.textBoxMinT.Text);
                    this.entityList[this.entIndex].mMaxTime = float.Parse(this.textBoxMaxT.Text);
                    this.entityList[this.entIndex].mOffsetTime = float.Parse(this.textBoxOffsetT.Text);
                    this.entityList[this.entIndex].mSpeedTime = float.Parse(this.textBoxSpeedT.Text);
                    this.entityList[this.entIndex].mIsReverseTime = this.checkBoxIsReverse.Checked;

                    this.entityList[this.entIndex].mNormalMaxTime = float.Parse(this.textBoxNormalMaxTime.Text);
                    this.entityList[this.entIndex].mNormalSpeedTime = float.Parse(this.textBoxNormalSpeedTime.Text);
                    this.entityList[this.entIndex].mUnnormalMaxTime = float.Parse(this.textBoxUnnormalMaxTime.Text);
                    this.entityList[this.entIndex].mUnnormalSpeedTime = float.Parse(this.textBoxUnnormalSpeedTime.Text);
                    this.entityList[this.entIndex].mProperties = int.Parse(this.textBoxProperties.Text);

                    this.entityList[this.entIndex].mList[2 * this.pointIndex] = short.Parse(this.textBoxXP.Text);
                    this.entityList[this.entIndex].mList[2 * this.pointIndex + 1] = short.Parse(this.textBoxYP.Text);

                    this.UpdateForm();
                }
                catch { }
            }
        }

        private bool isUpdateForm = false;
        private void UpdateForm()
        {
            this.isUpdateForm = true;

            this.textBoxWidth.Text = Options.CameraWidth.ToString();
            this.textBoxHeight.Text = Options.CameraHeight.ToString();

            this.textBoxSize.Text = this.entityList[this.entIndex].getWidth().ToString();

            this.textBoxMinT.Text = this.entityList[this.entIndex].mMinTime.ToString();
            this.textBoxMaxT.Text = this.entityList[this.entIndex].mMaxTime.ToString();
            this.textBoxOffsetT.Text = this.entityList[this.entIndex].mOffsetTime.ToString();
            this.textBoxSpeedT.Text = Math.Abs(this.entityList[this.entIndex].mSpeedTime).ToString();
            this.checkBoxIsReverse.Checked = this.entityList[this.entIndex].mIsReverseTime;

            this.textBoxNormalMaxTime.Text = this.entityList[this.entIndex].mNormalMaxTime.ToString();
            this.textBoxNormalSpeedTime.Text = this.entityList[this.entIndex].mNormalSpeedTime.ToString();
            this.textBoxUnnormalMaxTime.Text = this.entityList[this.entIndex].mUnnormalMaxTime.ToString();
            this.textBoxUnnormalSpeedTime.Text = this.entityList[this.entIndex].mUnnormalSpeedTime.ToString();
            this.textBoxProperties.Text = this.entityList[this.entIndex].mProperties.ToString();

            this.labelPointsCount.Text = string.Format("Points count: {0}", this.entityList[this.entIndex].mListCount);
            this.textBoxXP.Text = this.entityList[this.entIndex].mList[2 * this.pointIndex].ToString();
            this.textBoxYP.Text = this.entityList[this.entIndex].mList[2 * this.pointIndex + 1].ToString();

            this.isUpdateForm = false;

            foreach (Chiky entity in this.entityList)
            {
                entity.reset();
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            onManagedUpdate(this.stopwatch.ElapsedMilliseconds / 1000f);
            this.Invalidate();
            this.stopwatch.Restart();
        }

        public void onManagedDraw(Graphics graphics)
        {
            graphics.Clear(Color.White);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;

            graphics.FillRectangle(this.unsableAreaBrush, Options.CameraX, Options.CameraY, Options.CameraWidth, Options.MenuHeight);
            graphics.FillRectangle(this.unsableAreaBrush, Options.CameraX, Options.CameraY + Options.CameraHeight - Options.TouchHeight, Options.CameraWidth, Options.TouchHeight);

            #region Draw lines.

            float linesStep;
            float linesBegin;

            linesStep = Options.CameraWidth / lineCount;
            linesBegin = Options.CameraX;
            while (0 < linesBegin)
            {
                linesBegin -= linesStep;
                graphics.DrawLine(Pens.Silver, linesBegin, 0, linesBegin, this.ClientSize.Height);
            }
            linesBegin = Options.CameraX;
            while (linesBegin < this.ClientSize.Width)
            {
                graphics.DrawLine(Pens.Silver, linesBegin, 0, linesBegin, this.ClientSize.Height);
                linesBegin += linesStep;
            }

            linesStep = Options.CameraHeight / lineCount;
            linesBegin = Options.CameraY;
            while (0 < linesBegin)
            {
                linesBegin -= linesStep;
                graphics.DrawLine(Pens.Silver, 0, linesBegin, this.ClientSize.Width, linesBegin);
            }
            linesBegin = Options.CameraY;
            while (linesBegin < this.ClientSize.Width)
            {
                graphics.DrawLine(Pens.Silver, 0, linesBegin, this.ClientSize.Width, linesBegin);
                linesBegin += linesStep;
            }
            #endregion Draw lines.

            graphics.DrawRectangle(Pens.Gray, Options.CameraX, Options.CameraY, Options.CameraWidth, Options.MenuHeight);
            graphics.DrawRectangle(Pens.Gray, Options.CameraX, Options.CameraY + Options.CameraHeight - Options.TouchHeight, Options.CameraWidth, Options.TouchHeight);
            graphics.DrawRectangle(Pens.Gray, Options.CameraX, Options.CameraY, Options.CameraWidth, Options.CameraHeight);

            #region Draw dots.
            if (this.checkBoxIsShowDots.Checked)
            {
                foreach (EntityBezier entity in this.entityList)
                {
                    for (int j = 0; j < entity.dots.Length; j++)
                    {
                        graphics.FillEllipse(Brushes.Silver, Options.CameraX + entity.dots[j].X - this.dotRadius, Options.CameraY + entity.dots[j].Y - this.dotRadius, 2 * this.dotRadius, 2 * this.dotRadius);
                    }
                }
            }
            #endregion Draw dots.

            #region Draw points.
            if (this.checkBoxIsShowPoints.Checked)
            {
                foreach (EntityBezier entity in this.entityList)
                {
                    for (int j = 0; j < entity.mListCount; j++)
                    {
                        float x = entity.getFloatAtListX(j);
                        float y = entity.getFloatAtListY(j);
                        graphics.FillEllipse(Brushes.Silver, x - this.pointRadius, y - this.pointRadius, 2 * this.pointRadius, 2 * this.pointRadius);
                        graphics.DrawString(j.ToString(), this.Font, Brushes.Black, x + this.pointRadius, y + this.pointRadius);
                    }
                }
            }
            #endregion Draw points.

            #region Draw entity.
            foreach (EntityBezier entity in this.entityList)
            {
                graphics.FillEllipse(Brushes.Red, Options.CameraX + entity.getX(), Options.CameraY + entity.getY(), entity.getWidth(), entity.getHeight());
            }
            #endregion Draw entity.

            if (this.checkBoxIsShowPoints.Checked)
            {
                EntityBezier entity = this.entityList[this.entIndex];

                graphics.FillEllipse(Brushes.Green, entity.getFloatAtListX(this.pointIndex) - this.pointRadius, entity.getFloatAtListY(this.pointIndex) - this.pointRadius, 2 * this.pointRadius, 2 * this.pointRadius);
            }
        }

        public void onManagedUpdate(float pSecondsElapsed)
        {
            foreach (Entity entity in this.entityList)
            {
                entity.onManagedUpdate(pSecondsElapsed);
            }
        }

        private void Form_Paint(object sender, PaintEventArgs e)
        {
            this.onManagedDraw(e.Graphics);
        }

        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            int pointCount;
            Chiky chiky = this.entityList[this.entIndex];
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    this.Close();
                    break;
                case Keys.D1:
                    chiky.mOffsetTime = (float)this.rand.NextDouble();
                    chiky.mSpeedTime = (float)this.rand.NextDouble();
                    chiky.mNormalMaxTime = 3 * (float)this.rand.NextDouble();
                    chiky.mNormalSpeedTime = (float)this.rand.NextDouble();
                    chiky.mUnnormalMaxTime = 3 * (float)this.rand.NextDouble();
                    chiky.mUnnormalSpeedTime = 2 * (float)this.rand.NextDouble();
                    chiky.mProperties = Chiky.isUnnormalMoveFlag;
                    chiky.mListCount = 0;
                    pointCount = this.rand.Next(4) + 1;
                    for (int i = 0; i < pointCount; ++i)
                    {
                        chiky.Add((short)this.rand.Next(100), (short)this.rand.Next(100));
                    }
                    this.UpdateForm();
                    break;
                case Keys.D2:
                    pointCount = chiky.mListCount;
                    for (int i = 0; i < pointCount; ++i)
                    {
                        chiky.Add((short)(100 - chiky.mList[2 * (pointCount - i - 1)]), chiky.mList[2 * (pointCount - i - 1) + 1]);
                    }
                    this.UpdateForm();
                    break;
                case Keys.D3:
                    pointCount = chiky.mListCount;
                    for (int i = 0; i < pointCount; ++i)
                    {
                        chiky.Add((short)(100 - chiky.mList[2 * (pointCount - i - 1)]), (short)(100 - chiky.mList[2 * (pointCount - i - 1) + 1]));
                    }
                    this.UpdateForm();
                    break;
                case Keys.D4:
                    pointCount = chiky.mListCount;
                    for (int i = 0; i < pointCount; ++i)
                    {
                        chiky.Add(chiky.mList[2 * (pointCount - i - 1)], (short)(100 - chiky.mList[2 * (pointCount - i - 1) + 1]));
                    }
                    this.UpdateForm();
                    break;
                case Keys.D5:
                    pointCount = chiky.mListCount;
                    for (int i = 0; i < pointCount; ++i)
                    {
                        short t = chiky.mList[2 * (pointCount - i - 1)];
                        chiky.mList[2 * (pointCount - i - 1)] = chiky.mList[2 * (pointCount - i - 1) + 1];
                        chiky.mList[2 * (pointCount - i - 1) + 1] = t;
                    }
                    this.UpdateForm();
                    break;
            }
        }

        private void Form_Resize(object sender, EventArgs e)
        {
            Options.CameraX = this.ClientSize.Width / 2 - Options.CameraWidth / 2;
            Options.CameraY = this.ClientSize.Height / 2 - Options.CameraHeight / 2;

            this.UpdateForm();
        }

        private bool isMouseDown = false;
        private void MoveTestForm_MouseDown(object sender, MouseEventArgs e)
        {
            bool isFind = false;
            for (int i = 0; i < this.entityList.Count && !isFind; i++)
            {
                EntityBezier entity = this.entityList[i];
                for (int j = 0; j < entity.mListCount && !isFind; j++)
                {
                    float x = entity.getFloatAtListX(j) - e.X;
                    float y = entity.getFloatAtListY(j) - e.Y;
                    if (Math.Sqrt(x * x + y * y) < this.pointRadius)
                    {
                        isFind = true;
                        this.isMouseDown = true;
                        this.entIndex = i;
                        this.pointIndex = j;
                        this.UpdateForm();
                        this.timer.Stop();
                    }
                }
            }
        }

        private void MoveTestForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.isMouseDown)
            {
                EntityBezier entity = this.entityList[this.entIndex];

                entity.setFloatAtListX(pointIndex, e.X, e.Y);

                this.Invalidate();
                this.UpdateForm();
            }
        }

        private void MoveTestForm_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.isMouseDown)
            {
                EntityBezier entity = this.entityList[this.entIndex];

                float step = 100 / lineCount;
                entity.mList[2 * pointIndex] = (short)(Math.Round(entity.mList[2 * pointIndex] / step) * step);
                entity.mList[2 * pointIndex + 1] = (short)(Math.Round(entity.mList[2 * pointIndex + 1] / step) * step);

                this.UpdateForm();
                this.timer.Start();

                this.isMouseDown = false;
            }
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            this.UpdateData();
        }

        private void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            this.UpdateData();
        }

        private void buttonAddEntity_Click(object sender, EventArgs e)
        {
            Chiky entety = new Chiky() { mSpeedTime = 1 };
            entety.Add(50, 50);
            this.entityList.Insert(this.entIndex, entety);
            this.pointIndex = 0;
            this.UpdateForm();
        }

        private void buttonDelEntity_Click(object sender, EventArgs e)
        {
            if (this.entityList.Count > 1)
            {
                this.entityList.RemoveAt(this.entIndex);
                if (this.entIndex >= this.entityList.Count)
                {
                    this.entIndex = this.entityList.Count - 1;
                }
                this.pointIndex = 0;
                this.UpdateForm();
            }
        }

        private void buttonAddPoint_Click(object sender, EventArgs e)
        {
            this.entityList[this.entIndex].Insert(this.pointIndex, this.entityList[this.entIndex].mList[2 * this.pointIndex], this.entityList[this.entIndex].mList[2 * this.pointIndex + 1]);
            this.UpdateForm();
        }

        private void buttonDelPoint_Click(object sender, EventArgs e)
        {
            if (this.entityList[this.entIndex].mListCount > 1)
            {
                this.entityList[this.entIndex].RemoveAt(this.pointIndex);
                if (this.pointIndex >= this.entityList[this.entIndex].mListCount)
                {
                    this.pointIndex = this.entityList[this.entIndex].mListCount - 1;
                }
                this.UpdateForm();
            }
        }

        private void buttonStartStopTimer_Click(object sender, EventArgs e)
        {
            this.timer.Enabled = !this.timer.Enabled;
            if (this.timer.Enabled)
            {
                this.buttonStartStopTimer.Text = "Stop timer";
            }
            else
            {
                this.buttonStartStopTimer.Text = "Start timer";
            }
        }

        // Example:
        // <?xml version="1.0" encoding="UTF-8"?>
        // <level r1="0" g1="138" b1="255" r2="0" g2="255" b2="255" bluebird="false">
        // <chiky name="tooflya.com" scale="0.5" minTime="0.1" maxTime="1.1" speedTime="0.5" offsetTime="0.3" isRTime="true" normalMaxTime="1" normalSpeedTime="0.5" unnormalMaxTime="0.5" unnormalSpeedTime="1" properties="3">
        // <ctrPoint x="10" y="50"/>
        // <ctrPoint x="90" y="50"/>
        // </chiky>
        // </level>
        private void buttonLoad_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                entityList.Clear();
                using (StreamReader sr = new StreamReader(this.openFileDialog.FileName))
                {
                    Chiky entity = null;
                    while (!sr.EndOfStream)
                    {
                        try
                        {
                            string entity_string = sr.ReadLine();
                            Dictionary<string, string> entity_dictionary = new Dictionary<string, string>();
                            int index;

                            entity_string = entity_string.Trim();
                            index = entity_string.IndexOf(' ');
                            if (index < 0)
                            {
                                index = entity_string.IndexOf('>');
                            }
                            if (index > 0)
                            {
                                entity_dictionary.Add("type", entity_string.Substring(1, index - 1));
                                entity_string = entity_string.Remove(0, index + 1);

                                while (entity_string.Length > 2)
                                {
                                    entity_string = entity_string.TrimStart();
                                    index = entity_string.IndexOf('=');
                                    string key = entity_string.Substring(0, index);
                                    entity_string = entity_string.Remove(0, index + 1);
                                    entity_string = entity_string.Remove(0, entity_string.IndexOf('"') + 1);
                                    index = entity_string.IndexOf('"');
                                    string value = entity_string.Substring(0, index);
                                    entity_string = entity_string.Remove(0, index + 1);
                                    entity_dictionary.Add(key, value);
                                    entity_string = entity_string.TrimStart();
                                }
                            }

                            switch (entity_dictionary["type"])
                            {
                                case "chiky":
                                    entity = new Chiky();

                                    float scale = float.Parse(entity_dictionary["scale"]);
                                    entity.setWidth(scale * entity.getBaseWidth());
                                    entity.setHeight(scale * entity.getBaseWidth());

                                    entity.mMinTime = float.Parse(entity_dictionary["minTime"]);
                                    entity.mMaxTime = float.Parse(entity_dictionary["maxTime"]);
                                    entity.mSpeedTime = float.Parse(entity_dictionary["speedTime"]);
                                    entity.mOffsetTime = float.Parse(entity_dictionary["offsetTime"]);
                                    entity.mIsReverseTime = bool.Parse(entity_dictionary["isRTime"]);
                                    entity.mNormalMaxTime = float.Parse(entity_dictionary["normalMaxTime"]);
                                    entity.mNormalSpeedTime = float.Parse(entity_dictionary["normalSpeedTime"]);
                                    entity.mUnnormalMaxTime = float.Parse(entity_dictionary["unnormalMaxTime"]);
                                    entity.mUnnormalSpeedTime = float.Parse(entity_dictionary["unnormalSpeedTime"]);
                                    entity.mProperties = int.Parse(entity_dictionary["properties"]);
                                    break;
                                case "ctrPoint":
                                    short x = short.Parse(entity_dictionary["x"]);
                                    short y = short.Parse(entity_dictionary["y"]);
                                    if (entity != null)
                                    {
                                        entity.Add(x, y);
                                    }
                                    break;
                                case "/chiky":
                                    entity.reset();
                                    this.entityList.Add(entity);
                                    break;
                            }
                        }
                        catch { }
                    }
                }
                if (this.entityList.Count < 1)
                {
                    this.entityList.Add(new Chiky());
                    this.entityList[0].Add(50, 50);
                }
                this.UpdateForm();
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (this.saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                using (StreamWriter sw = new StreamWriter(this.saveFileDialog.FileName))
                {
                    sw.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
                    sw.WriteLine("<level r1=\"255\" g1=\"255\" b1=\"255\" r2=\"255\" g2=\"255\" b2=\"255\" bluebird=\"true\">");
                    foreach (Chiky entity in this.entityList)
                    {
                        sw.WriteLine("<chiky scale=\"{0}\" minTime=\"{1}\" maxTime=\"{2}\" speedTime=\"{3}\" offsetTime=\"{4}\" isRTime=\"{5}\" normalMaxTime=\"{6}\" normalSpeedTime=\"{7}\" unnormalMaxTime=\"{8}\" unnormalSpeedTime=\"{9}\" properties=\"{10}\">",
                            entity.getWidth() / entity.getBaseWidth(),
                            entity.mMinTime,
                            entity.mMaxTime,
                            Math.Abs(entity.mSpeedTime),
                            entity.mOffsetTime,
                            entity.mIsReverseTime,
                            entity.mNormalMaxTime,
                            entity.mNormalSpeedTime,
                            entity.mUnnormalMaxTime,
                            entity.mUnnormalSpeedTime,
                            entity.mProperties);
                        for (int i = 0; i < entity.mListCount; i++)
                        {
                            sw.WriteLine("<ctrPoint x=\"{0}\" y=\"{1}\"/>", entity.mList[2 * i], entity.mList[2 * i + 1]);
                        }
                        sw.WriteLine("</chiky>");
                    }
                    sw.WriteLine("</level>");
                }
            }
        }

        private void trackBar_Scroll(object sender, EventArgs e)
        {
            EntityBezier.mKoefSpeedTime = 0.001f * this.trackBar.Value;
        }
    }
}
