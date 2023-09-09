using System;
using System.Collections.Generic;
using UnityEngine;

namespace Patterns.ObjectPool.Core
{
	[Serializable]
	public class PoolInfo
	{
		public string poolId;
		public PooledObject[] prefabs;
		public int poolSize;
		public bool fixedSize;
		public Transform parentForPooledObjects;

		public static IEnumerable<string> GetAllPoolsNames() => PoolsNames.GetAllPoolsNames();
	}
}