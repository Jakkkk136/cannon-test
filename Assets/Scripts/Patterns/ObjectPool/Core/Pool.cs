using System.Collections.Generic;
using System.Linq;
using Extensions;
using UnityEngine;

namespace Patterns.ObjectPool.Core
{
	internal sealed class Pool
	{
		public readonly Queue<PooledObject> availableObjQueue = new Queue<PooledObject>();
		private readonly bool fixedSize;
		private readonly Transform parentForPooledObjects;
		private readonly string poolName;
		private readonly List<PooledObject> poolObjectPrefabs;
		private readonly Vector3 poolObjectsPos = new Vector3(100f, 100f, 100f);
		private Transform parent;

		private int poolSize;

		public Pool(string poolName, PooledObject[] poolObjectPrefabs, int initialCount, bool fixedSize,
			Transform parentForPooledObjects = null)
		{
			this.poolName = poolName;
			this.poolObjectPrefabs = poolObjectPrefabs.ToList();
			this.fixedSize = fixedSize;
			this.parentForPooledObjects = parentForPooledObjects;

			for (var index = 0; index < initialCount; index++) AddObjectToPoolInternal(NewObjectInstance());

			if (this.poolObjectPrefabs.Count > 1) ShuffleQueue();
		}


		private PooledObject AddObjectToPoolInternal(PooledObject po)
		{
			po.transform.parent = parent;
			po.gameObject.SetActive(false);
			po.transform.position = poolObjectsPos;
			availableObjQueue.Enqueue(po);

			po.isPooled = true;

			//Debug.LogWarning($"Object: {po.name} returned to pool: {poolName} status pulled: {po.isPooled} ___ {Time.time}");

			return po;
		}

		private void ShuffleQueue()
		{
			var temp = availableObjQueue.ToList();
			temp.Shuffle();

			availableObjQueue.Clear();

			foreach (PooledObject pooledObject in temp) availableObjQueue.Enqueue(pooledObject);
		}

		public void AddObjectToPool(PooledObject po, int instancesCount = 1)
		{
			poolObjectPrefabs.Add(po);

			//populate the pool
			for (var index = 0; index < instancesCount; index++) AddObjectToPoolInternal(NewObjectInstance());

			if (poolObjectPrefabs.Count > 1) ShuffleQueue();
		}

		private PooledObject NewObjectInstance()
		{
			//create parent
			if (parent == null)
			{
				parent = new GameObject(poolName + "_Pool").transform;
				parent.parent = parentForPooledObjects != null ? parentForPooledObjects : null;
				parent.localPosition = Vector3.zero;
				parent.localRotation = Quaternion.identity;
			}

			PooledObject pooledObject =
				Object.Instantiate(poolObjectPrefabs[poolSize % poolObjectPrefabs.Count], parent);
			pooledObject.enabled = true;
			pooledObject.poolName = poolName;
			pooledObject.SetInitialIndexInPool(poolSize);

			poolSize++;

			return pooledObject;
		}

		public T NextAvailableObject<T>(Vector3 position, Quaternion rotation) where T : PooledObject
		{
			PooledObject result = null;

			if (availableObjQueue.Count > 0)
				result = availableObjQueue.Dequeue();
			else if (fixedSize == false)
				result = NewObjectInstance();
			else
				return null;

			result.isPooled = false;

			result.transform.SetPositionAndRotation(position,
				rotation == Quaternion.identity ? result.transform.rotation : rotation);

			result.gameObject.SetActive(true);

			return result as T;
		}

		public void ReturnObjectToPool(PooledObject po)
		{
			if (poolName.Equals(po.poolName))
			{
				if (po.isPooled)
					Debug.LogWarning($"Trying to return to pool already pooled object {po.gameObject.name}");
				else
					AddObjectToPoolInternal(po);
			}
		}
	}
}