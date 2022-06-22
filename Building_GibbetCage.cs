public class Building_GibbetCage : Building_Storage, ThoughtGiver
{
	public Thought GiveObservedThought()
	{
		if (base.Position.ContainedStorable() == null)
		{
			return new Thought_Observation(ThoughtType.ObservedGibbetCageEmpty, this);
		}
		return null;
	}
}
