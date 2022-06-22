using System.Collections;
using System.Linq;

public class WorkGiver_Hauler : WorkGiver
{
	public override IEnumerable PotentialWorkTargets => Find.Map.thingLister.spawnedHaulables.Concat(Find.DesignationManager.DesignatedThingsToHaul);

	public WorkGiver_Hauler(Pawn pawn)
		: base(pawn)
	{
		wType = WorkType.Hauling;
	}

	public override Job StartingJobOn(Thing t)
	{
		return HaulAIUtility.HaulJobFor(pawn, t, ignoreDesignation: false);
	}
}
