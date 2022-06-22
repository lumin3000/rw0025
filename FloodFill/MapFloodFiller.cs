using System;
using System.Collections.Generic;

namespace FloodFill
{
	public static class MapFloodFiller
	{
		private enum FloodFillMode
		{
			Reachability,
			Room
		}

		private static int[,,] reachGrid;

		private static int newRegionIndex;

		private static FloodFillMode mode;

		private static int numSquaresMade;

		private static bool failed;

		public static int maxUsedIndex;

		private static int[] processed;

		private static Queue<FloodFillRange> ranges;

		private static int mapSizePowTwo;

		private static ushort gridSizeX;

		private static ushort gridSizeY;

		private static ushort gridSizeXMinus1;

		private static ushort gridSizeZLog2;

		private static Thing[,,] blockerGrid;

		private static Thing checkBlocker;

		private static int mapWidth;

		private static int mapHeight;

		public static List<IntVec3> SquaresEnclosedWith(IntVec3 root)
		{
			mode = FloodFillMode.Room;
			reachGrid = new int[Find.Map.Size.x, 1, Find.Map.Size.z];
			blockerGrid = Find.Grids.barrierGrid;
			PrepareFloodFill();
			FloodFillFrom(root);
			maxUsedIndex++;
			if (failed)
			{
				return null;
			}
			List<IntVec3> list = new List<IntVec3>();
			for (int i = 0; i < Find.Map.Size.x; i++)
			{
				for (int j = 0; j < Find.Map.Size.y; j++)
				{
					for (int k = 0; k < Find.Map.Size.z; k++)
					{
						if (reachGrid[i, j, k] == newRegionIndex)
						{
							list.Add(new IntVec3(i, j, k));
						}
					}
				}
			}
			return list;
		}

		public static void MakeReachabilityRegionFrom(IntVec3 root, int[,,] passedReachGrid)
		{
			mode = FloodFillMode.Reachability;
			reachGrid = passedReachGrid;
			blockerGrid = Find.Grids.blockerGrid;
			PrepareFloodFill();
			FloodFillFrom(root);
			maxUsedIndex++;
		}

		private static void PrepareFloodFill()
		{
			mapSizePowTwo = Find.Map.info.PowerOfTwoOverMapSize;
			gridSizeX = (ushort)mapSizePowTwo;
			gridSizeY = (ushort)mapSizePowTwo;
			gridSizeXMinus1 = (ushort)(gridSizeX - 1);
			gridSizeZLog2 = (ushort)Math.Log((int)gridSizeY, 2.0);
			mapWidth = Find.Map.Size.x;
			mapHeight = Find.Map.Size.z;
			if (processed == null)
			{
				processed = new int[mapSizePowTwo * mapSizePowTwo];
			}
			int count = mapWidth * mapHeight / 100;
			ranges = new Queue<FloodFillRange>(count);
			newRegionIndex = maxUsedIndex + 1;
			numSquaresMade = 0;
			failed = false;
		}

		private static void FloodFillFrom(IntVec3 root)
		{
			LinearFill(root.x, root.z);
			while (ranges.Count > 0 && !failed && !failed)
			{
				FloodFillRange floodFillRange = ranges.Dequeue();
				if (floodFillRange.y == 0 || floodFillRange.y == mapHeight - 1)
				{
					HitMapEdge();
				}
				int num = CoordsToIndex(floodFillRange.minX, floodFillRange.y + 1);
				int num2 = CoordsToIndex(floodFillRange.minX, floodFillRange.y - 1);
				int rootY = floodFillRange.y - 1;
				int rootY2 = floodFillRange.y + 1;
				for (int i = floodFillRange.minX; i <= floodFillRange.maxX; i++)
				{
					if (floodFillRange.y > 0 && !IndexWasProcessed(num2) && IndexPassable(num2))
					{
						LinearFill(i, rootY);
					}
					if (floodFillRange.y < mapHeight - 1 && !IndexWasProcessed(num) && IndexPassable(num))
					{
						LinearFill(i, rootY2);
					}
					num++;
					num2++;
				}
			}
		}

		private static void LinearFill(int rootX, int rootY)
		{
			int num = CoordsToIndex(rootX, rootY);
			int num2 = rootX;
			int num3 = num;
			do
			{
				IntVec3 intVec = IndexToCoords(num3);
				reachGrid[intVec.x, intVec.y, intVec.z] = newRegionIndex;
				processed[num3] = newRegionIndex;
				num2--;
				num3--;
				if (num2 < 0)
				{
					HitMapEdge();
					break;
				}
			}
			while (IndexPassable(num3));
			num2++;
			int num4 = rootX;
			num3 = num;
			do
			{
				IntVec3 intVec2 = IndexToCoords(num3);
				reachGrid[intVec2.x, intVec2.y, intVec2.z] = newRegionIndex;
				processed[num3] = newRegionIndex;
				num4++;
				num3++;
				if (num4 >= mapWidth)
				{
					HitMapEdge();
					break;
				}
			}
			while (IndexPassable(num3));
			num4--;
			numSquaresMade += num4 - num2 + 1;
			FloodFillRange item = new FloodFillRange(num2, num4, rootY);
			ranges.Enqueue(item);
		}

		private static bool IndexPassable(int index)
		{
			int num = index & gridSizeXMinus1;
			int num2 = index >> (int)gridSizeZLog2;
			if (mode == FloodFillMode.Reachability)
			{
				checkBlocker = blockerGrid[num, 0, num2];
				if (checkBlocker != null && checkBlocker.def.passability == Traversability.Impassable)
				{
					return false;
				}
			}
			else
			{
				checkBlocker = blockerGrid[num, 0, num2];
				if (checkBlocker != null)
				{
					return false;
				}
			}
			return true;
		}

		private static bool IndexWasProcessed(int index)
		{
			return processed[index] == newRegionIndex;
		}

		private static int CoordsToIndex(int x, int y)
		{
			return (y << (int)gridSizeZLog2) + x;
		}

		private static IntVec3 IndexToCoords(int index)
		{
			int newX = index & gridSizeXMinus1;
			int newZ = index >> (int)gridSizeZLog2;
			return new IntVec3(newX, 0, newZ);
		}

		private static void HitMapEdge()
		{
			if (mode == FloodFillMode.Room)
			{
				failed = true;
			}
		}
	}
}
