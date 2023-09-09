using System;
using System.Collections.Generic;
using Core.Configs;
using Core.CustomPhysics.Data;
using Core.CustomPhysics.Jobs;
using Core.Projectiles;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Core.CustomPhysics
{
	[Serializable]
	public class ProjectilePhysicStepper : MonoBehaviour
	{
		[SerializeField] private LayerMask collisionMask;

		[Header("Sets from config")]
		[SerializeField] private float projectileDrag = 0.01f;
		[SerializeField] private float projectileMass = 1f;
		[SerializeField] private float gravityForce;
		[SerializeField] private float projectileLifetime = 10f;
		[SerializeField] private int projectileCollisionsToDestroy = 2;

		[Space]
		[SerializeField] private ProjectileCollisionProcessor projectileCollisionProcessor;

		private List<Projectile> activeProjectiles = new List<Projectile>();
		private NativeList<ProjectileData> activeProjectilesDatas;

		private GameConfig config;
		private JobHandle[] dependencyHandles;
		private NativeArray<RaycastHit> hits;

		private List<ProjectileCreationTicket> projectileCreationTickets = new List<ProjectileCreationTicket>();
		private NativeArray<RaycastCommand> raycastCommands;
		private NativeList<int> raycastHitsWithCollisionIndexes;

		public float GravityForce => gravityForce;

		private void Awake()
		{
			config = GameConfig.Instance;
			config.OnValuesChanged += SetParamsFromConfig;
			SetParamsFromConfig();
		}

		private void Update()
		{
			ProcessFromPreviousFrame();

			if (activeProjectiles.Count == 0) return;

			dependencyHandles[0] = new CreateProjectilesRaycastCommandsJob
			{
				activeProjectiles = activeProjectilesDatas,
				raycastCommands = raycastCommands,
				collisionMask = collisionMask
			}.Schedule(activeProjectilesDatas.Length, default);

			dependencyHandles[1] = RaycastCommand.ScheduleBatch(
				raycastCommands,
				hits,
				0,
				dependencyHandles[0]);

			dependencyHandles[2] = new ProjectileMovementJob
			{
				hits = hits,
				raycastHitsWithCollisionsIndexes = raycastHitsWithCollisionIndexes,
				activeProjectiles = activeProjectilesDatas,

				projectileDrag = projectileDrag,
				gravityForce = gravityForce,
				deltaTime = Time.fixedDeltaTime,
				projectileCollisionsToDestroy = projectileCollisionsToDestroy
			}.Schedule(activeProjectilesDatas.Length, dependencyHandles[1]);
		}

		private void OnEnable()
		{
			activeProjectilesDatas = new NativeList<ProjectileData>(Allocator.Persistent);
			raycastCommands = new NativeArray<RaycastCommand>(300, Allocator.Persistent);
			hits = new NativeArray<RaycastHit>(300, Allocator.Persistent);
			raycastHitsWithCollisionIndexes = new NativeList<int>(300, Allocator.Persistent);

			dependencyHandles = new JobHandle[3];

			projectileCollisionProcessor = new ProjectileCollisionProcessor(hits, raycastHitsWithCollisionIndexes);
		}

		private void OnDestroy()
		{
			Array.ForEach(dependencyHandles, handle => handle.Complete());
			raycastCommands.Dispose();
			hits.Dispose();
			activeProjectilesDatas.Dispose();
			raycastHitsWithCollisionIndexes.Clear();

			config.OnValuesChanged -= SetParamsFromConfig;
		}

		private void SetParamsFromConfig()
		{
			projectileDrag = config.ProjectileDrag;
			projectileMass = config.ProjectileMass;
			gravityForce = config.GravityForce;
			projectileLifetime = config.ProjectileLifetime;
			projectileCollisionsToDestroy = config.ProjectileCollisionsToDestroy;
		}

		public float3 GetPositionInFuture(float3 initialVelocity, float3 startPos, float atTime)
		{
			// Calculate drag acceleration
			float3 dragAcceleration = -projectileDrag * initialVelocity / projectileMass;

			// Calculate net vertical acceleration (gravity + drag in the vertical direction)
			float netVerticalAcceleration = gravityForce + dragAcceleration.y;

			// Calculate position changes due to velocities and accelerations
			float3 positionChangeDueToVelocity = initialVelocity * atTime;
			float3 positionChangeDueToAcceleration = 0.5f * dragAcceleration * atTime * atTime;
			positionChangeDueToAcceleration.y += 0.5f * netVerticalAcceleration * atTime * atTime;

			// Combine position changes to get final predicted position
			float3 positionInFuture = startPos + positionChangeDueToVelocity + positionChangeDueToAcceleration;

			return positionInFuture;
		}

		private void ProcessFromPreviousFrame()
		{
			Array.ForEach(dependencyHandles, handle => handle.Complete());

			projectileCollisionProcessor.ProcessCollisions();

			float currentTime = Time.time;

			for (int i = activeProjectilesDatas.Length - 1; i >= 0; i--)
			{
				Transform projectile = activeProjectiles[i].transform;
				ProjectileData data = activeProjectilesDatas[i];

				projectile.position = data.currentPos;

				if (currentTime - data.creationTime > projectileLifetime || data.isMoving == false)
				{
					activeProjectilesDatas.RemoveAt(i);
					activeProjectiles[i].ResetObject();
					activeProjectiles.RemoveAt(i);
				}
			}

			float deltaTime = Time.deltaTime;

			foreach (ProjectileCreationTicket ticket in projectileCreationTickets)
			{
				activeProjectiles.Add(ticket.projectile);

				var newData = new ProjectileData
				{
					startPos = ticket.statPos,
					currentPos = ticket.statPos,
					currentVelocity = ticket.startVelocity,
					isMoving = true,
					creationTime = currentTime,
					mass = projectileMass,
					collisionCheckDistanceSqr = math.distancesq(ticket.startVelocity, 0f) * deltaTime
				};

				activeProjectilesDatas.Add(newData);
			}

			projectileCreationTickets.Clear();
		}

		public void InitProjectileMovement(Vector3 startPos, Vector3 velocity, Projectile projectile)
		{
			projectileCreationTickets.Add(new ProjectileCreationTicket
			{
				statPos = startPos,
				startVelocity = velocity,
				projectile = projectile
			});
		}
	}
}