using System;
using Entities;
using System.Collections.Generic;
using System.Drawing;

namespace WeaponTest
{
    class Evil
    {
        private float secondsElapsed = 0;
        internal readonly List<IEntity> enemies = new List<IEntity>();

        private int shotCount = 0;
        public int ShotCount = 10;


        public float RechargeTime = 5f; // time (seconds) for one recharge;

        public float ShotTime = 1f; // time (seconds) for one shot;
        public float BulletsWidth = 100f;
        public float BulletsHeight = 100f;
        public float BulletsSpeed = 50f; // points in second;

        public void onManagedDraw(Graphics graphics)
        {
            for (int i = 0; i < this.enemies.Count; ++i)
            {
                this.enemies[i].onManagedDraw(graphics);
            }
        }

        public void onManagedUpdate(float secondsElapsed)
        {
            for (int i = 0; i < this.enemies.Count; ++i)
            {
                this.enemies[i].onManagedUpdate(secondsElapsed);
            }

            this.secondsElapsed += secondsElapsed;

            if (this.shotCount >= this.ShotCount)
            {
                if (this.secondsElapsed > this.RechargeTime)
                {
                    this.secondsElapsed -= this.RechargeTime;
                    this.shotCount = 0;
                }
            }

            while (this.secondsElapsed >= this.ShotTime && this.shotCount < this.ShotCount)
            {
                this.secondsElapsed -= this.ShotTime;
                float x = Options.Random.Next(Options.CameraWidth);
                float y = 0;
                float _x = Options.Random.Next(Options.CameraWidth);
                float _y = Options.CameraHeight;

                Enemy enemy = new Enemy()
                {
                    CenterX = x,
                    CenterY = y,
                    Width = this.BulletsWidth,
                    Height = this.BulletsHeight,
                    Angle = (float)Math.Atan2(_y - y, _x - x),
                    Speed = this.BulletsSpeed,
                    Health = (float)Options.Random.NextDouble()*10,
                };
                enemy.onManagedUpdate(this.secondsElapsed);
                this.enemies.Add(enemy);
                ++this.shotCount;
            }


            for (int i = 0; i < this.enemies.Count; ++i)
            {
                if (this.enemies[i].Y > Options.CameraHeight || this.enemies[i].Y + this.enemies[i].Height < 0)
                {
                    this.enemies.RemoveAt(i);
                    --i;
                }

            }
        }

    }
}

