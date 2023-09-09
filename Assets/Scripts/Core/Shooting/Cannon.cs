using UnityEngine;

namespace Core.Shooting
{
	public class Cannon : MonoBehaviour
	{
		[SerializeField] private Transform shootFromPoint;
		[SerializeField] private Transform barrelTransform;

		public Transform ShootFromPoint => shootFromPoint;

		public void RotateToAimDirection(Vector3 aimDirection)
		{
			Vector3 cannonLookAtVector = aimDirection;
			cannonLookAtVector.y = 0f;

			var cannonRotation = Quaternion.LookRotation(cannonLookAtVector);
			transform.rotation = cannonRotation;

			Vector3 cannonBarrelLookArVector = aimDirection;

			Quaternion cannonBarrelRotation =
				Quaternion.LookRotation(cannonBarrelLookArVector) * Quaternion.Euler(90f, 0f, 0f);
			barrelTransform.rotation = cannonBarrelRotation;
		}
	}
}