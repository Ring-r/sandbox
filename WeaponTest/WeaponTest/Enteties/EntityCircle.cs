using System.Drawing;

namespace Entities
{
    class EntityCircle : Entity, IEntity
    {
        public float X
        {
            get
            {
                return this.mCenterX - this.mRadius;
            }
            set
            {
                this.mCenterX = value + this.mRadius;
            }
        }

        public float Y
        {
            get
            {
                return this.mCenterY - this.mRadius;
            }
            set
            {
                this.mCenterY = value + this.mRadius;
            }
        }

        public float LifeTime { get; set; }

        public float Health { get; set; }

        public void setPosition(float pX, float pY)
        {
            this.mCenterX = pX + this.mRadius;
            this.mCenterY = pY + this.mRadius;
        }

        private float mCenterX;

        public float CenterX
        {
            get
            {
                return this.mCenterX;
            }
            set
            {
                this.mCenterX = value;
            }
        }

        private float mCenterY;

        public float CenterY
        {
            get
            {
                return this.mCenterY;
            }
            set
            {
                this.mCenterY = value;
            }
        }

        public void setCenterPosition(float pCenterX, float pCenterY)
        {
            this.mCenterX = pCenterX;
            this.mCenterY = pCenterY;
        }

        private float mBaseRadius;

        public float BaseRadius
        {
            get
            {
                return this.mBaseRadius;
            }
            set
            {
                this.mBaseRadius = value;
            }
        }

        public float BaseSize
        {
            get
            {
                return 2 * this.mBaseRadius;
            }
            set
            {
                this.mBaseRadius = 0.5f * value;
            }
        }

        public float BaseWidth
        {
            get
            {
                return 2 * this.mBaseRadius;
            }
            set
            {
                this.mBaseRadius = 0.5f * value;
            }
        }

        public float BaseHeight
        {
            get
            {
                return 2 * this.mBaseRadius;
            }
            set
            {
                this.mBaseRadius = 0.5f * value;
            }
        }

        private float mRadius;

        public float Radius
        {
            get
            {
                return this.mRadius;
            }
            set
            {
                this.mRadius = value;
            }
        }

        public float Size
        {
            get
            {
                return 2 * this.mRadius;
            }
            set
            {
                this.mRadius = 0.5f * value;
            }
        }

        public float Width
        {
            get
            {
                return 2 * this.mRadius;
            }
            set
            {
                this.mRadius = 0.5f * value;
            }
        }

        public float Height
        {
            get
            {
                return 2 * this.mRadius;
            }
            set
            {
                this.mRadius = 0.5f * value;
            }
        }

        public override void onManagedDraw(Graphics graphics)
        {
            graphics.FillEllipse(this.Brush, this.X, this.Y, this.Size, this.Size);
            graphics.DrawEllipse(this.Pen, this.X, this.Y, this.Size, this.Size);
        }

        public override void onManagedUpdate(float pSecondsElapsed)
        {
        }
    }
}
