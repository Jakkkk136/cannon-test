using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Extensions
{
	[Serializable]
	public struct TransformLocalStruct
	{
		public Transform parent;
		public Vector3 localPosition;
		public Vector3 localEulerAngles;
		public Vector3 localScale;
		public bool active;
	}

	[Serializable]
	public struct TransformGlobalStruct
	{
		public Vector3 Position;
		public Vector3 EulerAngles;
		public Vector3 Scale;
		public bool active;
	}


	public static class TransformExtensions
	{
		private static Transform helperObject;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T FindAncestorOfType<T>(this Transform t, int deep = 100) where T : Component
		{
			Transform temporal = t;
			T comp = temporal.GetComponent<T>();
			while (comp == null && temporal.parent != null && deep > 0)
			{
				temporal = temporal.parent;
				comp = temporal.GetComponent<T>();
				deep--;
			}

			return comp;
		}

		public static void InverseLookAt(this Transform current, Transform awayFrom)
		{
			current.rotation = Quaternion.LookRotation(current.position - awayFrom.position);
		}

		/// <summary>
		///     Global scale is a lossy scale!
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void SetGlobalScale(this Transform transform, Vector3 globalScale)
		{
			if (helperObject == null)
			{
				helperObject = new GameObject(nameof(TransformExtensions) + "Helper Transform").transform;
				helperObject.gameObject.hideFlags = HideFlags.HideAndDontSave;
			}

			helperObject.localScale = globalScale;
			helperObject.parent = transform.parent;
			transform.localScale = helperObject.localScale;
			helperObject.parent = null;
		}

		public static Transform AddChild(this Transform transform, string name = "GameObject")
		{
			Transform child = new GameObject(name).transform;
			child.parent = transform;
			child.localEulerAngles = Vector3.zero;
			child.localPosition = Vector3.zero;
			child.localScale = new Vector3(1, 1, 1);
			child.position = new Vector3(0, 0, 0);
			return child;
		}

		public static Transform AddChild(this Transform transform, GameObject origin, string name = "GameObject")
		{
			Transform child = Object.Instantiate(origin, transform).transform;
			child.name = name;
			return child;
		}

		public static void RemoveAllChildren(this Transform transform)
		{
			foreach (Transform child in transform)
				if (Application.isPlaying)
					Object.Destroy(child.gameObject);
				else
					Object.DestroyImmediate(child.gameObject);
		}

		public static TransformLocalStruct ToLocalStruct(this Transform transform) =>
			new TransformLocalStruct
			{
				localPosition = transform.localPosition,
				localEulerAngles = transform.localEulerAngles,
				localScale = transform.localScale,
				active = transform.gameObject.activeSelf,
				parent = transform.parent
			};

		public static TransformGlobalStruct ToGlobalStruct(this Transform transform) =>
			new TransformGlobalStruct
			{
				Position = transform.localPosition,
				EulerAngles = transform.localEulerAngles,
				Scale = transform.localScale,
				active = transform.gameObject.activeSelf
			};

		public static void FromGlobalStruct(this Transform transform, TransformGlobalStruct data,
			bool ignoreActive = true)
		{
			// transform from Local to World
			transform.position = data.Position;
			transform.eulerAngles = data.EulerAngles;
			transform.SetGlobalScale(data.Scale);
			if (!ignoreActive) transform.gameObject.SetActive(data.active);
		}

		/// <summary> Convert struct to local position for transform </summary>
		public static void FromLocalStructToLocal(this Transform transform, TransformLocalStruct data,
			bool ignoreActive = true)
		{
			// transform from Local to World
			transform.localPosition = data.localPosition;
			transform.localEulerAngles = data.localEulerAngles;
			transform.localScale = data.localScale;
			if (!ignoreActive) transform.gameObject.SetActive(data.active);
		}

		public static void FromLocalStructToWorld(this Transform transform, TransformLocalStruct data,
			bool ignoreActive = true)
		{
			// transform from Local to World + parent Position!
			transform.position = data.parent.localToWorldMatrix * data.localPosition;
			transform.position += data.parent.position;
			transform.localEulerAngles = data.localEulerAngles;
			transform.localScale = data.localScale;
			if (!ignoreActive) transform.gameObject.SetActive(data.active);
		}

		public static string WholePath(this Transform current)
		{
			string name = current.name;
			Transform parent = current.transform.parent;
			while (parent != null)
			{
				name = parent.name + "/" + name;
				parent = parent.parent;
			}

			return name;
		}

		public static Vector3 GetGlobalScale(this Transform tr) => tr.lossyScale;

		public static void SetTransform(this Transform current, Transform otherTransform, bool applyScale)
		{
			current.SetPositionAndRotation(otherTransform.position, otherTransform.rotation);
			if (applyScale) current.SetGlobalScale(otherTransform.GetGlobalScale());
		}

		public static Vector3 Down(this Transform self) => Quaternion.Euler(180, 0, 0) * self.up;

		public static Vector3 Left(this Transform self) => Quaternion.Euler(0, 180, 0) * self.right;

		public static Vector3 Backward(this Transform self) => Quaternion.Euler(0, 0, 180) * self.forward;

		public static Vector3 GetRandomPointAround(this Transform transformToFindPointAround, float fromAngle,
			float toAngle,
			float distance, float yPos)
		{
			Vector3 point = transformToFindPointAround.position;

			float randomAngle = Random.Range(fromAngle, toAngle);

			point.x += distance * Mathf.Sin(Mathf.Deg2Rad * randomAngle);
			point.y = yPos;
			point.z += distance * Mathf.Cos(Mathf.Deg2Rad * randomAngle);

			return point;
		}

		public static void RemoveAllComponentsFromChilds<T>(this Transform t) where T : Object
		{
			bool isGame = Application.isPlaying;

			foreach (T comp in t.GetComponentsInChildren<T>())
				if (isGame)
					Object.Destroy(comp);
				else
					Object.DestroyImmediate(comp);
		}

		public static void ProcessChildTransforms(this Transform thisTransform, Action<Transform> action)
		{
			for (var i = 0; i < thisTransform.childCount; i++)
			{
				Transform currentChild = thisTransform.GetChild(i);
				action(currentChild);
				ProcessChildTransforms(currentChild, action);
			}
		}
	}
}