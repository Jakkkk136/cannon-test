using System;
using DefaultNamespace.Configs;
using Unity.Collections;
using UnityEngine;

namespace DefaultNamespace.CustomPhysics
{
	[Serializable]
	public class ProjectileCollisionProcessor
	{
		private NativeArray<RaycastHit> projectileRaycastHits;
		private NativeList<int> raycastHitsWithCollisions;

		private PaintManager paintManager;
		private GameConfig config;

		[Header("Sets from config")]
		[SerializeField] private Color paintOnCollisionColor;
		[SerializeField] private float paintHardness;
		[SerializeField] private float paintStrength;
		[SerializeField] private float paintRadius;

		public ProjectileCollisionProcessor(NativeArray<RaycastHit> projectileRaycastHits, NativeList<int> raycastHitsWithCollisions)
		{
			this.projectileRaycastHits = projectileRaycastHits;
			this.raycastHitsWithCollisions = raycastHitsWithCollisions;

			paintManager = PaintManager.Instance;
			config = GameConfig.Instance;
			
			SetValuesFromConfig();

			config.OnValuesChanged -= SetValuesFromConfig;
			config.OnValuesChanged += SetValuesFromConfig;
		}
		
		public void ProcessCollisions()
		{
			foreach (int hitWithCollision in raycastHitsWithCollisions)
			{
				RaycastHit hit = projectileRaycastHits[hitWithCollision];
				
				if(hit.collider == null) continue;
                
				if (hit.collider.TryGetComponent(out Paintable p))
				{
					paintManager.Paint(p, hit.point, paintRadius, paintHardness, paintStrength, paintOnCollisionColor);
				}
			}
			
			raycastHitsWithCollisions.Clear();
		}

		private void SetValuesFromConfig()
		{
			paintOnCollisionColor = config.PaintOnCollisionColor;
			paintHardness = config.PaintHardness;
			paintStrength = config.PaintStrength;
			paintRadius = config.PaintRadius;
		}
	}
}