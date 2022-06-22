using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class RoofCollapseChecker
{
	private const float RoofMaxSupportDistance = 5.9f;

	private const int CrushDamageAmount = 75;

	private static List<IntVec3> collapsingSquares;

	private static List<Thing> crushedThingsThisFrame;

	private static readonly int NumRadialSq;

	private static readonly AudioClip RoofCollapseSound;

	static RoofCollapseChecker()
	{
		collapsingSquares = new List<IntVec3>();
		crushedThingsThisFrame = new List<Thing>();
		RoofCollapseSound = Res.LoadSound("Building/RockCollapse");
		NumRadialSq = Gen.NumSquaresInRadius(5.9f);
	}

	public static void RoofCollapseCheckerUpdate_First()
	{
		if (collapsingSquares.Count <= 0)
		{
			return;
		}
		foreach (IntVec3 collapsingSquare in collapsingSquares)
		{
			DropRoofInSquare(collapsingSquare);
		}
		foreach (IntVec3 collapsingSquare2 in collapsingSquares)
		{
			RoofDefinition roofDefinition = Find.RoofGrid.RoofDefAt(collapsingSquare2);
			foreach (KeyValuePair<string, int> filthLeaving in roofDefinition.filthLeavings)
			{
				FilthUtility.AddFilthAt(collapsingSquare2, filthLeaving.Key, filthLeaving.Value);
			}
			if (roofDefinition.vanishOnCollapse)
			{
				Find.RoofGrid.SetSquareRoofed(collapsingSquare2, EntityType.Undefined);
			}
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("A roof has collapsed because it was too far from any support.");
		stringBuilder.AppendLine();
		if (crushedThingsThisFrame.Count > 0)
		{
			stringBuilder.AppendLine(" These things were crushed:");
			foreach (Thing item in crushedThingsThisFrame)
			{
				if ((item.def.category == EntityCategory.Building || item.def.category == EntityCategory.Pawn || item.def.category == EntityCategory.SmallObject) && item.def.eType != EntityType.Filth && item.def.eType != EntityType.DebrisRock && item.def.eType != EntityType.DebrisSlag)
				{
					stringBuilder.AppendLine("    -" + item.Label);
				}
			}
		}
		else
		{
			stringBuilder.Append(" Nothing was crushed.");
		}
		Find.LetterStack.ReceiveLetter(new Letter(stringBuilder.ToString(), collapsingSquares[0]));
		GenSound.PlaySoundAt(collapsingSquares[0], RoofCollapseSound, 0.08f);
		collapsingSquares.Clear();
		crushedThingsThisFrame.Clear();
	}

	private static void DropRoofInSquare(IntVec3 sq)
	{
		RoofDefinition roofDefinition = Find.RoofGrid.RoofDefAt(sq);
		if (roofDefinition.DropDebrisDefinition.passability == Traversability.Impassable)
		{
			foreach (Thing item in Find.Grids.ThingsAt(sq).ListFullCopy())
			{
				if (!crushedThingsThisFrame.Contains(item))
				{
					crushedThingsThisFrame.Add(item);
				}
				if (item.def.eType != EntityType.Area_Stockpile)
				{
					item.Destroy();
				}
			}
		}
		else
		{
			foreach (Thing item2 in Find.Grids.ThingsAt(sq).ToList())
			{
				if (!crushedThingsThisFrame.Contains(item2))
				{
					crushedThingsThisFrame.Add(item2);
				}
				DamageInfo d = new DamageInfo(DamageType.Bludgeon, 75);
				item2.TakeDamage(d);
			}
			Thing thing = Find.Grids.BlockerAt(sq);
			if (thing != null && !thing.destroyed)
			{
				if (!crushedThingsThisFrame.Contains(thing))
				{
					crushedThingsThisFrame.Add(thing);
				}
				if (thing.def.eType != EntityType.Area_Stockpile)
				{
					thing.Destroy();
				}
			}
		}
		ThingMaker.Spawn(roofDefinition.DropDebrisDefinition, sq);
		for (int i = 0; i < 2; i++)
		{
			Vector3 spawnLoc = sq.ToVector3Shifted();
			spawnLoc += Gen.RandomHorizontalVector(0.6f);
			MoteMaker.ThrowDustPuff(spawnLoc, 2f);
		}
		Find.RoomManager.BarrierRemovedAt(sq);
	}

	public static void Notify_RoofHolderDestroyed(Thing t)
	{
		if (!Find.Map.initialized)
		{
			return;
		}
		EntityType[,] roofGrid = Find.RoofGrid.roofGrid;
		for (int i = 0; i < NumRadialSq; i++)
		{
			IntVec3 intVec = t.Position + Gen.RadialPattern[i];
			if (intVec.InBounds() && roofGrid[intVec.x, intVec.z] != 0 && !IsSupported(intVec))
			{
				collapsingSquares.Add(intVec);
			}
		}
	}

	public static bool IsSupported(IntVec3 roofLoc)
	{
		Grids grids = Find.Grids;
		for (int i = 0; i < NumRadialSq; i++)
		{
			IntVec3 sq = roofLoc + Gen.RadialPattern[i];
			if (!sq.InBounds())
			{
				continue;
			}
			Thing thing = grids.blockerGrid[sq.x, sq.y, sq.z];
			if (thing != null && thing.def.holdsRoof)
			{
				if (DebugSettings.drawRoofs)
				{
					Find.DebugDrawer.MakeDebugSquare(sq, string.Empty, 50, 100);
					Find.DebugDrawer.MakeDebugSquare(roofLoc, string.Empty, 99, 100);
				}
				return true;
			}
		}
		return false;
	}
}
