using System;
using System.Collections.Generic;
using System.Linq;

public class PawnManager
{
	public List<Pawn> AllPawns = new List<Pawn>();

	public Dictionary<TeamType, List<Pawn>> PawnsOnTeam = new Dictionary<TeamType, List<Pawn>>();

	public Dictionary<TeamType, List<Pawn>> PawnsWithHostilityTo = new Dictionary<TeamType, List<Pawn>>();

	public List<Pawn> Hostiles => PawnsWithHostilityTo[TeamType.Colonist];

	public List<Pawn> Colonists => PawnsOnTeam[TeamType.Colonist];

	public IEnumerable<Pawn> ColonistsAndPrisoners => Colonists.Concat(PawnsOnTeam[TeamType.Prisoner]);

	public PawnManager()
	{
		foreach (int value in Enum.GetValues(typeof(TeamType)))
		{
			PawnsOnTeam.Add((TeamType)value, new List<Pawn>());
			PawnsWithHostilityTo.Add((TeamType)value, new List<Pawn>());
		}
	}

	public void RegisterPawn(Pawn p)
	{
		if (!AllPawns.Contains(p))
		{
			AllPawns.Add(p);
		}
		if (!PawnsOnTeam[p.Team].Contains(p))
		{
			PawnsOnTeam[p.Team].Add(p);
			if (p.Team == TeamType.Colonist)
			{
				Find.Storyteller.intenderPopulation.Notify_PopulationGained();
			}
		}
		foreach (int value in Enum.GetValues(typeof(TeamType)))
		{
			if (((TeamType)value).IsHostileToTeam(p.Team) && !PawnsWithHostilityTo[(TeamType)value].Contains(p))
			{
				PawnsWithHostilityTo[(TeamType)value].Add(p);
			}
		}
	}

	public void DeRegisterPawn(Pawn p)
	{
		if (AllPawns.Contains(p))
		{
			AllPawns.Remove(p);
		}
		if (PawnsOnTeam[p.Team].Contains(p))
		{
			PawnsOnTeam[p.Team].Remove(p);
		}
		foreach (int value in Enum.GetValues(typeof(TeamType)))
		{
			if (PawnsWithHostilityTo[(TeamType)value].Contains(p))
			{
				PawnsWithHostilityTo[(TeamType)value].Remove(p);
			}
		}
	}

	public List<Pawn> PawnsThingIsHostileTo(Thing t)
	{
		return PawnsWithHostilityTo[t.Team];
	}
}
