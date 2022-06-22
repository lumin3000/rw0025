public class MapCondition_Eclipse : MapCondition
{
	private int startTick;

	private int LerpDuration = 200;

	public float SkyLerpStrength
	{
		get
		{
			int num = Find.TickManager.tickCount - startTick;
			if (num < LerpDuration)
			{
				return (float)num / (float)LerpDuration;
			}
			if (ticksToExpire < LerpDuration)
			{
				return (float)ticksToExpire / (float)LerpDuration;
			}
			return 1f;
		}
	}

	public MapCondition_Eclipse()
	{
	}

	public MapCondition_Eclipse(int ticksToExpire)
		: base(MapConditionType.Eclipse, ticksToExpire)
	{
		startTick = Find.TickManager.tickCount;
	}

	public override void MapConditionTick()
	{
		base.MapConditionTick();
		if (ticksToExpire == LerpDuration)
		{
			UI_Messages.Message("The eclipse is ending.");
		}
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe.LookField(ref startTick, "StartTick");
	}
}
