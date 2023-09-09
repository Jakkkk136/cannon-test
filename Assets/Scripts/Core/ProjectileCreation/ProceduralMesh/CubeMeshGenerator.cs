using UnityEngine;

namespace Core.ProjectileCreation.ProceduralMesh
{
	public struct CubeMeshGenerator : IMeshGenerator
	{
		public Mesh CreateMesh()
		{
			var mesh = new Mesh();

			Vector3[] vertices =
			{
				new Vector3(0, 0, 0),
				new Vector3(1, 0, 0),
				new Vector3(1, 0, 1),
				new Vector3(0, 0, 1),
				new Vector3(0, 1, 0),
				new Vector3(1, 1, 0),
				new Vector3(1, 1, 1),
				new Vector3(0, 1, 1)
			};

			int[] triangles =
			{
				0, 2, 1, // bottom
				0, 3, 2,
				2, 3, 6, // front
				3, 7, 6,
				0, 4, 7, // back
				0, 7, 3,
				1, 5, 2, // right
				2, 5, 6,
				0, 1, 5, // left
				0, 5, 4,
				4, 5, 6, // top
				4, 6, 7
			};

			Vector3[] normals =
			{
				-Vector3.up, // 0
				-Vector3.up, // 1
				-Vector3.up, // 2
				-Vector3.up, // 3
				Vector3.up,  // 4
				Vector3.up,  // 5
				Vector3.up,  // 6
				Vector3.up   // 7
			};

			Vector2[] uv =
			{
				new Vector2(0, 0),
				new Vector2(1, 0),
				new Vector2(1, 1),
				new Vector2(0, 1),
				new Vector2(0, 0),
				new Vector2(1, 0),
				new Vector2(1, 1),
				new Vector2(0, 1)
			};

			mesh.vertices = vertices;
			mesh.triangles = triangles;
			mesh.normals = normals;
			mesh.uv = uv;

			mesh.RecalculateBounds();

			return mesh;
		}
	}
}