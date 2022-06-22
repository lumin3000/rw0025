public class TargetPack : Saveable
{
	public Thing thing;

	private IntVec3 locInt;

	private bool locSetInt;

	public IntVec3 Loc
	{
		get
		{
			if (thing != null)
			{
				return thing.Position;
			}
			return locInt;
		}
		set
		{
			locSetInt = true;
			locInt = value;
		}
	}

	public bool ThingDestroyed => thing != null && thing.destroyed;

	public bool HasThing => thing != null;

	public TargetPack()
	{
		thing = null;
	}

	public TargetPack(Thing TargetThing)
	{
		thing = TargetThing;
	}

	public TargetPack(IntVec3 TargetLoc)
	{
		Loc = TargetLoc;
	}

	public TargetPack(Thing newTargetThing, IntVec3 newTargetLoc)
	{
		thing = newTargetThing;
		Loc = newTargetLoc;
	}

	public void ExposeData()
	{
		Scribe.LookThingRef(ref thing, "TargetThing", this);
		Scribe.LookField(ref locInt, "TargetLoc");
	}

	public override string ToString()
	{
		if (thing != null)
		{
			return "(Thing=" + thing.ThingID + ")";
		}
		return string.Concat("(Loc=", Loc, ")");
	}

	public bool SameAs(TargetPack other)
	{
		if (thing != null && thing == other.thing)
		{
			return true;
		}
		if (locSetInt && other.locSetInt && locInt == other.locInt)
		{
			return true;
		}
		return false;
	}

	public static implicit operator TargetPack(Thing t)
	{
		return new TargetPack(t);
	}

	public static implicit operator Thing(TargetPack t)
	{
		return t.thing;
	}

	public static implicit operator TargetPack(IntVec3 vec)
	{
		return new TargetPack(vec);
	}
}
