public class Thought : Saveable
{
	public ThoughtType thType;

	public float effectMultiplier = 1f;

	public int age;

	public ThoughtDefinition Def => thType.GetDefinition();

	public Thought()
	{
	}

	public Thought(ThoughtType ThType)
	{
		thType = ThType;
	}

	public float BaseEffectOn(ThoughtEffectType EfType)
	{
		return EfType switch
		{
			ThoughtEffectType.Happiness => Def.baseHappinessEffect, 
			ThoughtEffectType.Fear => Def.baseFearEffect, 
			_ => 0f, 
		};
	}

	public virtual void ExposeData()
	{
		Scribe.LookField(ref thType, "ThoughtType");
		Scribe.LookField(ref age, "Age", 1);
	}

	public void ThoughtTick()
	{
		age++;
	}

	public void Renew()
	{
		age = 0;
	}
}
