using UnityEngine;

public class JitterHandler
{
	protected Vector3 JitterOffset = new Vector3(0f, 0f, 0f);

	public float DamageJitterDistance = 0.17f;

	public float JitterDropPerTick = 0.018f;

	public float JitterMax = 0.35f;

	public Vector3 CurrentJitterOffset => JitterOffset;

	public void JitterHandlerTick()
	{
		if (JitterOffset.sqrMagnitude < JitterDropPerTick * JitterDropPerTick)
		{
			JitterOffset = new Vector3(0f, 0f, 0f);
		}
		else
		{
			JitterOffset -= JitterOffset.normalized * JitterDropPerTick;
		}
	}

	public void Notify_DamageApplied(DamageInfo dinfo)
	{
		if (dinfo.type.HasForcefulImpact())
		{
			AddOffset(DamageJitterDistance, dinfo.direction);
		}
	}

	public void AddOffset(float Distance, float Direction)
	{
		JitterOffset += Quaternion.AngleAxis(Direction, Vector3.up) * Vector3.forward * Distance;
		if (JitterOffset.sqrMagnitude > JitterMax * JitterMax)
		{
			JitterOffset *= JitterMax / JitterOffset.magnitude;
		}
	}
}
