public class Visitor : Saveable
{
	public string name = "VisitorNeedsName";

	public int ticksUntilDeparture = 40000;

	public virtual string FullTitle => "Error";

	public virtual void ExposeData()
	{
		Scribe.LookField(ref name, "Name");
		Scribe.LookField(ref ticksUntilDeparture, "TicksUntilDeparture");
	}

	public void VisitorTick()
	{
		ticksUntilDeparture--;
		if (ticksUntilDeparture <= 0)
		{
			Depart();
		}
	}

	protected virtual void Depart()
	{
		if (Find.BuildingManager.PlayerHasBuildingOfType(EntityType.Building_CommsConsole))
		{
			UI_Messages.Message(FullTitle + " has left comms range.");
		}
		Find.VisitorManager.VisitorDeparts(this);
	}

	public virtual void OpenComms(Pawn negotiator)
	{
	}
}
