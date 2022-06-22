using System.Collections.Generic;
using System.Linq;

public class Incident_Raid : IncidentDefinition
{
	public Incident_Raid()
	{
		uniqueSaveKey = 5412;
		chance = 8f;
		global = true;
		favorability = IncidentFavorability.Bad;
		threatLevel = IncidentThreatLevel.BigThreat;
		pointsScaleable = true;
	}

	public override bool TryExecute(IncidentParms parms)
	{
		PawnPoolRequest pawnPoolRequest = new PawnPoolRequest();
		pawnPoolRequest.points = parms.points;
		List<Pawn> list = PawnPoolMaker.GenerateRaidPawns(pawnPoolRequest).ToList();
		if (parms.forceIncap)
		{
			foreach (Pawn item in list)
			{
				item.healthTracker.forceIncap = true;
			}
		}
		AIKing_Config aIKing_Config = new AIKing_Config();
		aIKing_Config.team = TeamType.Raider;
		AIKing aIKing = new AIKing(aIKing_Config, list);
		aIKing.DropInitialPawns();
		return true;
	}
}
