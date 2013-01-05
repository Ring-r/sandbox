namespace MoveTest
{
    class Entity
    {
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

        protected float mBaseWidth = Options.mSize;
        protected float mWidth = Options.mSize;
        public float getWidth()
        {
            return this.mWidth;
        }

        protected float mBaseHeight = Options.mSize;
        protected float mHeight = Options.mSize;
        public float getHeight()
        {
            return this.mHeight;
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

        public void init()
        {
            this.mX = 0;
            this.mY = 0;
            this.mBaseWidth = Options.mSize;
            this.mWidth = Options.mSize;
            this.mBaseHeight = Options.mSize;
            this.mHeight = Options.mSize;
            this.mScale = 1;
        }
    }
}
