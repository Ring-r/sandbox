using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace WeaponTest
{
    public partial class MainForm : Form
    {
        private readonly List<Weapon> weapons = new List<Weapon>();
        private readonly Stopwatch stopwatch = new Stopwatch();

        public MainForm()
        {
            InitializeComponent();

            this.weapons.Add(new Weapon() { Angle = -(float)Math.PI * 3 / 180 });
            this.weapons.Add(new Weapon() { Angle = 0 });
            this.weapons.Add(new Weapon() { Angle = +(float)Math.PI * 3 / 180 });
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            this.weapons.ForEach((weapon) => weapon.onManagedUpdate(this.stopwatch.ElapsedMilliseconds / 1000f));
            this.stopwatch.Restart();
            this.Invalidate();
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            this.weapons.ForEach((weapon) => weapon.onManagedDraw(e.Graphics));
        }

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            this.weapons.ForEach((weapon) => weapon.Aim(e.X, e.Y));
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            this.weapons.ForEach((weapon) => { if (weapon.ShootAuto) weapon.Shoot(e.X, e.Y); });
        }

        private void MainForm_MouseUp(object sender, MouseEventArgs e)
        {
            this.weapons.ForEach((weapon) => weapon.Shoot(e.X, e.Y));
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

        private void MainForm_Resize(object sender, EventArgs e)
        {
            Options.CameraWidth = this.ClientSize.Width;
            Options.CameraHeight = this.ClientSize.Height;
        }
    }
}
