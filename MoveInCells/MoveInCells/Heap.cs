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
            nstart = count - 1;
            count = (count << 1) - 1;
            heap = new Entity[count];


            // TODO: Check.
            entities.CopyTo(this.heap, nstart);
            Entity inf = new Entity() { T = float.PositiveInfinity };
            for (int i = nstart + n; i < count; ++i)
            {
                this.heap[i] = inf;
            }

            int count_ = count;
            do
            {
                for (int i = count_ >> 1; i < count_; i += 2)
                {
                    heap[(i - 1) >> 1] = heap[i].T < heap[i + 1].T ? heap[i] : heap[i + 1];
                }
                count_ = count_ >> 1;
            } while ((count_ >> 1) > 0);
        }

        public void Recalculate(int i)
        {
            i = nstart + i;
            int i_sosed, i_parent;
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
