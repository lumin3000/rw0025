using System;
using System.Collections.Generic;

public class TargetingParameters
{
	public bool canTargetLocations;

	public bool canTargetSelf;

	public bool canTargetPawns;

	public bool canTargetFires;

	public bool canTargetBuildings;

	public bool canTargetSmallObjects;

	public List<TeamType> targetTeams = new List<TeamType>();

	public Predicate<TargetPack> validator;

	public bool onlyTargetFlammables;

	public Thing targetSpecificThing;

	public bool mustBeSelectable;

	public bool neverTargetDoors;

	public bool neverTargetIncapacitated;

	public bool onlyTargetBarriers;

	public bool onlyTargetDamagedThings;

	public bool worldObjectTargetsMustBeAutoAttackable;

	public bool onlyTargetIncapacitatedPawns;

	public static List<TeamType> AllTeams
	{
		get
		{
			List<TeamType> list = new List<TeamType>();
			foreach (int value in Enum.GetValues(typeof(TeamType)))
			{
				list.Add((TeamType)value);
			}
			return list;
		}
	}

	public bool CanTarget(TargetPack targ)
	{
		if (validator != null && !validator(targ))
		{
			return false;
		}
		if (targ.thing == null)
		{
			return canTargetLocations;
		}
		if (neverTargetDoors && targ.thing.def.eType == EntityType.Door)
		{
			return false;
		}
		if (onlyTargetDamagedThings && targ.thing.health == targ.thing.def.maxHealth)
		{
			return false;
		}
		if (onlyTargetFlammables && !targ.thing.def.Flammable)
		{
			return false;
		}
		if (mustBeSelectable && !targ.thing.Selectable)
		{
			return false;
		}
		if (targetSpecificThing != null && targ.thing == targetSpecificThing)
		{
			return true;
		}
		if (canTargetFires && targ.thing.def.eType == EntityType.Fire)
		{
			return true;
		}
		if (canTargetPawns && targ.thing.def.eType == EntityType.Pawn)
		{
			if (((Pawn)targ.thing).Incapacitated)
			{
				if (neverTargetIncapacitated)
				{
					return false;
				}
			}
			else if (onlyTargetIncapacitatedPawns)
			{
				return false;
			}
			if (!targetTeams.Contains(targ.thing.Team))
			{
				return false;
			}
			return true;
		}
		if (canTargetBuildings && targ.thing.def.category == EntityCategory.Building)
		{
			if (onlyTargetBarriers && !targ.thing.def.isBarrier)
			{
				return false;
			}
			if (!targetTeams.Contains(targ.thing.Team))
			{
				return false;
			}
			return true;
		}
		if (canTargetSmallObjects)
		{
			if (worldObjectTargetsMustBeAutoAttackable && !targ.thing.def.isAutoAttackableWorldObject)
			{
				return false;
			}
			return true;
		}
		return false;
	}

	public static TargetingParameters ForSelf(Pawn p)
	{
		TargetingParameters targetingParameters = new TargetingParameters();
		targetingParameters.targetSpecificThing = p;
		return targetingParameters;
	}

	public static TargetingParameters ForArrest(Pawn p)
	{
		TargetingParameters targetingParameters = new TargetingParameters();
		targetingParameters.canTargetPawns = true;
		targetingParameters.targetTeams.Add(TeamType.Neutral);
		targetingParameters.targetTeams.Add(TeamType.Colonist);
		targetingParameters.targetTeams.Add(TeamType.Traveler);
		targetingParameters.targetTeams.Add(TeamType.Prisoner);
		targetingParameters.neverTargetIncapacitated = true;
		targetingParameters.validator = delegate(TargetPack targ)
		{
			if (!targ.HasThing)
			{
				return false;
			}
			if (!targ.thing.Team.CanBeArrested())
			{
				return false;
			}
			return (targ.thing.Team != TeamType.Prisoner || !targ.thing.Position.IsInPrisonCell()) ? true : false;
		};
		return targetingParameters;
	}

	public static TargetingParameters ForAttack(Pawn p)
	{
		TargetingParameters targetingParameters = new TargetingParameters();
		targetingParameters.canTargetPawns = true;
		targetingParameters.targetTeams = AllTeams;
		targetingParameters.canTargetBuildings = true;
		targetingParameters.canTargetSmallObjects = true;
		targetingParameters.worldObjectTargetsMustBeAutoAttackable = true;
		targetingParameters.validator = (TargetPack targ) => targ.HasThing && (TeamType.Colonist.IsHostileToTeam(targ.thing.Team) || (targ.thing is Pawn && !((Pawn)targ.thing).raceDef.humanoid));
		return targetingParameters;
	}

	public static TargetingParameters ForRescue(Pawn p)
	{
		TargetingParameters targetingParameters = new TargetingParameters();
		targetingParameters.canTargetPawns = true;
		targetingParameters.targetTeams = AllTeams;
		targetingParameters.onlyTargetIncapacitatedPawns = true;
		return targetingParameters;
	}
}
