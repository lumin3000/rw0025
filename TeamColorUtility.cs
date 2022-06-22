using System.Collections.Generic;
using UnityEngine;

public static class TeamColorUtility
{
	private static readonly Dictionary<TeamType, Color> TeamColorsDict;

	static TeamColorUtility()
	{
		TeamColorsDict = new Dictionary<TeamType, Color>();
		TeamColorsDict.Add(TeamType.Colonist, new Color(0.9f, 0.9f, 0.9f));
		TeamColorsDict.Add(TeamType.Neutral, new Color(0.4f, 0.85f, 0.9f));
		TeamColorsDict.Add(TeamType.Traveler, new Color(0.4f, 0.85f, 0.9f));
		TeamColorsDict.Add(TeamType.Prisoner, new Color(1f, 0.85f, 0.5f));
		TeamColorsDict.Add(TeamType.Psychotic, new Color(0.9f, 0.2f, 0.2f));
		TeamColorsDict.Add(TeamType.Raider, new Color(0.9f, 0.2f, 0.2f));
	}

	public static Color TeamNameColorOf(TeamType team)
	{
		if (TeamColorsDict.TryGetValue(team, out var value))
		{
			return value;
		}
		if (Random.value < 0.01f)
		{
			Debug.LogError("Missing team color for " + team);
		}
		return Color.white;
	}
}
