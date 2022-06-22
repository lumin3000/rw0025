using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WorkGiver_Constructor : WorkGiver
{
	public override IEnumerable PotentialWorkTargets
	{
		get
		{
			IEnumerable<Building> first = Find.BuildingManager.AllBuildingsColonistOfType(EntityType.BuildingFrame);
			IEnumerable<Building> second = Find.BuildingManager.AllBuildingsColonistOfType(EntityType.Blueprint);
			return first.Concat(second);
		}
	}

	public WorkGiver_Constructor(Pawn newPawn)
		: base(newPawn)
	{
		wType = WorkType.Construction;
	}

	public override Job StartingJobOn(Thing t)
	{
		if (t.Team != TeamType.Colonist)
		{
			return null;
		}
		Blueprint blueprint = t as Blueprint;
		if (blueprint != null)
		{
			Thing smallThingBlockingPlacement = blueprint.SmallThingBlockingPlacement;
			if (smallThingBlockingPlacement != null)
			{
				return HaulAIUtility.HaulJobFor(pawn, smallThingBlockingPlacement, ignoreDesignation: true);
			}
			foreach (IntVec3 item in Gen.SquaresOccupiedBy(blueprint))
			{
				if (item.IsBurningImmobile())
				{
					return null;
				}
			}
			if (!pawn.CanReserve(blueprint, ReservationType.Construction))
			{
				return null;
			}
			return new Job(JobType.Construct, t);
		}
		BuildingFrame buildingFrame = t as BuildingFrame;
		if (buildingFrame != null)
		{
			if (!pawn.CanReserve(buildingFrame, ReservationType.Construction))
			{
				return null;
			}
			if (buildingFrame.IsBurningImmobile())
			{
				return null;
			}
			return new Job(JobType.Construct, t);
		}
		return null;
	}
}
