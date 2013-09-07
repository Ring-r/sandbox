using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MoveInCells
{
	public class Controller
	{
		private const float moveStep = 10.0f;
		private const float rotateStep = (float)Math.PI / 180 * 10;

		private Entity entity = null;
        private readonly HashSet<Keys> keys = new HashSet<Keys>();

		private int isMove = 0;
        private int isRotate = 0;

        private bool isNeedToUpdate = false;

		public void SetEntity (Entity entity)
		{
			this.entity = entity;
		}

		public void AddKey(Keys key)
		{
			this.keys.Add (key);
			this.isNeedToUpdate = true;
		}

		public void RemoveKey(Keys key)
		{
            this.keys.Remove(key);
            this.isNeedToUpdate = true;
		}

		private void KeysEvent()
        {
            this.isRotate = 0;
            if (keys.Contains(Keys.Left))
                this.isRotate -= 1;
            if (keys.Contains(Keys.Right))
                this.isRotate += 1;
            this.isMove = 0;
            if (keys.Contains(Keys.Up))
                this.isMove += 1;
            if (keys.Contains(Keys.Down))
                this.isMove -= 1;
        }

		public bool Update()
		{
			bool isUpdate = false;
            if (this.isNeedToUpdate)
            {
                this.KeysEvent();
				float v = isMove * moveStep;
                float a = entity.A + isRotate * rotateStep;
                if (this.entity.V != v || this.entity.A != a)
                {
                    this.entity.SetV(v, a);
					isUpdate = true;
                }
            }
			return isUpdate;
		}
	}
}

