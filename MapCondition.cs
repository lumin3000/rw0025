public class MapCondition : Saveable
{
	public MapConditionType conditionType;

	protected int ticksToExpire = 2000;

	public string Label => Gen.SplitCamelCase(conditionType.ToString());

	public bool Expired => ticksToExpire <= 0;

	public MapCondition()
	{
	}

	public MapCondition(MapConditionType conditionType, int ticksToExpire)
	{
		this.conditionType = conditionType;
		this.ticksToExpire = ticksToExpire;
	}

	public virtual void ExposeData()
	{
		Scribe.LookField(ref conditionType, "ConditionType");
		Scribe.LookField(ref ticksToExpire, "TicksToExpire");
	}

	public virtual void MapConditionTick()
	{
		ticksToExpire--;
	}
}
