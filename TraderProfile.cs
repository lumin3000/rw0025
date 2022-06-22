using System.Collections.Generic;
using UnityEngine;

public class TraderProfile
{
	public class TraderDataResource
	{
		private const float TradeAmountScale = 1.6f;

		public EntityType eType;

		public PriceType playerBuyPriceModifier = PriceType.Normal;

		public PriceType playerSellPriceModifier = PriceType.Normal;

		public int stockAmountMin;

		public int stockAmountMax;

		public TraderDataResource(EntityType rType, PriceType buyPriceModifier, int stockAmountMin, int stockAmountMax)
		{
			eType = rType;
			playerBuyPriceModifier = buyPriceModifier;
			playerSellPriceModifier = buyPriceModifier - 1;
			this.stockAmountMin = stockAmountMin;
			this.stockAmountMax = stockAmountMax;
		}

		public int RandomAmount()
		{
			return Mathf.RoundToInt((float)Random.Range(stockAmountMin, stockAmountMax + 1) * 1.6f);
		}
	}

	public List<TraderDataResource> resourceData = new List<TraderDataResource>();

	public bool buysSlaves;

	public bool buysWeapons;

	public void AddResource(EntityType rType, PriceType PType, int stock)
	{
		resourceData.Add(new TraderDataResource(rType, PType, stock, stock));
	}

	public void AddResource(EntityType rType, PriceType PType, int minStock, int maxStock)
	{
		resourceData.Add(new TraderDataResource(rType, PType, minStock, maxStock));
	}
}
