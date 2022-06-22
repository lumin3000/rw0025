using UnityEngine;

public class UI_VerbTargeter
{
	public Verb targetingVerb;

	private bool resolvingTargetClick;

	public void TargeterOnGUI()
	{
		if (targetingVerb != null && (Find.Selector.NumSelected != 1 || targetingVerb.owner != Find.Selector.SingleSelectedThing || targetingVerb.owner.destroyed))
		{
			targetingVerb = null;
		}
		if (Event.current.type == EventType.MouseDown)
		{
			if (Event.current.button == 0 && targetingVerb != null)
			{
				TargetPack targetPack = GenUI.ClickTargetUnderMouse(targetingVerb.VerbDef.targetParams);
				if (targetPack != null)
				{
					JobType jType = ((!targetingVerb.VerbDef.isWeapon) ? JobType.UseVerbOnThing : JobType.AttackStatic);
					Job job = new Job(jType);
					job.verbToUse = targetingVerb;
					job.targetA = targetPack;
					targetingVerb.OwnerPawn.MindHuman.TakeOrderedJob(job);
				}
				targetingVerb = null;
				resolvingTargetClick = true;
				Event.current.Use();
			}
			if (Event.current.button == 1 && targetingVerb != null)
			{
				targetingVerb = null;
				Event.current.Use();
			}
		}
		if (Event.current.type == EventType.MouseUp && resolvingTargetClick)
		{
			resolvingTargetClick = false;
			Event.current.Use();
		}
		if (targetingVerb != null)
		{
			targetingVerb.DrawTargetingGUI_OnGUI();
		}
	}

	public void TargeterUpdate()
	{
		if (targetingVerb != null)
		{
			targetingVerb.DrawTargetingGUI_Update();
		}
	}

	public void VerbCommandStart(Verb verb)
	{
		if (verb.VerbDef.targetable)
		{
			targetingVerb = verb;
			return;
		}
		Job job = new Job(JobType.UseVerbOnThing);
		job.verbToUse = verb;
		verb.OwnerPawn.jobs.StartJob(job);
	}
}
