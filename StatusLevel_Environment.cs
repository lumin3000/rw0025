using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class StatusLevel_Environment : StatusLevel
{
	private const int NumSamples = 1;

	private const int TicksBetweenSamples = 100;

	private const float SampleNumSquares = 20f;

	private const float AirPressureEffect = 0.35f;

	private Queue<float> beautySamples = new Queue<float>();

	private int ticksToNextSample;

	public override string Label => "Environment";

	public override int RateOfChange => 0;

	public EnvironmentBeauty CurBeauty
	{
		get
		{
			if (base.curLevel > 85f)
			{
				return EnvironmentBeauty.Beautiful;
			}
			if (base.curLevel > 70f)
			{
				return EnvironmentBeauty.VeryPretty;
			}
			if (base.curLevel > 58f)
			{
				return EnvironmentBeauty.Pretty;
			}
			if (base.curLevel < 15f)
			{
				return EnvironmentBeauty.Hideous;
			}
			if (base.curLevel < 30f)
			{
				return EnvironmentBeauty.VeryUgly;
			}
			if (base.curLevel < 42f)
			{
				return EnvironmentBeauty.Ugly;
			}
			return EnvironmentBeauty.Neutral;
		}
	}

	public StatusLevel_Environment(Pawn pawn)
		: base(pawn)
	{
	}

	public override void StatusLevelTick()
	{
		if (pawn.IsSleeping())
		{
			base.curLevel = 50f;
			return;
		}
		ticksToNextSample--;
		if (ticksToNextSample <= 0)
		{
			SampleEnvironmentBeauty();
			ticksToNextSample = 100;
		}
		base.curLevel = 50f + RollingAveragedLocalBeauty();
	}

	public float RollingAveragedLocalBeauty()
	{
		float num = 0f;
		foreach (float beautySample in beautySamples)
		{
			float num2 = beautySample;
			num += num2;
		}
		return num / (float)beautySamples.Count;
	}

	private void SampleEnvironmentBeauty()
	{
		float item = CurrentInstantBeauty();
		beautySamples.Enqueue(item);
		if (beautySamples.Count > 1)
		{
			beautySamples.Dequeue();
		}
	}

	public float CurrentInstantBeauty()
	{
		float num = 0f;
		Room roomAt = Find.Grids.GetRoomAt(pawn.Position);
		int num2 = 0;
		for (int i = 0; (float)i < 20f; i++)
		{
			IntVec3 intVec = pawn.Position + Gen.RadialPattern[i];
			if (!intVec.InBounds() || Find.Grids.GetRoomAt(intVec) != roomAt)
			{
				continue;
			}
			foreach (Thing item in Find.Grids.ThingsAt(intVec))
			{
				float num3 = BeautyIntFromCategory(item.def.beauty);
				if (item.def.eType == EntityType.Filth && !Find.RoofGrid.Roofed(intVec))
				{
					num3 *= 0.3f;
				}
				num += num3;
			}
			num += (float)BeautyIntFromCategory(Find.TerrainGrid.TerrainAt(intVec).beauty);
			num2++;
		}
		return num / (float)num2;
	}

	private static int BeautyIntFromCategory(BeautyCategory cat)
	{
		switch (cat)
		{
		case BeautyCategory.Horrifying:
			return -500;
		case BeautyCategory.Hideous:
			return -200;
		case BeautyCategory.Ugly:
			return -60;
		case BeautyCategory.UglyTiny:
			return -12;
		case BeautyCategory.Neutral:
			return 0;
		case BeautyCategory.NiceTiny:
			return 12;
		case BeautyCategory.Nice:
			return 60;
		case BeautyCategory.Gorgeous:
			return 200;
		case BeautyCategory.Enchanting:
			return 500;
		default:
			Debug.LogError("Unknown beauty category " + cat);
			return 0;
		}
	}

	public override TooltipDef GetTooltipDef()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(base.TooltipBase);
		stringBuilder.AppendLine();
		stringBuilder.AppendLine();
		stringBuilder.Append("Environment reflects how pleasing a person finds their surroundings.");
		stringBuilder.AppendLine();
		stringBuilder.AppendLine();
		stringBuilder.Append("Environment is high when people are in spacious rooms with beautiful decorations. It is poor when their surroundings are ugly.");
		return new TooltipDef(stringBuilder.ToString(), 772773);
	}
}
