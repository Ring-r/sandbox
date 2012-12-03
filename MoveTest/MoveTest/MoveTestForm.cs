﻿using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;

namespace MoveTest
{
    public partial class MoveTestForm : Form
    {
        #region Данные визуализации.

        private readonly int dotRadius = 2;
        private readonly int pointRadius = 5;

        private readonly int lineCount = 20;

        #endregion Данные визуализации.

        private readonly List<EntityB> entityList = new List<EntityB>();

        private int entIndex = 0;
        private int pointIndex = 0;

        private readonly Stopwatch stopwatch = new Stopwatch();

        public MoveTestForm()
        {
            InitializeComponent();

            this.entityList.Add(new EntityB() { mSpeedT = 1 });

            this.entityList[this.entIndex].mListX.Add(50);
            this.entityList[this.entIndex].mListY.Add(50);

            Options.mWidth = 400;
            Options.mHeight = 600;

            this.stopwatch.Start();
        }

        private void UpdateData()
        {
            if (!this.isUpdateForm)
            {
                try
                {
                    Options.mWidth = float.Parse(this.textBoxWidth.Text);
                    Options.mHeight = float.Parse(this.textBoxHeight.Text);
                    Options.mX = this.ClientSize.Width / 2 - Options.mWidth / 2;
                    Options.mY = this.ClientSize.Height / 2 - Options.mHeight / 2;

                    this.entityList[this.entIndex].mMinT = float.Parse(this.textBoxMinT.Text);
                    this.entityList[this.entIndex].mMaxT = float.Parse(this.textBoxMaxT.Text);
                    this.entityList[this.entIndex].mOffsetT = float.Parse(this.textBoxOffsetT.Text);
                    this.entityList[this.entIndex].mSpeedT = float.Parse(this.textBoxSpeedT.Text);
                    this.entityList[this.entIndex].mIsReverseT = this.checkBoxIsReverse.Checked;

                    this.entityList[this.entIndex].mListX[this.pointIndex] = float.Parse(this.textBoxXP.Text);
                    this.entityList[this.entIndex].mListY[this.pointIndex] = float.Parse(this.textBoxYP.Text);

                    this.UpdateForm();
                }
                catch { }
            }
        }

