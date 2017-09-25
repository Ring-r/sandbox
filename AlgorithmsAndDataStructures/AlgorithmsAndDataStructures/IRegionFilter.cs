using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace AlgorithmsAndDataStructures
{
    public interface IRegionFilter
    {
        void Init(List<Point3D> pointList);
        void Init(List<List<Point3D>> pointListList);
        bool Contains(double x, double y);
    }
}
