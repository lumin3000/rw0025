using System.Collections.Generic;

public static class FogFloodFiller
{
	public static void FloodUnfogRecursive(IntVec3 root)
	{
		bool[,] fogGrid = Find.FogGrid.fogGrid;
		FogGrid fogGrid2 = Find.FogGrid;
		if (!root.InBounds() || !fogGrid[root.x, root.z] || fogGrid2.ShouldBeFogged(root))
		{
			return;
		}
		fogGrid2.Unfog(root);
		foreach (IntVec3 item in root.AdjacentSquares8Way())
		{
			FloodUnfogRecursive(item);
		}
	}

	public static void FloodUnfogQueue(IntVec3 root)
	{
		bool[,] fogGrid = Find.FogGrid.fogGrid;
		FogGrid fogGrid2 = Find.FogGrid;
		Stack<IntVec3> stack = new Stack<IntVec3>();
		stack.Push(root);
		while (stack.Count > 0)
		{
			IntVec3 intVec = stack.Pop();
			if (!fogGrid[intVec.x, intVec.z] || fogGrid2.ShouldBeFogged(intVec))
			{
				continue;
			}
			fogGrid2.Unfog(intVec);
			foreach (IntVec3 item in intVec.AdjacentSquares8Way())
			{
				if (item.InBounds())
				{
					stack.Push(item);
				}
			}
		}
	}
}
