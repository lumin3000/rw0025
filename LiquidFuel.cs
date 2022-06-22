public class LiquidFuel : Thing
{
	private const int DryOutTime = 1500;

	private int spawnTick;

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe.LookField(ref spawnTick, "SpawnTick");
	}

	public override void SpawnSetup()
	{
		base.SpawnSetup();
		spawnTick = Find.TickManager.tickCount;
	}

	public void Refill()
	{
		spawnTick = Find.TickManager.tickCount;
	}

	public override void Tick()
	{
		if (spawnTick + 1500 < Find.TickManager.tickCount)
		{
			Destroy();
		}
	}
}
