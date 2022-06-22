using System.Collections.Generic;

public class WaterSplash : Projectile
{
	protected override void Impact(Thing HitThing)
	{
		base.Impact(HitThing);
		List<Thing> list = new List<Thing>();
		foreach (Thing item in Find.Grids.ThingsAt(base.Position))
		{
			if (item.def.eType == EntityType.Fire)
			{
				list.Add(item);
			}
		}
		foreach (Thing item2 in list)
		{
			item2.Destroy();
		}
	}
}
