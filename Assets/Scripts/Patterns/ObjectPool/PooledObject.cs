using System.Collections.Generic;
using Patterns.ObjectPool.Core;
using UnityEngine;

namespace Patterns.ObjectPool
{
	public class PooledObject : MonoBehaviour
	{
		public string poolName;
		public bool isPooled;

		public int InitialIndexInPool { get; private set; }

		public static IEnumerable<string> GetAllPoolsNames() => PoolsNames.GetAllPoolsNames();

		public virtual void ResetObject()
		{
			ObjectPoolController.Instance.ReturnObjectToPool(this);
		}

		public void SetInitialIndexInPool(int index)
		{
			InitialIndexInPool = index;
		}
	}
}