using Unity.Mathematics;

namespace DefaultNamespace.CustomPhysics.Data
{
	public struct ProjectileData
	{
		public float3 currentPos, currentVelocity, startPos;
		public int collisionsCount;
		public bool isMoving;
		public float mass, creationTime, collisionCheckDistanceSqr;
	}
}