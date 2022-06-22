internal class Building_PowerPlantSteam : Building_PowerPlant
{
	private const float FullSteamPower = 4000f;

	private IntermittentSteamSprayer steamSprayer;

	private Building_SteamGeyser geyser;

	public override void SpawnSetup()
	{
		base.SpawnSetup();
		int num = 0;
		foreach (IntVec3 item in Gen.SquaresOccupiedBy(this))
		{
			Building_SteamGeyser building_SteamGeyser = (Building_SteamGeyser)Find.Grids.ThingAt(item, EntityType.SteamGeyser);
			if (building_SteamGeyser != null)
			{
				geyser = building_SteamGeyser;
				num++;
			}
			if (num == 4)
			{
				break;
			}
		}
		if (num >= 4)
		{
			geyser.harvester = this;
			powerComp.powerOutput = 4000f;
		}
		else
		{
			geyser = null;
			powerComp.powerOutput = 0f;
		}
		steamSprayer = new IntermittentSteamSprayer(this);
	}

	public override void Tick()
	{
		base.Tick();
		if (geyser != null)
		{
			steamSprayer.SteamSprayerTick();
		}
	}

	public override void Destroy()
	{
		base.Destroy();
		if (geyser != null)
		{
			geyser.harvester = null;
		}
	}
}
