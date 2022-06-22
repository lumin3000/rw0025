using UnityEngine;

public class Incident_ResourcePodCrash : IncidentDefinition
{
	private const float FogClearRadius = 4.5f;

	public Incident_ResourcePodCrash()
	{
		uniqueSaveKey = 22116;
		chance = 6f;
		global = true;
		favorability = IncidentFavorability.Good;
	}

	public override bool TryExecute(IncidentParms parms)
	{
		EntityType eType = ((!(Random.value < 0.5f)) ? EntityType.Food : EntityType.Metal);
		int num = Random.Range(8, 13);
		IntVec3 intVec = GenMap.RandomSquareWith((IntVec3 sq) => sq.Standable() && !Find.RoofGrid.SquareIsRoofed(sq) && !sq.IsFogged());
		for (int i = 0; i < num; i++)
		{
			ThingResource thingResource = (ThingResource)ThingMaker.MakeThing(eType);
			thingResource.stackCount = Random.Range(25, 35);
			thingResource.SetForbidden(value: true);
			DropPodUtility.MakeDropPodAt(DropPodUtility.DropPodSpotNear(intVec), new DropPodContentsInfo(thingResource));
		}
		int num2 = Random.Range(3, 6);
		for (int j = 0; j < num2; j++)
		{
			DropPodUtility.MakeDropPodAt(DropPodUtility.DropPodSpotNear(intVec), new DropPodContentsInfo(ThingMaker.MakeThing(EntityType.DebrisSlag)));
		}
		Find.LetterStack.ReceiveLetter(new Letter("You have detected a cargo unit from your ship re-entering the atmosphere. It crashed nearby.\n\nYou might find something useful in the wreckage.", intVec));
		return true;
	}
}
