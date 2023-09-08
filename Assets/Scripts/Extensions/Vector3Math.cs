using System;
using UnityEngine;

public static class Vector3Math
{
	public static Vector2 Mult(this Vector2 a, Vector2 b) => new Vector3(a.x * b.x, a.y * b.y);

	public static Vector3 Mult(this Vector3 a, Vector3 b) => new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);

	public static bool CompareApproximately(this Vector3 a, Vector3 b, float gap) => Vector3.SqrMagnitude(a - b) < gap;

	public static Vector3 Direction(this Vector3 pos, Vector3 target) => (target - pos).normalized;


	public static SerializableVector3 Serialized(this Vector3 obj) => new SerializableVector3(obj.x, obj.y, obj.z);

	public static SerializableVector3Int Serialized(this Vector3Int obj) =>
		new SerializableVector3Int(obj.x, obj.y, obj.z);

	public static SerializableVector2 Serialized(this Vector2 obj) => new SerializableVector2(obj.x, obj.y);

	public static SerializableVector2Int Serialized(this Vector2Int obj) => new SerializableVector2Int(obj.x, obj.y);

	public static Vector3Int SetX(this Vector3Int vector, int x) => new Vector3Int(x, vector.y, vector.z);

	public static Vector3Int SetY(this Vector3Int vector, int y) => new Vector3Int(vector.x, y, vector.z);

	public static Vector3Int SetZ(this Vector3Int vector, int z) => new Vector3Int(vector.x, vector.y, z);

	public static Vector2Int SetX(this Vector2Int vector, int x) => new Vector2Int(x, vector.y);

	public static Vector2Int SetY(this Vector2Int vector, int y) => new Vector2Int(vector.x, y);

	public static Vector3 SetX(this Vector3 vector, float value) => new Vector3(value, vector.y, vector.z);

	public static Vector3 SetY(this Vector3 vector, float value) => new Vector3(vector.x, value, vector.z);

	public static Vector3 SetZ(this Vector3 vector, float value) => new Vector3(vector.x, vector.y, value);

	public static Vector2 SetX(this Vector2 vector, float value) => new Vector2(value, vector.y);

	public static Vector2 SetY(this Vector2 vector, float value) => new Vector2(vector.x, value);

	[Serializable]
	public struct SerializableVector3
	{
		public float x;
		public float y;
		public float z;

		public SerializableVector3(float fromX, float fromY, float fromZ)
		{
			x = fromX;
			y = fromY;
			z = fromZ;
		}

		public static implicit operator Vector3(SerializableVector3 d) => new Vector3(d.x, d.y, d.z);
	}

	[Serializable]
	public struct SerializableVector3Int
	{
		public int x;
		public int y;
		public int z;

		public SerializableVector3Int(int fromX, int fromY, int fromZ)
		{
			x = fromX;
			y = fromY;
			z = fromZ;
		}

		public static implicit operator Vector3Int(SerializableVector3Int d) => new Vector3Int(d.x, d.y, d.z);
	}

	[Serializable]
	public struct SerializableVector2
	{
		public float x;
		public float y;

		public SerializableVector2(float fromX, float fromY)
		{
			x = fromX;
			y = fromY;
		}

		public static implicit operator Vector2(SerializableVector2 d) => new Vector2(d.x, d.y);
	}

	[Serializable]
	public struct SerializableVector2Int
	{
		public int x;
		public int y;

		public SerializableVector2Int(int fromX, int fromY)
		{
			x = fromX;
			y = fromY;
		}

		public static implicit operator Vector2Int(SerializableVector2Int d) => new Vector2Int(d.x, d.y);
	}
}