using UnityEngine;

public static class GenTeam
{
	public static bool IsHostileTo(this Entity t, Entity other)
	{
		return t.Team.IsHostileToTeam(other.Team);
	}

	public static bool IsHostileToTeam(this TeamType team, TeamType otherTeam)
	{
		if (team == TeamType.Psychotic || otherTeam == TeamType.Psychotic)
		{
			return true;
		}
		if (otherTeam == team)
		{
			return false;
		}
		if (team == TeamType.Neutral || otherTeam == TeamType.Neutral)
		{
			return false;
		}
		if (team == TeamType.Prisoner || otherTeam == TeamType.Prisoner)
		{
			return false;
		}
		if (team == TeamType.Colonist)
		{
			switch (otherTeam)
			{
			case TeamType.Raider:
				return true;
			case TeamType.Traveler:
				return false;
			}
		}
		if (team == TeamType.Raider)
		{
			switch (otherTeam)
			{
			case TeamType.Colonist:
				return true;
			case TeamType.Traveler:
				return true;
			}
		}
		if (team == TeamType.Traveler)
		{
			switch (otherTeam)
			{
			case TeamType.Colonist:
				return false;
			case TeamType.Raider:
				return true;
			}
		}
		Debug.LogError(string.Concat("Missing team definition: ", team, " vs ", otherTeam));
		return false;
	}

	public static bool CanBeArrested(this TeamType team)
	{
		return team == TeamType.Colonist || team == TeamType.Neutral || team == TeamType.Traveler || team == TeamType.Prisoner;
	}
}
