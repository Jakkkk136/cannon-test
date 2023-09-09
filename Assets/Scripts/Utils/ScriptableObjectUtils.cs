

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Utils
{
#if UNITY_EDITOR

	public static class ScriptableObjectUtils
	{
		public static T[] GetAllInstances<T>() where T : ScriptableObject
		{
			string[]
				guids = AssetDatabase.FindAssets("t:" +
				                                 typeof(T)
					                                 .Name); //FindAssets uses tags check documentation for more info
			var a = new T[guids.Length];
			for (var i = 0; i < guids.Length; i++) //probably could get optimized 
			{
				string path = AssetDatabase.GUIDToAssetPath(guids[i]);
				a[i] = AssetDatabase.LoadAssetAtPath<T>(path);
			}

			return a;
		}
	}

#endif
}