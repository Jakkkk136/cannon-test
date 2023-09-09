using UnityEngine;

namespace Core.ProjectileCreation.ProceduralMesh
{
	public struct RandomizedCubeGenerator : IMeshGenerator
	{
		public float minOffset;
		public float maxOffset;

		public RandomizedCubeGenerator(float minOffset, float maxOffset)
		{
			this.minOffset = minOffset;
			this.maxOffset = maxOffset;
		}

		public Mesh CreateMesh()
		{
			Mesh cubeMesh = default(CubeMeshGenerator).CreateMesh();
			Vector3[] vertices = cubeMesh.vertices;

			for (var i = 0; i < vertices.Length; i++)
			{
				var offset = new Vector3(
					Random.Range(minOffset, maxOffset),
					Random.Range(minOffset, maxOffset),
					Random.Range(minOffset, maxOffset)
				);

				vertices[i] += offset;
			}

			cubeMesh.vertices = vertices;
			cubeMesh.RecalculateNormals();
			cubeMesh.RecalculateBounds();

			return cubeMesh;
		}
	}
}