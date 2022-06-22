using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public static class TraitDefinitionsHardcoded
{
	public static IEnumerable<TraitDefinition> TraitDefinitions_FromWords()
	{
		foreach (string str in AllWordTraits())
		{
			yield return new TraitDefinition
			{
				effect = TraitEffect.NoEffect,
				label = str
			};
		}
	}

	private static IEnumerable<string> AllWordTraits()
	{
		return GenFile.StringsFromFile("Traits/NoEffectTraits");
	}

	public static IEnumerable<TraitDefinition> AllHardcodedTraitDefinitions()
	{
		MethodInfo[] methods = typeof(TraitDefinitionsHardcoded).GetMethods();
		foreach (MethodInfo method in methods)
		{
			if (!method.Name.StartsWith("TraitDefinitions_"))
			{
				continue;
			}
			foreach (TraitDefinition item in (IEnumerable)method.Invoke(null, null))
			{
				yield return item;
			}
		}
	}

	public static IEnumerable<TraitDefinition> TraitDefinitions_Effective()
	{
		yield return new TraitDefinition
		{
			effect = TraitEffect.ShootQuick,
			label = "Gunslinger",
			description = "Half aim time under 4 range\n\nThis person knows how to draw and aim a gun with blinding speed. It doesn't help at range, but when the target is close, this person always gets the shot off first."
		};
		yield return new TraitDefinition
		{
			effect = TraitEffect.MoveFast,
			label = "Runner",
			description = "+30% Movement Speed when running\n\nThis person is a well-conditioned runner."
		};
		yield return new TraitDefinition
		{
			effect = TraitEffect.MoreHealth,
			label = "Tough",
			description = "+50 Health\n\nThis person is tough, mentally and physically."
		};
	}
}
