using System.Collections.Generic;

public static class PowerNetMaker
{
	public static PowerNet NewPowerNetStartingFrom(Building root)
	{
		List<Building> list = new List<Building>();
		HashSet<Building> hashSet = new HashSet<Building>();
		hashSet.Add(root);
		do
		{
			HashSet<Building> hashSet2 = hashSet.HashSetFullCopy();
			foreach (Building item in hashSet)
			{
				list.Add(item);
			}
			hashSet.Clear();
			foreach (Building item2 in hashSet2)
			{
				foreach (IntVec3 item3 in Gen.AdjacentSquaresCardinal(item2))
				{
					foreach (Thing item4 in Find.Grids.ThingsAt(item3))
					{
						if (item4.def.transmitsPower)
						{
							Building building = item4 as Building;
							if (building != null && !hashSet.Contains(building) && !hashSet2.Contains(building) && !list.Contains(building))
							{
								hashSet.Add(building);
								break;
							}
						}
					}
				}
			}
		}
		while (hashSet.Count > 0);
		return new PowerNet(list);
	}

	public static void UpdateVisualLinkagesFor(PowerNet net)
	{
	}
}
