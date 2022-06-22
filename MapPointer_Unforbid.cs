using System.Collections.Generic;
using UnityEngine;

public abstract class MapPointer_Unforbid : MapPointer
{
	protected EntityType typeToSeek;

	private List<Thing> forbiddables;

	protected override string LabelText => "Unforbid this " + forbiddables[0].def.label.ToLower() + " so your colonists will haul it to the stockpile (select it and hit the Unforbid control).";

	protected override Vector3 Location
	{
		get
		{
			if (forbiddables == null)
			{
				InitializePointer();
			}
			IntVec3 intVec = default(IntVec3);
			int num = 0;
			foreach (Thing forbiddable in forbiddables)
			{
				if (!forbiddable.destroyed)
				{
					num++;
					intVec += forbiddable.Position;
				}
			}
			return intVec.ToVector3Shifted() / num;
		}
	}

	public override bool Completed
	{
		get
		{
			if (forbiddables == null)
			{
				InitializePointer();
			}
			int num = 0;
			int num2 = 0;
			foreach (Thing forbiddable in forbiddables)
			{
				if (forbiddable.destroyed)
				{
					num2++;
				}
				else if (!forbiddable.IsForbidden())
				{
					num++;
				}
			}
			return num >= forbiddables.Count - num2;
		}
	}

	public override void InitializePointer()
	{
		forbiddables = new List<Thing>();
		Thing thing = null;
		int num = Gen.NumSquaresInRadius(15f);
		for (int i = 0; i < num; i++)
		{
			IntVec3 square = Genner_PlayerStuff.PlayerStartSpot + Gen.RadialPattern[i];
			Thing thing2 = Find.Grids.ThingAt(square, typeToSeek);
			if (thing2 != null)
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
			IntVec3 square2 = thing.Position + Gen.RadialPattern[j];
			Thing thing3 = Find.Grids.ThingAt(square2, typeToSeek);
			if (thing3 != null)
			{
				forbiddables.Add(thing3);
			}
		}
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe.LookListThingRef(ref forbiddables, "Forbiddables", this);
	}
}
