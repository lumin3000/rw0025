public class FertilityGrid
{
	public float FertilityAt(IntVec3 loc)
	{
		return CalculateFertilityAt(loc);
	}

	private float CalculateFertilityAt(IntVec3 loc)
	{
		Thing thing = Find.Grids.BlockerAt(loc);
		if (thing != null && thing.def.fertility >= 0f)
		{
			return thing.def.fertility;
		}
		return Find.TerrainGrid.TerrainAt(loc).fertility;
	}
}
