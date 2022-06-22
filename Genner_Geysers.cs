public class Genner_Geysers : Genner_Scatterer
{
	private const int ClearSpaceSize = 30;

	private const int MinDistToPlayerStart = 15;

	public Genner_Geysers()
	{
		minSpacing = 35f;
		numberPerHundredSquare = 0.9f;
	}

	protected override bool SpotIsValid(IntVec3 tryLoc)
	{
		float num = MapGenerator.sanctity.SanctityAt(tryLoc);
		if (num > 0f)
		{
			return false;
		}
		foreach (IntVec3 placeSpot in PlaceSpots)
		{
			if ((placeSpot - tryLoc).LengthHorizontal < minSpacing)
			{
				return false;
			}
		}
		if ((Find.Map.Center - tryLoc).LengthHorizontalSquared < 15f)
		{
			return false;
		}
		for (int i = 0; i < 60; i++)
		{
			IntVec3 intVec = tryLoc + Gen.RadialPattern[i];
			if (!intVec.InBounds() || !Find.TerrainGrid.TerrainAt(intVec).surfacesSupported.Contains(SurfaceType.Heavy))
			{
				return false;
			}
		}
		return true;
	}

	public override void AddThingAt(IntVec3 loc)
	{
		foreach (IntVec3 item in GridShapeMaker.IrregularLump(loc, 30))
		{
			Find.Grids.BlockerAt(item)?.Destroy();
		}
		ThingMaker.Spawn(EntityType.SteamGeyser, loc, IntRot.random);
	}
}
