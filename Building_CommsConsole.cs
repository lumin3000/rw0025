using System;
using System.Collections.Generic;
using UnityEngine;

public class Building_CommsConsole : Building, Interactive
{
	private CompPowerTrader powerComp;

	public bool CanUseCommsNow => powerComp.PowerOn && this.HasAir() && !this.IsForbidden();

	public override void SpawnSetup()
	{
		base.SpawnSetup();
		powerComp = GetComp<CompPowerTrader>();
	}

	public JobCondition InteractedWith(ReservationType iType, Pawn interactor)
	{
		if (!CanUseCommsNow)
		{
			Debug.LogWarning(string.Concat(this, " tried to open comms when it couldn't."));
			return JobCondition.Incompletable;
		}
		OpenTradeMenu(interactor);
		return JobCondition.Succeeded;
	}

	private void OpenTradeMenu(Pawn negotiator)
	{
		if (Find.VisitorManager.VisitorList.Count == 0)
		{
			DiaNode diaNode = new DiaNode("There is nobody in range to communicate with.");
			diaNode.optionList.Add(DiaOption.DefaultOK);
			DialogBoxHelper.InitDialogTree(diaNode);
			return;
		}
		if (Find.MapConditionManager.ConditionIsActive(MapConditionType.SolarFlare))
		{
			DiaNode diaNode2 = new DiaNode("The solar flare is blocking radio transmissions.");
			diaNode2.optionList.Add(DiaOption.DefaultOK);
			DialogBoxHelper.InitDialogTree(diaNode2);
			return;
		}
		DiaNode diaNode3 = new DiaNode("Negotiator: " + negotiator.characterName + " (social skill level " + negotiator.skills.LevelOf(SkillType.Social) + ")\n\nOpen comms channel to:");
		foreach (Visitor visitor in Find.VisitorManager.VisitorList)
		{
			DiaOption diaOption = new DiaOption(visitor.FullTitle);
			Visitor ThisVis = visitor;
			diaOption.ChosenCallback = delegate
			{
				ThisVis.OpenComms(negotiator);
			};
			diaNode3.optionList.Add(diaOption);
		}
		DiaOption diaOption2 = new DiaOption("Cancel");
		diaOption2.ResolveTree = true;
		diaNode3.optionList.Add(diaOption2);
		DialogBoxHelper.InitDialogTree(diaNode3);
	}

	public override IEnumerable<FloatMenuChoice> GetFloatMenuChoicesFor(Pawn myPawn)
	{
		if (CanUseCommsNow && !myPawn.MindHuman.drafted)
		{
			Pawn myPawn2 = default(Pawn);
			Action useAct = delegate
			{
				myPawn2.MindHuman.TakeOrderedJob(new Job(JobType.UseCommsConsole, new TargetPack(this)));
			};
			yield return new FloatMenuChoice("Open communications", useAct);
		}
	}
}
