using System;
using UnityEngine;
using UnityEngine.UI;

namespace Core.UI
{
	public class PowerSlider : MonoBehaviour
	{
		[SerializeField] private Slider slider;
		[SerializeField] private Transform handleTransform;
		[SerializeField] private Text valueText;
		[SerializeField] private Text descriptionText;
		[SerializeField] private Gradient textGradient;
		[SerializeField] private AnimationCurve textScaleAnimationCurve;

		public Action<float> OnValueChanged;

		public float GetValue => slider.value;

		private void Start()
		{
			slider.onValueChanged.AddListener(OnSliverValueChanged);
			slider.value = 0.5f;
		}

		private void OnSliverValueChanged(float val)
		{
			valueText.text = Mathf.RoundToInt(val * 100f).ToString();

			Vector3 valueTextPos = valueText.transform.position;
			valueTextPos.y = handleTransform.position.y;
			valueText.transform.position = valueTextPos;

			valueText.color = descriptionText.color = textGradient.Evaluate(val);

			float scaleGradient = textScaleAnimationCurve.Evaluate(val);
			valueText.transform.localScale = new Vector3(scaleGradient, scaleGradient, scaleGradient);

			OnValueChanged?.Invoke(val);
		}
	}
}