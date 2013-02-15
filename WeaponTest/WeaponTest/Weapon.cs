using System;
using System.Collections.Generic;
using System.Drawing;
using Entities;

namespace WeaponTest
{
    class Weapon
    {
        private float mSecondsElapsed = 0;
        private readonly List<IEntity> balls = new List<IEntity>();

        public float BallsWidth = 5f;
        public float BallsHeight = 5f;
        public float BallsSpeed = 500f; // points in second;
        public float ShootTime = 1f; // time (seconds) for one shoot;
        public int ShootCount = 0;
        public int ShootCountMax = 5;
        public float RechargeTime = 2f;
        public bool ShootAuto = false;
        public float Angle = 0;

        private int startCursorX = 0;
        private int startCursorY = 0;
        private DateTime startTime;
        private int cursorY = 0;
        private int cursorX = 0;


        public void onManagedDraw(Graphics graphics)
        {
            float angle;
            if (this.ShootCount < this.ShootCountMax)
            {
                angle = this.mSecondsElapsed >= this.ShootTime ? 360 : 360 * this.mSecondsElapsed / this.ShootTime;
                graphics.FillPie(Brushes.Silver, Options.CameraWidth / 2 - 100, 0, 200, 200, 0, angle);
            }
            else
            {
                angle = 360 * this.mSecondsElapsed / this.RechargeTime;
                graphics.FillPie(Brushes.Black, Options.CameraWidth / 2 - 100, 0, 200, 200, 0, angle);
            }

            for (int i = 0; i < this.balls.Count; ++i)
            {
                this.balls[i].onManagedDraw(graphics);
            }
        }

        public void onManagedUpdate(float pSecondsElapsed)
        {
            for (int i = 0; i < this.balls.Count; ++i)
            {
                this.balls[i].onManagedUpdate(pSecondsElapsed);
            }

            this.mSecondsElapsed += pSecondsElapsed;

            if (this.ShootCount >= this.ShootCountMax)
            {
                if (this.mSecondsElapsed > this.RechargeTime)
                {
                    this.mSecondsElapsed -= this.RechargeTime;
                    this.ShootCount = 0;
                }
            }

            if (this.ShootAuto)
            {
                while (this.mSecondsElapsed >= this.ShootTime && this.ShootCount < this.ShootCountMax)
                {
                    Ball ball = new Ball()
                    {
                        CenterX = Options.CameraWidth / 2,
                        CenterY = Options.CameraHeight,
                        Width = this.BallsWidth,
                        Height = this.BallsHeight,
                        Angle = (float)Math.Atan2(this.cursorY - Options.CameraHeight, this.cursorX - Options.CameraWidth / 2) + this.Angle,
                        Speed = this.BallsSpeed,
                    };
                    ball.onManagedUpdate(this.mSecondsElapsed);
                    this.balls.Add(ball);
                    this.mSecondsElapsed -= this.ShootTime;
                    ++this.ShootCount;
                }
            }

            for (int i = 0; i < this.balls.Count; ++i)
            {
                if (this.balls[i].Y > Options.CameraHeight || this.balls[i].Y + this.balls[i].Height < 0)
                {
                    this.balls.RemoveAt(i);
                    --i;
                }
            }
        }

        public void Aim(int cursorX, int cursorY)
        {
            this.startCursorX = cursorX;
            this.startCursorY = cursorY;
            this.startTime = DateTime.Now;
        }

        public void Shoot(int cursorX, int cursorY)
        {
            this.cursorX = cursorX;
            this.cursorY = cursorY;
            if (this.mSecondsElapsed > this.ShootTime && this.ShootCount < this.ShootCountMax)
            {
                float x = this.cursorX - this.startCursorX;
                float y = this.cursorY - this.startCursorY;
                float dist = (float)Math.Sqrt(x * x + y * y);
                Ball ball = new Ball()
                {
                    CenterX = this.startCursorX,
                    CenterY = this.startCursorY,
                    Width = this.BallsWidth,
                    Height = this.BallsHeight,
                    Angle = (float)Math.Atan2(y, x) + this.Angle,
                    Speed = Math.Min(this.BallsSpeed, dist / (float)(DateTime.Now - this.startTime).TotalMilliseconds * 1000),
                };
                this.balls.Add(ball);
                this.mSecondsElapsed = 0;
                ++this.ShootCount;
            }
        }
    }
}
