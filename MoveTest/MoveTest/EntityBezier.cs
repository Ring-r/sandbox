using System.Drawing;

namespace MoveTest
{
    class EntityBezier : Entity
    {
        #region Time.

        private static readonly byte mListCapacity = 10;
        private static readonly float[] mTempList = new float[2 * mListCapacity];

        public static float mKoefSpeedTime = 1;

        private float mTime = 0;

        public float mMinTime = 0;
        public float mMaxTime = 1;
        public float mSpeedTime = 0;
        public float mOffsetTime = 0;
        public bool mIsReverseTime = true;

        public short[] mList = new short[2 * mListCapacity];
        public byte mListCount = 0;

        #region Not for java code.

        public float getFloatAtListX(int index)
        {
            return Options.mX + this.mList[2 * index] / 100f * Options.cameraWidth;
        }
        public float getFloatAtListY(int index)
        {
            return Options.mY + this.mList[2 * index + 1] / 100f * Options.cameraHeight;
        }
        public void setFloatAtListX(int index, float pX, float pY)
        {
            this.mList[2 * index] = (short)((pX - Options.mX) / Options.cameraWidth * 100);
            this.mList[2 * index + 1] = (short)((pY - Options.mY) / Options.cameraHeight * 100);
        }

        public void RemoveAt(int index)
        {
            if (this.mListCount > 0)
            {
                for (int i = index; i < this.mListCount - 1; i++)
                {
                    this.mList[2 * i] = this.mList[2 * (i + 1)];
                    this.mList[2 * i + 1] = this.mList[2 * (i + 1) + 1];
                }
                this.mListCount--;
            }
        }
        public void Add(short x, short y)
        {
            if (this.mListCount < mListCapacity)
            {
                this.mList[2 * this.mListCount] = x;
                this.mList[2 * this.mListCount + 1] = y;
                this.mListCount++;
            }
        }
        public void Insert(int index, short x, short y)
        {
            if (this.mListCount == 0)
            {
                this.Add(x, y);
            }
            else if (this.mListCount < mListCapacity)
            {
                for (int i = mListCount; i > index; i--)
                {
                    this.mList[2 * i] = this.mList[2 * (i - 1)];
                    this.mList[2 * i + 1] = this.mList[2 * (i - 1) + 1];
                }
                this.mList[2 * index] = x;
                this.mList[2 * index + 1] = y;
                this.mListCount++;
            }
        }

        #endregion Not for java code.

        public void init()
        {
            this.mTime = 0;

            this.mMinTime = 0;
            this.mMaxTime = 1;
            this.mSpeedTime = 0;
            this.mOffsetTime = 0;
            this.mIsReverseTime = true;
            this.mListCount = 0;
        }

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
                this.onManagedUpdate(tempTimeStep / this.mSpeedTime / EntityBezier.mKoefSpeedTime);
                this.dots[i].X = this.getCenterX();
                this.dots[i].Y = this.getCenterY();
            }
            this.mTime = tempT;
        }

        #endregion Dots (path) (temp).

        private void updatePoints()
        {
            int count = this.mListCount << 1;
            for (int i = 0; i < count; i++)
            {
                mTempList[i] = this.mList[i];
            }
            for (int j = 1; j < this.mListCount; j++)
            {
                count = (this.mListCount - j) << 1;
                for (int i = 0; i < count; i++)
                {
                    mTempList[i] = mTempList[i] + (mTempList[i + 2] - mTempList[i]) * this.mTime;
                }
            }
            this.setCenterPosition(
                this.mWidth / 2 + mTempList[0] * (Options.cameraWidth - this.mWidth) / 100,
                this.mHeight / 2 + mTempList[1] * (Options.cameraHeight - this.mHeight) / 100);
        }

        public virtual void reset()
        {
            this.calculateDots();
            this.mTime = this.mOffsetTime;
            this.onManagedUpdate(0);
        }

        public virtual void onManagedUpdate(float pSecondsElapsed)
        {
            this.mTime += this.mSpeedTime * pSecondsElapsed * mKoefSpeedTime;

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
