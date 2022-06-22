using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Deal
{
	protected List<int> dealAmounts = new List<int>();

	public Deal()
	{
		dealAmounts = new List<int>();
		int length = Enum.GetValues(typeof(EntityType)).Length;
		for (int i = 0; i < length; i++)
		{
			dealAmounts.Add(0);
		}
	}

	public int AmountPlayerBuying(EntityType rType)
	{
		return dealAmounts[(int)rType];
	}

	protected int PostDealAmountOf(EntityType rType, Transactor trans)
	{
		int num = 0;
		if (trans == Transactor.Player)
		{
			num += Find.ResourceManager.TotalAmountOf(rType);
			num += TradeSession.BoughtAmount(rType);
			num += AmountPlayerBuying(rType);
		}
		if (trans == Transactor.Trader)
		{
			num += Find.ActiveTrader.stockCommodities[rType];
			num -= TradeSession.BoughtAmount(rType);
			num -= AmountPlayerBuying(rType);
		}
		return num;
	}

	public AcceptanceReport TryIncrementAmount(EntityType r)
	{
		if (PostDealAmountOf(r, Transactor.Trader) <= 0)
		{
			return new AcceptanceReport("Trader has no more.");
		}
		List<int> list;
		List<int> list2 = (list = dealAmounts);
		int index;
		int index2 = (index = (int)r);
		index = list[index];
		list2[index2] = index + 1;
		TransactionType trans = TransactionType.PlayerBuys;
		if (dealAmounts[(int)r] <= 0)
		{
			trans = TransactionType.TraderBuys;
		}
		List<int> list3;
		List<int> list4 = (list3 = dealAmounts);
		int index3 = (index = 14);
		index = list3[index];
		list4[index3] = index - Find.ActiveTrader.PriceOf(r, trans);
		return AcceptanceReport.WasAccepted;
	}

	public AcceptanceReport TryDecrementAmount(EntityType r)
	{
		if (PostDealAmountOf(r, Transactor.Player) <= 0)
		{
			return new AcceptanceReport("No more to sell.");
		}
		TransactionType trans = TransactionType.PlayerBuys;
		if (dealAmounts[(int)r] <= 0)
		{
			trans = TransactionType.TraderBuys;
		}
		List<int> list;
		List<int> list2 = (list = dealAmounts);
		int index;
		int index2 = (index = (int)r);
		index = list[index];
		list2[index2] = index - 1;
		List<int> list3;
		List<int> list4 = (list3 = dealAmounts);
		int index3 = (index = 14);
		index = list3[index];
		list4[index3] = index + Find.ActiveTrader.PriceOf(r, trans);
		return AcceptanceReport.WasAccepted;
	}

	public AcceptanceReport TryExecuteDeal()
	{
		if (PostDealAmountOf(EntityType.Money, Transactor.Trader) < 0)
		{
			((DialogBox_Trade)Find.UIRoot.dialogs.TopDialog).panelCommodities.FlashMoney();
			return new AcceptanceReport("Trader cannot afford this deal.");
		}
		if (PostDealAmountOf(EntityType.Money, Transactor.Player) < 0)
		{
			((DialogBox_Trade)Find.UIRoot.dialogs.TopDialog).panelCommodities.FlashMoney();
			return new AcceptanceReport("The colony cannot afford this deal.");
		}
		IEnumerable<EntityType> enumerable = from EntityType r in Enum.GetValues(typeof(EntityType))
			where AmountPlayerBuying(r) != 0
			select r;
		if (enumerable.Count() == 0)
		{
			return AcceptanceReport.WasAccepted;
		}
		GenSound.PlaySoundOnCamera(UISounds.BuyThing, 0.1f);
		foreach (EntityType item in enumerable)
		{
			TradeSession.RegisterBuy(item, AmountPlayerBuying(item));
		}
		return AcceptanceReport.WasAccepted;
	}

	public void ChangeDealAmount(EntityType rType, int NewAmount)
	{
		if (rType == EntityType.Money)
		{
			Debug.LogError("Cannot change deal amounts on money. It is a dependent variable calculated from the others.");
		}
	}
}
