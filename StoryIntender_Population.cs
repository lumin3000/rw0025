using System.Text;
using UnityEngine;

public class StoryIntender_Population : Saveable
{
	public float desiredPopulationMin = 3f;

	public float desiredPopulationMax = 10f;

	public float desiredPopulationCritical = 13f;

	public int desiredPopulationGainInterval = 70000;

	private int lastPopGainTime = 30000;

	public virtual float PopulationIntent
	{
		get
		{
			float adjustedPopulation = AdjustedPopulation;
			float num = ((adjustedPopulation <= desiredPopulationMin) ? 1f : ((adjustedPopulation >= desiredPopulationCritical) ? (-1f) : ((!(adjustedPopulation <= desiredPopulationMax)) ? (-1f * Mathf.InverseLerp(desiredPopulationMax, desiredPopulationCritical, adjustedPopulation)) : (1f - Mathf.InverseLerp(desiredPopulationMin, desiredPopulationMax, adjustedPopulation)))));
			if (TimeSinceLastGain < desiredPopulationGainInterval && num > 0f)
			{
				float num2 = (float)TimeSinceLastGain / (float)desiredPopulationGainInterval;
				if (num2 < 0.25f)
				{
					num2 = 0.25f;
				}
				num *= num2;
			}
			return num;
		}
	}

	public float AdjustedPopulation
	{
		get
		{
			float num = 0f;
			num += (float)Find.PawnManager.Colonists.Count;
			return num + (float)Find.PawnManager.PawnsOnTeam[TeamType.Prisoner].Count * 0.5f;
		}
	}

	public string DebugReadout
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("IntenderPopulation");
			stringBuilder.AppendLine("   adjusted population: " + AdjustedPopulation);
			stringBuilder.AppendLine("   intent : " + PopulationIntent);
			stringBuilder.AppendLine("   last gain time : " + lastPopGainTime);
			stringBuilder.AppendLine("   time since last gain: " + TimeSinceLastGain);
			return stringBuilder.ToString();
		}
	}

	private int TimeSinceLastGain => Find.TickManager.tickCount - lastPopGainTime;

	public void ExposeData()
	{
		Scribe.LookField(ref lastPopGainTime, "LastPopGainTime");
	}

	public void Notify_PopulationGained()
	{
		if (Find.Map.initialized)
		{
			lastPopGainTime = Find.TickManager.tickCount;
		}
	}

	public void Notify_PopulationGainIncident()
	{
		if (Find.Map.initialized)
		{
			lastPopGainTime = Find.TickManager.tickCount;
		}
	}
}
