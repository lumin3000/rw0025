using System;

public class PathGrid
{
	public int[] pathGrid;

	private static int mapSizePowTwo;

	private static ushort gridSizeX;

	private static ushort gridSizeY;

	private static ushort gridSizeXMinus1;

	private static ushort gridSizeZLog2;

	private int lookingInd;

	private Thing lookingBlocker;

	public PathGrid()
	{
		ResetPathGrid();
	}

	public void ResetPathGrid()
	{
		mapSizePowTwo = Find.Map.info.PowerOfTwoOverMapSize;
		gridSizeX = (ushort)mapSizePowTwo;
		gridSizeY = (ushort)mapSizePowTwo;
		gridSizeXMinus1 = (ushort)(gridSizeX - 1);
		gridSizeZLog2 = (ushort)Math.Log((int)gridSizeY, 2.0);
		pathGrid = new int[gridSizeX * gridSizeY];
	}

	public int PerceivedPathCostAt(IntVec3 loc)
	{
		return pathGrid[CoordsToIndex(loc)];
	}

	public void RecalculatePathCostUnder(Thing t)
	{
		if (!Find.Map.initialized)
		{
			return;
		}
		foreach (IntVec3 item in Gen.SquaresOccupiedBy(t))
		{
			RecalculatePathCostAt(item);
		}
	}

	public void RecalculatePathCostAt(IntVec3 loc)
	{
		if (loc.InBounds())
		{
			pathGrid[CoordsToIndex(loc)] = CalculatedCostAt(loc, perceived: true);
		}
	}

	public void RecalculateAllPathCosts()
	{
		foreach (IntVec3 allSquare in Find.Map.AllSquares)
		{
			RecalculatePathCostAt(allSquare);
		}
	}

	private static int CoordsToIndex(IntVec3 vec)
	{
		return (vec.z << (int)gridSizeZLog2) + vec.x;
	}

	private static IntVec3 IndexToCoords(int index)
	{
		int newX = index & gridSizeXMinus1;
		int newZ = index >> (int)gridSizeZLog2;
		return new IntVec3(newX, 0, newZ);
	}

	public static int CalculatedCostAt(IntVec3 loc, bool perceived)
	{
		int num = 0;
		num += Find.TerrainGrid.TerrainAt(loc).pathCost;
		foreach (Thing item in Find.Grids.ThingsAt(loc))
		{
			num += item.def.pathCost;
		}
		if (perceived)
		{
			IntVec3[] adjacentSquaresAndInside = Gen.AdjacentSquaresAndInside;
			for (int i = 0; i < adjacentSquaresAndInside.Length; i++)
			{
				IntVec3 intVec = adjacentSquaresAndInside[i];
				IntVec3 square = loc + intVec;
				Fire fire = Find.Grids.ThingAt<Fire>(square);
				if (fire != null && fire.parent == null)
				{
					num = ((intVec.x != 0 || intVec.z != 0) ? (num + 150) : (num + 1000));
				}
			}
		}
		return num;
	}
}
