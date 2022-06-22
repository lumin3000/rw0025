public class Incident_TraderArrivalGeneral : Incident_TraderArrival
{
	public Incident_TraderArrivalGeneral()
	{
		uniqueSaveKey = 56138;
		chance = 13f;
	}

	protected override Trader GenerateNewTrader()
	{
		TraderType traderType;
		do
		{
			traderType = Gen.RandomEnumValue<TraderType>();
		}
		while (traderType == TraderType.Slaver);
		return new Trader(traderType);
	}
}
