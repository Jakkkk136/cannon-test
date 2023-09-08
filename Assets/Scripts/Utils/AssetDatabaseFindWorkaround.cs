using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace _Scripts.Helpers
{
#if UNITY_EDITOR
	public static class AssetDatabaseFindWorkaround
	{
		public static List<GameObject> GetAssetsOfTypeAsGameObjects<T>(string atPath = "") where T : MonoBehaviour
		{
			string[] guids = AssetDatabase.FindAssets("t:Prefab",
				new[] { string.IsNullOrWhiteSpace(atPath) ? "Assets" : "Assets/Art/Prefabs" });
			var toSelect = new List<int>();
			foreach (string guid in guids)
			{
				string path = AssetDatabase.GUIDToAssetPath(guid);
				Object[] toCheck = AssetDatabase.LoadAllAssetsAtPath(path);
				foreach (Object obj in toCheck)
				{
					var go = obj as GameObject;
					if (go == null) continue;

					T comp = go.GetComponent<T>();
					if (comp != null)
					{
						toSelect.Add(go.GetInstanceID());
					}
					else
					{
						T[] comps = go.GetComponentsInChildren<T>();
						if (comps.Length > 0) toSelect.Add(go.GetInstanceID());
					}
				}
			}

			return toSelect.Select(t => EditorUtility.InstanceIDToObject(t) as GameObject).ToList();
		}

		public static List<T> GetAssetsOfTypeAsType<T>(string atPath = "") where T : MonoBehaviour
		{
			string[] guids = AssetDatabase.FindAssets("t:Prefab",
				new[] { string.IsNullOrWhiteSpace(atPath) ? "Assets" : "Assets/Art/Prefabs" });
			var toSelect = new List<int>();
			foreach (string guid in guids)
			{
				string path = AssetDatabase.GUIDToAssetPath(guid);
				Object[] toCheck = AssetDatabase.LoadAllAssetsAtPath(path);
				foreach (Object obj in toCheck)
				{
					var go = obj as GameObject;
					if (go == null) continue;

					T comp = go.GetComponent<T>();
					if (comp != null)
					{
						toSelect.Add(go.GetInstanceID());
					}
					else
					{
						T[] comps = go.GetComponentsInChildren<T>();
						if (comps.Length > 0) toSelect.Add(go.GetInstanceID());
					}
				}
			}

			return toSelect.Select(t => EditorUtility.InstanceIDToObject(t) as T).ToList();
		}

		public static List<T> GetScriptableObjectAssetsOfTypeAsType<T>(string atPath = "") where T : ScriptableObject
		{
			string[] guids = AssetDatabase.FindAssets("t:ScriptableObject",
				new[] { string.IsNullOrWhiteSpace(atPath) ? "Assets" : "Assets/_Configs" });
			var toSelect = new List<int>();
			foreach (string guid in guids)
			{
				string path = AssetDatabase.GUIDToAssetPath(guid);
				Object[] toCheck = AssetDatabase.LoadAllAssetsAtPath(path);
				foreach (Object obj in toCheck)
				{
					var so = obj as T;
					if (so == null) continue;

					toSelect.Add(so.GetInstanceID());
				}
			}

			return toSelect.Select(t => EditorUtility.InstanceIDToObject(t) as T).ToList();
		}
	}
#endif
}