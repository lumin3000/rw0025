using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public static class PathSolver
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct PathFinderNodeFast
	{
		public int g;

		public int f;

		public ushort parentX;

		public ushort parentZ;

		public int status;
	}

	internal class ComparePathFindingNodeGrid : IComparer<int>
	{
		private PathFinderNodeFast[] grid;

		public ComparePathFindingNodeGrid(PathFinderNodeFast[] grid)
		{
			this.grid = grid;
		}

		public int Compare(int a, int b)
		{
			if (grid[a].f > grid[b].f)
			{
				return 1;
			}
			if (grid[a].f < grid[b].f)
			{
				return -1;
			}
			return 0;
		}
	}

	private const int DefaultMoveTicksCardinal = 14;

	private const int DefaultMoveTicksDiagonal = 19;

	private const int SearchLimit = 100000;

	private const int HEstimate = 10;

	private static FastPriorityQueue<int> openList;

	private static PathFinderNodeFast[] calcGrid;

	private static int statusOpenValue;

	private static int statusClosedValue;

	private static int h;

	private static int locIdx;

	private static int newLocIdx;

	private static ushort locX;

	private static ushort locZ;

	private static IntVec3 locIntVec3;

	private static ushort newLocX;

	private static ushort newLocZ;

	private static int closedNodeCounter;

	private static ushort gridSizeX;

	private static ushort gridSizeY;

	private static ushort gridSizeXMinus1;

	private static ushort gridSizeZLog2;

	private static sbyte[,] directionList;

	private static int endLocIdx;

	private static int newG;

	private static int newSquareCost;

	private static int destinationX;

	private static int destinationZ;

	private static PathRequest req;

	private static PathingParameters pathParams;

	private static PawnPath newPath;

	private static int mapSizePowTwo;

	private static int moveTicksCardinal;

	private static int moveTicksDiagonal;

	private static int[] pathGrid;

	private static Thing[,,] blockerGrid;

	static PathSolver()
	{
		openList = null;
		calcGrid = null;
		statusOpenValue = 1;
		statusClosedValue = 2;
		h = 0;
		locIdx = 0;
		newLocIdx = 0;
		locX = 0;
		locZ = 0;
		locIntVec3 = default(IntVec3);
		newLocX = 0;
		newLocZ = 0;
		closedNodeCounter = 0;
		gridSizeX = 0;
		gridSizeY = 0;
		gridSizeXMinus1 = 0;
		gridSizeZLog2 = 0;
		directionList = new sbyte[8, 2]
		{
			{ 0, -1 },
			{ 1, 0 },
			{ 0, 1 },
			{ -1, 0 },
			{ 1, -1 },
			{ 1, 1 },
			{ -1, 1 },
			{ -1, -1 }
		};
		endLocIdx = 0;
		newG = 0;
		newSquareCost = 0;
		destinationX = -1;
		destinationZ = -1;
		req = null;
		pathParams = null;
		newPath = null;
		mapSizePowTwo = Find.Map.info.PowerOfTwoOverMapSize;
		gridSizeX = (ushort)mapSizePowTwo;
		gridSizeY = (ushort)mapSizePowTwo;
		gridSizeXMinus1 = (ushort)(gridSizeX - 1);
		gridSizeZLog2 = (ushort)Math.Log((int)gridSizeY, 2.0);
		if (calcGrid == null || calcGrid.Length != gridSizeX * gridSizeY)
		{
			calcGrid = new PathFinderNodeFast[gridSizeX * gridSizeY];
		}
		openList = new FastPriorityQueue<int>(new ComparePathFindingNodeGrid(calcGrid));
	}

	public static PawnPath FindPath(PathRequest newReq)
	{
		if (newReq.adjacentIsOK && newReq.dest.thing != null)
		{
			Debug.LogWarning("Cannot path to a Thing with toAdjacent on because pathing to adjacent is assumed. Setting it false.");
			newReq.adjacentIsOK = false;
		}
		req = newReq;
		if (!Find.ReachabilityRegions.ReachableBetween(req.start, req.dest, req.adjacentIsOK))
		{
			return PawnPath.NotFound;
		}
		if (req.pawn == null)
		{
			Debug.LogWarning("Pathing with req.pawn=null.");
		}
		destinationX = req.dest.Loc.x;
		destinationZ = req.dest.Loc.z;
		pathParams = req.pathParams;
		newPath = new PawnPath();
		pathGrid = Find.PathGrid.pathGrid;
		blockerGrid = Find.Grids.blockerGrid;
		if (req.pawn != null)
		{
			moveTicksCardinal = req.pawn.TicksPerMoveCardinal;
			moveTicksDiagonal = req.pawn.TicksPerMoveDiagonal;
		}
		else
		{
			moveTicksCardinal = 14;
			moveTicksDiagonal = 19;
		}
		statusOpenValue += 2;
		statusClosedValue += 2;
		closedNodeCounter = 0;
		openList.Clear();
		locIdx = (req.start.z << (int)gridSizeZLog2) + req.start.x;
		endLocIdx = (destinationZ << (int)gridSizeZLog2) + destinationX;
		calcGrid[locIdx].g = 0;
		calcGrid[locIdx].f = 0;
		calcGrid[locIdx].parentX = (ushort)req.start.x;
		calcGrid[locIdx].parentZ = (ushort)req.start.z;
		calcGrid[locIdx].status = statusOpenValue;
		openList.Push(locIdx);
		while (true)
		{
			if (openList.Count <= 0)
			{
				Debug.LogWarning(string.Concat(req.pawn.ThingID, " aka ", req.pawn.Label, " pathing from ", req.start, " to ", req.dest, " ran out of squares to process."));
				DebugDrawRichData();
				return PawnPath.NotFound;
			}
			locIdx = openList.Pop();
			if (calcGrid[locIdx].status == statusClosedValue)
			{
				continue;
			}
			locX = (ushort)(locIdx & gridSizeXMinus1);
			locZ = (ushort)(locIdx >> (int)gridSizeZLog2);
			locIntVec3.x = locX;
			locIntVec3.z = locZ;
			if (DebugSettings.drawPaths)
			{
				Find.DebugDrawer.MakeDebugSquare(locIntVec3, calcGrid[locIdx].g.ToString(), calcGrid[locIdx].g, 2000);
			}
			if (req.dest.thing == null)
			{
				if (locIdx == endLocIdx)
				{
					return FinalizedPath();
				}
				if (req.adjacentIsOK && locIntVec3.AdjacentTo8Way(req.dest.Loc))
				{
					return FinalizedPath();
				}
			}
			else if (locIntVec3.AdjacentTo8WayOrInside(req.dest.thing))
			{
				return FinalizedPath();
			}
			if (closedNodeCounter > 100000)
			{
				break;
			}
			for (int i = 0; i < 8; i++)
			{
				newLocX = (ushort)(locX + directionList[i, 0]);
				newLocZ = (ushort)(locZ + directionList[i, 1]);
				newLocIdx = (newLocZ << (int)gridSizeZLog2) + newLocX;
				IntVec3 intVec = new IntVec3(newLocX, 0, newLocZ);
				if (newLocX >= Find.Map.Size.x || newLocZ >= Find.Map.Size.z)
				{
					if (DebugSettings.drawPaths)
					{
						Find.DebugDrawer.MakeDebugSquare(intVec, "oob", 75, 100);
					}
					continue;
				}
				if (!intVec.Walkable(pathParams))
				{
					if (DebugSettings.drawPaths)
					{
						Find.DebugDrawer.MakeDebugSquare(intVec, "walk", 22, 100);
					}
					continue;
				}
				if (i > 3)
				{
					if (i == 4)
					{
						IntVec3 intVec2 = new IntVec3(locX, 0, locZ - 1);
						if (!intVec2.Walkable(pathParams))
						{
							if (DebugSettings.drawPaths)
							{
								Find.DebugDrawer.MakeDebugSquare(intVec2, "corn", 90, 100);
							}
							continue;
						}
						intVec2 = new IntVec3(locX + 1, 0, locZ);
						if (!intVec2.Walkable(pathParams))
						{
							if (DebugSettings.drawPaths)
							{
								Find.DebugDrawer.MakeDebugSquare(intVec2, "corn", 90, 100);
							}
							continue;
						}
					}
					if (i == 5)
					{
						IntVec3 intVec2 = new IntVec3(locX, 0, locZ + 1);
						if (!intVec2.Walkable(pathParams))
						{
							if (DebugSettings.drawPaths)
							{
								Find.DebugDrawer.MakeDebugSquare(intVec2, "corn", 90, 100);
							}
							continue;
						}
						intVec2 = new IntVec3(locX + 1, 0, locZ);
						if (!intVec2.Walkable(pathParams))
						{
							if (DebugSettings.drawPaths)
							{
								Find.DebugDrawer.MakeDebugSquare(intVec2, "corn", 90, 100);
							}
							continue;
						}
					}
					if (i == 6)
					{
						IntVec3 intVec2 = new IntVec3(locX, 0, locZ + 1);
						if (!intVec2.Walkable(pathParams))
						{
							if (DebugSettings.drawPaths)
							{
								Find.DebugDrawer.MakeDebugSquare(intVec2, "corn", 90, 100);
							}
							continue;
						}
						intVec2 = new IntVec3(locX - 1, 0, locZ);
						if (!intVec2.Walkable(pathParams))
						{
							if (DebugSettings.drawPaths)
							{
								Find.DebugDrawer.MakeDebugSquare(intVec2, "corn", 90, 100);
							}
							continue;
						}
					}
					if (i == 7)
					{
						IntVec3 intVec2 = new IntVec3(locX, 0, locZ - 1);
						if (!intVec2.Walkable(pathParams))
						{
							if (DebugSettings.drawPaths)
							{
								Find.DebugDrawer.MakeDebugSquare(intVec2, "corn", 90, 100);
							}
							continue;
						}
						intVec2 = new IntVec3(locX - 1, 0, locZ);
						if (!intVec2.Walkable(pathParams))
						{
							if (DebugSettings.drawPaths)
							{
								Find.DebugDrawer.MakeDebugSquare(intVec2, "corn", 90, 100);
							}
							continue;
						}
					}
				}
				if (i > 3)
				{
					newSquareCost = moveTicksDiagonal;
				}
				else
				{
					newSquareCost = moveTicksCardinal;
				}
				newSquareCost += pathGrid[newLocIdx];
				Thing thing = blockerGrid[newLocX, 0, newLocZ];
				if (thing != null)
				{
					Building_Door building_Door = thing as Building_Door;
					if (building_Door != null && !building_Door.WillOpenFor(req.pawn))
					{
						if (!req.pawn.Team.IsHostileToTeam(thing.Team))
						{
							if (DebugSettings.drawPaths)
							{
								Find.DebugDrawer.MakeDebugSquare(thing.Position, "door", 34, 100);
							}
							continue;
						}
						newSquareCost += req.pathParams.lockedHostileDoorExtraCost;
					}
				}
				newG = newSquareCost + calcGrid[locIdx].g;
				if (((calcGrid[newLocIdx].status != statusOpenValue && calcGrid[newLocIdx].status != statusClosedValue) || calcGrid[newLocIdx].g > newG) && calcGrid[newLocIdx].status != statusOpenValue)
				{
					calcGrid[newLocIdx].parentX = locX;
					calcGrid[newLocIdx].parentZ = locZ;
					calcGrid[newLocIdx].g = newG;
					calcGrid[newLocIdx].status = statusOpenValue;
					h = 10 * (Math.Abs(newLocX - destinationX) + Math.Abs(newLocZ - destinationZ));
					calcGrid[newLocIdx].f = newG + h;
					openList.Push(newLocIdx);
				}
			}
			closedNodeCounter++;
			calcGrid[locIdx].status = statusClosedValue;
		}
		Debug.LogWarning(string.Concat(req.pawn, " pathing from ", req.start, " to ", req.dest, " hit search limit."));
		DebugDrawRichData();
		return PawnPath.NotFound;
	}

	private static PawnPath FinalizedPath()
	{
		IntVec3 position = new IntVec3(locX, 0, locZ);
		PathFinderNode item = default(PathFinderNode);
		while (true)
		{
			PathFinderNodeFast pathFinderNodeFast = calcGrid[(position.z << (int)gridSizeZLog2) + position.x];
			item.ParentPosition = new IntVec3(pathFinderNodeFast.parentX, 0, pathFinderNodeFast.parentZ);
			item.Position = position;
			newPath.nodeList.Add(item);
			if (item.Position == item.ParentPosition)
			{
				break;
			}
			position = item.ParentPosition;
		}
		newPath.cost = calcGrid[locIdx].g;
		newPath.nodeList.Reverse();
		newPath.found = true;
		newPath.pathingPawn = req.pawn;
		return newPath;
	}

	private static void DebugDrawRichData()
	{
		if (DebugSettings.drawPaths)
		{
			while (openList.Count > 0)
			{
				int num = openList.Pop();
				IntVec3 sq = new IntVec3(num & gridSizeXMinus1, 0, num >> (int)gridSizeZLog2);
				Find.DebugDrawer.MakeDebugSquare(sq, "open", 0, 100);
			}
		}
	}
}
