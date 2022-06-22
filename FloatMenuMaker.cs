using System;
using System.Collections.Generic;
using System.Linq;

public static class FloatMenuMaker
{
	public static List<FloatMenuChoice> ChoicesAtFor(IntVec3 clickSq, Pawn myPawn)
	{
		List<FloatMenuChoice> list = new List<FloatMenuChoice>();
		if (myPawn.Team != TeamType.Colonist)
		{
			return list;
		}
		if (myPawn.Incapacitated)
		{
			UI_Messages.Message(myPawn.Label + " is incapacitated.", UIMessageSound.Reject);
			return list;
		}
		if (myPawn.MindHuman.drafted)
		{
			TargetPack attackTarg = GenUI.ClickTargetUnderMouse(TargetingParameters.ForAttack(myPawn));
			if (attackTarg != null && attackTarg.thing != null)
			{
				if (myPawn.equipment.Primary != null)
				{
					string label;
					Action act;
					if (!myPawn.equipment.Primary.verb.CanHitTarget(attackTarg))
					{
						label = (myPawn.Position.WithinHorizontalDistanceOf(attackTarg.Loc, myPawn.equipment.Primary.verb.VerbDef.range) ? ("Fire at " + attackTarg.thing.Label + " (cannot hit target)") : ("Fire at " + attackTarg.thing.Label + " (out of range)"));
						act = null;
					}
					else if (myPawn.story.WorkIsDisabled(WorkType.Soldier))
					{
						label = "Fire at " + attackTarg.thing.Label + " (" + myPawn.Label + " cannot fight)";
						act = null;
					}
					else
					{
						label = "Fire at " + attackTarg.thing.Label;
						act = delegate
						{
							myPawn.MindHuman.TakeOrderedJob(new Job(JobType.AttackStatic, attackTarg));
						};
					}
					list.Add(new FloatMenuChoice(label, act, FloatMenuPriority.High));
				}
				string choiceLabel = "Melee attack " + attackTarg.thing.Label;
				Action act2;
				if (myPawn.story.WorkIsDisabled(WorkType.Soldier))
				{
					choiceLabel = choiceLabel + " (" + myPawn.Label + " cannot fight)";
					act2 = null;
				}
				else
				{
					act2 = delegate
					{
						Job job = new Job(JobType.AttackMelee, attackTarg);
						Pawn pawn = attackTarg.thing as Pawn;
						if (pawn != null)
						{
							job.killIncappedTarget = pawn.Incapacitated;
							choiceLabel += " to death";
						}
						myPawn.MindHuman.TakeOrderedJob(job);
					};
				}
				list.Add(new FloatMenuChoice(choiceLabel, act2, FloatMenuPriority.High));
			}
			TargetPack arrestTarg = GenUI.ClickTargetUnderMouse(TargetingParameters.ForArrest(myPawn));
			if (arrestTarg != null && arrestTarg.thing != null && arrestTarg.thing != myPawn && ((Pawn)(Thing)arrestTarg).raceDef.humanoid)
			{
				Pawn pTarg = (Pawn)arrestTarg.thing;
				Action act3 = delegate
				{
					Building_Bed building_Bed2 = BedUtility.FindBedFor(pTarg, TeamType.Prisoner, checkSocialProperness: false);
					if (building_Bed2 == null)
					{
						UI_Messages.Message(string.Concat(myPawn, " cannot arrest ", pTarg.characterName, ": No available enclosed prisoner-marked bed."));
					}
					else if (!myPawn.CanReserve(arrestTarg, ReservationType.Total))
					{
						UI_Messages.Message(string.Concat(myPawn, " cannot arrest ", pTarg.characterName, ": Reserved by someone else."));
					}
					else
					{
						Job newJob2 = new Job(JobType.Arrest, pTarg, building_Bed2);
						myPawn.MindHuman.TakeOrderedJob(newJob2);
					}
				};
				list.Add(new FloatMenuChoice("Arrest " + arrestTarg.thing.Label, act3, FloatMenuPriority.Medium));
			}
			int num = Gen.NumSquaresInRadius(2.9f);
			for (int i = 0; i < num; i++)
			{
				IntVec3 curLoc = Gen.RadialPattern[i] + clickSq;
				if (!curLoc.Standable())
				{
					continue;
				}
				if (myPawn.CanReach(curLoc, adjacentIsOK: false) && curLoc != myPawn.Position)
				{
					Action act4 = delegate
					{
						myPawn.MindHuman.TakeOrderedJob(new Job(JobType.Goto, new TargetPack(curLoc)));
					};
					FloatMenuChoice floatMenuChoice = new FloatMenuChoice("Go here", act4, FloatMenuPriority.Low);
					floatMenuChoice.autoTakeable = true;
					list.Add(floatMenuChoice);
				}
				break;
			}
		}
		TargetPack targetPack = GenUI.ClickTargetUnderMouse(TargetingParameters.ForRescue(myPawn));
		if (targetPack != null && targetPack.thing != null)
		{
			Pawn victim = (Pawn)targetPack.thing;
			if (victim.raceDef.humanoid && !victim.IsInBed() && myPawn.CanReserve(victim, ReservationType.Total))
			{
				Action act5 = delegate
				{
					TeamType teamType = victim.Team;
					if (victim.Team.IsHostileToTeam(TeamType.Colonist))
					{
						teamType = TeamType.Prisoner;
					}
					Building_Bed building_Bed = BedUtility.FindBedFor(victim, teamType, checkSocialProperness: false);
					if (building_Bed == null)
					{
						if (teamType != TeamType.Colonist)
						{
							UI_Messages.Message(string.Concat(myPawn, " cannot capture ", victim, ": No available enclosed prisoner-marked bed."));
						}
						else
						{
							UI_Messages.Message(string.Concat(myPawn, " cannot rescue ", victim, ": No available usable non-prisoner bed."));
						}
					}
					else
					{
						JobType jType = ((teamType != TeamType.Colonist) ? JobType.Capture : JobType.Rescue);
						Job newJob = new Job(jType, victim, building_Bed);
						myPawn.MindHuman.TakeOrderedJob(newJob);
					}
				};
				string text = ((targetPack.thing.Team == TeamType.Colonist) ? "Rescue" : "Capture");
				list.Add(new FloatMenuChoice(text + " " + targetPack.thing.Label, act5));
			}
		}
		Equipment equipment = Find.Grids.ThingAt<Equipment>(clickSq);
		if (equipment != null)
		{
			Action act6 = delegate
			{
				equipment.GetComp<CompForbiddable>().forbidden = false;
				myPawn.MindHuman.TakeOrderedJob(new Job(JobType.Equip, new TargetPack(equipment)));
			};
			list.Add(new FloatMenuChoice("Equip " + equipment.Label, act6));
		}
		if (myPawn.equipment.Primary != null)
		{
			Building_EquipmentRack building_EquipmentRack = Find.Grids.ThingAt<Building_EquipmentRack>(clickSq);
			if (building_EquipmentRack != null)
			{
				foreach (IntVec3 sq in Gen.SquaresOccupiedBy(building_EquipmentRack))
				{
					if (sq.ContainedStorable() == null && myPawn.CanReserve(sq, ReservationType.Store))
					{
						Action act7 = delegate
						{
							Equipment equipment2 = myPawn.equipment.DropEquipment(myPawn.equipment.Primary);
							equipment2.SetForbidden(value: false);
							myPawn.MindHuman.TakeOrderedJob(new Job(JobType.HaulToSlot, equipment2, sq));
						};
						list.Add(new FloatMenuChoice("Deposit " + myPawn.equipment.Primary.Label + " in " + building_EquipmentRack.def.label.ToLower(), act7));
						break;
					}
				}
			}
			TargetPack targetPack2 = GenUI.ClickTargetUnderMouse(TargetingParameters.ForSelf(myPawn));
			if (targetPack2 != null && targetPack2.thing != null)
			{
				Action act8 = delegate
				{
					myPawn.equipment.DropEquipment(myPawn.equipment.Primary);
					myPawn.MindHuman.TakeOrderedJob(new Job(JobType.Wait, 20));
				};
				list.Add(new FloatMenuChoice("Drop " + myPawn.equipment.Primary.Label, act8));
			}
		}
		foreach (Thing item in Find.Grids.ThingsAt(clickSq))
		{
			foreach (FloatMenuChoice item2 in item.GetFloatMenuChoicesFor(myPawn))
			{
				list.Add(item2);
			}
		}
		if (!myPawn.MindHuman.drafted)
		{
			foreach (Thing item3 in Find.Grids.ThingsAt(clickSq))
			{
				foreach (int value in Enum.GetValues(typeof(WorkType)))
				{
					JobGiver_WorkRoot thinkNode = myPawn.ThinkNodeRoot.GetThinkNode<JobGiver_WorkRoot>();
					List<Job> list2 = thinkNode.ForcingJobsOn(item3, (WorkType)value).ToList();
					if (list2.Count == 0)
					{
						continue;
					}
					foreach (Job forcingJob in list2)
					{
						string label2;
						Action act9;
						if (!myPawn.WorkSettings.ActiveWorksByPriority.Contains((WorkType)value))
						{
							label2 = "Cannot prioritize (" + myPawn.characterName + " is not a " + ((WorkType)value).GetDefinition().pawnLabel + ")";
							act9 = null;
						}
						else if (item3.IsForbidden())
						{
							label2 = "Cannot prioritize (" + item3.Label + " is forbidden)";
							act9 = null;
						}
						else if (item3.def.eType == EntityType.Building_ResearchBench)
						{
							label2 = "Cannot prioritize (research is a long-term task)";
							act9 = null;
						}
						else
						{
							string text2 = ((value != 9 || item3.def.eType == EntityType.Blueprint || item3.def.eType == EntityType.BuildingFrame) ? ((WorkType)value).GetDefinition().gerundLabel.ToLower() : "repairing");
							label2 = "Prioritize " + text2 + " " + item3.Label;
							Thing lambdaThing = item3;
							WorkType lambdaWork = (WorkType)value;
							act9 = delegate
							{
								myPawn.ThinkNodeRoot.GetThinkNode<JobGiver_WorkRoot>().StartForcedWorkOn(forcingJob, lambdaThing, lambdaWork);
							};
						}
						list.Add(new FloatMenuChoice(label2, act9));
					}
				}
			}
		}
		return list.OrderByDescending((FloatMenuChoice ch) => ch.priority).ToList();
	}
}
