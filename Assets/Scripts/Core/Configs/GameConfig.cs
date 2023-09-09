using System;
using Core.ProjectileCreation.ProceduralMesh.Enums;
using Patterns.Singelton;
using UnityEngine;

namespace Core.Configs
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
		[SerializeField] private float shootCooldown = 0.5f;

		[Header("Feedback")]
		[SerializeField] private float maxFeedbackDuration = 0.5f;
		[SerializeField] private AnimationCurve feedbackAnimationCurve;
		[SerializeField] private float maxCameraShakeOffset = 0.5f;
		[SerializeField] private float maxCannonRecoil = 0.2f;

		public Action OnValuesChanged;
		
		//Projectile physics
		public float ProjectileDrag => projectileDrag;
		public float ProjectileMass => projectileMass;
		public float GravityForce => gravityForce;
		public float ProjectileLifetime => projectileLifetime;
		public int ProjectileCollisionsToDestroy => projectileCollisionsToDestroy;

		//Projectile visuals
		public float ProjectilesScale => projectilesScale;
		public eMeshType MeshType => meshType;

		//Painting
		public Color PaintOnCollisionColor => paintOnCollisionColor;
		public float PaintHardness => paintHardness;
		public float PaintStrength => paintStrength;
		public float PaintRadius => paintRadius;

		//Shooting
		public Vector2 ShotPowerMultiplierRange => shotPowerMultiplierRange;
		public float HorizontalForceScale => horizontalForceScale;
		public float ShootCooldown => shootCooldown;

		//Feedback
		public float MaxFeedbackDuration => maxFeedbackDuration;
		public AnimationCurve FeedbackAnimationCurve => feedbackAnimationCurve;
		public float MaxCameraShakeOffset => maxCameraShakeOffset;
		public float MaxCannonRecoil => maxCannonRecoil;

		private void OnValidate()
		{
			OnValuesChanged?.Invoke();
		}
	}
}