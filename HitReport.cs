using System.Text;
using UnityEngine;

public class HitReport
{
	private const float DarknessPenaltyMultiplier = 0.6f;

	public const float ProneMultiplier = 0.2f;

	public const float PronePenaltyMinDistance = 5f;

	public TargetPack target;

	public CoverUtility.CoverGiverSet covers;

	public float shotDistance;

	public float hitChanceThroughSkill = 1f;

	public float hitChanceThroughEquipment = 1f;

	public PsychGlow targetLighting = PsychGlow.Lit;

	public float hitChanceThroughTargetSize = 1f;

	public float hitChanceThroughWeather = 1f;

	public float HitChanceThroughCover => 1f - covers.overallBlockChance;

	private float HitChanceThroughDarkness
	{
		get
		{
			if (targetLighting == PsychGlow.Dark)
			{
				return 0.6f;
			}
			return 1f;
		}
	}

	public float TotalNonWildShotChance => hitChanceThroughSkill * hitChanceThroughEquipment * hitChanceThroughWeather * HitChanceThroughDarkness * hitChanceThroughTargetSize;

	public float TotalHitChance
	{
		get
		{
			float value = TotalNonWildShotChance * HitChanceThroughCover;
			return Mathf.Clamp(value, 0f, 1f);
		}
	}

	public string GetTextReadout()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine(" " + (int)(TotalHitChance * 100f) + "%");
		stringBuilder.AppendLine("   Range/Skill\t\t" + GenText.AsPercent(hitChanceThroughSkill));
		if (hitChanceThroughEquipment < 0.99f)
		{
			stringBuilder.AppendLine("   Range/Equpment\t" + GenText.AsPercent(hitChanceThroughEquipment));
		}
		if (targetLighting == PsychGlow.Dark)
		{
			stringBuilder.AppendLine("   Darkness       " + GenText.AsPercent(0.6f));
		}
		if (target.HasThing)
		{
			Pawn pawn = target.thing as Pawn;
			if (pawn != null && pawn.Incapacitated && shotDistance > 5f)
			{
				stringBuilder.AppendLine("   Target prone      " + GenText.AsPercent(0.2f));
			}
			if (hitChanceThroughTargetSize != 1f)
			{
				stringBuilder.AppendLine("   Target size       " + GenText.AsPercent(hitChanceThroughTargetSize));
			}
		}
		if (hitChanceThroughWeather < 0.99f)
		{
			stringBuilder.AppendLine("   Weather      " + GenText.AsPercent(hitChanceThroughWeather));
		}
		if (HitChanceThroughCover < 1f)
		{
			stringBuilder.AppendLine("   Cover\t\t\t\t" + GenText.AsPercent(HitChanceThroughCover));
			foreach (CoverUtility.CoverGiver giver in covers.Givers)
			{
				stringBuilder.AppendLine("     " + giver.CoverThing.Label + " stops " + GenText.AsPercent(giver.BlockChance));
			}
		}
		else
		{
			stringBuilder.AppendLine("   (no cover)");
		}
		return stringBuilder.ToString();
	}
}
