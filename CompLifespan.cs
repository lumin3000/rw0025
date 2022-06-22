public class CompLifespan : ThingComp
{
	public int LifespanTicksLeft;

	public override void CompTick()
	{
		LifespanTicksLeft--;
		if (LifespanTicksLeft <= 0)
		{
			parent.Destroy();
		}
	}
}
