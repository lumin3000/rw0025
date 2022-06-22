using UnityEngine;

public static class BuildingTrashUtility
{
	private const int AttackJobTimeLimit = 1000;

	public static bool IsGoodTrashTargetFor(Building b, Pawn pawn)
	{
		if (b.def.eType == EntityType.Sandbags || b.def.eType == EntityType.Building_PowerConduit || b.def.eType == EntityType.Building_Grave || b.def.eType == EntityType.Building_BlastingCharge)
		{
			return false;
		}
		if (!b.def.useStandardHealth || !pawn.CanReach(b, adjacentIsOK: true) || b.IsBurningImmobile())
		{
			return false;
		}
		if (!pawn.Team.IsHostileToTeam(b.Team))
		{
			return false;
		}
		return true;
	}

	public static Job AttackJobOnFor(Building b, Pawn pawn)
	{
		Job job = null;
		if (Random.value < 0.7f)
		{
			foreach (Verb allEquipmentVerb in pawn.equipment.AllEquipmentVerbs)
			{
				if (allEquipmentVerb.VerbDef.isBuildingDestroyer)
				{
					job = new Job(JobType.UseVerbOnThing, b);
					job.verbToUse = allEquipmentVerb;
					break;
				}
			}
		}
		float value = Random.value;
		if (value < 0.7f && b.def.Flammable)
		{
			job = new Job(JobType.Ignite, b);
		}
		else if (job == null)
		{
			job = new Job(JobType.AttackMelee, b);
		}
		job.TimeLimit = 1000;
		return job;
	}
}
