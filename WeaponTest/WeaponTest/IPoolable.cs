using System;

namespace Entities
{
	public interface IPoolable
	{
		IPool Parent{ get; set; }
		int Id{ get; set; }

		IPoolable DeepCopy();
		void Initialize();
	}
}

