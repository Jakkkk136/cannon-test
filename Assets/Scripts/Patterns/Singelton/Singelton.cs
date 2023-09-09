using System;
using Patterns.Singelton.SingeltonAttributes;
using UnityEngine;

namespace Patterns.Singelton
{
	public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>, new()
	{
		private static T _instance;

		public bool IsCreatedOnLevel { get; private set; }

		public static T Instance
		{
			get
			{
				if (_instance != null) return _instance;

				_instance = FindObjectOfType<T>(true) ?? CreateNewInstance();

				return _instance;
			}
		}

		private static T CreateNewInstance()
		{
			var createNewInstanceAttribute =
				(AutoCreateSingeltonAttribute)Attribute.GetCustomAttribute(typeof(T),
					typeof(AutoCreateSingeltonAttribute));
			if (createNewInstanceAttribute == null) return null;

			var newGoForInstance = new GameObject($"{typeof(T)}_GameObject");
			T newInstance = newGoForInstance.AddComponent<T>();

			newInstance.IsCreatedOnLevel = true;
			return newInstance;
		}
	}
}