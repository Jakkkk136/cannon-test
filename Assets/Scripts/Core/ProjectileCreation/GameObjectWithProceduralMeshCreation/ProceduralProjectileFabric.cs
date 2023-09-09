using System.Collections.Generic;
using Core.Configs;
using Core.ProjectileCreation.ProceduralMesh;
using Core.ProjectileCreation.ProceduralMesh.Enums;
using Core.Projectiles;
using Extensions;
using Patterns.ObjectPool;
using Patterns.ObjectPool.Core;
using UnityEngine;

namespace Core.ProjectileCreation.GameObjectWithProceduralMeshCreation
{
	public class ProceduralProjectileFabric : MonoBehaviour
	{
		
		[SerializeField] private ProceduralMeshGenerator meshGenerator;
		[SerializeField] private Material projectileMaterial;
		
		[Space]
		[Header("Pooling")]
		[SerializeField] private string projectilesPoolName;
		[SerializeField] private PooledObjectWithLifetime projectileExplosionFx;
		
		[Space]
		[Header("Sets from config")]
		[SerializeField] private eMeshType meshType;
		[SerializeField] private float projectilesScale;

		private readonly HashSet<Projectile> activeProjectiles = new HashSet<Projectile>();

		private ObjectPoolController poolController;
		private GameConfig config;

		private void Awake()
		{
			poolController = ObjectPoolController.Instance;
            
			config = GameConfig.Instance;
			SetParamsFromConfig();
			CreatePools();
			
			config.OnValuesChanged += SetParamsFromConfig;
		}

		private void OnDestroy()
		{
			config.OnValuesChanged -= SetParamsFromConfig;
		}

		private void OnValidate()
		{
			foreach (Projectile activeProjectile in activeProjectiles)
				if (activeProjectile.meshType != meshType)
				{
					activeProjectile.meshType = meshType;
					activeProjectile.SetupVisual(meshGenerator.GetMesh(meshType), projectileMaterial);
				}
		}

		private void SetParamsFromConfig()
		{
			meshType = config.MeshType;
			projectilesScale = config.ProjectilesScale;

			OnValidate();
		}

		private void CreatePools()
		{
			//Projectile pool
			Projectile projectile = new GameObject("Projectile").AddComponent<Projectile>();
			
			projectile.Init(this, poolController, projectileExplosionFx.poolName);
			projectile.transform.SetGlobalScale(new Vector3(projectilesScale, projectilesScale, projectilesScale));
			
			poolController.CreateNewPoolInRuntime(projectilesPoolName, 10, false, null, projectile);
			
			//Projectile explosion fx pool
			poolController.CreateNewPoolInRuntime(projectileExplosionFx.poolName, 10, false, null, projectileExplosionFx);
		}

		public Projectile GetProjectile()
		{
			Projectile projectile = poolController.GetObjectFromPool<Projectile>(projectilesPoolName, Vector3.down * 100, Quaternion.identity);

			projectile.meshType = meshType;
			projectile.SetupVisual(meshGenerator.GetMesh(meshType), projectileMaterial);

			activeProjectiles.Add(projectile);

			return projectile;
		}
	}
}