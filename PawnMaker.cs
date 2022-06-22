using UnityEngine;

public static class PawnMaker
{
	public static Pawn GeneratePawn(string newPawnKind)
	{
		return GeneratePawn(newPawnKind, PawnKindDefDatabase.KindDefNamed(newPawnKind).defaultTeam);
	}

	public static Pawn GeneratePawn(string newPawnKind, TeamType Team)
	{
		Pawn pawn = (Pawn)ThingMaker.MakeThing(EntityType.Pawn);
		pawn.kindDef = PawnKindDefDatabase.KindDefNamed(newPawnKind);
		pawn.Team = Team;
		pawn.raceDef = RaceDefDatabase.DefinitionNamed(pawn.kindDef.raceName);
		if (pawn.raceDef.humanoid)
		{
			pawn.ownership = new Pawn_Ownership(pawn);
			pawn.skills = new Pawn_SkillsTracker(pawn);
			pawn.talker = new Pawn_TalkTracker(pawn);
			pawn.psychology = new Pawn_PsychologyTracker(pawn);
			pawn.story = new Pawn_StoryTracker(pawn);
			pawn.mind = new Pawn_MindHuman(pawn);
		}
		else
		{
			pawn.mind = new Pawn_MindAnimal(pawn);
		}
		if (pawn.raceDef.EatsFood)
		{
			pawn.food = new Pawn_FoodTracker(pawn);
		}
		if (pawn.raceDef.needsRest)
		{
			pawn.rest = new Pawn_RestTracker(pawn);
		}
		PawnMakerUtility.AddOrRemovePawnTrackersFor(pawn);
		if (pawn.raceDef.hasGenders)
		{
			if (Random.value < 0.5f)
			{
				pawn.gender = Gender.Male;
			}
			else
			{
				pawn.gender = Gender.Female;
			}
		}
		else
		{
			pawn.gender = Gender.Sexless;
		}
		pawn.age = RandomCharacterAge();
		PawnMakerUtility.GiveAppropriateKeysTo(pawn);
		pawn.health = pawn.healthTracker.MaxHealth;
		if (pawn.kindDef.setupMethod != null)
		{
			pawn.kindDef.setupMethod(pawn);
		}
		pawn.characterName = NameMaker.NewName(pawn.kindDef, pawn.gender);
		if (pawn.raceDef.hasIdentity)
		{
			pawn.story.RandomizeHistory();
			pawn.story.MakeSkillsFromHistory();
			pawn.traits.MakeRandomTraits();
		}
		pawn.drawer.renderer.ResolveGraphics();
		return pawn;
	}

	private static int RandomCharacterAge()
	{
		switch (Random.Range(1, 13))
		{
		case 1:
			return Random.Range(15, 20);
		case 2:
			return Random.Range(18, 25);
		case 3:
			return Random.Range(21, 30);
		case 4:
			return Random.Range(21, 30);
		case 5:
			return Random.Range(21, 30);
		case 6:
			return Random.Range(31, 40);
		case 7:
			return Random.Range(31, 40);
		case 8:
			return Random.Range(31, 40);
		case 9:
			return Random.Range(41, 50);
		case 10:
			return Random.Range(41, 50);
		case 11:
			return Random.Range(51, 60);
		case 12:
			return Random.Range(61, 70);
		default:
			Debug.LogWarning("Didn't get age");
			return 25;
		}
	}
}
