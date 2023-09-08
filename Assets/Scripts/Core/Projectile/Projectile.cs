using DefaultNamespace.MeshGeneration.Enums;
using DefaultNamespace.ProjectileCreation.GameObjectWithProceduralMeshCreation;
using UnityEngine;

namespace DefaultNamespace
{
	public class Projectile : MonoBehaviour
	{
		public eMeshType meshType;
		public MeshFilter meshFilter;
		public MeshRenderer meshRenderer;

		private ProceduralProjectileFabric fabric;

		public void Init(ProceduralProjectileFabric fabric)
		{
			this.fabric = fabric;
		}

		public void ReturnToPool()
		{
			fabric.ReturnProjectileInPool(this);
		}

		public void SetupVisual(Mesh mesh, Material material)
		{
			meshFilter ??= gameObject.AddComponent<MeshFilter>();
			meshRenderer ??= gameObject.AddComponent<MeshRenderer>();

			meshFilter.mesh = mesh;
			meshRenderer.material = material;
		}
	}
}