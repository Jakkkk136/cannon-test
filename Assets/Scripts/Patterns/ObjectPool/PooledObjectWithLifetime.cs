using UnityEngine;

namespace Patterns.ObjectPool
{
	public class PooledObjectWithLifetime : PooledObject
	{
		[Header("Settings")] [SerializeField] private float lifeTime = 2f;
		private float timeToLive;

		protected virtual void Update()
		{
			if (isPooled == false && Time.time >= timeToLive) ResetObject();
		}

		protected virtual void OnEnable()
		{
			UpdateTimeToLive();
		}

		protected void UpdateTimeToLive()
		{
			timeToLive = Time.time + lifeTime;
		}

		public void SetLifeTime(float lifeTime)
		{
			this.lifeTime = lifeTime;
			UpdateTimeToLive();
		}
	}
}