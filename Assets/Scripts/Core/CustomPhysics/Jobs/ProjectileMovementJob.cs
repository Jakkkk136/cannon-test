using Core.CustomPhysics.Data;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Core.CustomPhysics.Jobs
{
	[BurstCompile]
	public struct ProjectileMovementJob : IJobFor
	{
		[ReadOnly] public NativeArray<RaycastHit> hits;
		public NativeList<ProjectileData> activeProjectiles;
		public NativeList<int> raycastHitsWithCollisionsIndexes;

		public float projectileDrag;
		public float gravityForce;
		public float deltaTime;

		public int projectileCollisionsToDestroy;

		public void Execute(int index)
		{
			ProjectileData data = activeProjectiles[index];

			RaycastHit hit = hits[index];
			bool haveCollision = math.distancesq(hit.point, data.currentPos) <= math.sqrt(
				data.collisionCheckDistanceSqr);

			if (haveCollision)
			{
				if (math.distancesq(data.currentPos, 0f) >= data.collisionCheckDistanceSqr)
				{
					raycastHitsWithCollisionsIndexes.Add(index);
					data.collisionsCount++;
				}

				data.currentVelocity = math.reflect(data.currentVelocity, hit.normal);

				if (data.collisionsCount >= projectileCollisionsToDestroy) data.isMoving = false;
			}

			if (data.isMoving)
			{
				float3 dragForce = -projectileDrag * data.currentVelocity;
				float3 dragAcceleration = dragForce / data.mass;

				data.currentVelocity += (new float3(0, gravityForce, 0) + dragAcceleration) * deltaTime;
				data.currentPos += data.currentVelocity * deltaTime;
			}

			data.collisionCheckDistanceSqr = math.distancesq(data.currentVelocity, 0f) * deltaTime;
			activeProjectiles[index] = data;
		}
	}
}