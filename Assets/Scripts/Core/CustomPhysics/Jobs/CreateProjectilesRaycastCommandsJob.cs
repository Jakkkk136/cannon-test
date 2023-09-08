using DefaultNamespace.CustomPhysics.Data;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace DefaultNamespace.CustomPhysics
{
	[BurstCompile]
	public struct CreateProjectilesRaycastCommandsJob : IJobFor
	{
		[ReadOnly] public NativeList<ProjectileData> activeProjectiles;

		public LayerMask collisionMask;
		public NativeArray<RaycastCommand> raycastCommands;

		public void Execute(int index)
		{
			ProjectileData data = activeProjectiles[index];

			var raycastCommand = new RaycastCommand
			{
				from = data.currentPos,
				direction = data.currentVelocity,
				distance = data.collisionCheckDistanceSqr,
				layerMask = collisionMask,
				maxHits = 1
			};

			raycastCommands[index] = raycastCommand;
		}
	}
}