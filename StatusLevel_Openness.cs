public class StatusLevel_Openness : StatusLevel
{
	private const float SampleNumSquares = 100f;

	public override string Label => "Openness";

	public override int RateOfChange => 0;

	public EnvironmentOpenness CurOpenness
	{
		get
		{
			if (base.curLevel <= 6f)
			{
				return EnvironmentOpenness.VeryCramped;
			}
			if (base.curLevel <= 26f)
			{
				return EnvironmentOpenness.Cramped;
			}
			return EnvironmentOpenness.Okay;
		}
	}

	public StatusLevel_Openness(Pawn pawn)
	{
		base.pawn = pawn;
	}

	public override TooltipDef GetTooltipDef()
	{
		return null;
	}

	public override void StatusLevelTick()
	{
		base.curLevel = CurrentInstantOpenness();
	}

	public float CurrentInstantOpenness()
	{
		Room roomAt = Find.Grids.GetRoomAt(pawn.Position);
		float num = 100f;
		for (int i = 0; (float)i < 100f; i++)
		{
			IntVec3 intVec = pawn.Position + Gen.RadialPattern[i];
			if (Find.Grids.GetRoomAt(intVec) != roomAt)
			{
				num -= 1f;
			}
			else if (!intVec.Walkable())
			{
				num -= 1f;
			}
			else if (!intVec.InBounds())
			{
				num -= 1f;
			}
			else if (!intVec.Standable())
			{
				num -= 0.5f;
			}
		}
		return num * 1f;
	}
}
