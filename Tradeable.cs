using System;
using UnityEngine;

public class Tradeable : Saveable
{
	private const float BaseSellPriceReduction = 0.65f;

	protected static readonly AudioClip DefaultTakeSound = UISounds.BuyThing;

	public virtual int BasePrice => -1;

	public virtual string Label => "Error";

	public virtual string InfoStringShort => "Need info string short.";

	public virtual AudioClip TakeSound => DefaultTakeSound;

	public virtual DialogBox NewInfoDialog
	{
		get
		{
			DialogBoxConfig dialogBoxConfig = new DialogBoxConfig();
			dialogBoxConfig.text = "No info.";
			return new DialogBox_GeneralChoice(dialogBoxConfig);
		}
	}

	public int BuyPrice
	{
		get
		{
			Pawn playerNegotiator = TradeSession.playerNegotiator;
			float num = -0.005f * (float)playerNegotiator.skills.LevelOf(SkillType.Social);
			float num2 = 1f + num;
			return (int)Math.Round((float)BasePrice * num2);
		}
	}

	public int SellPrice
	{
		get
		{
			Pawn playerNegotiator = TradeSession.playerNegotiator;
			float num = 0.005f * (float)playerNegotiator.skills.LevelOf(SkillType.Social);
			float num2 = 0.65f + num;
			int num3 = (int)Math.Round((float)BasePrice * num2);
			if (num3 >= BuyPrice)
			{
				Debug.Log("Skill of negotitator trying to put sell price above buy price.");
				num3 = BuyPrice;
			}
			return num3;
		}
	}

	public virtual void GiveToPlayer()
	{
		Debug.LogWarning("Missing code: GiveToPlayer.");
	}

	public virtual void TakeFromPlayer()
	{
		Debug.LogWarning("Missing code: TakeFromPlayer.");
	}

	public virtual void ExposeData()
	{
	}

	public void TryBuy()
	{
		if (Find.ResourceManager.Money < BuyPrice)
		{
			UI_Messages.Message("The colony cannot afford this.", UIMessageSound.Reject);
			return;
		}
		GenSound.PlaySoundOnCamera(TakeSound, 0.25f);
		Find.ResourceManager.Money -= BuyPrice;
		GiveToPlayer();
		Find.ActiveTrader.stockTradeables.Remove(this);
		Find.ActiveTrader.Money += BuyPrice;
	}

	public void TrySell()
	{
		if (Find.ActiveTrader.Money < SellPrice)
		{
			UI_Messages.Message("Trader cannot afford this.", UIMessageSound.Reject);
			return;
		}
		GenSound.PlaySoundOnCamera(TakeSound, 0.25f);
		Find.ResourceManager.Money += SellPrice;
		TakeFromPlayer();
		if (!(this is Tradeable_Pawn))
		{
			Find.ActiveTrader.stockTradeables.Add(this);
		}
		Find.ActiveTrader.Money -= SellPrice;
	}

	public override string ToString()
	{
		return "Tradeable:" + Label;
	}
}
