﻿using System;
using System.Drawing;
using Entities;
using Pools;

namespace WeaponTest
{
    class Bullet : EntityRectangle, IPoolable
    {
        public int FragmetsCount = 0;
        public float FragmetsPart = 0.5f;

        #region IPoolable implementation
        public IPoolable DeepCopy()
        {
            throw new System.NotImplementedException();
        }

        public void Initialize()
        {
            throw new System.NotImplementedException();
        }

        public IPool Parent { get; set; }

        public int Id
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }
        #endregion

        public override void onManagedDraw(Graphics graphics)
        {
            graphics.TranslateTransform(this.CenterX, this.CenterY);
            graphics.RotateTransform(90 + this.Angle / (float)Math.PI * 180);
            graphics.TranslateTransform(-this.CenterX, -this.CenterY);
            base.onManagedDraw(graphics);
            graphics.ResetTransform();
        }

        public override void onManagedUpdate(float secondsElapsed)
        {
            this.CenterX += this.VectorX * this.Speed * secondsElapsed;
            this.CenterY += this.VectorY * this.Speed * secondsElapsed;

            this.LifeTime -= secondsElapsed;

            if (this.LifeTime <= 0 || this.Health <= 0)
            {
                for (int i = 0; i < this.FragmetsCount; ++i)
                {
                    Bullet bullet = new Bullet()
                    {
                        Parent = this.Parent,
                        CenterX = this.CenterX,
                        CenterY = this.CenterY,
                        Width = this.Width * this.FragmetsPart,
                        Height = this.Height * this.FragmetsPart,
                        Angle = (float)(2 * Math.PI * Options.Random.NextDouble()),
                        Speed = this.Speed / this.FragmetsPart,
                        LifeTime = this.LifeTime,
                        Health = this.Health * this.FragmetsPart,
                    };
                    (this.Parent as Weapon).bullets.Add(bullet);
                }
            }

            if (this.LifeTime <= 0 || this.Health <= 0 || this.Y > Options.CameraHeight || this.Y + this.Height < 0)
            {
                (this.Parent as Weapon).bullets.Remove(this);
            }
        }
    }
}
