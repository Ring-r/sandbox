using System.Drawing;

namespace Entities
{
    class EntityRectangle : Entity
    {
        private float mX;
        public float X
        {
            get
            {
                return this.mX;
            }
            set
            {
                this.mX = value;
            }
        }
        private float mY;
        public float Y
        {
            get
            {
                return this.mY;
            }
            set
            {
                this.mY = value;
            }
        }
        public void setPosition(float pX, float pY)
        {
            this.mX = pX;
            this.mY = pY;
        }

        public float CenterX
        {
            get
            {
                return this.mX + this.mWidth / 2;
            }
            set
            {
                this.mX = value - this.mWidth / 2;
            }
        }
        public float CenterY
        {
            get
            {
                return this.mY + this.mHeight / 2;
            }
            set
            {
                this.mY = value - this.mHeight / 2;
            }
        }
        public void setCenterPosition(float pCenterX, float pCenterY)
        {
            this.mX = pCenterX - this.mWidth / 2;
            this.mY = pCenterY - this.mHeight / 2;
        }

        private float mBaseWidth;
        public float BaseWidth
        {
            get
            {
                return this.mBaseWidth;
            }
            set
            {
                this.mBaseWidth = value;
            }
        }
        private float mBaseHeight;
        public float BaseHeight
        {
            get
            {
                return this.mBaseHeight;
            }
            set
            {
                this.mBaseHeight = value;
            }
        }
        public void setBaseSize(float pBaseWidth, float pBaseHeight)
        {
            this.mBaseWidth = pBaseWidth;
            this.mBaseHeight = pBaseHeight;
        }

        private float mWidth;
        public float Width
        {
            get
            {
                return this.mWidth;
            }
            set
            {
                this.mWidth = value;
            }
        }
        private float mHeight;
        public float Height
        {
            get
            {
                return this.mHeight;
            }
            set
            {
                this.mHeight = value;
            }
        }
        public void setSize(float pWidth, float pHeight)
        {
            this.mWidth = pWidth;
            this.mHeight = pHeight;
        }

        public override void onManagedDraw(Graphics graphics)
        {
            graphics.FillRectangle(this.Brush, this.X, this.Y, this.Width, this.Height);
            graphics.DrawRectangle(this.Pen, this.X, this.Y, this.Width, this.Height);
        }

        public override void onManagedUpdate(float pSecondsElapsed)
        {
        }
    }
}
