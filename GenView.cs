using UnityEngine;

public static class GenView
{
	private const int ViewRectMargin = 5;

	private const int ParticleSaturationThreshold = 18;

	private static IntRect viewRect;

	public static float ParticleSaturation(this IntVec3 sq)
	{
		int num = 0;
		foreach (IntVec3 item in sq.AdjacentSquares8WayAndInside())
		{
			foreach (Thing item2 in Find.Grids.ThingsAt(sq))
			{
				if (item2.def.eType == EntityType.Mote)
				{
					num++;
				}
			}
		}
		return (float)num / 18f;
	}

	public static bool ParticleSaturated(this IntVec3 sq)
	{
		return sq.ParticleSaturation() > 1f;
	}

	public static bool ShouldSpawnMotesAt(this Vector3 loc)
	{
		return loc.ToIntVec3().ShouldSpawnMotesAt();
	}

	public static bool ShouldSpawnMotesAt(this IntVec3 loc)
	{
		if (!loc.InBounds())
		{
			return false;
		}
		viewRect = Find.CameraMap.CurrentViewRect;
		viewRect.Expand(5);
		return viewRect.Contains(loc);
	}

	public static Vector3 RandomPositionOnOrNearScreen()
	{
		viewRect = Find.CameraMap.CurrentViewRect;
		viewRect.Expand(5);
		viewRect.ClipInsideMap();
		return viewRect.RandomVector3;
	}
}
