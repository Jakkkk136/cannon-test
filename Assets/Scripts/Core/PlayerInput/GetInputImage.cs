using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.PlayerInput
{
	public class GetInputImage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		public static Action OnPlayerPressOnScreen;

		private bool pointerEnter;

		private void Update()
		{
			if (pointerEnter && Input.GetMouseButton(0)) OnPlayerPressOnScreen?.Invoke();
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			pointerEnter = true;
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			pointerEnter = false;
		}
	}
}