using Unity.Mathematics;
using UnityEngine;

namespace _Scripts.Helpers.Curves
{
	public static class Curves
	{
		public static Vector3 Bezier(Vector3 p0, Vector3 p1, Vector3 p2, float t)
		{
			Vector3 position = (1.0f - t) * (1.0f - t) * p0 + 2.0f * (1.0f - t) * t * p1 + t * t * p2;
			return position;
		}

		public static float3 Bezier(float3 p0, float3 p1, float3 p2, float t)
		{
			float3 position = (1.0f - t) * (1.0f - t) * p0 + 2.0f * (1.0f - t) * t * p1 + t * t * p2;
			return position;
		}
	}
}