public class Building_PowerPlant : Building
{
	protected CompPowerTrader powerComp;

	public override void SpawnSetup()
	{
		base.SpawnSetup();
		powerComp = GetComp<CompPowerTrader>();
		powerComp.PowerOn = true;
	}

	public override string GetInspectString()
	{
		string empty = string.Empty;
		return empty + "\n" + base.GetInspectString();
	}
}
