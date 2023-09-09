using UnityEngine.UIElements.Experimental;

namespace Utils
{
	public static class EasingUtil
	{
		public enum eEase
		{
			none,
			InOutQuad,
			InQuad,
			InCirc,
			InCubic
		}

		public static float Ease(eEase e, float t)
		{
			switch (e)
			{
				case eEase.InOutQuad:
					return Easing.InOutQuad(t);
				case eEase.InQuad:
					return Easing.InQuad(t);
				case eEase.InCirc:
					return Easing.InCirc(t);
				case eEase.InCubic:
					return Easing.InCubic(t);
				default:
					return 0f;
			}
		}
	}
}