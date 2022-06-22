public class DialogBox_DebugLister_SpawnPawn : DialogBox_DebugLister
{
	protected override void DoList()
	{
		foreach (PawnKindDefinition allKindDef in PawnKindDefDatabase.AllKindDefs)
		{
			PawnKindDefinition localKindDef = allKindDef;
			AddTool(localKindDef.kindLabel, delegate
			{
				Pawn newThing = PawnMaker.GeneratePawn(localKindDef.kindLabel);
				ThingMaker.Spawn(newThing, Gen.MouseWorldSquare());
			});
		}
	}
}
