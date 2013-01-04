using System;

namespace MoveTest
{
    class Chiky : EntityBezier
    {
        private enum States { NormalMove, UnnormalMove };

        public static readonly int isPauseUpdateFlag = 1;
        public static readonly int isUnnormalMoveFlag = 2;

        public int mProperties = 0;

        private States mState = States.NormalMove;

        #region NormalMove state.
        private float mNormalTime = 0; // Seconds.
        public float mNormalMaxTime = float.MaxValue; // Seconds.
        public float mNormalSpeedTime = 0; // Seconds.
        #endregion NormalMove state.

        #region UnnormalMove state.
        private float mUnnormalTime = 0; // Seconds.
        public float mUnnormalMaxTime = 0; // Seconds.
        public float mUnnormalSpeedTime = 0; // Seconds.
        #endregion UnnormalMove state.

        public void initScale(float scale)
        {
            this.mWidth = Options.mSize * scale;
            this.mHeight = Options.mSize * scale;
        }

        private bool IsProperty(int flag)
        {
            return (this.mProperties & flag) == flag;
        }

        private void onManagedUpdateNormalMove(float pSecondsElapsed)
        {
            this.mNormalTime += pSecondsElapsed * mKoefSpeedTime;
            if (this.mNormalTime > this.mNormalMaxTime)
            {
                if (this.IsProperty(isUnnormalMoveFlag))
                {
                    this.mState = States.UnnormalMove;
                    this.mUnnormalTime = this.mNormalTime - this.mNormalMaxTime;
                    this.mSpeedTime = this.mSpeedTime == 0 ? this.mUnnormalSpeedTime : Math.Sign(this.mSpeedTime) * this.mUnnormalSpeedTime;
                }
            }
        }

        private void onManagedUpdateUnnormalMove(float pSecondsElapsed)
        {
            this.mUnnormalTime += pSecondsElapsed * mKoefSpeedTime;
            if (this.mUnnormalTime > this.mUnnormalMaxTime)
            {
                this.mState = States.NormalMove;
                this.mNormalTime = this.mUnnormalTime - this.mUnnormalMaxTime;
                this.mSpeedTime = this.mSpeedTime == 0 ? this.mNormalSpeedTime : Math.Sign(this.mSpeedTime) * this.mNormalSpeedTime;
            }
        }

        public void onCreate()
        {
            this.init();

            this.mProperties = 0;
            this.mState = States.NormalMove;

            // > NormalMove state.
            this.mNormalTime = 0; // Seconds.
            this.mNormalMaxTime = float.MaxValue; // Seconds.
            this.mNormalSpeedTime = 0; // Seconds.
            // < NormalMove state.

            // > UnnormalMove state.
            this.mUnnormalTime = 0; // Seconds.
            this.mUnnormalMaxTime = 0; // Seconds.
            this.mUnnormalSpeedTime = 0; // Seconds.
            // < UnnormalMove state.
        }

        public override void reset()
        {
            base.reset();
            this.mNormalTime = 0; // Seconds.
            this.mUnnormalTime = 0; // Seconds.
        }

        public override void onManagedUpdate(float pSecondsElapsed)
        {
            if (!this.IsProperty(isPauseUpdateFlag))
            {
                base.onManagedUpdate(pSecondsElapsed);
                switch (this.mState)
                {
                    case States.NormalMove:
                        this.onManagedUpdateNormalMove(pSecondsElapsed);
                        break;
                    case States.UnnormalMove:
                        this.onManagedUpdateUnnormalMove(pSecondsElapsed);
                        break;
                }
            }
        }
    }
}
