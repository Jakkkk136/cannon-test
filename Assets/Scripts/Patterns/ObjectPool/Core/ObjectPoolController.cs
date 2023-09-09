using System;
using System.Collections.Generic;
using System.Linq;
using Patterns.Singelton;
using UnityEngine;

namespace Patterns.ObjectPool.Core
{
	public sealed class ObjectPoolController : Singleton<ObjectPoolController>
	{
		[Header("Pre-created pools in Editor")]
		public List<PoolInfo> poolInfo;

		[Header("Pools created at runtime")]
		public List<PoolInfo> poolsCreatedInRuntime;

		private readonly Dictionary<string, Pool> poolDictionary = new Dictionary<string, Pool>();

		private void Awake()
		{
			CheckForDuplicatePoolNames();
			CreatePoolsFromEditorData();
		}

		public bool IsPoolPresented(string poolName) => poolDictionary.ContainsKey(poolName);

		private void CheckForDuplicatePoolNames()
		{
			for (var index = 0; index < poolInfo.Count; index++)
			{
				string poolId = poolInfo[index].poolId;
				if (string.IsNullOrEmpty(poolId)) Debug.LogError($"Pool at index {index} have no name");

				for (int internalIndex = index + 1; internalIndex < poolInfo.Count; internalIndex++)
					if (poolId.Equals(poolInfo[internalIndex].poolId, StringComparison.InvariantCultureIgnoreCase))
						Debug.LogError($"Pools {index} and {internalIndex} have the same name {poolId}");
			}
		}

		private void CreatePoolsFromEditorData()
		{
			foreach (PoolInfo currentPoolInfo in poolInfo) CreatePool(currentPoolInfo);
		}

		public void AddNewObjectToPool(string poolID, int instancesCount = 1, params PooledObject[] poolPrefabs)
		{
			if (poolDictionary.ContainsKey(poolID) == false)
			{
				Debug.LogError($"Can't add object to pool {poolID} __ pool is not presented");
				return;
			}

			foreach (PooledObject pooledObject in poolPrefabs)
				poolDictionary[poolID].AddObjectToPool(pooledObject, instancesCount);
		}

		public void CreateNewPoolInRuntime(string poolID, int poolSize, bool fixedSize,
			Transform parentForPooledObjects, params PooledObject[] poolPrefabs)
		{
			if (poolDictionary.ContainsKey(poolID))
			{
				Debug.LogError($"Pool as already created, attempt to create second with the same name: {poolID}");
				return;
			}

			var newPoolInfo = new PoolInfo
			{
				fixedSize = fixedSize, poolId = poolID,
				poolSize = poolSize, prefabs = poolPrefabs, parentForPooledObjects = parentForPooledObjects
			};

			poolsCreatedInRuntime.Add(newPoolInfo);

			CreatePool(newPoolInfo);
		}

		private void CreatePool(PoolInfo info)
		{
			var pool = new Pool(info.poolId, info.prefabs,
				info.poolSize, info.fixedSize, info.parentForPooledObjects);

			poolDictionary[info.poolId] = pool;
		}

		public IEnumerable<T> GetAllObjectsInPool<T>(string poolId) where T : PooledObject =>
			poolDictionary[poolId].availableObjQueue.Cast<T>().Reverse();


		public T GetObjectFromPool<T>(string poolName, Vector3 position, Quaternion rotation) where T : PooledObject
		{
			if (poolDictionary.ContainsKey(poolName))
				return poolDictionary[poolName].NextAvailableObject<T>(position, rotation);

			Debug.LogError("Invalid pool name specified: " + poolName);
			return null;
		}

		public void ReturnObjectToPool(PooledObject poolObject)
		{
			if (poolDictionary.TryGetValue(poolObject.poolName, out Pool pool))
				pool.ReturnObjectToPool(poolObject);
			else
				Debug.LogError(
					$"Object {poolObject.name} is not presented in pools with pool ID {poolObject.poolName}");
		}
	}
}