using System.Linq;

public class Building_Chair : Building, Interactive
{
	public IntVec3 SpotInFrontOfChair => base.Position + rotation.FacingSquare;

	public bool IsFacingTable => (from t in Find.Grids.ThingsAt(SpotInFrontOfChair)
		where t.def.actAsTable
		select t).Any();

	public JobCondition InteractedWith(ReservationType w, Pawn p)
	{
		if (p.rest != null)
		{
			p.rest.Rest.TickResting(def.restEffectiveness);
			if (p.rest.DoneResting)
			{
				return JobCondition.Succeeded;
			}
		}
		return JobCondition.Ongoing;
	}
}
