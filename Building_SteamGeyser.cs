internal class Building_SteamGeyser : Building
{
	private IntermittentSteamSprayer steamSprayer;

	public Building harvester;

	public override void SpawnSetup()
	{
		base.SpawnSetup();
		steamSprayer = new IntermittentSteamSprayer(this);
	}

	public override void Tick()
	{
		if (harvester == null)
		{
			steamSprayer.SteamSprayerTick();
		}
	}
}
