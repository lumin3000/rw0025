using System.Collections.Generic;
using UnityEngine;

public class Genner_DroppedResources : Genner_Scatterer
{
	private int CacheSizeMin = 2;

	private int CacheSizeMax = 6;

	private int ResAmountMin = 45;

	private int ResAmountMax = 60;

	public Genner_DroppedResources()
	{
		minSpacing = 15f;
		numberPerHundredSquare = 1.5f;
		spotMustBeStandable = true;
	}

	public override void AddThingAt(IntVec3 loc)
	{
		EntityType rType = ((!(Random.value < 0.5f)) ? EntityType.Food : EntityType.Metal);
		int amount = Random.Range(CacheSizeMin, CacheSizeMax + 1);
		AddResourcePileAt(loc, rType, amount);
	}

	public void AddResourcePileAt(IntVec3 loc, EntityType rType, int amount)
	{
		List<Thing> list = new List<Thing>();
		for (int i = 0; i < amount; i++)
		{
			ThingResource thingResource = (ThingResource)ThingMaker.MakeThing(rType);
			thingResource.stackCount = Random.Range(ResAmountMin, ResAmountMax + 1);
			if (rType == EntityType.Food)
			{
				thingResource.stackCount = Random.Range(30, 40);
			}
			list.Add(thingResource);
		}
		int num = Random.Range(1, 5);
		for (int j = 0; j < num; j++)
		{
			list.Add(ThingMaker.MakeThing(EntityType.DebrisSlag));
		}
		int num2 = 0;
		foreach (Thing item in list)
		{
			IntVec3 intVec;
			do
			{
				intVec = loc + Gen.RadialPattern[num2];
				num2++;
				if (num2 >= Gen.RadialPattern.Length)
				{
					Debug.LogWarning("Warning: Genner for dropped resources failed to place at " + loc);
					return;
				}
			}
			while (Random.value < 0.5f || !intVec.Standable());
			ThingWithComponents thingWithComponents = item as ThingWithComponents;
			if (thingWithComponents != null && thingWithComponents.GetComp<CompForbiddable>() != null)
			{
				thingWithComponents.GetComp<CompForbiddable>().forbidden = true;
			}
			ThingMaker.Spawn(item, intVec);
		}
	}
}
