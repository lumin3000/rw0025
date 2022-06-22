using System.Collections.Generic;
using System.Linq;

public class TradePanel_People : TradePanel_Listing
{
	protected override bool WillTrade => Find.ActiveTrader.TradesSlaves;

	protected override string NoTradeString => Find.ActiveTrader.name + " does not deal in slaves.\n\nThey will neither sell workers nor buy prisoners.";

	protected override IEnumerable<Tradeable> Buyables => Find.ActiveTrader.stockTradeables.Where((Tradeable p) => p is Tradeable_Pawn);

	protected override IEnumerable<Tradeable> Sellables
	{
		get
		{
			foreach (Pawn p in Find.PawnManager.PawnsOnTeam[TeamType.Prisoner])
			{
				if (p.prisoner.Secure)
				{
					yield return new Tradeable_Pawn(p);
				}
			}
		}
	}

	public TradePanel_People()
	{
		title = "People";
		buySectionTitle = "Purchasable slaves:";
		sellSectionTitle = "Sellable prisoners:";
	}
}
