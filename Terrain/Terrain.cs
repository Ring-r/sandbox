﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Walk {
	static class Program {
		[STAThread]
		static void Main() {
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}

		public static readonly Random rand = new Random();
	}

	public class MainForm : Form {
		private System.ComponentModel.IContainer components = new System.ComponentModel.Container();

		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		public MainForm() {
			this.SuspendLayout();
			this.DoubleBuffered = true;
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Terrain Prototype";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.KeyUp += this.MainForm_KeyUp;
			this.Resize += this.MainForm_Resize;
			this.Paint += this.MainForm_Paint;
			this.ResumeLayout(false);

			this.CreateLevel();
		}

		private void MainForm_KeyUp(object sender, KeyEventArgs e) {
			if (e.KeyCode == Keys.Escape) {
				this.Close();
			}
			if (e.KeyCode == Keys.Enter) {
				this.CreateLevel();
				this.Invalidate();
			}
		}

		private void MainForm_Resize(object sender, EventArgs e) {
			this.Invalidate();
		}

		private const int count = 256;
		private const float height = 0.5f;

		private float[] perlinNoise = null;

		private void CreateLevel() {
			this.perlinNoise = this.CreatePerlinNoise(count, height);
		}

		private float[] CreatePerlinNoise(int count, float height) {
			float[] res = new float[count];

			int step = count / 2;
			do {
				float heightBegin = (float)Program.rand.NextDouble() * height;
				for (int i = 0; i < count; i += step) {
					float heightEnd = (float)Program.rand.NextDouble() * height;
					float heightStep = (heightEnd - heightBegin) / step;
					for (int j = 0; j < step; j += 1) {
						res[i + j] += heightBegin + heightStep * j;
					}
					heightBegin = heightEnd;
				}
				step /= 2;
				height /= 2;
			} while (step > 1);
			return res;
		}

		private void MainForm_Paint(object sender, PaintEventArgs e) {
			e.Graphics.Clear(Color.White);
			if (this.perlinNoise == null) {
				return;
			}

			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

			float xKoef = (float)this.ClientSize.Width / count;
			float yKoef = (float)this.ClientSize.Height / 2 / height;

			PointF[] points = new PointF[this.perlinNoise.Length];
			for (int i = 0; i < points.Length; ++i) {
				points[i] = new PointF(i * xKoef, this.ClientSize.Height - this.perlinNoise[i] * yKoef);
			}
			e.Graphics.DrawLines(Pens.Green, points);
		}
	}
}