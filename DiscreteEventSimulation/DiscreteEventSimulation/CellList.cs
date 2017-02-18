using System.Collections.Generic;

namespace DiscreteEventSimulation
{
    public class CellList : List<Entity>
    {
        public new void Add(Entity entity)
        {
            base.Add(entity);
            entity.indexInCell = Count - 1;
        }

        public new void Remove(Entity entity)
        {
            var lastEntity = this[Count - 1];
            lastEntity.indexInCell = entity.indexInCell;
            this[lastEntity.indexInCell] = lastEntity;
            RemoveAt(Count - 1);
        }
    }
}
