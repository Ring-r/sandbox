using System.Drawing;

namespace MoveTest
{
    class Entity
    {
        private const int mSize = 32;

        protected float mX = 0;
        public float getX()
        {
            return this.mX;
        }
        public void setX(float pX)
        {
            this.mX = pX;
        }

        protected float mY = 0;
        public float getY()
        {
            return this.mY;
        }
        public void setY(float pY)
        {
            this.mY = pY;
        }

        public float getCenterX()
        {
            return this.mX + this.mWidth / 2;
        }
        public float getCenterY()
        {
            return this.mY + this.mHeight / 2;
        }
        public void setCenterPosition(float pX, float pY)
        {
            this.mX = pX - this.mWidth / 2;
            this.mY = pY - this.mHeight / 2;
        }

        protected float mBaseWidth = mSize;
        public float getBaseWidth()
        {
            return this.mBaseWidth;
        }
        protected float mWidth = mSize;
        public float getWidth()
        {
            return this.mWidth;
        }
        public void setWidth(float pWidth)
        {
            this.mWidth = pWidth;
        }

        protected float mBaseHeight = mSize;
        public float getBaseHeight()
        {
            return this.mBaseHeight;
        }
        protected float mHeight = mSize;
        public float getHeight()
        {
            return this.mHeight;
        }
        public void setHeight(float pHeight)
        {
            this.mHeight = pHeight;
        }

        protected float mScale = 1;
        public float getScaledWidth()
        {
            return this.mWidth * this.mScale;
        }
        public float getScaledHeight()
        {
            return this.mWidth * this.mScale;
        }

        public virtual void onManagedDraw(Graphics graphics)
        {
        }

        public virtual void onManagedUpdate(float pSecondsElapsed)
        {
        }
    }
}
