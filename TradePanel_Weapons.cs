using System.Collections.Generic;
using System.Linq;

public class TradePanel_Weapons : TradePanel_Listing
{
	protected override bool WillTrade => Find.ActiveTrader.TradesWeapons;

	protected override string NoTradeString => Find.ActiveTrader.name + " does not deal in weapons.";

	protected override IEnumerable<Tradeable> Buyables => Find.ActiveTrader.stockTradeables.Where((Tradeable w) => w is Tradeable_Equipment);

	protected override IEnumerable<Tradeable> Sellables => EquipmentFinderUtility.AllStoredEquipment.Select((Equipment eq) => new Tradeable_Equipment(eq)).Cast<Tradeable>();

	public TradePanel_Weapons()
	{
		title = "Weapons";
		buySectionTitle = "Purchasable weapons:";
		sellSectionTitle = "Sellable weapons:";
	}
}
