using System;
using UnityEngine;

public class Stance_Cooldown : Stance_Busy
{
	private const float RadiusPerTick = 0.002f;

	public Stance_Cooldown(int ticks)
		: this(ticks, null, null)
	{
	}

	public Stance_Cooldown(int ticks, TargetPack focusTarg, Action finishCallback)
		: base(ticks, focusTarg, finishCallback)
	{
	}

	public override void StanceDraw()
	{
		if (Find.Selector.IsSelected(stanceTracker.pawn))
		{
			GenRender.RenderCircle(stanceTracker.pawn.drawer.DrawPos + new Vector3(0f, 0.2f, 0f), (float)stanceTicksLeft * 0.002f);
		}
	}

	public override void ResetStanceTicks()
	{
	}
}
