using System.Collections.Generic;
using System.Linq;

public class Building_PlantGrower : Building
{
	public int NumPlantingSpaces => Gen.SquaresOccupiedBy(this).Count();

	public IEnumerable<Plant> PlantsOnMe
	{
		get
		{
			foreach (IntVec3 sq in Gen.SquaresOccupiedBy(this))
			{
				foreach (Thing t in Find.Grids.ThingsAt(sq))
				{
					if (t.def.IsPlant)
					{
						yield return (Plant)t;
					}
				}
			}
		}
	}

	public override void Destroy()
	{
		base.Destroy();
		if (!def.plantsDestroyWithMe)
		{
			return;
		}
		foreach (Plant item in PlantsOnMe.ToList())
		{
			item.Destroy();
		}
	}

	public override string GetInspectString()
	{
		return base.GetInspectString();
	}
}
