using System.Collections;
using System.Collections.Generic;
using System.Linq;

internal class WorkGiver_PlantWork : WorkGiver
{
	public override IEnumerable PotentialWorkTargets
	{
		get
		{
			IEnumerable<Designation> plantEnum = Find.DesignationManager.designationList.Where((Designation des) => des.dType == DesignationType.CutPlant || des.dType == DesignationType.HarvestPlant);
			foreach (Designation plantDes in plantEnum)
			{
				yield return plantDes.target.thing;
			}
		}
	}

	public WorkGiver_PlantWork(Pawn pawn)
		: base(pawn)
	{
		wType = WorkType.PlantCutting;
	}

	public override Job StartingJobOn(Thing t)
	{
		if (!t.def.IsPlant)
		{
			return null;
		}
		if (!pawn.CanReserve(t, ReservationType.Total))
		{
			return null;
		}
		foreach (Designation item in Find.DesignationManager.AllDesignationsOn(t))
		{
			if (item.dType == DesignationType.HarvestPlant)
			{
				if (item.dType == DesignationType.HarvestPlant && !((Plant)t).HarvestableNow)
				{
					return null;
				}
				return new Job(JobType.Harvest, t);
			}
			if (item.dType == DesignationType.CutPlant)
			{
				return new Job(JobType.CutPlant, t);
			}
		}
		return null;
	}
}
