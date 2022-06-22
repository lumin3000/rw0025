using System.Text;

public abstract class Incident_TraderArrival : IncidentDefinition
{
	public Incident_TraderArrival()
	{
		global = true;
		favorability = IncidentFavorability.Good;
	}

	public override bool TryExecute(IncidentParms parms)
	{
		if (Find.VisitorManager.VisitorList.Count >= 4)
		{
			return false;
		}
		Trader trader = GenerateNewTrader();
		if (Find.BuildingManager.PlayerHasBuildingOfType(EntityType.Building_CommsConsole))
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("A trade ship is passing nearby.");
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			stringBuilder.Append("They are known as " + trader.name + ". They are a " + trader.TypeName + ".");
			Find.LetterStack.ReceiveLetter(new Letter(stringBuilder.ToString()));
		}
		Find.VisitorManager.AddVisitor(trader);
		return true;
	}

	protected abstract Trader GenerateNewTrader();
}
