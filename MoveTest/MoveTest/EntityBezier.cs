using System.Collections.Generic;
using System.Drawing;

namespace MoveTest
{
    class EntityBezier
    {
        #region Position.

        private float mX;
        private float mY;
        private float mWidth = 50;
        private float mHeight = 50;

        public float getX()
        {
            return this.mX;
        }
        public float getY()
        {
            return this.mY;
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
        public float getWidth()
        {
            return this.mWidth;
        }
        public float getHeight()
        {
            return this.mHeight;
        }

        #endregion Position.

        #region Time.

        private static readonly byte mListCapacity = 10;
        private static readonly float[] mListX = new float[mListCapacity];
        private static readonly float[] mListY = new float[mListCapacity];

        private float mTime = 0;

        public float mMinTime = 0;
        public float mMaxTime = 1;
        public float mSpeedTime = 0;
        public float mOffsetTime = 0;
        public bool mIsReverseTime = true;

        public static float mKoefSpeedTime = 1;

        public byte[] mListBX = new byte[mListCapacity];
        public byte[] mListBY = new byte[mListCapacity];
        public byte mListCount = 0;

        #region Not for java code.

        public float getAtListX(int index)
        {
            return Options.mX + this.mListBX[index] / 100f * Options.mWidth;
        }
        public float getAtListY(int index)
        {
            return Options.mY + this.mListBY[index] / 100f * Options.mHeight;
        }
        public void setAtListX(int index, float pX, float pY)
        {
            this.mListBX[index] = (byte)((pX - Options.mX) / Options.mWidth * 100);
            this.mListBY[index] = (byte)((pY - Options.mY) / Options.mHeight * 100);
        }

        public void RemoveAt(int index)
        {
            if (this.mListCount > 0)
            {
                for (int i = index; i < this.mListCount - 1; i++)
                {
                    this.mListBX[i] = this.mListBX[i + 1];
                    this.mListBY[i] = this.mListBY[i + 1];
                }
                this.mListCount--;
            }
        }
        public void Add(byte x, byte y)
        {
            if (this.mListCount < mListCapacity)
            {
                this.mListBX[this.mListCount] = x;
                this.mListBY[this.mListCount] = y;
                this.mListCount++;
            }
        }
        public void Insert(int index, byte x, byte y)
        {
            if (this.mListCount == 0)
            {
                this.Add(x, y);
            }
            else if (this.mListCount < mListCapacity)
            {
                for (int i = mListCount; i > index; i--)
                {
                    this.mListBX[i] = this.mListBX[i - 1];
                    this.mListBY[i] = this.mListBY[i - 1];
                }
                this.mListBX[index] = x;
                this.mListBY[index] = y;
                this.mListCount++;
            }
        }

        #endregion Not for java code.

        #endregion Time.

        #region Dots (path) (temp).

        private const int dotsCount = 100;
        public readonly PointF[] dots = new PointF[dotsCount];

        public void calculateDots()
        {
            float tempT = this.mTime;
            this.mTime = this.mMinTime;
            float tempTimeStep = (this.mMaxTime - this.mMinTime) / dots.Length;
            for (int i = 0; i < dots.Length; i++)
            {
                this.update(tempTimeStep / this.mSpeedTime / EntityBezier.mKoefSpeedTime);
                this.dots[i].X = this.getCenterX();
                this.dots[i].Y = this.getCenterY();
            }
            this.mTime = tempT;
        }

        #endregion Dots (path) (temp).

        private void updatePoints()
        {
            for (int i = 0; i < this.mListCount; i++)
            {
                mListX[i] = this.mListBX[i];
                mListY[i] = this.mListBY[i];
            }
            for (int j = 1; j < this.mListCount; j++)
            {
                int count = this.mListCount - j;
                for (int i = 0; i < count; i++)
                {
                    mListX[i] = mListX[i] + (mListX[i + 1] - mListX[i]) * this.mTime;
                    mListY[i] = mListY[i] + (mListY[i + 1] - mListY[i]) * this.mTime;
                }
            }
            this.setCenterPosition(mListX[0] / 100 * Options.mWidth, mListY[0] / 100 * Options.mHeight);
        }

        public void reset()
        {
            this.calculateDots();
            this.mTime = this.mOffsetTime;
            this.update(0);
        }

        public void update(float pdT)
        {
            this.mTime += this.mSpeedTime * pdT * mKoefSpeedTime;

            #region Time correction.
            if (this.mTime < this.mMinTime)
            {
                if (this.mIsReverseTime)
                {
                    this.mTime = 2 * this.mMinTime - this.mTime;
                    if (this.mSpeedTime < 0)
                    {
                        this.mSpeedTime = -this.mSpeedTime;
                    }
                }
                else
                {
                    this.mTime += this.mMaxTime - this.mMinTime;
                }
            }
            else if (this.mMaxTime < this.mTime)
            {
                if (this.mIsReverseTime)
                {
                    this.mTime = 2 * this.mMaxTime - this.mTime;
                    if (0 < this.mSpeedTime)
                    {
                        this.mSpeedTime = -this.mSpeedTime;
                    }
                }
                else
                {
                    this.mTime -= this.mMaxTime - this.mMinTime;
                }
            }
            #endregion Time correction.

            this.updatePoints();
        }
    }
}
