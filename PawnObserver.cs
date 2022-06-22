using UnityEngine;

public class PawnObserver
{
	private const int ObservationInterval = 400;

	private const float SampleNumSquares = 100f;

	private Pawn pawn;

	private int ticksUntilObserve;

	public PawnObserver(Pawn pawn)
	{
		this.pawn = pawn;
		ticksUntilObserve = Random.Range(0, 400);
	}

	public void PawnObserverTick()
	{
		ticksUntilObserve--;
		if (ticksUntilObserve <= 0)
		{
			ObserveSurroundingThings();
			ticksUntilObserve = 400 + Random.Range(0, 5);
		}
	}

	private void ObserveSurroundingThings()
	{
		Room roomAt = Find.Grids.GetRoomAt(pawn.Position);
		for (int i = 0; (float)i < 100f; i++)
		{
			IntVec3 intVec = pawn.Position + Gen.RadialPattern[i];
			if (!intVec.InBounds() || Find.Grids.GetRoomAt(intVec) != roomAt)
			{
				continue;
			}
			foreach (Thing item in Find.Grids.ThingsAt(intVec))
			{
				ThoughtGiver thoughtGiver = item as ThoughtGiver;
				if (thoughtGiver != null)
				{
					Thought thought = thoughtGiver.GiveObservedThought();
					if (thought != null)
					{
						pawn.psychology.thoughts.GainThought(thought);
					}
				}
			}
		}
	}
}
