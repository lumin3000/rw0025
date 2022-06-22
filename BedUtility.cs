using System.Collections.Generic;
using System.Linq;

public static class BedUtility
{
	public static Building_Bed FindBedFor(Pawn p)
	{
		return FindBedFor(p, p.Team, checkSocialProperness: true);
	}

	public static Building_Bed FindBedFor(Pawn p, TeamType pTeam, bool checkSocialProperness)
	{
		GenScan.CloseToThingValidator closeToThingValidator = delegate(Thing t)
		{
			if (!p.CanReach(t, adjacentIsOK: false))
			{
				return false;
			}
			if (!t.HasAir())
			{
				return false;
			}
			Building_Bed building_Bed2 = (Building_Bed)t;
			if (building_Bed2.forPrisoners && pTeam == TeamType.Colonist)
			{
				return false;
			}
			if (building_Bed2.forPrisoners && Find.Grids.GetRoomAt(building_Bed2.Position) == null)
			{
				return false;
			}
			if (!building_Bed2.forPrisoners && pTeam != TeamType.Colonist)
			{
				return false;
			}
			if (building_Bed2.owner != null && building_Bed2.owner != p)
			{
				return false;
			}
			return (!checkSocialProperness || building_Bed2.IsSociallyProperForUseBy(p, pTeam)) ? true : false;
		};
		if (p.ownership != null && p.ownership.ownedBed != null && closeToThingValidator(p.ownership.ownedBed))
		{
			return p.ownership.ownedBed;
		}
		IEnumerable<ThingDefinition> enumerable = from def in ThingDefDatabase.AllThingDefinitions
			where def.eType == EntityType.Building_Bed
			orderby def.restEffectiveness descending
			select def;
		foreach (ThingDefinition item in enumerable)
		{
			Building_Bed building_Bed = (Building_Bed)GenScan.ClosestReachableThing(p.Position, Find.BuildingManager.AllBuildingsColonistOfDef(item), closeToThingValidator);
			if (building_Bed != null)
			{
				if (p.ownership != null)
				{
					p.ownership.UnclaimBed();
				}
				return building_Bed;
			}
		}
		return null;
	}

	public static Building_Bed CurrentBed(this Pawn p)
	{
		Job curJob = p.jobs.CurJob;
		if (curJob == null || curJob.jType != JobType.Sleep)
		{
			return null;
		}
		Building_Bed building_Bed = curJob.targetA.thing as Building_Bed;
		if (building_Bed.CurSleeper != p)
		{
			return null;
		}
		return building_Bed;
	}

	public static bool IsInBed(this Pawn p)
	{
		return p.CurrentBed() != null;
	}

	public static bool IsSleeping(this Pawn p)
	{
		return p.IsInBed();
	}
}
