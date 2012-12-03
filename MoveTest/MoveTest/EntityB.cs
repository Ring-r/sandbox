using System.Collections.Generic;
using System.Drawing;

namespace MoveTest
{
    class EntityB
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

        private float mT = 0;

        public float mMinT = 0;
        public float mMaxT = 1;
        public float mSpeedT = 0;
        public float mOffsetT = 0;
        public bool mIsReverseT = true;

        public static float mKoefSpeedT = 1;

        #endregion Time.

        #region PointsB.

        public List<float> mListX = new List<float>();
        public List<float> mListY = new List<float>();

        private void updatePoints()
        {
            if (this.mListX.Count > 0)
            {
                float[] mXsB = this.mListX.ToArray();
                float[] mYsB = this.mListY.ToArray();
                for (int j = 1; j < mXsB.Length; j++)
                {
                    int count = mXsB.Length - j;
                    for (int i = 0; i < count; i++)
                    {
                        mXsB[i] = mXsB[i] + (mXsB[i + 1] - mXsB[i]) * this.mT;
                        mYsB[i] = mYsB[i] + (mYsB[i + 1] - mYsB[i]) * this.mT;
                    }
                }
                this.setCenterPosition(mXsB[0] / 100 * Options.mWidth, mYsB[0] / 100 * Options.mHeight);
            }
        }

        #region Not for java code.

        public float getAtListX(int index)
        {
            return Options.mX + this.mListX[index] / 100 * Options.mWidth;
        }
        public float getAtListY(int index)
        {
            return Options.mY + this.mListY[index] / 100 * Options.mHeight;
        }
        public void setAtListX(int index, float pX, float pY)
        {
            this.mListX[index] = (pX - Options.mX) / Options.mWidth * 100;
            this.mListY[index] = (pY - Options.mY) / Options.mHeight * 100;
        }

        #endregion Not for java code.

        #endregion PointsB.

        #region Dots (path) (temp).

        private const int dotsCount = 100;
        public readonly PointF[] dots = new PointF[dotsCount];

        public void calculateDots()
        {
            float tempT = this.mT;
            this.mT = this.mMinT;
            float tempTimeStep = (this.mMaxT - this.mMinT) / dots.Length;
            for (int i = 0; i < dots.Length; i++)
            {
                this.update(tempTimeStep / this.mSpeedT / EntityB.mKoefSpeedT);
                this.dots[i].X = this.getCenterX();
                this.dots[i].Y = this.getCenterY();
            }
            this.mT = tempT;
        }

        #endregion Dots (path) (temp).

        public void reset()
        {
            this.calculateDots();
            this.mT = this.mOffsetT;
            this.update(0);
        }

        public void update(float pdT)
        {
            this.mT += this.mSpeedT * pdT * mKoefSpeedT;

            #region Time correction.
            if (this.mT < this.mMinT)
            {
                if (this.mIsReverseT)
                {
                    this.mT = 2 * this.mMinT - this.mT;
                    if (this.mSpeedT < 0)
                    {
                        this.mSpeedT = -this.mSpeedT;
                    }
                }
                else
                {
                    this.mT += this.mMaxT - this.mMinT;
                }
            }
            else if (this.mMaxT < this.mT)
            {
                if (this.mIsReverseT)
                {
                    this.mT = 2 * this.mMaxT - this.mT;
                    if (0 < this.mSpeedT)
                    {
                        this.mSpeedT = -this.mSpeedT;
                    }
                }
                else
                {
                    this.mT -= this.mMaxT - this.mMinT;
                }
            }
            #endregion Time correction.

            this.updatePoints();
        }
    }
}
