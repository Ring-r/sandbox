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
        #region Данные визуализации.

        private readonly int dotRadius = 2;
        private readonly int pointRadius = 5;

        private readonly int lineCount = 20;

        #endregion Данные визуализации.

        private readonly List<Chiky> chikyList = new List<Chiky>();

        private int entIndex = 0;
        private int pointIndex = 0;

        private readonly Stopwatch stopwatch = new Stopwatch();

        public MoveTestForm()
        {
            InitializeComponent();

            this.chikyList.Add(new Chiky() { mSpeedTime = 1 });

            this.chikyList[this.entIndex].Add(50, 50);

            Options.cameraWidth = 400;
            Options.cameraHeight = 600;

            this.stopwatch.Start();
        }

        private void UpdateData()
        {
            if (!this.isUpdateForm)
            {
                try
                {
                    Options.cameraWidth = float.Parse(this.textBoxWidth.Text);
                    Options.cameraHeight = float.Parse(this.textBoxHeight.Text);
                    Options.mX = this.ClientSize.Width / 2 - Options.cameraWidth / 2;
                    Options.mY = this.ClientSize.Height / 2 - Options.cameraHeight / 2;

                    this.chikyList[this.entIndex].mMinTime = float.Parse(this.textBoxMinT.Text);
                    this.chikyList[this.entIndex].mMaxTime = float.Parse(this.textBoxMaxT.Text);
                    this.chikyList[this.entIndex].mOffsetTime = float.Parse(this.textBoxOffsetT.Text);
                    this.chikyList[this.entIndex].mSpeedTime = float.Parse(this.textBoxSpeedT.Text);
                    this.chikyList[this.entIndex].mIsReverseTime = this.checkBoxIsReverse.Checked;

                    this.chikyList[this.entIndex].mNormalMaxTime = float.Parse(this.textBoxNormalMaxTime.Text);
                    this.chikyList[this.entIndex].mNormalSpeedTime = float.Parse(this.textBoxNormalSpeedTime.Text);
                    this.chikyList[this.entIndex].mUnnormalMaxTime = float.Parse(this.textBoxUnnormalMaxTime.Text);
                    this.chikyList[this.entIndex].mUnnormalSpeedTime = float.Parse(this.textBoxUnnormalSpeedTime.Text);
                    this.chikyList[this.entIndex].mProperties = int.Parse(this.textBoxProperties.Text);

                    this.chikyList[this.entIndex].mList[2 * this.pointIndex] = short.Parse(this.textBoxXP.Text);
                    this.chikyList[this.entIndex].mList[2 * this.pointIndex + 1] = short.Parse(this.textBoxYP.Text);

                    this.UpdateForm();
                }
                catch { }
            }
        }

        private bool isUpdateForm = false;
        private void UpdateForm()
        {
            this.isUpdateForm = true;

            this.textBoxWidth.Text = Options.cameraWidth.ToString();
            this.textBoxHeight.Text = Options.cameraHeight.ToString();

            this.textBoxMinT.Text = this.chikyList[this.entIndex].mMinTime.ToString();
            this.textBoxMaxT.Text = this.chikyList[this.entIndex].mMaxTime.ToString();
            this.textBoxOffsetT.Text = this.chikyList[this.entIndex].mOffsetTime.ToString();
            this.textBoxSpeedT.Text = Math.Abs(this.chikyList[this.entIndex].mSpeedTime).ToString();
            this.checkBoxIsReverse.Checked = this.chikyList[this.entIndex].mIsReverseTime;

            this.textBoxNormalMaxTime.Text = this.chikyList[this.entIndex].mNormalMaxTime.ToString();
            this.textBoxNormalSpeedTime.Text = this.chikyList[this.entIndex].mNormalSpeedTime.ToString();
            this.textBoxUnnormalMaxTime.Text = this.chikyList[this.entIndex].mUnnormalMaxTime.ToString();
            this.textBoxUnnormalSpeedTime.Text = this.chikyList[this.entIndex].mUnnormalSpeedTime.ToString();
            this.textBoxProperties.Text = this.chikyList[this.entIndex].mProperties.ToString();

            this.labelPointsCount.Text = string.Format("Points count: {0}", this.chikyList[this.entIndex].mListCount);
            this.textBoxXP.Text = this.chikyList[this.entIndex].mList[2 * this.pointIndex].ToString();
            this.textBoxYP.Text = this.chikyList[this.entIndex].mList[2 * this.pointIndex + 1].ToString();

            this.isUpdateForm = false;

            foreach (Chiky entity in this.chikyList)
            {
                entity.reset();
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            foreach (EntityBezier entity in this.chikyList)
            {
                entity.onManagedUpdate(this.stopwatch.ElapsedMilliseconds / 1000f);
            }
            this.Invalidate();
            this.stopwatch.Restart();
        }

        private void Form_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            #region Draw lines.

            float linesStep;
            float linesBegin;

            linesStep = Options.cameraWidth / lineCount;
            linesBegin = Options.mX;
            while (0 < linesBegin)
            {
                linesBegin -= linesStep;
                e.Graphics.DrawLine(Pens.Silver, linesBegin, 0, linesBegin, this.ClientSize.Height);
            }
            linesBegin = Options.mX;
            while (linesBegin < this.ClientSize.Width)
            {
                e.Graphics.DrawLine(Pens.Silver, linesBegin, 0, linesBegin, this.ClientSize.Height);
                linesBegin += linesStep;
            }

            linesStep = Options.cameraHeight / lineCount;
            linesBegin = Options.mY;
            while (0 < linesBegin)
            {
                linesBegin -= linesStep;
                e.Graphics.DrawLine(Pens.Silver, 0, linesBegin, this.ClientSize.Width, linesBegin);
            }
            linesBegin = Options.mY;
            while (linesBegin < this.ClientSize.Width)
            {
                e.Graphics.DrawLine(Pens.Silver, 0, linesBegin, this.ClientSize.Width, linesBegin);
                linesBegin += linesStep;
            }
            #endregion Draw lines.

            e.Graphics.DrawRectangle(Pens.Gray, Options.mX, Options.mY, Options.cameraWidth, Options.cameraHeight);

            #region Draw dots.
            if (this.checkBoxIsShowDots.Checked)
            {
                foreach (EntityBezier entity in this.chikyList)
                {
                    for (int j = 0; j < entity.dots.Length; j++)
                    {
                        e.Graphics.FillEllipse(Brushes.Silver, Options.mX + entity.dots[j].X - this.dotRadius, Options.mY + entity.dots[j].Y - this.dotRadius, 2 * this.dotRadius, 2 * this.dotRadius);
                    }
                }
            }
            #endregion Draw dots.

            #region Draw points.
            if (this.checkBoxIsShowPoints.Checked)
            {
                foreach (EntityBezier entity in this.chikyList)
                {
                    for (int j = 0; j < entity.mListCount; j++)
                    {
                        float x = entity.getFloatAtListX(j);
                        float y = entity.getFloatAtListY(j);
                        e.Graphics.FillEllipse(Brushes.Silver, x - this.pointRadius, y - this.pointRadius, 2 * this.pointRadius, 2 * this.pointRadius);
                        e.Graphics.DrawString(j.ToString(), this.Font, Brushes.Black, x + this.pointRadius, y + this.pointRadius);
                    }
                }
            }
            #endregion Draw points.

            #region Draw entity.
            foreach (EntityBezier entity in this.chikyList)
            {
                e.Graphics.FillEllipse(Brushes.Red, Options.mX + entity.getX(), Options.mY + entity.getY(), entity.getWidth(), entity.getHeight());
            }
            #endregion Draw entity.

            if (this.checkBoxIsShowPoints.Checked)
            {
                EntityBezier entity = this.chikyList[this.entIndex];

                e.Graphics.FillEllipse(Brushes.Green, entity.getFloatAtListX(this.pointIndex) - this.pointRadius, entity.getFloatAtListY(this.pointIndex) - this.pointRadius, 2 * this.pointRadius, 2 * this.pointRadius);
            }
        }

        private void Form_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void Form_Resize(object sender, EventArgs e)
        {
            Options.mX = this.ClientSize.Width / 2 - Options.cameraWidth / 2;
            Options.mY = this.ClientSize.Height / 2 - Options.cameraHeight / 2;

            this.UpdateForm();
        }

        private bool isMouseDown = false;
        private void MoveTestForm_MouseDown(object sender, MouseEventArgs e)
        {
            bool isFind = false;
            for (int i = 0; i < this.chikyList.Count && !isFind; i++)
            {
                EntityBezier entity = this.chikyList[i];
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
                EntityBezier entity = this.chikyList[this.entIndex];

                entity.setFloatAtListX(pointIndex, e.X, e.Y);

                this.Invalidate();
                this.UpdateForm();
            }
        }

        private void MoveTestForm_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.isMouseDown)
            {
                EntityBezier entity = this.chikyList[this.entIndex];

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
            this.chikyList.Insert(this.entIndex, entety);
            this.pointIndex = 0;
            this.UpdateForm();
        }

        private void buttonDelEntity_Click(object sender, EventArgs e)
        {
            if (this.chikyList.Count > 1)
            {
                this.chikyList.RemoveAt(this.entIndex);
                if (this.entIndex >= this.chikyList.Count)
                {
                    this.entIndex = this.chikyList.Count - 1;
                }
                this.pointIndex = 0;
                this.UpdateForm();
            }
        }

        private void buttonAddPoint_Click(object sender, EventArgs e)
        {
            this.chikyList[this.entIndex].Insert(this.pointIndex, this.chikyList[this.entIndex].mList[2 * this.pointIndex], this.chikyList[this.entIndex].mList[2 * this.pointIndex + 1]);
            this.UpdateForm();
        }

        private void buttonDelPoint_Click(object sender, EventArgs e)
        {
            if (this.chikyList[this.entIndex].mListCount > 1)
            {
                this.chikyList[this.entIndex].RemoveAt(this.pointIndex);
                if (this.pointIndex >= this.chikyList[this.entIndex].mListCount)
                {
                    this.pointIndex = this.chikyList[this.entIndex].mListCount - 1;
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

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                chikyList.Clear();
                using (StreamReader sr = new StreamReader(this.openFileDialog.FileName))
                {
                    Chiky entity = null;
                    while (!sr.EndOfStream)
                    {
                        try
                        {
                            // Example:
                            // <chiky scale="0.5" minTime="0.1" maxTime="1.1" speedTime="0.5" offsetTime="0.3" isRTime="true" normalMaxTime="1" normalSpeedTime="0.5" unnormalMaxTime="0.5" unnormalSpeedTime="1" properties="3">
                            // <ctrPoint x="10" y="50"/>
                            // <ctrPoint x="90" y="50"/>
                            // </chiky>
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

                                    // TODO: entity.mScale = float.Parse(entity_dictionary["scale"]);
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
                                    this.chikyList.Add(entity);
                                    break;
                            }
                        }
                        catch { }
                    }
                }
                if (this.chikyList.Count < 1)
                {
                    this.chikyList.Add(new Chiky());
                    this.chikyList[0].Add(50, 50);
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
                    foreach (Chiky entity in this.chikyList)
                    {
                        // Example:
                        // <chiky name="tooflya.com" scale="0.5" minTime="0.1" maxTime="1.1" speedTime="0.5" offsetTime="0.3" isRTime="true" normalMaxTime="1" normalSpeedTime="0.5" unnormalMaxTime="0.5" unnormalSpeedTime="1" properties="3">
                        // <ctrPoint x="10" y="50"/>
                        // <ctrPoint x="90" y="50"/>
                        // </chiky>
                        sw.WriteLine("<chiky name=\"{0}\" scale=\"{1}\" minTime=\"{2}\" maxTime=\"{3}\" speedTime=\"{4}\" offsetTime=\"{5}\" isRTime=\"{6}\" normalMaxTime=\"{7}\" normalSpeedTime=\"{8}\" unnormalMaxTime=\"{9}\" unnormalSpeedTime=\"{10}\" properties=\"{11}\" PointCount=\"{12}\">",
                            "",
                            1,
                            entity.mMinTime,
                            entity.mMaxTime,
                            Math.Abs(entity.mSpeedTime),
                            entity.mOffsetTime,
                            entity.mIsReverseTime,
                            entity.mNormalMaxTime,
                            entity.mNormalSpeedTime,
                            entity.mUnnormalMaxTime,
                            entity.mUnnormalSpeedTime,
                            entity.mProperties,
                            entity.mListCount);
                        for (int i = 0; i < entity.mListCount; i++)
                        {
                            sw.WriteLine("<ctrPoint x=\"{0}\" y=\"{1}\"/>", entity.mList[2 * i], entity.mList[2 * i + 1]);
                        }
                        sw.WriteLine("</chiky>");
                    }
                }
            }
        }

        private void trackBar_Scroll(object sender, EventArgs e)
        {
            EntityBezier.mKoefSpeedTime = 0.001f * this.trackBar.Value;
        }
    }
}
