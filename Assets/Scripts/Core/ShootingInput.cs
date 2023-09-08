using System;
using UnityEngine;

namespace DefaultNamespace
{
	[Serializable]
	public class ShootingInput
	{
		[SerializeField] private Camera mainCam;
		[SerializeField] private float aimPlaneDistance;

		private Transform shootFromPoint;

		public void Init(Transform shootFromPoint)
		{
			this.shootFromPoint = shootFromPoint;
		}

		public Vector3 GetInputVectorDirection()
		{
			Vector3 inputVector = Vector3.zero;

			Vector3 mousePos = Input.mousePosition;
			var inputPlane = new Plane(-mainCam.transform.forward, aimPlaneDistance);
			Ray inputRay = mainCam.ScreenPointToRay(mousePos);
			bool haveIntersectionWithInputPlane = inputPlane.Raycast(inputRay, out float raycastDistance);
			if (haveIntersectionWithInputPlane)
			{
				Vector3 aimPoint = inputRay.GetPoint(raycastDistance);
				inputVector = (aimPoint - shootFromPoint.position).normalized;
			}
			else
			{
				Debug.LogError("Cant find aim point");
			}
			
			float inputAndCameraForwardDot = Vector3.Dot(inputVector, mainCam.transform.forward);
			inputVector *= inputAndCameraForwardDot;
			inputVector.y = Mathf.Max(0, inputVector.y);
            
			return inputVector;
		}
	}
}