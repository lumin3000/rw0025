using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PawnGraphicDatabase
{
	public const string HeadsPathRoot = "Icons/Pawn/Heads";

	public const string BodiesPathRoot = "Icons/Pawn/Bodies";

	private static Dictionary<string, PawnBodyGraphic> bodiesMale;

	private static Dictionary<string, PawnBodyGraphic> bodiesFemale;

	private static Dictionary<string, PawnBodyGraphic> allBodies;

	private static Dictionary<string, PawnHeadGraphic> headsMale;

	private static Dictionary<string, PawnHeadGraphic> headsFemale;

	static PawnGraphicDatabase()
	{
		bodiesMale = new Dictionary<string, PawnBodyGraphic>();
		bodiesFemale = new Dictionary<string, PawnBodyGraphic>();
		allBodies = new Dictionary<string, PawnBodyGraphic>();
		headsMale = new Dictionary<string, PawnHeadGraphic>();
		headsFemale = new Dictionary<string, PawnHeadGraphic>();
		IEnumerable<string> enumerable = (from mat in MaterialLoader.MatsFromTexturesInFolder("Icons/Pawn/Bodies/Male")
			select mat.mainTexture.name.Split('_')[0]).Distinct().ToList();
		IEnumerable<string> enumerable2 = (from mat in MaterialLoader.MatsFromTexturesInFolder("Icons/Pawn/Bodies/Female")
			select mat.mainTexture.name.Split('_')[0]).Distinct().ToList();
		IEnumerable<string> enumerable3 = (from mat in MaterialLoader.MatsFromTexturesInFolder("Icons/Pawn/Bodies/Sexless")
			select mat.mainTexture.name.Split('_')[0]).Distinct().ToList();
		foreach (string item in enumerable)
		{
			bodiesMale.Add(item, new PawnBodyGraphic("Icons/Pawn/Bodies/Male/" + item));
			allBodies.Add(item, new PawnBodyGraphic("Icons/Pawn/Bodies/Male/" + item));
		}
		foreach (string item2 in enumerable2)
		{
			bodiesFemale.Add(item2, new PawnBodyGraphic("Icons/Pawn/Bodies/Female/" + item2));
			allBodies.Add(item2, new PawnBodyGraphic("Icons/Pawn/Bodies/Female/" + item2));
		}
		foreach (string item3 in enumerable3)
		{
			allBodies.Add(item3, new PawnBodyGraphic("Icons/Pawn/Bodies/Sexless/" + item3));
		}
		IEnumerable<string> enumerable4 = (from mat in MaterialLoader.MatsFromTexturesInFolder("Icons/Pawn/Heads/Male")
			select mat.mainTexture.name.Split('_')[0]).Distinct().ToList();
		IEnumerable<string> enumerable5 = (from mat in MaterialLoader.MatsFromTexturesInFolder("Icons/Pawn/Heads/Female")
			select mat.mainTexture.name.Split('_')[0]).Distinct().ToList();
		foreach (string item4 in enumerable4)
		{
			headsMale.Add(item4, new PawnHeadGraphic("Icons/Pawn/Heads/Male/" + item4));
		}
		foreach (string item5 in enumerable5)
		{
			headsFemale.Add(item5, new PawnHeadGraphic("Icons/Pawn/Heads/Female/" + item5));
		}
	}

	public static PawnBodyGraphic GetBodyNamed(string graphName)
	{
		if (!allBodies.ContainsKey(graphName))
		{
			Debug.Log("Tried to get pawn body " + graphName + " that was not found. Defaulting...");
			return allBodies.First().Value;
		}
		return allBodies[graphName];
	}

	public static PawnHeadGraphic GetHeadNamed(string graphName, Gender gender)
	{
		Dictionary<string, PawnHeadGraphic> dictionary = ((gender != Gender.Male) ? headsFemale : headsMale);
		if (!dictionary.ContainsKey(graphName))
		{
			Debug.Log(string.Concat("Tried to get pawn head ", graphName, " with gender ", gender, " that was not found. Defaulting..."));
			return dictionary.First().Value;
		}
		return dictionary[graphName];
	}

	public static PawnHeadGraphic GetHeadRandom(Gender gender)
	{
		switch (gender)
		{
		case Gender.Male:
			return headsMale.RandomElement().Value;
		case Gender.Female:
			return headsFemale.RandomElement().Value;
		default:
			Debug.LogWarning("Tried to get random head of unsupported gender. Defaulting to first male head.");
			return headsMale.First().Value;
		}
	}
}
