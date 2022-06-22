using System;
using System.Collections.Generic;
using UnityEngine;

public class Incident_Traveler : IncidentDefinition
{
	public Incident_Traveler()
	{
		uniqueSaveKey = 56342;
		chance = 8f;
		global = true;
		favorability = IncidentFavorability.Good;
		populationEffect = IncidentPopulationEffect.Increase;
	}

	public override bool TryExecute(IncidentParms parms)
	{
		IntVec3 spawnSpot = GenMap.RandomEdgeSquareWith((IntVec3 sq) => !sq.Isolated());
		IntVec3 intVec = spawnSpot;
		Predicate<IntVec3> squareGood = (IntVec3 sq) => Find.ReachabilityRegions.ReachableBetween(spawnSpot, sq, adjacentIsOK: false) && !Find.RoofGrid.SquareIsRoofed(sq);
		bool succeeded = false;
		if (spawnSpot.x == 0)
		{
			intVec = GenMap.RandomEdgeSquareWith((IntVec3 sq) => sq.x == Find.Map.Size.x - 1 && squareGood(sq), out succeeded);
		}
		else if (spawnSpot.x == Find.Map.Size.x - 1)
		{
			intVec = GenMap.RandomEdgeSquareWith((IntVec3 sq) => sq.x == 0 && squareGood(sq), out succeeded);
		}
		else if (spawnSpot.z == 0)
		{
			intVec = GenMap.RandomEdgeSquareWith((IntVec3 sq) => sq.z == Find.Map.Size.z - 1 && squareGood(sq), out succeeded);
		}
		else if (spawnSpot.z == Find.Map.Size.z - 1)
		{
			intVec = GenMap.RandomEdgeSquareWith((IntVec3 sq) => sq.z == 0 && squareGood(sq), out succeeded);
		}
		if (!succeeded)
		{
			intVec = GenMap.RandomEdgeSquareWith((IntVec3 sq) => (sq - spawnSpot).LengthHorizontalSquared > 10000f && squareGood(sq), out succeeded);
		}
		if (!succeeded)
		{
			intVec = GenMap.RandomEdgeSquareWith((IntVec3 sq) => (sq - spawnSpot).LengthHorizontalSquared > 2500f && squareGood(sq), out succeeded);
		}
		if (!succeeded)
		{
			Debug.LogWarning(string.Concat("Failed to do traveler incident from ", spawnSpot, ": couldn't find anywhere for the traveler to go."));
			return false;
		}
		if (!intVec.Walkable())
		{
			Debug.LogWarning(string.Concat("Failed to do traveler incident from ", spawnSpot, ": unwalkable travel dest ", intVec));
			return false;
		}
		List<string> list = new List<string>();
		list.Add("Traveler");
		list.Add("Traveler");
		list.Add("Drifter");
		list.Add("Refugee");
		Pawn pawn = PawnMaker.GeneratePawn(list.RandomElement(), TeamType.Traveler);
		ThingMaker.Spawn(pawn, spawnSpot);
		pawn.MindState.travelDestination = intVec;
		string baseText = "A traveler named NAME is passing by. HECAP is a " + pawn.story.Adulthood.title.ToLower() + ".";
		baseText = GenText.TextAdjustedFor(pawn, baseText);
		Find.LetterStack.ReceiveLetter(new Letter(baseText, pawn));
		return true;
	}
}
