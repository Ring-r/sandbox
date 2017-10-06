using System.Collections.Generic;

namespace AlgorithmsAndDataStructures
{
    public interface IRegionFilter
    {
        void Init(List<Vector3d> pointList);
        void Init(List<List<Vector3d>> pointListList);
        bool Contains(double x, double y);
    }
}
