using System;
using System.Drawing;
using Entities;

namespace WeaponTest
{
    class Ball : EntityRectangle
    {
        public override void onManagedDraw(Graphics graphics)
        {
            graphics.TranslateTransform(this.CenterX, this.CenterY);
            graphics.RotateTransform(this.Angle / (float)Math.PI * 180 + 90);
            graphics.TranslateTransform(-this.CenterX, -this.CenterY);
            base.onManagedDraw(graphics);
            graphics.ResetTransform();
        }

        public override void onManagedUpdate(float pSecondsElapsed)
        {
            this.CenterX += this.VectorX * this.Speed * pSecondsElapsed;
            this.CenterY += this.VectorY * this.Speed * pSecondsElapsed;
        }
    }
}
