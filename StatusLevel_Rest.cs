using System.Collections.Generic;
using System.Text;

public class StatusLevel_Rest : StatusLevel
{
	public const float BaseRestGainPerTick = 0.0125f;

	private const float BaseRestFallPerTick = 0.0026f;

	private const int RecentLevelsQueueLength = 20;

	private Queue<float> recentLevels = new Queue<float>();

	public override string Label => "Rest";

	public override bool ShouldTrySatisfy
	{
		get
		{
			if (SkyManager.curSkyGlowPercent < 0.1f)
			{
				return base.curLevel < 70f;
			}
			return CurFatigueLevel >= FatigueLevel.VeryTired;
		}
	}

	public FatigueLevel CurFatigueLevel
	{
		get
		{
			if (base.curLevel < 0.01f)
			{
				return FatigueLevel.Exhausted;
			}
			if (base.curLevel < 20f)
			{
				return FatigueLevel.VeryTired;
			}
			if (base.curLevel < 30f)
			{
				return FatigueLevel.Tired;
			}
			return FatigueLevel.Rested;
		}
	}

	public float RestFallPerTick => CurFatigueLevel switch
	{
		FatigueLevel.Rested => 0.0026f, 
		FatigueLevel.Tired => 0.0013f, 
		FatigueLevel.VeryTired => 0.00065f, 
		FatigueLevel.Exhausted => 0.00039000003f, 
		_ => 999f, 
	};

	public override int RateOfChange
	{
		get
		{
			float num = base.curLevel - recentLevels.Peek();
			if (num > 0.03f)
			{
				return 2;
			}
			if (num < -0.03f)
			{
				return -1;
			}
			return 0;
		}
	}

	public StatusLevel_Rest(Pawn pawn)
		: base(pawn)
	{
		base.curLevel = 80f;
	}

	public override void StatusLevelTick()
	{
		base.curLevel -= RestFallPerTick;
		recentLevels.Enqueue(base.curLevel);
		if (recentLevels.Count > 20)
		{
			recentLevels.Dequeue();
		}
	}

	public void TickResting(float RestEffectiveness)
	{
		base.curLevel += 0.0125f * RestEffectiveness;
		if (base.curLevel > 100f)
		{
			base.curLevel = 100f;
		}
	}

	public override TooltipDef GetTooltipDef()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(base.TooltipBase);
		stringBuilder.AppendLine();
		stringBuilder.AppendLine();
		stringBuilder.Append("Rest is how much quality relaxation and sleep a person gets.");
		stringBuilder.AppendLine();
		stringBuilder.AppendLine();
		stringBuilder.Append("It rises to a maximum when someone sleeps in a bed. Outside of bed, it falls over time.");
		return new TooltipDef(stringBuilder.ToString(), 11721);
	}
}
