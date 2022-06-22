using System;
using UnityEngine;

public static class GlowFlooder
{
	private class LightSquare : IComparable<LightSquare>
	{
		public int x;

		public int z;

		public float dist;

		public bool visited;

		public LightSquare(int x, int z)
		{
			this.x = x;
			this.z = z;
		}

		public int CompareTo(LightSquare obj)
		{
			return dist.CompareTo(obj.dist);
		}
	}

	private static LightSquare[,,] calcGrid;

	public static void ResetStaticData()
	{
		calcGrid = new LightSquare[Find.Map.Size.x, Find.Map.Size.y, Find.Map.Size.z];
		for (int i = 0; i < Find.Map.Size.x; i++)
		{
			for (int j = 0; j < Find.Map.Size.y; j++)
			{
				for (int k = 0; k < Find.Map.Size.z; k++)
				{
					calcGrid[i, j, k] = new LightSquare(i, k);
				}
			}
		}
	}

	public static void AddFloodGlowFor(CompGlower glower, Color32[,,] glowGrid)
	{
		IntVec3 position = glower.parent.Position;
		Thing[,,] blockerGrid = Find.Grids.blockerGrid;
		IntRect intRect = IntRect.FromLimits(position.x - glower.RadiusIntCeiling, position.z - glower.RadiusIntCeiling, position.x + glower.RadiusIntCeiling, position.z + glower.RadiusIntCeiling);
		intRect.ClipInsideMap();
		FastPriorityQueue<LightSquare> fastPriorityQueue = new FastPriorityQueue<LightSquare>();
		for (int i = intRect.minZ; i <= intRect.maxZ; i++)
		{
			for (int j = intRect.minX; j <= intRect.maxX; j++)
			{
				LightSquare lightSquare = calcGrid[j, 0, i];
				lightSquare.visited = false;
				if (j == glower.parent.Position.x && i == glower.parent.Position.z)
				{
					lightSquare.dist = 1f;
				}
				else
				{
					lightSquare.dist = 2.1474836E+09f;
				}
				fastPriorityQueue.Push(lightSquare);
			}
		}
		while (fastPriorityQueue.Count != 0)
		{
			LightSquare lightSquare2 = fastPriorityQueue.Pop();
			if (lightSquare2.dist > 99999f)
			{
				break;
			}
			lightSquare2.visited = true;
			if (Debug.isDebugBuild && DebugSettings.reportGlow)
			{
				Find.DebugDrawer.MakeDebugSquare(new IntVec3(lightSquare2.x, 0, lightSquare2.z), lightSquare2.dist.ToString("#.#"), (int)(lightSquare2.dist * 100f), 3000);
			}
			for (int k = 0; k < 8; k++)
			{
				IntVec3 sq = new IntVec3(lightSquare2.x, 0, lightSquare2.z) + Gen.AdjacentSquares[k];
				if (!sq.InBounds())
				{
					continue;
				}
				LightSquare lightSquare3 = calcGrid[sq.x, sq.y, sq.z];
				if (lightSquare3.visited)
				{
					continue;
				}
				Thing thing = blockerGrid[sq.x, sq.y, sq.z];
				if (thing == null || !thing.def.blockLight)
				{
					float num = ((k >= 4) ? 1.41f : 1f);
					if (!(lightSquare2.dist + num > glower.glowRadius) && !(lightSquare2.dist + num >= lightSquare3.dist))
					{
						lightSquare3.dist = lightSquare2.dist + num;
						fastPriorityQueue.Push(lightSquare3);
					}
				}
			}
		}
		float num2 = -1f / glower.glowRadius;
		foreach (IntVec3 item in intRect)
		{
			ColorInt colorInt = default(ColorInt);
			float dist = calcGrid[item.x, 0, item.z].dist;
			if (dist <= glower.glowRadius)
			{
				float to = 1f / (dist * dist);
				float from = 1f + num2 * dist;
				float num3 = Mathf.Lerp(from, to, 0.4f);
				colorInt = glower.glowColor * num3;
			}
			if (colorInt.r > 0 || colorInt.g > 0 || colorInt.b > 0)
			{
				colorInt.ClampToNonNegative();
				ColorInt colorInt2 = glowGrid[item.x, item.y, item.z].AsColorInt();
				Color32 toColor = (colorInt2 + colorInt).ToColor32;
				glowGrid[item.x, item.y, item.z] = toColor;
			}
		}
	}
}
