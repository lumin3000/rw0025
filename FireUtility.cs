using System.Linq;

public static class FireUtility
{
	public static bool CanEverAttachFire(this Thing t)
	{
		if (t.destroyed)
		{
			return false;
		}
		if (!t.def.Flammable)
		{
			return false;
		}
		if (t.def.category != EntityCategory.Pawn)
		{
			return false;
		}
		return true;
	}

	public static void TryStartFireIn(IntVec3 sq, float fireSize)
	{
		bool flag = false;
		foreach (Thing item in Find.Grids.ThingsAt(sq))
		{
			if (item.def.eType == EntityType.Fire)
			{
				return;
			}
			if (item.def.Flammable)
			{
				flag = true;
			}
		}
		if (flag)
		{
			Fire fire = ThingMaker.MakeThing(EntityType.Fire) as Fire;
			fire.fireSize = fireSize;
			ThingMaker.Spawn(fire, sq, IntRot.north);
		}
	}

	public static void TryIgnite(this Thing t, float fireSize)
	{
		if (t.CanEverAttachFire() && !t.HasAttachment(EntityType.Fire))
		{
			Fire fire = ThingMaker.MakeThing(EntityType.Fire) as Fire;
			fire.fireSize = fireSize;
			fire.AttachTo(t);
			ThingMaker.Spawn(fire, t.Position, IntRot.north);
			(t as Pawn)?.jobs.EndCurrentJob(JobCondition.ForcedInterrupt);
		}
	}

	public static bool IsBurningImmobile(this Thing t)
	{
		foreach (IntVec3 item in Gen.SquaresOccupiedBy(t))
		{
			foreach (Fire item2 in (from thing in Find.Grids.ThingsAt(item)
				where thing.def.eType == EntityType.Fire
				select thing).Cast<Fire>())
			{
				if (item2.parent == null)
				{
					return true;
				}
			}
		}
		return false;
	}

	public static bool IsBurningImmobile(this IntVec3 sq)
	{
		foreach (Fire item in (from thing in Find.Grids.ThingsAt(sq)
			where thing.def.eType == EntityType.Fire
			select thing).Cast<Fire>())
		{
			if (item.parent == null)
			{
				return true;
			}
		}
		return false;
	}
}
