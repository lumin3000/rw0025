using System.Collections;

public class WorkGiver_GrowerSow : WorkGiver
{
	public override IEnumerable PotentialWorkTargets
	{
		get
		{
			foreach (Building_PlantGrower item in Find.BuildingManager.AllBuildingsColonistOfClass<Building_PlantGrower>())
			{
				yield return item;
			}
		}
	}

	public WorkGiver_GrowerSow(Pawn newPawn)
		: base(newPawn)
	{
		wType = WorkType.Growing;
	}

	public override Job StartingJobOn(Thing t)
	{
		Building_PlantGrower building_PlantGrower = t as Building_PlantGrower;
		if (building_PlantGrower != null)
		{
			IntVec3 intVec = default(IntVec3);
			bool flag = false;
			foreach (IntVec3 item in Gen.SquaresOccupiedBy(building_PlantGrower))
			{
				if (building_PlantGrower.def.PlantDefinitionToGrow.CanPlantAt(item) && pawn.CanReserve(item, ReservationType.Sowing))
				{
					flag = true;
					intVec = item;
					break;
				}
			}
			if (flag)
			{
				Job job = new Job(JobType.Sow, intVec);
				job.plantDefToSow = building_PlantGrower.def.PlantDefinitionToGrow;
				return job;
			}
			foreach (IntVec3 item2 in Gen.SquaresOccupiedBy(building_PlantGrower))
			{
				foreach (Thing item3 in Find.Grids.ThingsAt(item2))
				{
					if (item3.def.BlockPlanting && pawn.CanReserve(item3, ReservationType.Total) && item3.def.IsPlant && item3.def != building_PlantGrower.def.PlantDefinitionToGrow)
					{
						return new Job(JobType.CutPlant, item3);
					}
				}
			}
		}
		return null;
	}
}
