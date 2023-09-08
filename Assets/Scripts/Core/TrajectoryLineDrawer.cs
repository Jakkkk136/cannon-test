using System.Collections.Generic;
using DefaultNamespace.CustomPhysics;
using UnityEngine;

public class TrajectoryLineDrawer : MonoBehaviour
{
	[SerializeField] protected LineRenderer _lineRenderer;
	[SerializeField] protected int lineSegments;

	[SerializeField] private ProjectilePhysicStepper projectilePhysicStepper;

	protected List<Vector3> linePoints = new List<Vector3>();


	public void UpdateTrajectory(Vector3 shootForce, Vector3 shootFromPos)
	{
		float flightDuration = 2f * shootForce.y / -projectilePhysicStepper.GravityForce;

		float stepTime = flightDuration / lineSegments;

		linePoints.Clear();

		for (var i = 0; i < lineSegments; i++)
		{
			float stepTimePassed = stepTime * i;

			Vector3 movementVector =
				projectilePhysicStepper.GetPositionInFuture(shootForce, shootFromPos, stepTimePassed);

			linePoints.Add(movementVector);
		}

		_lineRenderer.positionCount = linePoints.Count;
		_lineRenderer.SetPositions(linePoints.ToArray());
	}
}