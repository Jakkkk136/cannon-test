using System;
using Core.ProjectileCreation.GameObjectWithProceduralMeshCreation;
using Core.ProjectileCreation.ProceduralMesh.Enums;
using Patterns.ObjectPool;
using Patterns.ObjectPool.Core;
using UnityEngine;

namespace Core.Projectiles
{
	[Serializable]
	public class Projectile : PooledObject
	{
		[SerializeField] public eMeshType meshType;
		[SerializeField] public MeshFilter meshFilter;
		[SerializeField] public MeshRenderer meshRenderer;
		
		[SerializeField] private ProceduralProjectileFabric fabric;
		[SerializeField] private ObjectPoolController poolController;
		[SerializeField] private string projectileExplosionFxPoolName;
		
		public void Init(ProceduralProjectileFabric fabric, ObjectPoolController poolController, string projectileExplosionFxPoolName)
		{
			this.fabric = fabric;
			this.poolController = poolController;
			this.projectileExplosionFxPoolName = projectileExplosionFxPoolName;
			
			meshFilter ??= gameObject.AddComponent<MeshFilter>();
			meshRenderer ??= gameObject.AddComponent<MeshRenderer>();
		}
		
		public override void ResetObject()
		{
			poolController.GetObjectFromPool<PooledObjectWithLifetime>(projectileExplosionFxPoolName,
				transform.position, Quaternion.identity);
			
			base.ResetObject();
		}

		public void SetupVisual(Mesh mesh, Material material)
		{
			meshFilter.mesh = mesh;
			meshRenderer.material = material;
		}
	}
}