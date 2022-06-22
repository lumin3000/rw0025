using System.Collections.Generic;
using UnityEngine;

public static class CoverUtility
{
	public class CoverGiver
	{
		public Thing CoverThing;

		public float BlockChance;

		public CoverGiver(Thing newCoverThing, float newHitChance)
		{
			CoverThing = newCoverThing;
			BlockChance = newHitChance;
		}
	}

	public class CoverGiverSet
	{
		public float overallBlockChance;

		public List<CoverGiver> Givers = new List<CoverGiver>();

		public void AddCover(CoverGiver newGiver)
		{
			Givers.Add(newGiver);
			overallBlockChance += (1f - overallBlockChance) * newGiver.BlockChance;
		}

		public Thing RandomBlockingCoverWeighted()
		{
			float num = 0f;
			foreach (CoverGiver giver in Givers)
			{
				num += giver.BlockChance;
			}
			float num2 = Random.value * num;
			while (num2 > Givers[0].BlockChance)
			{
				num2 -= Givers[0].BlockChance;
				Givers.RemoveAt(0);
			}
			return Givers[0].CoverThing;
		}
	}

	public const float CoverPercent_Corner = 0.75f;

	public static CoverGiverSet CoverGiverSetAtFrom(IntVec3 TargetLoc, IntVec3 ShooterLoc)
	{
		CoverGiverSet coverGiverSet = new CoverGiverSet();
		foreach (IntVec3 item in TargetLoc.AdjacentSquares8Way())
		{
			if (item.InBounds())
			{
				CoverGiver coverGiver = AdjustedCoverGiverInSquare(ShooterLoc, TargetLoc, item);
				if (coverGiver != null)
				{
					coverGiverSet.AddCover(coverGiver);
				}
			}
		}
		return coverGiverSet;
	}

	private static CoverGiver AdjustedCoverGiverInSquare(IntVec3 ShooterLoc, IntVec3 TargetLoc, IntVec3 AdjSq)
	{
		CoverGiver coverGiver = RawCoverGiverIn(AdjSq);
		if (coverGiver == null)
		{
			return null;
		}
		if (ShooterLoc == TargetLoc)
		{
			return null;
		}
		float angleFlat = (ShooterLoc - TargetLoc).AngleFlat;
		float angleFlat2 = (AdjSq - TargetLoc).AngleFlat;
		float num = GenGeo.AngleDifferenceBetween(angleFlat2, angleFlat);
		if (!TargetLoc.AdjacentToCardinal(AdjSq))
		{
			num *= 1.75f;
		}
		if (num < 15f)
		{
			coverGiver.BlockChance *= 1f;
		}
		else if (num < 27f)
		{
			coverGiver.BlockChance *= 0.8f;
		}
		else if (num < 40f)
		{
			coverGiver.BlockChance *= 0.6f;
		}
		else if (num < 52f)
		{
			coverGiver.BlockChance *= 0.4f;
		}
		else
		{
			if (!(num < 65f))
			{
				return null;
			}
			coverGiver.BlockChance *= 0.2f;
		}
		float lengthHorizontal = (ShooterLoc - AdjSq).LengthHorizontal;
		if (lengthHorizontal < 1.9f)
		{
			coverGiver.BlockChance *= 0.3333f;
		}
		else if (lengthHorizontal < 2.9f)
		{
			coverGiver.BlockChance *= 0.66666f;
		}
		return coverGiver;
	}

	public static CoverGiver RawCoverGiverIn(IntVec3 Sq)
	{
		Thing thing = Find.Grids.BlockerAt(Sq);
		if (thing != null)
		{
			if (!thing.def.canBeSeenOver)
			{
				return new CoverGiver(thing, 0.75f);
			}
			return new CoverGiver(thing, thing.def.coverPercent);
		}
		return null;
	}

	public static float SimpleCoverScoreAt(IntVec3 Sq)
	{
		float num = 0f;
		foreach (IntVec3 item in Sq.AdjacentSquares8Way())
		{
			if (item.InBounds())
			{
				CoverGiver coverGiver = RawCoverGiverIn(item);
				if (coverGiver != null)
				{
					num += coverGiver.BlockChance;
				}
			}
		}
		return num;
	}
}
