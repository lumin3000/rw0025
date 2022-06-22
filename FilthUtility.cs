using System.Collections.Generic;
using System.Linq;

public static class FilthUtility
{
	public static bool AddFilthAt(IntVec3 sq, string filthName)
	{
		return AddFilthAt(sq, filthName, null);
	}

	public static bool AddFilthAt(IntVec3 sq, ThingDefinition filthDef)
	{
		return AddFilthAt(sq, filthDef, null, propagate: true);
	}

	public static bool AddFilthAt(IntVec3 sq, string filthName, Thing source)
	{
		List<string> list;
		if (source == null)
		{
			list = null;
		}
		else
		{
			list = new List<string>();
			list.Add(source.Label);
		}
		return AddFilthAt(sq, ThingDefDatabase.ThingDefNamed(filthName), list, propagate: true);
	}

	public static void AddFilthAt(IntVec3 sq, string filthLabel, int count)
	{
		for (int i = 0; i < count; i++)
		{
			AddFilthAt(sq, filthLabel);
		}
	}

	public static void AddFilthAt(IntVec3 sq, ThingDefinition filthDef, List<string> sources)
	{
		AddFilthAt(sq, filthDef, sources, propagate: true);
	}

	private static bool AddFilthAt(IntVec3 sq, ThingDefinition filthDef, List<string> sources, bool propagate)
	{
		Filth filth = (Filth)(from t in Find.Grids.ThingsAt(sq)
			where t.def == filthDef
			select t).FirstOrDefault();
		if (!sq.Walkable() || (filth != null && !filth.CanBeThickened))
		{
			if (propagate)
			{
				foreach (IntVec3 item in Gen.AdjacentSquares8WayRandomized)
				{
					IntVec3 sq2 = sq + item;
					if (AddFilthAt(sq2, filthDef, sources, propagate: false))
					{
						return true;
					}
				}
			}
			filth?.AddSources(sources);
			return false;
		}
		if (filth != null)
		{
			filth.ThickenFilth();
			filth.AddSources(sources);
		}
		else
		{
			Filth filth2 = (Filth)ThingMaker.MakeThing(filthDef);
			filth2.AddSources(sources);
			ThingMaker.Spawn(filth2, sq);
		}
		return true;
	}
}
