public class Building_AirMiner : Building_AirNetworkable
{
	protected const float AirProductionPerTick = 0.016666668f;

	private CompPowerTrader powerComp;

	public override void SpawnSetup()
	{
		base.SpawnSetup();
		StoredAirMax = 10f;
		powerComp = GetComp<CompPowerTrader>();
	}

	public override void Tick()
	{
		base.Tick();
		if (powerComp.PowerOn && Find.Grids.GetRoomAt(base.Position) == null && airNet != null)
		{
			airNet.GainAir(0.016666668f);
		}
	}
}
