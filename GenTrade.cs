using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GenTrade
{
	private const int MaxNumPeople = 4;

	private const int MinNumPeople = 2;

	private const int MaxNumGuns = 4;

	private const int MinNumGuns = 3;

	public static List<Tradeable> RandomTradeableList_People()
	{
		List<Tradeable> list = new List<Tradeable>();
		int num = Random.Range(2, 4);
		for (int i = 0; i < num; i++)
		{
			Tradeable_Pawn item = new Tradeable_Pawn(PawnMaker.GeneratePawn("Slave", TeamType.Neutral));
			list.Add(item);
		}
		return list;
	}

	public static List<Tradeable> RandomTradeableList_Guns()
	{
		List<Tradeable> list = new List<Tradeable>();
		int num = Random.Range(3, 4);
		for (int i = 0; i < num; i++)
		{
			Equipment newEq = (Equipment)ThingMaker.MakeThing(RandomPurchasableGunDef());
			list.Add(new Tradeable_Equipment(newEq));
		}
		return list;
	}

	private static ThingDefinition RandomPurchasableGunDef()
	{
		return ThingDefDatabase.AllThingDefinitions.Where((ThingDefinition t) => t.isGun && t.purchasable).RandomElement();
	}
}
