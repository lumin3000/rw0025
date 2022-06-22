using System;
using System.Collections.Generic;

public static class TradeSession
{
	private const int DroppedCommodityStackSize = 75;

	public static Pawn playerNegotiator;

	public static Dictionary<EntityType, int> purchased = new Dictionary<EntityType, int>();

	public static Deal curDeal = new Deal();

	public static void Reset()
	{
		playerNegotiator = null;
		purchased.Clear();
		curDeal = new Deal();
	}

	public static int BoughtAmount(EntityType resType)
	{
		if (purchased.ContainsKey(resType))
		{
			return purchased[resType];
		}
		return 0;
	}

	public static void TryExecuteDeal()
	{
		AcceptanceReport acceptanceReport = curDeal.TryExecuteDeal();
		if (!acceptanceReport.accepted)
		{
			UI_Messages.Message(acceptanceReport.reasonText, UIMessageSound.Reject);
		}
		else
		{
			curDeal = new Deal();
		}
	}

	public static void RegisterBuy(EntityType r, int amount)
	{
		if (r == EntityType.Money)
		{
			Find.ResourceManager.Money += amount;
			Dictionary<EntityType, int> stockCommodities;
			Dictionary<EntityType, int> dictionary = (stockCommodities = Find.ActiveTrader.stockCommodities);
			EntityType key;
			EntityType key2 = (key = EntityType.Money);
			int num = stockCommodities[key];
			dictionary[key2] = num - amount;
			return;
		}
		if (purchased.ContainsKey(r))
		{
			Dictionary<EntityType, int> dictionary2;
			Dictionary<EntityType, int> dictionary3 = (dictionary2 = purchased);
			EntityType key;
			EntityType key3 = (key = r);
			int num = dictionary2[key];
			dictionary3[key3] = num + amount;
		}
		else
		{
			purchased.Add(r, amount);
		}
		if (purchased.ContainsKey(r) && purchased[r] == 0)
		{
			purchased.Remove(r);
		}
	}

	public static void ResolveSession()
	{
		foreach (KeyValuePair<EntityType, int> item in purchased)
		{
			EntityType key = item.Key;
			int num = item.Value;
			if (num > 0)
			{
				Dictionary<EntityType, int> stockCommodities;
				Dictionary<EntityType, int> dictionary = (stockCommodities = Find.ActiveTrader.stockCommodities);
				EntityType key2;
				EntityType key3 = (key2 = key);
				int num2 = stockCommodities[key2];
				dictionary[key3] = num2 - num;
				while (num > 0)
				{
					int num3 = Math.Min(num, 75);
					ThingResource thingResource = (ThingResource)ThingMaker.MakeThing(key.DefinitionOfType());
					thingResource.stackCount = num3;
					num -= num3;
					DropPodContentsInfo contents = new DropPodContentsInfo(thingResource);
					DropPodUtility.MakeDropPodAt(Find.BuildingManager.TradeDropLocation(), contents);
				}
			}
			else if (num < 0)
			{
				Find.ResourceManager.Gain(key, num);
				Dictionary<EntityType, int> stockCommodities2;
				Dictionary<EntityType, int> dictionary2 = (stockCommodities2 = Find.ActiveTrader.stockCommodities);
				EntityType key2;
				EntityType key4 = (key2 = key);
				int num2 = stockCommodities2[key2];
				dictionary2[key4] = num2 - num;
			}
		}
		Reset();
	}
}
