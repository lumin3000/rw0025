using System;
using System.Collections.Generic;
using System.Linq;

public static class GridShapeMaker
{
	public static IEnumerable<IntVec3> IrregularLump(IntVec3 center, int numSquares)
	{
		List<IntVec3> lumpSquares = new List<IntVec3>();
		for (int i = 0; i < numSquares * 2; i++)
		{
			IntVec3 intVec = center + Gen.RadialPattern[i];
			if (intVec.InBounds())
			{
				lumpSquares.Add(intVec);
			}
		}
		Func<IntVec3, int> NumNeighbors = delegate(IntVec3 sq)
		{
			int num2 = 0;
			foreach (IntVec3 item in sq.AdjacentSquaresCardinal())
			{
				if (lumpSquares.Contains(item))
				{
					num2++;
				}
			}
			return num2;
		};
		while (lumpSquares.Count > numSquares)
		{
			int fewestNeighbors = 99;
			foreach (IntVec3 item2 in lumpSquares)
			{
				int num = NumNeighbors(item2);
				if (num < fewestNeighbors)
				{
					fewestNeighbors = num;
				}
			}
			List<IntVec3> srcList = lumpSquares.Where((IntVec3 sq) => NumNeighbors(sq) == fewestNeighbors).ToList();
			lumpSquares.Remove(srcList.RandomElement());
		}
		return lumpSquares;
	}
}
