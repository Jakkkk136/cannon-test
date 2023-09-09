using Core.Configs;
using UnityEngine;

namespace Core.Shooting
{
	public class ShootingFeedback : MonoBehaviour
	{
		[SerializeField] private ShootingController shootingController;
		[SerializeField] private Cannon cannon;
		[SerializeField] private Transform cameraTransform;

		[Header("Sets from config")]
		[SerializeField] private float maxFeedbackDuration = 0.5f;
		[SerializeField] private AnimationCurve feedbackAnimationCurve;
		[SerializeField] private float maxCameraShakeOffset = 0.5f;
		[SerializeField] private float maxCannonRecoil = 0.2f;
		
		private GameConfig config;

		private Vector3 originalCameraPosition;
		private Vector3 originalCannonPosition;
		private Vector3 lastShootVelocity;

		private float feedbackForceMultiplier;
		private float lastShootTime = -100000;
		private float shootCooldown;
		private float maxShootSqrMagnitude;
		private float lastShotSqrMagnitude;
		private float currentFeedBackDuration;
		
		private void Awake()
		{
			config = GameConfig.Instance;

			originalCameraPosition = cameraTransform.localPosition;
			originalCannonPosition = cannon.transform.localPosition;
            
			SetParamsFromConfig();
            
			shootingController.OnShootVelocity += OnPlayerShoot;
			config.OnValuesChanged += SetParamsFromConfig;
		}

		private void OnDestroy()
		{
			shootingController.OnShootVelocity -= OnPlayerShoot;
			config.OnValuesChanged -= SetParamsFromConfig;
		}

		private void Update()
		{
			ApplyFeedback();
		}

		private void ApplyFeedback()
		{
			if (Time.time - lastShootTime <= currentFeedBackDuration)
			{
				float t = (Time.time - lastShootTime) / currentFeedBackDuration;
				float feedbackValue = feedbackAnimationCurve.Evaluate(t);

				cameraTransform.localPosition = originalCameraPosition +
				                                Random.insideUnitSphere * (maxCameraShakeOffset * feedbackValue * feedbackForceMultiplier);

				cannon.transform.localPosition =
					originalCannonPosition - cannon.transform.forward * (maxCannonRecoil * feedbackValue * feedbackForceMultiplier);
			}
			else
			{
				cameraTransform.localPosition = originalCameraPosition;
				cannon.transform.localPosition = originalCannonPosition;
			}
		}
        
		private void SetParamsFromConfig()
		{
			shootCooldown = config.ShootCooldown;
			maxFeedbackDuration = config.MaxFeedbackDuration;
			feedbackAnimationCurve = config.FeedbackAnimationCurve;
			maxCameraShakeOffset = config.MaxCameraShakeOffset;
			maxCannonRecoil = config.MaxCannonRecoil;
			
			maxShootSqrMagnitude = (config.ShotPowerMultiplierRange.y * config.HorizontalForceScale * config.ShotPowerMultiplierRange.y * config.HorizontalForceScale);
		}
		
		private void OnPlayerShoot(Vector3 shootVelocity)
		{
			lastShootVelocity = shootVelocity;
			lastShotSqrMagnitude = lastShootVelocity.sqrMagnitude;
			lastShootTime = Time.time;

			feedbackForceMultiplier = ( lastShotSqrMagnitude / maxShootSqrMagnitude);
			
			currentFeedBackDuration = Mathf.Min(shootCooldown, maxFeedbackDuration) * feedbackForceMultiplier;
		}
	}
}