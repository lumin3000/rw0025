public class Incident_TraderArrivalSlaver : Incident_TraderArrival
{
	public Incident_TraderArrivalSlaver()
	{
		uniqueSaveKey = 1551;
		chance = 4.5f;
		populationEffect = IncidentPopulationEffect.Increase;
	}

	protected override Trader GenerateNewTrader()
	{
		return new Trader(TraderType.Slaver);
	}
}
