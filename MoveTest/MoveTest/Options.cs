namespace MoveTest
{
    class Options
    {
        public static float mX = 0;
        public static float mY = 0;

        private static float cameraWidth = 380;
        public static float CameraWidth
        {
            get
            {
                return cameraWidth;
            }
            set
            {
                cameraWidth = value;
            }
        }

        private static float cameraHeight = 610;
        private static float menuHeight = cameraHeight / 10;
        private static float touchHeight = cameraHeight / 3;
        public static float CameraHeight
        {
            get
            {
                return cameraHeight;
            }
            set
            {
                cameraHeight = value;
                menuHeight = cameraHeight / 10;
                touchHeight = cameraHeight / 3;
            }
        }
        public static float MenuHeight
        {
            get
            {
                return menuHeight;
            }
        }
        public static float TouchHeight
        {
            get
            {
                return touchHeight;
            }
        }
    }
}
