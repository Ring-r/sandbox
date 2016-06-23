﻿using System;
using System.Drawing;

namespace MetroPenguinTest
{
	class Penguin
	{
		public float x = 0;
		public float y = 0;

		public int r = Options.minR;

		public float vx = 0;
		public float vy = 0;

		public float s = 0;

		public bool IsCollided { get; set; }

		public void Init(Size borders)
		{
			this.r = Options.Random.Next(Options.minR, Options.maxR);
			this.x = Options.RandomFloat(r, borders.Width - this.r);
			this.y = Options.RandomFloat(r, borders.Height - this.r);
			this.vx = 0;
			this.vy = 0;
			this.s = Options.maxS;
		}

		public void InitRandomDirection()
		{
			float a = (float)(2 * Math.PI * Options.Random.NextDouble());
			//float a = Options.Random.Next(2) != 0 ? (float)Math.PI : 0.0f;
			this.vx = (float)Math.Cos(a);
			this.vy = (float)Math.Sin(a);
			this.s = Options.RandomFloat(Options.minS, Options.maxS);
		}

		public void Move(float timeEllapsed)
		{
			var step = this.s * timeEllapsed;
			this.x += this.vx * step;
			this.y += this.vy * step;
		}

		public void CheckCollision(Penguin penguin)
		{
			float vx = this.x - penguin.x;
			float vy = this.y - penguin.y;
			float d = (float)Math.Sqrt(vx * vx + vy * vy);
			float r = this.r + penguin.r;

			if (Options.eps < d && d < r - Options.eps)
			{
				float coef = 0.5f * (r / d - 1);

				vx *= coef;
				vy *= coef;

				this.x += vx;
				this.y += vy;
				penguin.x -= vx;
				penguin.y -= vy;

				this.IsCollided = true;
				penguin.IsCollided = true;
			}
		}

		public void ReturnInBorders(Size borders)
		{
			if (this.x < this.r)
			{
				this.x = this.r;
				this.vx = Math.Abs(this.vx);
			}
			if (this.x > borders.Width - this.r)
			{
				this.x = borders.Width - this.r;
				this.vx = -Math.Abs(this.vx);
			}

			if (this.y < this.r)
			{
				this.y = this.r;
				this.vy = Math.Abs(this.vy);
			}
			if (this.y > borders.Height - this.r)
			{
				this.y = borders.Height - this.r;
				this.vy = -Math.Abs(this.vy);
			}
		}

		public void Draw(Graphics graphics, Brush brush, Pen pen)
		{
			graphics.FillEllipse(brush, this.x - this.r, this.y - this.r, 2 * this.r, 2 * this.r);
			graphics.DrawLine(pen, this.x, this.y, this.x + this.r * this.vx, this.y + this.r * this.vy);
		}

	}
}
