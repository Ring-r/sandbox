namespace DiscreteEventSimulation
{
    public class CellList
    {
        private int capacity = 0;
        private int count = 0;
        private Entity[] array = null;

        public int Count { get { return this.count; } }
        public Entity this[int index] { get { return this.array[index]; } }

        public CellList(int capacity)
        {
            this.capacity = capacity;
            this.array = new Entity[capacity];
        }
        public void Add(Entity entity)
        {
            this.array[this.count] = entity;
            entity.k = this.count;
            ++this.count;
        }
        public void Remove(Entity entity)
        {
            int index = entity.k;
            this.array[index] = this.array[this.count - 1];
            this.array[index].k = index;
            --this.count;
        }
    }
}
