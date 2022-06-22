using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public static class GenText
{
	public static string Possessive(this Pawn p)
	{
		if (p.gender == Gender.Male)
		{
			return "his";
		}
		return "her";
	}

	public static string PossessiveCap(this Pawn p)
	{
		if (p.gender == Gender.Male)
		{
			return "His";
		}
		return "Her";
	}

	public static string ProObj(this Pawn p)
	{
		if (p.gender == Gender.Male)
		{
			return "him";
		}
		return "her";
	}

	public static string ProObjCap(this Pawn p)
	{
		if (p.gender == Gender.Male)
		{
			return "Him";
		}
		return "Her";
	}

	public static string ProSubj(this Pawn p)
	{
		if (p.gender == Gender.Male)
		{
			return "he";
		}
		return "she";
	}

	public static string ProSubjCap(this Pawn p)
	{
		if (p.gender == Gender.Male)
		{
			return "He";
		}
		return "She";
	}

	public static string TextAdjustedFor(Pawn p, string baseText)
	{
		return baseText.Replace("NAME", p.characterName).Replace("HISCAP", p.PossessiveCap()).Replace("HIMCAP", p.ProObjCap())
			.Replace("HECAP", p.ProSubjCap())
			.Replace("HIS", p.Possessive())
			.Replace("HIM", p.ProObj())
			.Replace("HE", p.ProSubj());
	}

	public static bool IsValidFilename(string testName)
	{
		string str = new string(Path.GetInvalidFileNameChars());
		Regex regex = new Regex("[" + Regex.Escape(str) + "]");
		return !regex.IsMatch(testName);
	}

	public static string AsPercent(float pct)
	{
		return Mathf.RoundToInt(100f * pct) + "%";
	}
}
