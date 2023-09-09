using Core.Projectiles;
using Unity.Mathematics;

namespace Core.CustomPhysics.Data
{
	public struct ProjectileCreationTicket
	{
		public float3 statPos, startVelocity;
		public Projectile projectile;
	}
}