public class Genner_Minerals : Genner_Scatterer
{
	private const int MineralChunkSize = 29;

	public Genner_Minerals()
	{
		minSpacing = 5f;
		numberPerHundredSquare = 10f;
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
		if (Find.Grids.ThingAt(tryLoc, EntityType.Rock) == null)
		{
			return false;
		}
		return true;
	}

	public override void AddThingAt(IntVec3 loc)
	{
		foreach (IntVec3 item in GridShapeMaker.IrregularLump(loc, 29))
		{
			ThingMaker.Spawn(EntityType.Mineral, item);
		}
	}
}
