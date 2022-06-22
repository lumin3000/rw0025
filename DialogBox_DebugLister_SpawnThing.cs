using System.Collections.Generic;
using System.Linq;

public class DialogBox_DebugLister_SpawnThing : DialogBox_DebugLister
{
	private List<ThingDefinition> spawnableDefs = new List<ThingDefinition>();

	public DialogBox_DebugLister_SpawnThing()
	{
		spawnableDefs = ThingDefDatabase.AllThingDefinitions.Where((ThingDefinition def) => def.category == EntityCategory.Filth || def.category == EntityCategory.SmallObject || (def.category == EntityCategory.Special && def.eType != EntityType.Corpse && def.eType != EntityType.Blueprint && def.eType != EntityType.BuildingFrame)).ToList();
	}

	protected override void DoList()
	{
		foreach (ThingDefinition spawnableDef in spawnableDefs)
		{
			ThingDefinition localThingDef = spawnableDef;
			AddTool(localThingDef.label, delegate
			{
				ThingMaker.Spawn(localThingDef, Gen.MouseWorldSquare());
			});
			if (curOffset.y > winRect.height - 50f)
			{
				StartNextColumn();
			}
		}
	}
}
