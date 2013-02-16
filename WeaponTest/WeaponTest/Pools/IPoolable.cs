using System;

namespace Pools
{
	public interface IPoolable
	{
		IPool Parent{ get; set; }
		int Id{ get; set; }

		IPoolable DeepCopy();
		void Initialize();
	}
}

