using UnityEngine;

public class SkyTarget
{
	public float dayPercent;

	public float glowPercent;

	public float saturation = 1f;

	public SkyColorSet colors;

	public bool overrideShadowVector;

	public Vector2 shadowVector;

	public SkyTarget()
	{
	}

	public SkyTarget(float dayPercent, float skyGlowPercent, SkyColorSet colors)
	{
		this.dayPercent = dayPercent;
		glowPercent = skyGlowPercent;
		this.colors = colors;
	}

	public static SkyTarget Lerp(SkyTarget A, SkyTarget B, float factor)
	{
		SkyTarget skyTarget = new SkyTarget();
		skyTarget.dayPercent = Mathf.Lerp(A.dayPercent, B.dayPercent, factor);
		skyTarget.glowPercent = Mathf.Lerp(A.glowPercent, B.glowPercent, factor);
		skyTarget.saturation = Mathf.Lerp(A.saturation, B.saturation, factor);
		skyTarget.colors = default(SkyColorSet);
		skyTarget.colors.sky = Color.Lerp(A.colors.sky, B.colors.sky, factor);
		skyTarget.colors.shadow = Color.Lerp(A.colors.shadow, B.colors.shadow, factor);
		skyTarget.colors.weatherOverlays = Color.Lerp(A.colors.weatherOverlays, B.colors.weatherOverlays, factor);
		if (A.overrideShadowVector && B.overrideShadowVector)
		{
			Debug.LogWarning("Lerping between two SkyTargets that both override the shadow vector");
		}
		if (A.overrideShadowVector)
		{
			skyTarget.overrideShadowVector = true;
			skyTarget.shadowVector = A.shadowVector;
		}
		else if (B.overrideShadowVector)
		{
			skyTarget.overrideShadowVector = true;
			skyTarget.shadowVector = B.shadowVector;
		}
		return skyTarget;
	}
}