        private bool isUpdateForm = false;
        private void UpdateForm()
        {
            this.isUpdateForm = true;

            this.textBoxWidth.Text = Options.mWidth.ToString();
            this.textBoxHeight.Text = Options.mHeight.ToString();

            this.textBoxMinT.Text = this.entityList[this.entIndex].mMinT.ToString();
            this.textBoxMaxT.Text = this.entityList[this.entIndex].mMaxT.ToString();
            this.textBoxOffsetT.Text = this.entityList[this.entIndex].mOffsetT.ToString();
            this.textBoxSpeedT.Text = Math.Abs(this.entityList[this.entIndex].mSpeedT).ToString();
            this.checkBoxIsReverse.Checked = this.entityList[this.entIndex].mIsReverseT;

            this.labelPointsCount.Text = string.Format("Points count: {0}", this.entityList[this.entIndex].mListX.Count);
            this.textBoxXP.Text = this.entityList[this.entIndex].mListX[this.pointIndex].ToString();
            this.textBoxYP.Text = this.entityList[this.entIndex].mListY[this.pointIndex].ToString();

            this.isUpdateForm = false;

            foreach (EntityB entity in this.entityList)
            {
                entity.reset();
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            foreach (EntityB entity in this.entityList)
            {
                entity.update(this.stopwatch.ElapsedMilliseconds / 1000f);
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

            linesStep = Options.mWidth / lineCount;
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

            linesStep = Options.mHeight / lineCount;
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

            e.Graphics.DrawRectangle(Pens.Gray, Options.mX, Options.mY, Options.mWidth, Options.mHeight);

            #region Draw dots.
            if (this.checkBoxIsShowDots.Checked)
            {
                foreach (EntityB entity in this.entityList)
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
                foreach (EntityB entity in this.entityList)
                {
                    for (int j = 0; j < entity.mListX.Count; j++)
                    {
                        float x = entity.getAtListX(j);
                        float y = entity.getAtListY(j);
                        e.Graphics.FillEllipse(Brushes.Silver, x - this.pointRadius, y - this.pointRadius, 2 * this.pointRadius, 2 * this.pointRadius);
                        e.Graphics.DrawString(j.ToString(), this.Font, Brushes.Black, x + this.pointRadius, y + this.pointRadius);
                    }
                }
            }
            #endregion Draw points.

            #region Draw entity.
            foreach (EntityB entity in this.entityList)
            {
                e.Graphics.FillEllipse(Brushes.Red, Options.mX + entity.getX(), Options.mY + entity.getY(), entity.getWidth(), entity.getHeight());
            }
            #endregion Draw entity.

            if (this.checkBoxIsShowPoints.Checked)
            {
                EntityB entity = this.entityList[this.entIndex];

                e.Graphics.FillEllipse(Brushes.Green, entity.getAtListX(this.pointIndex) - this.pointRadius, entity.getAtListY(this.pointIndex) - this.pointRadius, 2 * this.pointRadius, 2 * this.pointRadius);
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
            Options.mX = this.ClientSize.Width / 2 - Options.mWidth / 2;
            Options.mY = this.ClientSize.Height / 2 - Options.mHeight / 2;

            this.UpdateForm();
        }

        private bool isMouseDown = false;
        private void MoveTestForm_MouseDown(object sender, MouseEventArgs e)
        {
            bool isFind = false;
            for (int i = 0; i < this.entityList.Count && !isFind; i++)
            {
                EntityB entity = this.entityList[i];
                for (int j = 0; j < entity.mListX.Count && !isFind; j++)
                {
                    float x = entity.getAtListX(j) - e.X;
                    float y = entity.getAtListY(j) - e.Y;
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
                EntityB entity = this.entityList[this.entIndex];

                entity.setAtListX(pointIndex, e.X, e.Y);

                this.Invalidate();
                this.UpdateForm();
            }
        }

        private void MoveTestForm_MouseUp(object sender, MouseEventArgs e)
        {
            if (this.isMouseDown)
            {
                EntityB entity = this.entityList[this.entIndex];

                float step = 100 / lineCount;
                entity.mListX[pointIndex] = (float)Math.Round(entity.mListX[pointIndex] / step) * step;
                entity.mListY[pointIndex] = (float)Math.Round(entity.mListY[pointIndex] / step) * step;

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
            EntityB entety = new EntityB() { mSpeedT = 1 };
            entety.mListX.Add(50);
            entety.mListY.Add(50);
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
            this.entityList[this.entIndex].mListX.Insert(this.pointIndex, this.entityList[this.entIndex].mListX[this.pointIndex]);
            this.entityList[this.entIndex].mListY.Insert(this.pointIndex, this.entityList[this.entIndex].mListY[this.pointIndex]);
            this.UpdateForm();
        }

        private void buttonDelPoint_Click(object sender, EventArgs e)
        {
            if (this.entityList[this.entIndex].mListX.Count > 1)
            {
                this.entityList[this.entIndex].mListX.RemoveAt(this.pointIndex);
                this.entityList[this.entIndex].mListY.RemoveAt(this.pointIndex);
                if (this.pointIndex >= this.entityList[this.entIndex].mListX.Count)
                {
                    this.pointIndex = this.entityList[this.entIndex].mListX.Count - 1;
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
                entityList.Clear();
                using (StreamReader sr = new StreamReader(this.openFileDialog.FileName))
                {
                    while (!sr.EndOfStream)
                    {
                        try
                        {
                            string entity_string = sr.ReadLine();
                            int index;
                            EntityB entity = new EntityB();
                            index = entity_string.IndexOf('"');
                            entity_string = entity_string.Remove(0, index + 1);
                            index = entity_string.IndexOf('"');

                            entity.mMinT = float.Parse(entity_string.Substring(0, index));
                            entity_string = entity_string.Remove(0, index + 1);
                            index = entity_string.IndexOf('"');
                            entity_string = entity_string.Remove(0, index + 1);
                            index = entity_string.IndexOf('"');

                            entity.mMaxT = float.Parse(entity_string.Substring(0, index));
                            entity_string = entity_string.Remove(0, index + 1);
                            index = entity_string.IndexOf('"');
                            entity_string = entity_string.Remove(0, index + 1);
                            index = entity_string.IndexOf('"');

                            entity.mOffsetT = float.Parse(entity_string.Substring(0, index));
                            entity_string = entity_string.Remove(0, index + 1);
                            index = entity_string.IndexOf('"');
                            entity_string = entity_string.Remove(0, index + 1);
                            index = entity_string.IndexOf('"');

                            entity.mSpeedT = float.Parse(entity_string.Substring(0, index));
                            entity_string = entity_string.Remove(0, index + 1);
                            index = entity_string.IndexOf('"');
                            entity_string = entity_string.Remove(0, index + 1);
                            index = entity_string.IndexOf('"');

                            entity.mIsReverseT = bool.Parse(entity_string.Substring(0, index));
                            entity_string = entity_string.Remove(0, index + 1);
                            index = entity_string.IndexOf('"');
                            entity_string = entity_string.Remove(0, index + 1);
                            index = entity_string.IndexOf('"');

                            int pointCount = int.Parse(entity_string.Substring(0, index));
                            entity_string = entity_string.Remove(0, index + 1);
                            index = entity_string.IndexOf('"');
                            entity_string = entity_string.Remove(0, index + 1);
                            index = entity_string.IndexOf('"');

                            for (int i = 0; i < pointCount; i++)
                            {
                                float x = float.Parse(entity_string.Substring(0, index));
                                entity_string = entity_string.Remove(0, index + 1);
                                index = entity_string.IndexOf('"');
                                entity_string = entity_string.Remove(0, index + 1);
                                index = entity_string.IndexOf('"');
                                entity.mListX.Add(x);

                                float y = float.Parse(entity_string.Substring(0, index));
                                entity_string = entity_string.Remove(0, index + 1);
                                index = entity_string.IndexOf('"');
                                entity_string = entity_string.Remove(0, index + 1);
                                index = entity_string.IndexOf('"');
                                entity.mListY.Add(y);
                            }

                            entity.reset();
                            this.entityList.Add(entity);
                        }
                        catch { }
                    }
                }
                if (this.entityList.Count < 1)
                {
                    this.entityList.Add(new EntityB());
                    this.entityList[0].mListX.Add(50);
                    this.entityList[0].mListY.Add(50);
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
                    foreach (EntityB entity in this.entityList)
                    {
                        sw.Write("<Entity MinT=\"{0}\", MaxT=\"{1}\", OffsetT=\"{2}\", SpeedT=\"{3}\", IsReturnT=\"{4}\", PointCount=\"{5}\"",
                            entity.mMinT,
                            entity.mMaxT,
                            entity.mOffsetT,
                            Math.Abs(entity.mSpeedT),
                            entity.mIsReverseT,
                            entity.mListX.Count);
                        for (int i = 0; i < entity.mListX.Count; i++)
                        {
                            sw.Write(", Point{0}X=\"{1}\", Point{0}Y=\"{2}\"", i, entity.mListX[i], entity.mListY[i]);
                        }
                        sw.WriteLine("\\>");
                    }
                }
            }
        }

        private void trackBar_Scroll(object sender, EventArgs e)
        {
            EntityB.mKoefSpeedT = 0.001f * this.trackBar.Value;
        }
    }
}
