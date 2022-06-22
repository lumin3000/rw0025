using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Trader : Visitor
{
	private TraderType traderType;

	public List<Tradeable> stockTradeables = new List<Tradeable>();

	public Dictionary<EntityType, int> stockCommodities = new Dictionary<EntityType, int>();

	public override string FullTitle => "Trading ship: " + name + " (" + TypeName + ")";

	public string TypeName
	{
		get
		{
			if (traderType == TraderType.Farmer)
			{
				return "farming vessel";
			}
			if (traderType == TraderType.Industrial)
			{
				return "industrial trader";
			}
			if (traderType == TraderType.Combat)
			{
				return "combat supplier";
			}
			if (traderType == TraderType.Slaver)
			{
				return "slave trader";
			}
			return "Error in TypeName";
		}
	}

	private TraderProfile MyTraderData => TraderProfilesHardcoded.DataForType(traderType);

	public bool TradesSlaves => MyTraderData.buysSlaves;

	public bool TradesWeapons => MyTraderData.buysWeapons;

	public int Money
	{
		get
		{
			if (stockCommodities.ContainsKey(EntityType.Money))
			{
				return stockCommodities[EntityType.Money];
			}
			return 0;
		}
		set
		{
			if (stockCommodities.ContainsKey(EntityType.Money))
			{
				stockCommodities[EntityType.Money] = value;
			}
			else
			{
				stockCommodities.Add(EntityType.Money, value);
			}
		}
	}

	public IEnumerable<EntityType> TradingCommodityTypes
	{
		get
		{
			foreach (TraderProfile.TraderDataResource r in MyTraderData.resourceData)
			{
				yield return r.eType;
			}
		}
	}

	public Trader()
	{
	}

	public Trader(TraderType TraderType)
	{
		traderType = TraderType;
		name = NameMaker.NewName(NameType.Trader, Gender.Sexless);
		stockTradeables = TraderProfilesHardcoded.NewStockTradeablesFor(TraderType);
		stockCommodities = TraderProfilesHardcoded.NewStockCommoditiesFor(TraderType);
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe.LookField(ref traderType, "TraderType");
		Scribe.LookList(ref stockTradeables, "TradeableList");
		Scribe.LookDict(ref stockCommodities, "CommodityList");
	}

	public override void OpenComms(Pawn negotiator)
	{
		TradeSession.Reset();
		TradeSession.playerNegotiator = negotiator;
		Find.UIMapRoot.dialogs.AddDialogBox(new DialogBox_Trade(this));
	}

	public void CommsClosing()
	{
		TradeSession.ResolveSession();
	}

	public int PriceOf(EntityType rType, TransactionType trans)
	{
		PriceType priceType = PriceTypeOf(rType, trans);
		float num = 1f;
		if (priceType == PriceType.VeryCheap)
		{
			num = 0.5f;
		}
		if (priceType == PriceType.Cheap)
		{
			num = 0.75f;
		}
		if (priceType == PriceType.Normal)
		{
			num = 1f;
		}
		if (priceType == PriceType.Expensive)
		{
			num = 2f;
		}
		if (priceType == PriceType.Exorbitant)
		{
			num = 4f;
		}
		return (int)Math.Round((float)rType.DefinitionOfType().basePrice * num);
	}

	public PriceType PriceTypeOf(EntityType resType, TransactionType trans)
	{
		if (resType == EntityType.Money)
		{
			return PriceType.Undefined;
		}
		TraderProfile.TraderDataResource traderDataResource = MyTraderData.resourceData.Where((TraderProfile.TraderDataResource r) => r.eType == resType).FirstOrDefault();
		if (traderDataResource == null)
		{
			Debug.LogError(string.Concat("Missing resource data for ", resType, ", transaction=", trans));
			return PriceType.Undefined;
		}
		if (trans == TransactionType.PlayerBuys)
		{
			return traderDataResource.playerBuyPriceModifier;
		}
		return traderDataResource.playerSellPriceModifier;
	}

	public bool WillTrade(EntityType resType)
	{
		foreach (TraderProfile.TraderDataResource resourceDatum in MyTraderData.resourceData)
		{
			if (resourceDatum.eType == resType)
			{
				return true;
			}
		}
		return false;
	}

	public override string ToString()
	{
		return FullTitle;
	}
}
