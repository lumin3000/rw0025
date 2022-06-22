using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pawn_EquipmentTracker : Saveable
{
	private const int MaxNumSecondaries = 2;

	private Pawn pawn;

	public Equipment Primary;

	public List<Equipment> Secondaries = new List<Equipment>();

	public IEnumerable<Equipment> AllEquipment
	{
		get
		{
			if (Primary != null)
			{
				yield return Primary;
			}
			foreach (Equipment secondary in Secondaries)
			{
				yield return secondary;
			}
		}
	}

	public IEnumerable<Verb> AllEquipmentVerbs
	{
		get
		{
			foreach (Equipment eq in AllEquipment)
			{
				foreach (Verb allVerb in eq.AllVerbs)
				{
					yield return allVerb;
				}
			}
		}
	}

	public Pawn_EquipmentTracker(Pawn newPawn)
	{
		pawn = newPawn;
	}

	public void ExposeData()
	{
		Scribe.LookSaveable(ref Primary, "Primary");
		Scribe.LookList(ref Secondaries, "Secondaries");
		if (Scribe.mode != LoadSaveMode.PostLoadInit)
		{
			return;
		}
		foreach (Equipment item in AllEquipment)
		{
			item.InitVerb();
			item.verb.owner = pawn;
			item.verb.equipment = item;
		}
	}

	public void EquipmentTrackerTick()
	{
		foreach (Equipment item in AllEquipment)
		{
			foreach (Verb allVerb in item.AllVerbs)
			{
				allVerb.VerbTick();
			}
		}
	}

	public void MakeRoomFor(Equipment eq)
	{
		if (eq.def.invType == InventoryType.Primary && Primary != null)
		{
			Thing t = DropEquipment(Primary);
			t.SetForbidden(value: false);
		}
		if (eq.def.invType == InventoryType.Secondary && Secondaries.ToList().Count >= 2)
		{
			Thing t2 = DropEquipment(Secondaries.Last());
			t2.SetForbidden(value: false);
		}
	}

	public Equipment DropEquipment(Equipment eq)
	{
		if (!AllEquipment.Contains(eq))
		{
			Debug.LogWarning(pawn.Label + " tried to drop equipment he didn't have: " + eq);
			return null;
		}
		if (Primary == eq)
		{
			Primary = null;
		}
		else
		{
			Secondaries.Remove(eq);
		}
		ThingMaker.Spawn(eq, ThingDropSpotFinder.BestDropSpotNear(pawn.Position));
		eq.GetComp<CompForbiddable>().forbidden = true;
		return eq;
	}

	public void DropAllEquipment()
	{
		if (Primary != null)
		{
			DropEquipment(Primary);
		}
		while (Secondaries.Any())
		{
			DropEquipment(Secondaries.First());
		}
	}

	public void MakeAndAddEquipment(string defName)
	{
		ThingDefinition def = ThingDefDatabase.ThingDefNamed(defName);
		Equipment newEq = (Equipment)ThingMaker.MakeThing(def);
		AddEquipment(newEq);
	}

	public void AddEquipment(Equipment newEq)
	{
		if (AllEquipment.Where((Equipment eq) => eq.def == newEq.def).Any())
		{
			Debug.LogError(string.Concat("Pawn ", pawn.Label, " got ability ", newEq, " while already having it."));
		}
		else if (newEq.def.invType == InventoryType.Primary && Primary != null)
		{
			Debug.LogError(string.Concat("Pawn ", pawn.Label, " got primary ability ", newEq, " while already having primary ability ", Primary));
		}
		else if (newEq.def.invType == InventoryType.Secondary && Secondaries.Count() >= 2)
		{
			Debug.LogError(string.Concat("Pawn ", pawn.Label, " got secondary ability ", newEq, " while already having the max number of secondaries."));
		}
		else
		{
			if (newEq.def.invType == InventoryType.Primary)
			{
				Primary = newEq;
			}
			else
			{
				Secondaries.Add(newEq);
			}
			newEq.InitVerb();
			newEq.verb.owner = pawn;
		}
	}

	public IEnumerable<Command> GetEquipmentCommandOptions()
	{
		int ind = 0;
		foreach (Equipment equipment in AllEquipment)
		{
			foreach (Verb verb in equipment.AllVerbs)
			{
				if (verb.VerbDef.hasStandardCommand)
				{
					Command_Verb newOpt = new Command_Verb
					{
						tipDef = new TooltipDef(equipment.Label + ": " + equipment.def.desc),
						icon = equipment.def.uiIcon,
						action = delegate
						{
							Find.TabDirect.targeter.VerbCommandStart(verb);
						},
						commandVerb = verb
					};
					if (verb.owner.Team != TeamType.Colonist)
					{
						newOpt.Disable("Cannot order non-controlled units.");
					}
					if (ind == 0)
					{
						newOpt.hotKey = KeyCode.F;
					}
					if (ind == 1)
					{
						newOpt.hotKey = KeyCode.R;
					}
					if (ind == 2)
					{
						newOpt.hotKey = KeyCode.H;
					}
					if (!pawn.MindHuman.drafted)
					{
						newOpt.Disable(pawn.characterName + " is not drafted.");
					}
					yield return newOpt;
					ind++;
				}
			}
		}
	}

	public bool TryStartAttack(TargetPack targ)
	{
		if (Primary == null)
		{
			Debug.LogWarning(string.Concat(pawn, " trying to attack ", targ, " without a primary equipment."));
			return false;
		}
		if (pawn.stances.FullBodyBusy)
		{
			return false;
		}
		if (pawn.story.DisabledWorkTags.Contains(WorkTags.Violent))
		{
			return false;
		}
		return Primary.verb.TryStartCastOn(targ);
	}
}
