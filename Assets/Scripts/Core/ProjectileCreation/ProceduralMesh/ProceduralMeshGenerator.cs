using System;
using System.Collections.Generic;
using DefaultNamespace.MeshGeneration.Enums;
using UnityEngine;

namespace DefaultNamespace.MeshGeneration
{
	public class ProceduralMeshGenerator : MonoBehaviour
	{
		[SerializeField] private float randomizedCubeGenerationMaxOffset;

		private Dictionary<eMeshType, MeshGeneratorInfo> MeshGeneratorsInfoMap;

		private void Awake()
		{
			SetupGetMeshGeneratorsInfoMap();
		}

		private void SetupGetMeshGeneratorsInfoMap()
		{
			MeshGeneratorsInfoMap = new Dictionary<eMeshType, MeshGeneratorInfo>
			{
				{ eMeshType.none, null },
				{
					eMeshType.cube, new MeshGeneratorInfo
					{
						GetMeshFunc = default(CubeMeshGenerator).CreateMesh,
						MeshVariationsCount = 1
					}
				},
				{
					eMeshType.randomizedCube, new MeshGeneratorInfo
					{
						GetMeshFunc = new RandomizedCubeGenerator(
							-randomizedCubeGenerationMaxOffset,
							randomizedCubeGenerationMaxOffset).CreateMesh,

						MeshVariationsCount = 8
					}
				}
			};
		}

		public Mesh GetMesh(eMeshType type)
		{
			if (type == eMeshType.none)
			{
				Debug.LogError("Trying to get mesh with type none");
				return null;
			}

			MeshGeneratorInfo info = MeshGeneratorsInfoMap[type];

			if (info.GeneratedMeshes == null)
			{
				info.GeneratedMeshes = new Mesh[info.MeshVariationsCount];

				for (var i = 0; i < info.MeshVariationsCount; i++) info.GeneratedMeshes[i] = info.GetMeshFunc();
			}

			return info.GeneratedMeshes[info.CurrentMeshIndex++ % info.MeshVariationsCount];
		}

		private class MeshGeneratorInfo
		{
			public int CurrentMeshIndex;
			public Mesh[] GeneratedMeshes;
			public Func<Mesh> GetMeshFunc;
			public int MeshVariationsCount;
		}
	}
}