using System;
using System.Collections.Generic;
using System.Windows.Media.Media3D;

namespace RegionFilter
{
	public interface IRegionFilter
	{
		void Update(List<Point3D> pointListList);
		void Update(List<List<Point3D>> pointListList);
		bool Contains(double x, double y);
	}
}
