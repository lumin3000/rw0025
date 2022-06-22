using System.Linq;

public class DialogBox_DebugLister_SpawnGun : DialogBox_DebugLister
{
	protected override void DoList()
	{
		foreach (ThingDefinition item in ThingDefDatabase.AllThingDefinitions.Where((ThingDefinition def) => def.isGun))
		{
			ThingDefinition localDef = item;
			AddTool(localDef.label, delegate
			{
				ThingMaker.Spawn(localDef, Gen.MouseWorldSquare());
			});
		}
	}
}
