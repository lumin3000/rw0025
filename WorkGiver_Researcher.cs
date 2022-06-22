using System.Collections;
using System.Linq;

public class WorkGiver_Researcher : WorkGiver
{
	public override IEnumerable PotentialWorkTargets
	{
		get
		{
			if (Find.ResearchManager.CurrentProj == null)
			{
				return Enumerable.Empty<Thing>();
			}
			return Find.BuildingManager.AllBuildingsColonistOfClass<Building_ResearchBench>();
		}
	}

	public WorkGiver_Researcher(Pawn pawn)
		: base(pawn)
	{
		wType = WorkType.Research;
	}

	public override Job StartingJobOn(Thing t)
	{
		if (t.def.eType != EntityType.Building_ResearchBench)
		{
			return null;
		}
		if (!pawn.CanReserve(t, ReservationType.Research))
		{
			return null;
		}
		if (!t.HasAir())
		{
			return null;
		}
		return new Job(JobType.Research, new TargetPack(t));
	}
}
