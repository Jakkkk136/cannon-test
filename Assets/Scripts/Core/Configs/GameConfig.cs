using System;
using _Scripts.Patterns;
using DefaultNamespace.MeshGeneration.Enums;
using UnityEngine;

namespace DefaultNamespace.Configs
{
	[CreateAssetMenu(fileName = "GameConfig", menuName = "GameConfigs/GameCongig", order = 0)]
	public class GameConfig : SingletonScriptableObject<GameConfig>
	{
		[Header("Projectile physics")]
		[SerializeField] private float projectileDrag = 0.5f;
		[SerializeField] private float projectileMass = 20f;
		[SerializeField] private float gravityForce = -100f;
		[SerializeField] private float projectileLifetime = 10f;
		[SerializeField] private int projectileCollisionsToDestroy = 2;

		[Header("Projectile visuals")]
		[SerializeField] private float projectilesScale = 1.25f;
		[SerializeField] private eMeshType meshType = eMeshType.randomizedCube;
		
		[Header("Painting")]
		[SerializeField] private Color paintOnCollisionColor = Color.blue;
		[SerializeField] private float paintHardness = 0.5f;
		[SerializeField] private float paintStrength = 0.5f;
		[SerializeField] private float paintRadius = 1.25f;

		[Header("Shooting")]
		[SerializeField] private Vector2 shotPowerMultiplierRange = new Vector2(50f, 140f);
		[SerializeField] private float horizontalForceScale = 0.8f;
		
		
		public float ProjectileDrag => projectileDrag;
		public float ProjectileMass => projectileMass;
		public float GravityForce => gravityForce;
		public float ProjectileLifetime => projectileLifetime;
		public int ProjectileCollisionsToDestroy => projectileCollisionsToDestroy;

		public float ProjectilesScale => projectilesScale;
		public eMeshType MeshType => meshType;

		public Color PaintOnCollisionColor => paintOnCollisionColor;
		public float PaintHardness => paintHardness;
		public float PaintStrength => paintStrength;
		public float PaintRadius => paintRadius;

		public Vector2 ShotPowerMultiplierRange => shotPowerMultiplierRange;
		public float HorizontalForceScale => horizontalForceScale;


		public Action OnValuesChanged;
		
		private void OnValidate()
		{
			OnValuesChanged?.Invoke();
		}
	}
}