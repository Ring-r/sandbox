using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Numbers
{
    public partial class MainForm : Form
    {
        private const int startEnergy = 100;
        private const int startInterval = 100;
        private const int startNumbersCount = 10;
        private const int startMaxNumber = 10;

        private Random random = new Random();
        private List<Number> numbers = new List<Number>();
        private List<NumbersPair> numbersPairs = new List<NumbersPair>();
        private int numberIndex = -1;
        private int numbersPairIndex = -1;

        private int maxNumber = startMaxNumber;

        private string helpStr = "Click to start!";


        private void RandomNumbersCreate(int count, int maxNumber = 10)
        {
            this.numbers.Clear();
            for (int i = 0; i < count; ++i)
            {
                int rx = random.Next(-5, 5);
                int ry = random.Next(-5, 5);
                Number number = new Number { Value = random.Next(maxNumber) };
                number.Size = TextRenderer.MeasureText(number.Value.ToString(), this.Font);
                int step = (this.ClientSize.Width + this.ClientSize.Height) / count;
                int x = i * step + random.Next(step);
                if (x < this.ClientSize.Height / 2)
                {
                    number.Position = new Point(0 + rx, this.ClientSize.Height / 2 + x + ry);
                }
                else if (x < this.ClientSize.Height / 2 + this.ClientSize.Width)
                {
                    number.Position = new Point(x - this.ClientSize.Height / 2 + rx, this.ClientSize.Height - number.Size.Height + ry + 5);
                }
                else
                {
                    number.Position = new Point(this.ClientSize.Width - number.Size.Width + rx + 5, this.ClientSize.Height - (x - this.ClientSize.Height / 2 - this.ClientSize.Width) + ry);
                }
                this.numbers.Add(number);
            }
        }

        private void RandomNumbersPairsCreate(int count, int maxNumber)
        {
            this.numbersPairs.Clear();
            for (int i = 0; i < count; ++i)
            {
                NumbersPair numbersPair = new NumbersPair { Value = random.Next(maxNumber), ValueNext = random.Next(maxNumber) };
                numbersPair.Size = TextRenderer.MeasureText(string.Format("{0} + {1}", numbersPair.Value, numbersPair.ValueNext), this.Font);
                numbersPair.Position = new PointF(random.Next(this.ClientSize.Width - numbersPair.Size.Width), random.Next(this.ClientSize.Height - numbersPair.Size.Height));
                numbersPair.Step = new PointF((float)(random.NextDouble() - 0.5), (float)(random.NextDouble() - 0.5));
                this.numbersPairs.Add(numbersPair);
            }
        }

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Escape:
                    this.Close();
                    break;
            }
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.White);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            if (this.timer.Enabled)
            {
                foreach (Number number in this.numbers)
                {
                    int size = Math.Max(number.Size.Width, number.Size.Height) + 5;
                    e.Graphics.FillEllipse(Brushes.Red, number.Position.X + number.Size.Width / 2 - size / 2 - 5, number.Position.Y + number.Size.Height / 2 - size / 2, size, size);
                    e.Graphics.DrawString(number.Value.ToString(), this.Font, Brushes.White, number.Position);
                }
                foreach (NumbersPair numbersPair in this.numbersPairs)
                {
                    e.Graphics.DrawString(string.Format("{0} + {1}", numbersPair.Value, numbersPair.ValueNext), this.Font, Brushes.Red, numbersPair.Position);
                }
            }
            else
            {
                Size heroSize = TextRenderer.MeasureText(this.helpStr, this.Font);
                e.Graphics.DrawString(this.helpStr, this.Font, Brushes.Red, new PointF((this.ClientSize.Width - heroSize.Width) / 2, (this.ClientSize.Height - heroSize.Height) / 2));
            }
        }

        private void Check()
        {
            while (true)
            {
                for (int i = 0; i < this.numbersPairs.Count; ++i)
                {
                    for (int j = 0; j < this.numbers.Count; ++j)
                    {
                        if (this.numbersPairs[i].Value + this.numbersPairs[i].ValueNext == this.numbers[j].Value)
                        {
                            this.Invalidate();
                            return;
                        }
                    }
                }
                this.RandomNumbersCreate(startNumbersCount, maxNumber);
                this.RandomNumbersPairsCreate(startNumbersCount, maxNumber);
            }
        }

        private void MainForm_MouseClick(object sender, MouseEventArgs e)
        {
            if (!this.timer.Enabled)
            {
                this.Start();
            }
        }

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            this.numbersPairIndex = -1;
            for (int i = 0; i < this.numbersPairs.Count && this.numbersPairIndex < 0; ++i)
            {
                if (this.numbersPairs[i].Position.X <= e.X && e.X < this.numbersPairs[i].Position.X + this.numbersPairs[i].Size.Width &&
                    this.numbersPairs[i].Position.Y <= e.Y && e.Y < this.numbersPairs[i].Position.Y + this.numbersPairs[i].Size.Height)
                {
                    this.numbersPairIndex = i;
                }
            }
        }

        private void MainForm_MouseUp(object sender, MouseEventArgs e)
        {
            this.numberIndex = -1;
            for (int i = 0; i < this.numbers.Count && this.numberIndex < 0; ++i)
            {
                if (this.numbers[i].Position.X <= e.X && e.X < this.numbers[i].Position.X + this.numbers[i].Size.Width &&
                    this.numbers[i].Position.Y <= e.Y && e.Y < this.numbers[i].Position.Y + this.numbers[i].Size.Height)
                {
                    this.numberIndex = i;
                }
            }
            if (this.numbersPairIndex >= 0 && this.numberIndex >= 0 &&
                this.numbersPairs[this.numbersPairIndex].Value + this.numbersPairs[this.numbersPairIndex].ValueNext == this.numbers[this.numberIndex].Value)
            {
                this.numbersPairs.RemoveAt(this.numbersPairIndex);
                this.numbers.RemoveAt(this.numberIndex);
                this.Invalidate();
                this.Check();
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            foreach (NumbersPair numbersPair in this.numbersPairs)
            {
                numbersPair.Position = new PointF(numbersPair.Position.X + numbersPair.Step.X, numbersPair.Position.Y + numbersPair.Step.Y);
            }
            this.Invalidate();
        }

        private void Start()
        {
            this.timer.Interval = startInterval;
            this.timer.Start();
            this.Check();
        }

        private void Finish()
        {
            this.timer.Stop();
            this.helpStr = "Loser!";
        }
    }
}
