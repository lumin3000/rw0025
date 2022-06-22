public class ThoughtDefinition
{
	public ThoughtType thoughtType;

	public string label = "No thought label";

	public string description = "No description of this thought.";

	public float baseHappinessEffect;

	public float baseFearEffect;

	public int stackLimit = 1;

	public float stackedEffectMultiplier = 0.75f;

	public int duration = 1;

	public ThoughtActivateCondition activeCondition;
}
