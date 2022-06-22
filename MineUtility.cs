using System.Linq;

public static class MineUtility
{
	public static Thing MineableInSquare(IntVec3 loc)
	{
		return (from t in Find.Grids.ThingsAt(loc)
			where t.def.mineable
			select t).FirstOrDefault();
	}
}
