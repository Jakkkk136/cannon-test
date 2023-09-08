using System;
using DefaultNamespace.Configs;
using DefaultNamespace.CustomPhysics;
using DefaultNamespace.PlayerInput;
using DefaultNamespace.ProjectileCreation.GameObjectWithProceduralMeshCreation;
using DefaultNamespace.UI;
using UnityEngine;

namespace DefaultNamespace
{
	public class ShootingController : MonoBehaviour
	{
		[SerializeField] private PowerSlider powerSlider;
		[SerializeField] private ShootingInput inputController;
		[Space]
		[SerializeField] private ProceduralProjectileFabric projectilesFabric;
		[SerializeField] private ProjectilePhysicStepper projectilePhysicStepper;
		[SerializeField] private TrajectoryLineDrawer trajectoryLineDrawer;
		[SerializeField] private Cannon cannon;

		[Space]
		[Header("Sets Dynamically")]
		[SerializeField] private Vector3 shootInputVector = Vector3.zero;

		[Space]
		[Header("Sets from config")]
		[SerializeField] private Vector2 shotPowerMultiplierRange;
		[SerializeField] private float horizontalForceScale = 1f;

		[Space]
		[Header("Sets from power slider")]
		[SerializeField] private float currentPowerMultiplier;

		private GameConfig config;
		private Vector3 lastInputVectorWithForce;
        
		private void Awake()
		{
			config = GameConfig.Instance;
			SetValuesFromConfig();
			
			inputController.Init(cannon.ShootFromPoint);

			config.OnValuesChanged += SetValuesFromConfig;
			powerSlider.OnValueChanged += OnPowerSliderValueChanged;
			GetInputImage.OnPlayerPressOnScreen += OnPlayerPressOnScreen;
		}

		private void OnDestroy()
		{
			config.OnValuesChanged -= SetValuesFromConfig;
			powerSlider.OnValueChanged -= OnPowerSliderValueChanged;
			GetInputImage.OnPlayerPressOnScreen -= OnPlayerPressOnScreen;
		}

		private void Update()
		{
			shootInputVector = inputController.GetInputVectorDirection();
			cannon.RotateToAimDirection(shootInputVector);
			
			lastInputVectorWithForce = shootInputVector * currentPowerMultiplier;

			lastInputVectorWithForce.x *= horizontalForceScale;
			lastInputVectorWithForce.z *= horizontalForceScale;
			
			trajectoryLineDrawer.UpdateTrajectory(lastInputVectorWithForce, cannon.ShootFromPoint.position);
		}

		private void SetValuesFromConfig()
		{
			shotPowerMultiplierRange = config.ShotPowerMultiplierRange;
			horizontalForceScale = config.HorizontalForceScale;

			currentPowerMultiplier =
				Mathf.Lerp(shotPowerMultiplierRange.x, shotPowerMultiplierRange.y, powerSlider.GetValue);
		}
		
        
		private void OnPowerSliderValueChanged(float val)
		{
			currentPowerMultiplier =
				Mathf.Lerp(shotPowerMultiplierRange.x, shotPowerMultiplierRange.y, val);
		}
		
		private void OnPlayerPressOnScreen()
		{
			Shoot(lastInputVectorWithForce);
		}
        
		private void Shoot(Vector3 force)
		{
			Projectile projectile = projectilesFabric.GetProjectile();
			projectilePhysicStepper.InitProjectileMovement(cannon.ShootFromPoint.position, force, projectile);
		}
	}
}