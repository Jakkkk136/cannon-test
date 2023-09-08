using System.Linq;
using _Scripts.Utils;
using UnityEngine;

namespace _Scripts.Patterns
{
	/// <summary>
	///     Created config Scriptable Object MUST be in Resources folder and MUST be the same as Class name
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
	{
		private static T instance;
		public static T Instance
		{
			get
			{
				if (instance != null) return instance;

				instance = Resources.Load<T>(typeof(T).Name);

#if UNITY_EDITOR
				if (instance == null) instance = ScriptableObjectUtils.GetAllInstances<T>().First();
#endif
				if (instance.name != instance.GetType().Name)
					Debug.LogError($"Wrong name of file, don't match with class name: {instance.name}");

				(instance as SingletonScriptableObject<T>)?.OnInitialize();

				return instance;
			}
		}

		// Optional overridable method for initializing the instance.
		protected virtual void OnInitialize()
		{
		}
	}
}