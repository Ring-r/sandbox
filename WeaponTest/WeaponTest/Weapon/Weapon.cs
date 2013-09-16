using System;
using System.Collections.Generic;
using System.Drawing;
using Entities;
using Pools;

namespace WeaponTest
{
    class Weapon : IPool
    {
        private float secondsElapsed = 0;
        internal readonly List<IEntity> bullets = new List<IEntity>(); // TODO: Use private.
        //
        private DateTime startTime;
        public int AimCursorX = 0;
        public int AimCursorY = 0;
        public int ShotCursorX = 0;
        public int ShotCursorY = 0;
        //
        public float Angle = 0;
        public bool AutoShots = false;
        //
        public float BulletsWidth = 5f;
        public float BulletsHeight = 5f;
        public float BulletsSpeed = 500f; // Points in second.
        public float BulletsHealth = 1f;
        public float BulletsLifeTime = 1f; // Seconds.
        //
        public int FragmetsCount = 3;
        public float FragmetsPart = 0.5f;
        //
        public float ShotTime = 0.1f; // Time (seconds) for prepare bullet to shot.
        private int shotCount = 0;
        public int ShotCount = 10;
        //
        public float RechargeTime = 3f; // Time (seconds) for one recharge.

        public void onManagedDraw(Graphics graphics)
        {
            float angle;
            if (this.shotCount < this.ShotCount)
            {
                angle = this.secondsElapsed >= this.ShotTime ? 360 : 360 * this.secondsElapsed / this.ShotTime;
                graphics.FillPie(Brushes.Silver, Options.CameraWidth / 2 - 100, 0, 200, 200, 0, angle);
            }
            else
            {
                if (this.RechargeTime != 0)
                {
                    angle = 360 * this.secondsElapsed / this.RechargeTime;
                    graphics.FillPie(Brushes.Black, Options.CameraWidth / 2 - 100, 0, 200, 200, 0, angle);
                }
            }

            graphics.DrawEllipse(Pens.Black, Options.CameraWidth / 2 - 100, 0, 200, 200);

            for (int i = 0; i < this.bullets.Count; ++i)
            {
                this.bullets[i].onManagedDraw(graphics);
            }
        }

        public void onManagedUpdate(float secondsElapsed)
        {
            for (int i = 0; i < this.bullets.Count; ++i)
            {
                this.bullets[i].onManagedUpdate(secondsElapsed);
            }

            this.secondsElapsed += secondsElapsed;

            bool isExitAutoShots;
            do
            {
                isExitAutoShots = true;

                if (this.shotCount >= this.ShotCount && this.secondsElapsed >= this.RechargeTime)
                {
                    this.secondsElapsed -= this.RechargeTime;
                    this.shotCount = 0;
                    isExitAutoShots = false;
                }

                if (this.AutoShots)
                {
                    while (this.shotCount < this.ShotCount && this.secondsElapsed >= this.ShotTime)
                    {
                        this.secondsElapsed -= this.ShotTime;
                        Bullet bullet = new Bullet()
                        {
                            Parent = this,
                            CenterX = this.AimCursorX,
                            CenterY = this.AimCursorY,
                            Width = this.BulletsWidth,
                            Height = this.BulletsHeight,
                            Angle = (float)Math.Atan2(this.ShotCursorY - this.AimCursorY, this.ShotCursorX - this.AimCursorX) + this.Angle,
                            Speed = this.BulletsSpeed,
                            LifeTime = this.BulletsLifeTime,
                            Health = this.BulletsHealth,
                            FragmetsCount = this.FragmetsCount,
                            FragmetsPart = this.FragmetsPart,
                        };
                        this.bullets.Add(bullet);
                        bullet.onManagedUpdate(this.secondsElapsed);
                        ++this.shotCount;
                        isExitAutoShots = false;
                    }
                }
            } while (!this.AutoShots && !isExitAutoShots);

            this.secondsElapsed = Math.Min(this.secondsElapsed, this.ShotTime);
        }

        public void Aim()
        {
            this.startTime = DateTime.Now;
        }

        public void Shot()
        {
            if (this.shotCount < this.ShotCount && this.secondsElapsed >= this.ShotTime)
            {
                float x = this.ShotCursorX - this.AimCursorX;
                float y = this.ShotCursorY - this.AimCursorY;
                float dist = (float)Math.Sqrt(x * x + y * y);
                Bullet bullet = new Bullet()
                    {
                        Parent = this,
                        CenterX = this.AimCursorX,
                        CenterY = this.AimCursorY,
                        Width = this.BulletsWidth,
                        Height = this.BulletsHeight,
                        Angle = (float)Math.Atan2(this.ShotCursorY - this.AimCursorY, this.ShotCursorX - this.AimCursorX) + this.Angle,
                        Speed = Math.Min(this.BulletsSpeed, dist / (float)(DateTime.Now - this.startTime).TotalSeconds),
                        LifeTime = this.BulletsLifeTime,
                        Health = this.BulletsHealth,
                        FragmetsCount = this.FragmetsCount,
                        FragmetsPart = this.FragmetsPart,
                    };
                this.bullets.Add(bullet);
                this.secondsElapsed = 0;
            }
        }
    }
}
