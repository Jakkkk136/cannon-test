﻿using System;
using UnityEngine;

namespace Patterns.Singelton
{
	public abstract class SingletonFromAbstractClass<T> : MonoBehaviour
	{
		private static T _instance;

		public static T Instance => _instance ?? throw new Exception("Instance is not set");

		protected virtual void SetInstance(T instance)
		{
			_instance = instance;
		}
	}
}