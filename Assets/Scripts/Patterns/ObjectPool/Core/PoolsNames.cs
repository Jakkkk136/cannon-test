using System.Collections.Generic;
using System.Linq;

namespace Patterns.ObjectPool.Core
{
	public static class PoolsNames
	{
		public static string EXAMPLE_NAME = "EXAMPLE_NAME";
		
		public static IEnumerable<string> GetAllPoolsNames()
		{
			return typeof(PoolsNames).GetFields().Select(fieldInfo => fieldInfo.Name);
		}
	}
}