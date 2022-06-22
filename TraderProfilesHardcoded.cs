using System.Collections.Generic;

public static class TraderProfilesHardcoded
{
	public static TraderProfile DataForType(TraderType ProfType)
	{
		TraderProfile traderProfile = new TraderProfile();
		if (ProfType == TraderType.Farmer)
		{
			traderProfile.AddResource(EntityType.Money, PriceType.Undefined, 1500, 2000);
			traderProfile.AddResource(EntityType.Food, PriceType.Cheap, 150, 200);
			traderProfile.AddResource(EntityType.Metal, PriceType.Expensive, 40, 60);
			traderProfile.AddResource(EntityType.Medicine, PriceType.Expensive, 1, 3);
			traderProfile.AddResource(EntityType.Uranium, PriceType.Expensive, 10, 20);
		}
		if (ProfType == TraderType.Industrial)
		{
			traderProfile.buysWeapons = true;
			traderProfile.AddResource(EntityType.Money, PriceType.Undefined, 3000, 5000);
			traderProfile.AddResource(EntityType.Food, PriceType.Normal, 0);
			traderProfile.AddResource(EntityType.Metal, PriceType.Cheap, 300, 500);
			traderProfile.AddResource(EntityType.Medicine, PriceType.Normal, 20, 30);
			traderProfile.AddResource(EntityType.Uranium, PriceType.Cheap, 50, 80);
		}
		if (ProfType == TraderType.Combat)
		{
			traderProfile.buysWeapons = true;
			traderProfile.AddResource(EntityType.Money, PriceType.Undefined, 3000, 4000);
			traderProfile.AddResource(EntityType.Food, PriceType.Expensive, 0);
			traderProfile.AddResource(EntityType.Metal, PriceType.Normal, 30, 60);
			traderProfile.AddResource(EntityType.Medicine, PriceType.Expensive, 30, 50);
			traderProfile.AddResource(EntityType.Uranium, PriceType.Normal, 0);
			traderProfile.AddResource(EntityType.Shells, PriceType.Cheap, 250, 300);
			traderProfile.AddResource(EntityType.Missiles, PriceType.Cheap, 35, 40);
		}
		if (ProfType == TraderType.Slaver)
		{
			traderProfile.buysSlaves = true;
			traderProfile.buysWeapons = true;
			traderProfile.AddResource(EntityType.Money, PriceType.Undefined, 2500, 4500);
			traderProfile.AddResource(EntityType.Food, PriceType.Expensive, 0);
			traderProfile.AddResource(EntityType.Metal, PriceType.Normal, 0);
			traderProfile.AddResource(EntityType.Medicine, PriceType.Expensive, 5, 15);
			traderProfile.AddResource(EntityType.Shells, PriceType.Cheap, 30, 50);
			traderProfile.AddResource(EntityType.Missiles, PriceType.Cheap, 2, 6);
		}
		return traderProfile;
	}

	public static List<Tradeable> NewStockTradeablesFor(TraderType tt)
	{
		return tt switch
		{
			TraderType.Slaver => GenTrade.RandomTradeableList_People(), 
			TraderType.Combat => GenTrade.RandomTradeableList_Guns(), 
			_ => new List<Tradeable>(), 
		};
	}

	public static Dictionary<EntityType, int> NewStockCommoditiesFor(TraderType tType)
	{
		TraderProfile traderProfile = DataForType(tType);
		Dictionary<EntityType, int> dictionary = new Dictionary<EntityType, int>();
		foreach (TraderProfile.TraderDataResource resourceDatum in traderProfile.resourceData)
		{
			int value = resourceDatum.RandomAmount();
			dictionary.Add(resourceDatum.eType, value);
		}
		return dictionary;
	}
}
