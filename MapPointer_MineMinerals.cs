using System.Collections.Generic;
using UnityEngine;

public class MapPointer_MineMinerals : MapPointer
{
	private List<Thing> mineables;

	protected override string LabelText => "Designate these minerals for mining with the Architect menu.";

	protected override Vector3 Location
	{
		get
		{
			if (mineables == null)
			{
				InitializePointer();
			}
			IntVec3 intVec = default(IntVec3);
			int num = 0;
			foreach (Thing mineable in mineables)
			{
				if (!mineable.destroyed)
				{
					num++;
					intVec += mineable.Position;
				}
			}
			return intVec.ToVector3Shifted() / num;
		}
	}

	public override bool Completed
	{
		get
		{
			if (mineables == null)
			{
				InitializePointer();
			}
			int num = 0;
			int num2 = 0;
			foreach (Thing mineable in mineables)
			{
				if (mineable.destroyed)
				{
					num2++;
				}
				else if (Find.DesignationManager.DesignationAt(mineable.Position, DesignationType.Mine) != null)
				{
					num++;
				}
			}
			return num >= Mathf.Min(6, mineables.Count - num2);
		}
	}

	public override void InitializePointer()
	{
		mineables = new List<Thing>();
		Thing thing = null;
		int num = Gen.NumSquaresInRadius(15f);
		for (int i = 0; i < num; i++)
		{
			IntVec3 loc = Genner_PlayerStuff.PlayerStartSpot + Gen.RadialPattern[i];
			Thing thing2 = Find.Grids.BlockerAt(loc);
			if (thing2 != null && thing2.def.eType == EntityType.Mineral)
			{
				thing = thing2;
				break;
			}
		}
		if (thing == null)
		{
			return;
		}
		int num2 = Gen.NumSquaresInRadius(10f);
		for (int j = 0; j < num2; j++)
		{
			IntVec3 loc2 = thing.Position + Gen.RadialPattern[j];
			Thing thing3 = Find.Grids.BlockerAt(loc2);
			if (thing3 != null && thing3.def.eType == EntityType.Mineral)
			{
				mineables.Add(thing3);
			}
		}
	}

	public override void ExposeData()
	{
		base.ExposeData();
	}
}
