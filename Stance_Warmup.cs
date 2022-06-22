using System;
using UnityEngine;

public class Stance_Warmup : Stance_Busy
{
	private bool targetStartedIncapped;

	public Stance_Warmup(int ticks)
		: this(ticks, null, null)
	{
	}

	public Stance_Warmup(int ticks, TargetPack focusTarg, Action finishCallback)
		: base(ticks, focusTarg, finishCallback)
	{
		if (focusTarg.HasThing && focusTarg.thing is Pawn)
		{
			targetStartedIncapped = ((Pawn)focusTarg.thing).Incapacitated;
		}
	}

	public override void StanceDraw()
	{
		float facing = 0f;
		if (focusTarg != null && focusTarg.Loc != stanceTracker.pawn.Position)
		{
			facing = ((focusTarg.thing == null) ? (focusTarg.Loc - stanceTracker.pawn.Position).AngleFlat : (focusTarg.thing.DrawPos - stanceTracker.pawn.Position.ToVector3Shifted()).AngleFlat());
		}
		GenRender.RenderPie(stanceTracker.pawn.drawer.DrawPos + new Vector3(0f, 0.2f, 0f), facing, (int)((float)stanceTicksLeft * pieSizeMultiplier));
	}

	public override void StanceTick()
	{
		if (!targetStartedIncapped && focusTarg.HasThing && focusTarg.thing is Pawn && ((Pawn)focusTarg.thing).Incapacitated)
		{
			stanceTracker.SetStance(new Stance_Mobile());
		}
		else
		{
			base.StanceTick();
		}
	}
}
