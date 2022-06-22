using System.Collections.Generic;

public class CompTouchTrigger : ThingComp
{
	public delegate void TouchCallbackMethod(Pawn p);

	private Grids Maps;

	private TouchCallbackMethod TouchCallback;

	public bool TouchHostile = true;

	public bool TouchNeutral = true;

	public bool TouchPlayer = true;

	public CompTouchTrigger()
	{
		Maps = Find.Grids;
	}

	public void SetTouchCallback(TouchCallbackMethod newTouchCallback)
	{
		TouchCallback = newTouchCallback;
	}

	public override void CompTick()
	{
		List<Pawn> list = new List<Pawn>();
		foreach (Thing item in Maps.ThingsAt(parent.Position))
		{
			if (item.def.eType == EntityType.Pawn)
			{
				list.Add(item as Pawn);
			}
		}
		foreach (Pawn item2 in list)
		{
			if ((item2.Team == TeamType.Raider && TouchHostile) || (item2.Team == TeamType.Neutral && TouchNeutral) || (item2.Team == TeamType.Colonist && TouchPlayer))
			{
				Touched(item2);
			}
		}
	}

	protected void Touched(Pawn p)
	{
		TouchCallback(p);
	}
}
