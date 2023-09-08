using System;

namespace _Scripts.Extensions
{
	public static class RoundExtension
	{
		public static int RoundOff(this int i, int roundTo) => (int)Math.Round(i / (float)roundTo) * roundTo;

		public static int RoundOff(this float i, int roundTo) => (int)Math.Round(i / roundTo) * roundTo;
	}
}