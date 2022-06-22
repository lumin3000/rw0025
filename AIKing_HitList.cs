using System.Collections.Generic;
using System.Linq;

public class AIKing_HitList
{
	protected AIKing King;

	public AIKing_HitList(AIKing King)
	{
		this.King = King;
	}

	public IEnumerable<Thing> GetHitListEnumerable()
	{
		IEnumerable<Thing> first = GenAI.PawnTargetsFor(King.Team);
		IEnumerable<Thing> second = Find.BuildingManager.AllBuildingsColonistCombatTargets.Cast<Thing>();
		return first.Concat(second);
	}
}
