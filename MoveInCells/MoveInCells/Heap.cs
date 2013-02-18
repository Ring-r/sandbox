using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoveInCells
{
    class Heap
    {
        private int nstart;
        private Entity[] heap;

        public void StartFill(Entity[] entities)
        {
            int n = entities.Length;
            int count = 1;
            while (count <= n)
            {
                count = count << 1;
            }
            heap = new Entity[count - 1];
            nstart = (count >> 1) - 1;

            // TODO: Check.
            entities.CopyTo(this.heap, nstart);
            Entity inf = new Entity() { T = float.PositiveInfinity };
            for (int i = nstart + n; i < count - 1; ++i)
            {
                this.heap[i] = inf;
            }

            for (int i = 0; i < nstart + 1; i += 2)
            {
                this.Recalculate(i);
            }
        }

        public void Recalculate(int i)
        {
            i = nstart + i;
            int i_sosed;
            int i_parent;
            do
            {
                i_sosed = (i & 1) == 1 ? i + 1 : i - 1;
                i_parent = (i - 1) >> 1;
                heap[i_parent] = heap[i].T < heap[i_sosed].T ? heap[i] : heap[i_sosed];
                i = i_parent;
            } while (i > 0);
        }

        public Entity GetFirst()
        {
            return this.heap[0];
        }
    }
}
