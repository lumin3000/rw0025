public class ColonyInfo : Saveable
{
	private string colonyName = string.Empty;

	public bool ColonyHasName => colonyName != string.Empty;

	public string ColonyName
	{
		get
		{
			if (!ColonyHasName)
			{
				return "Colony";
			}
			return colonyName;
		}
		set
		{
			colonyName = value;
		}
	}

	public void ColonyInfoTick()
	{
		if (!ColonyHasName && Find.TickManager.tickCount % 1000 == 0 && DateHandler.DaysPassed > 5 && Find.PawnManager.Colonists.Count >= 3 && !Find.GameEnder.gameEnding && Find.PawnManager.Hostiles.Count == 0)
		{
			Find.Dialogs.AddDialogBox(new DialogBox_NameColony());
		}
	}

	public void ExposeData()
	{
		Scribe.LookField(ref colonyName, "ColonyName");
	}
}
