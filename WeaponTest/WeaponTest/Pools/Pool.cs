using System;

namespace Pools
{
	public class Pool<DataType> : IPool
		where DataType : class, IPoolable
	{
		public int Count{ get; private set; }

		public int Capacity{ get; private set; }

		public DataType[] elements = null;

		public Pool (int capacity, DataType element)
		{
			this.Count = -1;
			this.Capacity = capacity;
			this.elements = new DataType[capacity];
			for (int i = this.elements.Length - 1; i > 0; --i) {
				this.elements [i] = (DataType)element.DeepCopy ();
				this.elements [i].Parent = this;
				this.elements [i].Id = i;
			}
			this.elements [0] = element;
			this.elements [0].Parent = this;
			this.elements [0].Id = 0;
		}

		public DataType GetByIndex (int index)
		{
			return this.elements [index];
		}

		public DataType CreateElement ()
		{
			if (this.Count + 1 < this.Capacity) {
				++this.Count;
				this.elements [this.Count].Initialize ();
				return this.elements [this.Count];
			}
			return null;
		}

		public void FreeElement (int index)
		{
			DataType elementTemp = this.elements [index];
			this.elements [index] = this.elements [this.Count];
			this.elements [this.Count] = elementTemp;

			elements [index].Id = index;
			elements [this.Count].Id = this.Count;

			--this.Count;
		}

		public void Clear ()
		{
			this.Count = 0;
		}

	}
}

