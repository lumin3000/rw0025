using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Building_Bed : Building, Interactive
{
	private const int TicksBetweenSleepZs = 100;

	private bool forPrisonersInt;

	public Pawn owner;

	private int ticksToSleepZ;

	public Pawn CurSleeper
	{
		get
		{
			foreach (Thing item in Find.Grids.ThingsAt(base.Position))
			{
				Pawn pawn = item as Pawn;
				if (pawn == null || pawn.jobs.CurJob == null || pawn.jobs.CurJob.jType != JobType.Sleep || pawn.jobs.CurJob.targetA.thing != this)
				{
					continue;
				}
				return pawn;
			}
			return null;
		}
	}

	public bool forPrisoners
	{
		get
		{
			return forPrisonersInt;
		}
		set
		{
			if (value != forPrisonersInt)
			{
				if (owner != null)
				{
					owner.ownership.UnclaimBed();
				}
				forPrisonersInt = value;
				Find.MapDrawer.MapChanged(base.Position, MapChangeType.Things);
			}
		}
	}

	public override Material DrawMat
	{
		get
		{
			if (forPrisoners)
			{
				return def.iconMat_ForPrisoner;
			}
			return def.drawMat;
		}
	}

	public override void SpawnSetup()
	{
		base.SpawnSetup();
		Room roomAt = Find.Grids.GetRoomAt(base.Position);
		if (roomAt != null && roomAt.IsPrisonCell)
		{
			forPrisoners = true;
		}
	}

	public override void Destroy()
	{
		if (owner != null)
		{
			owner.ownership.UnclaimBed();
		}
		base.Destroy();
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe.LookField(ref forPrisonersInt, "ForPrisoners");
	}

	public override IEnumerable<Command> GetCommandOptions()
	{
		Command option = new Command_Action
		{
			hotKey = KeyCode.F
		};
		if (!forPrisoners)
		{
			option.icon = Res.LoadTexture("UI/Commands/ForPrisonersOff");
			option.tipDef = new TooltipDef("Assign beds in this room for use by prisoners.");
			option.clickSound = UISounds.CheckboxTurnedOn;
			option.action = delegate
			{
				TrySetForPrisonersByInterface(newForPrisoners: true);
			};
		}
		else
		{
			option.icon = Res.LoadTexture("UI/Commands/ForPrisonersOn");
			option.tipDef = new TooltipDef("Assign beds in this room for use by colonists, not prisoners.");
			option.clickSound = UISounds.CheckboxTurnedOff;
			option.action = delegate
			{
				TrySetForPrisonersByInterface(newForPrisoners: false);
			};
		}
		yield return option;
	}

	private void TrySetForPrisonersByInterface(bool newForPrisoners)
	{
		List<Building_Bed> bedsInEnc = new List<Building_Bed>();
		Room roomAt = Find.Grids.GetRoomAt(base.Position);
		if (roomAt != null)
		{
			bedsInEnc = roomAt.ContainedBeds.ToList();
		}
		else
		{
			bedsInEnc.Add(this);
		}
		Action action = delegate
		{
			foreach (Building_Bed item in bedsInEnc)
			{
				item.forPrisoners = newForPrisoners;
			}
		};
		if (bedsInEnc.Count > 0 && bedsInEnc.Where((Building_Bed b) => b.owner != null && b != this).Count() > 0)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string text = "on";
			if (!newForPrisoners)
			{
				text = "off";
			}
			stringBuilder.Append("Turning " + text + " prisoner mode affects the whole room (since prisoners and colonists cannot sleep in the same room).\n\nThese people will lose ownership of their beds:");
			stringBuilder.AppendLine();
			foreach (Building_Bed item2 in bedsInEnc)
			{
				if (item2.owner != null)
				{
					stringBuilder.AppendLine();
					stringBuilder.Append(item2.owner.characterName);
				}
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			stringBuilder.Append("Are you sure you want to do this?");
			Find.UIMapRoot.dialogs.AddDialogBox(new DialogBox_Confirm(stringBuilder.ToString(), action));
		}
		else
		{
			action();
		}
	}

	public override string GetInspectString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(base.GetInspectString());
		stringBuilder.AppendLine();
		if (forPrisoners)
		{
			stringBuilder.Append("For prisoner use");
		}
		else
		{
			stringBuilder.Append("For colonist use");
		}
		stringBuilder.AppendLine();
		stringBuilder.Append("Owner: ");
		if (owner == null)
		{
			stringBuilder.Append("Nobody");
		}
		else
		{
			stringBuilder.Append(owner.Label);
		}
		return stringBuilder.ToString();
	}

	public JobCondition InteractedWith(ReservationType w, Pawn p)
	{
		if (owner != p)
		{
			if (p.Incapacitated)
			{
				p.Position = GenMap.RandomStandableLOSSquareNear(p.Position, 1);
			}
			return JobCondition.Incompletable;
		}
		if (!DebugSettings.worldBreathable && !this.HasAir() && !p.Incapacitated)
		{
			return JobCondition.Incompletable;
		}
		if (p.Incapacitated && owner != p)
		{
			Debug.LogWarning(string.Concat("Sleeping incapacitated ", p, " does not own the bed any more."));
		}
		if (Find.TickManager.tickCount % 750 == 0 && Find.Grids.GetRoomAt(base.Position) == null)
		{
			p.psychology.thoughts.GainThought(ThoughtType.SleptOutside);
		}
		p.rest.Rest.TickResting(def.restEffectiveness);
		if (Find.TickManager.tickCount % def.bed_HealTickInterval == 0 && p.healthTracker.Health < p.healthTracker.MaxHealth && !p.food.Food.Starving && this.HasAir())
		{
			p.TakeDamage(new DamageInfo(DamageType.Healing, 1));
			if (p.healthTracker.Health == p.healthTracker.MaxHealth)
			{
				UI_Messages.Message(p.Label + " is fully healed.", UIMessageSound.Benefit);
			}
		}
		ticksToSleepZ--;
		if (ticksToSleepZ <= 0)
		{
			if (!p.rest.DoneResting)
			{
				MoteMaker.ThrowSleepZ(p.Position);
			}
			if (p.healthTracker.Health < p.healthTracker.MaxHealth)
			{
				MoteMaker.ThrowHealingCross(p.Position);
			}
			ticksToSleepZ = 100;
		}
		if (p.Incapacitated)
		{
			return JobCondition.Ongoing;
		}
		if (p.rest.DoneResting)
		{
			if (p.healthTracker.Health >= p.healthTracker.MaxHealth)
			{
				return JobCondition.Succeeded;
			}
			if (p.food.Food.UrgentlyHungry)
			{
				return JobCondition.Succeeded;
			}
		}
		return JobCondition.Ongoing;
	}

	public override void DrawGUIOverlay()
	{
		if (Find.CameraMap.CurrentZoom == CameraZoomRange.Closest && (owner == null || !owner.IsInBed()))
		{
			string text = ((owner == null) ? "Unowned" : owner.characterName);
			GenWorldUI.DrawThingLabelFor(this, text, new Color(1f, 1f, 1f, 0.75f));
		}
	}
}
