using UnityEngine;

namespace URandom
{
	public class WaveToRgb
	{
		public const float MinVisibleWaveLength = 350f;

		public const float MaxVisibleWaveLength = 650f;

		public const float Gamma = 0.8f;

		public const int IntesityMax = 255;

		public static Color LinearToRgb(float linearvalue)
		{
			return WaveLengthToRgb(GetWaveLengthFromDataPoint(linearvalue, 0f, 1f));
		}

		private static float GetWaveLengthFromDataPoint(float x, float min, float max)
		{
			return (x - min) / (max - min) * 300f + 350f;
		}

		private static int Adjust(float color, float factor)
		{
			if (color == 0f)
			{
				return 0;
			}
			return (int)Mathf.Round(255f * Mathf.Pow(color * factor, 0.8f));
		}

		private static Color WaveLengthToRgb(float wave)
		{
			wave = Mathf.Floor(wave);
			float num = 0f;
			float num2 = 0f;
			float num3 = 0f;
			float num4 = 0f;
			if (wave >= 380f && wave < 440f)
			{
				num = (0f - (wave - 440f)) / 60f;
				num2 = 0f;
				num3 = 1f;
			}
			else if (wave >= 440f && wave < 490f)
			{
				num = 0f;
				num2 = (wave - 440f) / 50f;
				num3 = 1f;
			}
			else if (wave >= 490f && wave < 510f)
			{
				num = 0f;
				num2 = 1f;
				num3 = (0f - (wave - 510f)) / 20f;
			}
			else if (wave >= 510f && wave < 580f)
			{
				num = (wave - 510f) / 70f;
				num2 = 1f;
				num3 = 0f;
			}
			else if (wave >= 580f && wave < 645f)
			{
				num = 1f;
				num2 = (0f - (wave - 645f)) / 65f;
				num3 = 0f;
			}
			else if (wave >= 645f && wave <= 780f)
			{
				num = 1f;
				num2 = 0f;
				num3 = 0f;
			}
			else
			{
				num = 0f;
				num2 = 0f;
				num3 = 0f;
			}
			num4 = ((wave >= 380f && wave < 420f) ? (0.3f + 0.7f * (wave - 380f) / 40f) : ((wave >= 420f && wave < 700f) ? 1f : ((!(wave >= 700f) || !(wave <= 780f)) ? 0f : (0.3f + 0.7f * (780f - wave) / 80f))));
			num = Adjust(num, num4);
			num2 = Adjust(num2, num4);
			num3 = Adjust(num3, num4);
			num /= 255f;
			num2 /= 255f;
			num3 /= 255f;
			return new Color(num, num2, num3);
		}
	}
}
