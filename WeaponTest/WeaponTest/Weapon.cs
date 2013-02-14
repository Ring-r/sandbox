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

        private float ballsWidth = 5;
        private float ballsHeight = 5;
        private float ballsSpeed = 10; // points in second;
        private float shootSpeed = 1; // count in second;

        public void onManagedDraw(Graphics graphics)
        {
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
            while (this.mSecondsElapsed > this.shootSpeed)
            {
                Ball ball = new Ball() { CenterX = Options.CameraWidth / 2, CenterY = 0, Width = this.ballsWidth, Height = this.ballsHeight, Speed = this.ballsSpeed };
                ball.Angle = (float)Math.Atan2(Options.CursorY - ball.CenterY, Options.CursorX - ball.CenterX);
                this.mSecondsElapsed -= this.shootSpeed;
                ball.onManagedUpdate(this.mSecondsElapsed);
                this.balls.Add(ball);
            }
        }
    }
}
