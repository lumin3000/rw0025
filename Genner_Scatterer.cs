using System.Collections.Generic;

public abstract class Genner_Scatterer
{
	protected float minSpacing = 10f;

	protected float numberPerHundredSquare = 1f;

	protected bool spotMustBeStandable;

	protected List<IntVec3> PlaceSpots = new List<IntVec3>();

	protected int NumPlaceThings
	{
		get
		{
			int num = (int)(10000f / numberPerHundredSquare);
			return Find.Map.Area / num;
		}
	}

	public abstract void AddThingAt(IntVec3 Loc);

	public void AddThings()
	{
		for (int i = 0; i < NumPlaceThings; i++)
		{
			IntVec3 intVec = RandomThingLoc();
			PlaceSpots.Add(intVec);
			AddThingAt(intVec);
		}
	}

	private IntVec3 RandomThingLoc()
	{
		int num = 0;
		IntVec3 intVec;
		do
		{
			intVec = GenMapGen.RandomSpot_NotEdge(15);
			if (SpotIsValid(intVec))
			{
				return intVec;
			}
			num++;
		}
		while (num <= 200);
		return intVec;
	}

	protected virtual bool SpotIsValid(IntVec3 tryLoc)
	{
		if (MapGenerator.sanctity.SanctityAt(tryLoc) > 0f)
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
		if (spotMustBeStandable && !tryLoc.Standable())
		{
			return false;
		}
		return true;
	}
}
