using System;
using System.Collections.Generic;
using _Scripts.Extensions;
using DefaultNamespace.Configs;
using DefaultNamespace.MeshGeneration;
using DefaultNamespace.MeshGeneration.Enums;
using UnityEngine;

namespace DefaultNamespace.ProjectileCreation.GameObjectWithProceduralMeshCreation
{
	public class ProceduralProjectileFabric : MonoBehaviour
	{
		[SerializeField] private ProceduralMeshGenerator meshGenerator;
		[SerializeField] private Material projectileMaterial;

		[Space]
		[Header("Sets from config")]
		[SerializeField] private eMeshType meshType;
		[SerializeField] private float projectilesScale;

		private GameConfig config;
		private Transform projectilesParent;
		
		private readonly HashSet<Projectile> activeProjectiles = new HashSet<Projectile>();
		private readonly Stack<Projectile> ProjectileStack = new Stack<Projectile>();

		private void Awake()
		{
			ProjectileStack.Clear();
			projectilesParent = new GameObject("Projectiles Parent").transform;

			config = GameConfig.Instance;
			SetParamsFromConfig();

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

		public Projectile GetProjectile()
		{
			Projectile projectile;

			if (ProjectileStack.Count > 0)
			{
				projectile = ProjectileStack.Pop();
				projectile.gameObject.SetActive(true);
			}
			else
			{
				projectile = new GameObject("Projectile").AddComponent<Projectile>();
				projectile.Init(this);
				projectile.transform.position = Vector3.down * 100f;
				projectile.transform.SetGlobalScale(new Vector3(projectilesScale, projectilesScale, projectilesScale));
				projectile.transform.parent = projectilesParent;
			}

			if (projectile.meshType != meshType)
			{
				projectile.meshType = meshType;
				projectile.SetupVisual(meshGenerator.GetMesh(meshType), projectileMaterial);
			}

			activeProjectiles.Add(projectile);

			return projectile;
		}

		public void ReturnProjectileInPool(Projectile projectile)
		{
			projectile.gameObject.SetActive(false);
			projectile.transform.position = Vector3.down * 100f;
			ProjectileStack.Push(projectile);
		}
	}
}