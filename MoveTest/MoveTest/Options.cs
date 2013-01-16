namespace MoveTest
{
    class Options
    {
        private static float cameraX = 0;
        public static float CameraX
        {
            get
            {
                return cameraX;
            }
            set
            {
                cameraX = value;
            }
        }
        private static float cameraY = 0;
        public static float CameraY
        {
            get
            {
                return cameraY;
            }
            set
            {
                cameraY = value;
            }
        }

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
        private static float menuHeight = cameraHeight / 10;
        public static float MenuHeight
        {
            get
            {
                return menuHeight;
            }
        }
        private static float touchHeight = cameraHeight / 3;
        public static float TouchHeight
        {
            get
            {
                return touchHeight;
            }
        }
    }
}
