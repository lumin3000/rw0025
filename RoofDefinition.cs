public class RoofDefinition : EntityDefinition
{
	public bool vanishOnCollapse = true;

	public string collapseLeavingDefinitionName = string.Empty;

	public ThingDefinition DropDebrisDefinition => ThingDefDatabase.ThingDefNamed(collapseLeavingDefinitionName);

	public RoofDefinition()
	{
		category = EntityCategory.Roof;
	}
}
