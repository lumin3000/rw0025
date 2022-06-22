using UnityEngine;

public class Tradeable_Equipment : Tradeable
{
	public Equipment tradeEq;

	public override int BasePrice => tradeEq.def.basePrice;

	public override string Label => tradeEq.Label;

	public override string InfoStringShort => string.Empty;

	public override DialogBox NewInfoDialog
	{
		get
		{
			DialogBoxConfig dialogBoxConfig = new DialogBoxConfig();
			dialogBoxConfig.text = Label;
			dialogBoxConfig.text = dialogBoxConfig.text + "\n$" + BasePrice;
			tradeEq.InitVerb();
			dialogBoxConfig.text = dialogBoxConfig.text + "\n\n" + tradeEq.verb.InfoTextFull;
			return new DialogBox_GeneralChoice(dialogBoxConfig);
		}
	}

	public override AudioClip TakeSound => tradeEq.def.interactSound;

	public Tradeable_Equipment()
	{
	}

	public Tradeable_Equipment(Equipment newEq)
		: this()
	{
		tradeEq = newEq;
	}

	public override void ExposeData()
	{
		Scribe.LookSaveable(ref tradeEq, "TradeEquipment");
	}

	public override void GiveToPlayer()
	{
		IntVec3 pos = Find.BuildingManager.TradeDropLocation();
		DropPodContentsInfo contents = new DropPodContentsInfo(tradeEq);
		DropPodUtility.MakeDropPodAt(pos, contents);
	}

	public override void TakeFromPlayer()
	{
		tradeEq.Destroy();
	}
}
