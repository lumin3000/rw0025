public class TraitDefinition
{
	public TraitEffect effect;

	public string label = "Traitlabel";

	public string description = "Traitdesc";

	public override string ToString()
	{
		return effect.ToString();
	}
}
