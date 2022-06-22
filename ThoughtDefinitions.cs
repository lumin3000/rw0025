using System;
using System.Collections.Generic;

public static class ThoughtDefinitions
{
	public static List<ThoughtDefinition> AllThoughtDefs;

	static ThoughtDefinitions()
	{
		MakeDefinitions();
	}

	public static ThoughtDefinition GetDefinition(this ThoughtType l)
	{
		return AllThoughtDefs[(int)l];
	}

	public static string GetLabel(this ThoughtType l)
	{
		return l.GetDefinition().label;
	}

	private static void MakeDefinitions()
	{
		AllThoughtDefs = new List<ThoughtDefinition>();
		foreach (int value in Enum.GetValues(typeof(ThoughtType)))
		{
			AllThoughtDefs.Add(NewDefinitionOf((ThoughtType)value));
		}
	}

	private static ThoughtDefinition NewDefinitionOf(ThoughtType thType)
	{
		ThoughtDefinition thoughtDefinition = new ThoughtDefinition();
		thoughtDefinition.thoughtType = thType;
		if (thType == ThoughtType.Undefined)
		{
			thoughtDefinition.label = "Undefined thought";
			thoughtDefinition.description = "This is a bug.";
			thoughtDefinition.duration = 10000;
		}
		if (thType == ThoughtType.Hungry)
		{
			thoughtDefinition.label = "Hungry";
			thoughtDefinition.description = "I haven't eaten in a while.";
			thoughtDefinition.baseHappinessEffect = -8f;
			thoughtDefinition.activeCondition = (Pawn p) => p.food.Food.CurHungerLevel == HungerLevel.Hungry;
		}
		if (thType == ThoughtType.UrgentlyHungry)
		{
			thoughtDefinition.label = "Urgently hungry";
			thoughtDefinition.description = "The hunger pangs are clawing my insides. I need food or I'll starve soon.";
			thoughtDefinition.baseHappinessEffect = -20f;
			thoughtDefinition.activeCondition = (Pawn p) => p.food.Food.CurHungerLevel == HungerLevel.UrgentlyHungry;
		}
		if (thType == ThoughtType.Starving)
		{
			thoughtDefinition.label = "Starving";
			thoughtDefinition.description = "I just want something - anything - to eat. I can feel my body wasting away. It hurts...";
			thoughtDefinition.baseHappinessEffect = -50f;
			thoughtDefinition.activeCondition = (Pawn p) => p.food.Food.CurHungerLevel == HungerLevel.Starving;
		}
		if (thType == ThoughtType.Tired)
		{
			thoughtDefinition.label = "Tired";
			thoughtDefinition.description = "I haven't slept in a while.";
			thoughtDefinition.baseHappinessEffect = -8f;
			thoughtDefinition.activeCondition = (Pawn p) => p.rest != null && p.rest.Rest.CurFatigueLevel == FatigueLevel.Tired;
		}
		if (thType == ThoughtType.VeryTired)
		{
			thoughtDefinition.label = "Very tired";
			thoughtDefinition.description = "I'm so tired. I just want to lay down and rest for a few minutes...";
			thoughtDefinition.baseHappinessEffect = -20f;
			thoughtDefinition.activeCondition = (Pawn p) => p.rest != null && p.rest.Rest.CurFatigueLevel == FatigueLevel.VeryTired;
		}
		if (thType == ThoughtType.Exhausted)
		{
			thoughtDefinition.label = "Exhausted";
			thoughtDefinition.description = "I'm so exhausted I can barely stand. My eyelids have lead weights on them and my body weighs a ton.";
			thoughtDefinition.baseHappinessEffect = -40f;
			thoughtDefinition.activeCondition = (Pawn p) => p.rest != null && p.rest.Rest.CurFatigueLevel == FatigueLevel.Exhausted;
		}
		if (thType == ThoughtType.EnvironmentUgly)
		{
			thoughtDefinition.label = "Ugly environment";
			thoughtDefinition.description = "This place is unpleasant to be in.";
			thoughtDefinition.baseHappinessEffect = -5f;
			thoughtDefinition.activeCondition = (Pawn p) => p.psychology.Environment.CurBeauty == EnvironmentBeauty.Ugly;
		}
		if (thType == ThoughtType.EnvironmentVeryUgly)
		{
			thoughtDefinition.label = "Very ugly environment";
			thoughtDefinition.description = "This place is really unpleasant. I don't want to be here.";
			thoughtDefinition.baseHappinessEffect = -10f;
			thoughtDefinition.activeCondition = (Pawn p) => p.psychology.Environment.CurBeauty == EnvironmentBeauty.VeryUgly;
		}
		if (thType == ThoughtType.EnvironmentHideous)
		{
			thoughtDefinition.label = "Hideous environment";
			thoughtDefinition.description = "This place is unbearable. Get me out of here...";
			thoughtDefinition.baseHappinessEffect = -15f;
			thoughtDefinition.activeCondition = (Pawn p) => p.psychology.Environment.CurBeauty == EnvironmentBeauty.Hideous;
		}
		if (thType == ThoughtType.EnvironmentPretty)
		{
			thoughtDefinition.label = "Pleasant environment";
			thoughtDefinition.description = "This place is nice.";
			thoughtDefinition.baseHappinessEffect = 5f;
			thoughtDefinition.activeCondition = (Pawn p) => p.psychology.Environment.CurBeauty == EnvironmentBeauty.Pretty;
		}
		if (thType == ThoughtType.EnvironmentVeryPretty)
		{
			thoughtDefinition.label = "Very pleasant environment";
			thoughtDefinition.description = "I really like this place. It's beautiful.";
			thoughtDefinition.baseHappinessEffect = 10f;
			thoughtDefinition.activeCondition = (Pawn p) => p.psychology.Environment.CurBeauty == EnvironmentBeauty.VeryPretty;
		}
		if (thType == ThoughtType.EnvironmentBeautiful)
		{
			thoughtDefinition.label = "Beautiful environment";
			thoughtDefinition.description = "This place is beautiful. It lifts me up and makes me feel alive.";
			thoughtDefinition.baseHappinessEffect = 15f;
			thoughtDefinition.activeCondition = (Pawn p) => p.psychology.Environment.CurBeauty == EnvironmentBeauty.Beautiful;
		}
		if (thType == ThoughtType.EnvironmentCrampt)
		{
			thoughtDefinition.label = "Cramped environment";
			thoughtDefinition.description = "This place is really closed-in. I feel like I have no space to breathe.";
			thoughtDefinition.baseHappinessEffect = -5f;
			thoughtDefinition.activeCondition = (Pawn p) => p.psychology.Openness.CurOpenness == EnvironmentOpenness.Cramped;
		}
		if (thType == ThoughtType.EnvironmentVeryCrampt)
		{
			thoughtDefinition.label = "Very cramped environment";
			thoughtDefinition.description = "It's claustrophobic in here. I have nowhere to move. I feel like a rat in a tiny cage.";
			thoughtDefinition.baseHappinessEffect = -10f;
			thoughtDefinition.activeCondition = (Pawn p) => p.psychology.Openness.CurOpenness == EnvironmentOpenness.VeryCramped;
		}
		if (thType == ThoughtType.EnvironmentDark)
		{
			thoughtDefinition.label = "In darkness";
			thoughtDefinition.description = "I don't like spending time in the dark.";
			thoughtDefinition.baseHappinessEffect = -5f;
			thoughtDefinition.baseFearEffect = 5f;
			thoughtDefinition.activeCondition = (Pawn p) => !p.IsSleeping() && Find.GlowGrid.PsychGlowAt(p.Position) == PsychGlow.Dark;
		}
		if (thType == ThoughtType.Imprisoned)
		{
			thoughtDefinition.label = "Imprisoned";
			thoughtDefinition.description = "I hate being locked up.";
			thoughtDefinition.baseHappinessEffect = -15f;
			thoughtDefinition.baseFearEffect = 20f;
			thoughtDefinition.activeCondition = (Pawn p) => p.Team == TeamType.Prisoner;
		}
		if (thType == ThoughtType.SharedBedroom)
		{
			thoughtDefinition.label = "Sharing bedroom";
			thoughtDefinition.description = "Sharing a room with others is annoying. I want some privacy. ";
			thoughtDefinition.baseHappinessEffect = -8f;
			thoughtDefinition.activeCondition = (Pawn p) => p.ownership.ownedBed != null && p.ownership.OwnedPrivateRoom == null;
		}
		if (thType == ThoughtType.ColonistLeftUnburied)
		{
			thoughtDefinition.label = "Colonist left unburied";
			thoughtDefinition.description = "One of us died and we're just leaving them lying in the open. Nobody should be left like that. People deserve a decent burial.";
			thoughtDefinition.baseHappinessEffect = -10f;
			thoughtDefinition.activeCondition = delegate
			{
				foreach (Corpse spawnedCorpse in Find.ThingLister.spawnedCorpses)
				{
					if (spawnedCorpse.Age > 80000 && spawnedCorpse.sourcePawn.Team == TeamType.Colonist && !spawnedCorpse.IsInStorage())
					{
						return true;
					}
				}
				return false;
			};
		}
		if (thType == ThoughtType.ObservedLayingCorpse)
		{
			thoughtDefinition.label = "Observed corpse";
			thoughtDefinition.description = "I saw a dead body laying on the ground.";
			thoughtDefinition.baseHappinessEffect = -10f;
			thoughtDefinition.baseFearEffect = 5f;
			thoughtDefinition.duration = 10000;
			thoughtDefinition.stackedEffectMultiplier = 0.5f;
			thoughtDefinition.stackLimit = 5;
		}
		if (thType == ThoughtType.ObservedDeadColonistInDump)
		{
			thoughtDefinition.label = "Observed dead colonist in dump";
			thoughtDefinition.description = "I saw one of ours lying in a dump. Better than leaving them in the open, but that's still undignified.";
			thoughtDefinition.baseHappinessEffect = -5f;
			thoughtDefinition.baseFearEffect = 2f;
			thoughtDefinition.duration = 10000;
			thoughtDefinition.stackedEffectMultiplier = 0.7f;
			thoughtDefinition.stackLimit = 5;
		}
		if (thType == ThoughtType.ObservedGibbetCageEmpty)
		{
			thoughtDefinition.label = "Observed empty gibbet cage";
			thoughtDefinition.description = "Those gibbet cages look so cruel.";
			thoughtDefinition.baseHappinessEffect = -2f;
			thoughtDefinition.baseFearEffect = 5f;
			thoughtDefinition.duration = 10000;
			thoughtDefinition.stackedEffectMultiplier = 0.5f;
			thoughtDefinition.stackLimit = 5;
		}
		if (thType == ThoughtType.ObservedGibbetCageFullStranger)
		{
			thoughtDefinition.label = "Observed dead stranger in gibbet cage";
			thoughtDefinition.description = "I saw a dead stranger on display in a gibbet cage. What a cruel place.";
			thoughtDefinition.baseHappinessEffect = -3f;
			thoughtDefinition.baseFearEffect = 10f;
			thoughtDefinition.duration = 10000;
			thoughtDefinition.stackedEffectMultiplier = 0.5f;
			thoughtDefinition.stackLimit = 5;
		}
		if (thType == ThoughtType.ObservedGibbetCageFullColonist)
		{
			thoughtDefinition.label = "Observed dead colonist in gibbet cage";
			thoughtDefinition.description = "I saw one of our own dead in the gibbet cage. I've talked to that person. This place is terrifying.";
			thoughtDefinition.baseHappinessEffect = -12f;
			thoughtDefinition.baseFearEffect = 15f;
			thoughtDefinition.duration = 10000;
			thoughtDefinition.stackedEffectMultiplier = 0.5f;
			thoughtDefinition.stackLimit = 5;
		}
		if (thType == ThoughtType.NewColonyOptimism)
		{
			thoughtDefinition.label = "New colony optimism";
			thoughtDefinition.description = "I'm excited to be founding a new colony.";
			thoughtDefinition.baseHappinessEffect = 20f;
			thoughtDefinition.duration = 200000;
		}
		if (thType == ThoughtType.AteNutrientPaste)
		{
			thoughtDefinition.label = "Ate nutrient paste";
			thoughtDefinition.description = "I had to eat nutrient paste. I know it keeps you alive, but nobody wants to swallow that glop.";
			thoughtDefinition.baseHappinessEffect = -8f;
			thoughtDefinition.duration = 20000;
			thoughtDefinition.stackLimit = 3;
		}
		if (thType == ThoughtType.AteRawFood)
		{
			thoughtDefinition.label = "Ate raw food";
			thoughtDefinition.description = "I had to eat raw food. Can't we cook it, or at least synthesize some nutrient paste?";
			thoughtDefinition.baseHappinessEffect = -12f;
			thoughtDefinition.duration = 20000;
			thoughtDefinition.stackLimit = 3;
		}
		if (thType == ThoughtType.SleptOutside)
		{
			thoughtDefinition.label = "Slept outside";
			thoughtDefinition.description = "I had to sleep outdoors like an animal.";
			thoughtDefinition.baseHappinessEffect = -8f;
			thoughtDefinition.duration = 20000;
			thoughtDefinition.stackLimit = 1;
		}
		if (thType == ThoughtType.BattleWounded)
		{
			thoughtDefinition.label = "Wounded";
			thoughtDefinition.description = "That's my blood! I've been wounded in a fight!";
			thoughtDefinition.baseHappinessEffect = -8f;
			thoughtDefinition.baseFearEffect = 20f;
			thoughtDefinition.duration = 40000;
			thoughtDefinition.stackLimit = 1;
		}
		if (thType == ThoughtType.KnowPrisonerBeaten)
		{
			thoughtDefinition.label = "A prisoner was beaten";
			thoughtDefinition.description = "They're beating a prisoner.";
			thoughtDefinition.baseFearEffect = 8f;
			thoughtDefinition.duration = 20000;
			thoughtDefinition.stackLimit = 1;
		}
		if (thType == ThoughtType.KnowPrisonerSold)
		{
			thoughtDefinition.label = "A prisoner was sold";
			thoughtDefinition.description = "This colony sold a prisoner into slavery. That's a worrying thought.";
			thoughtDefinition.baseHappinessEffect = -8f;
			thoughtDefinition.baseFearEffect = 8f;
			thoughtDefinition.duration = 100000;
			thoughtDefinition.stackLimit = 5;
		}
		if (thType == ThoughtType.KnowPrisonerExecuted)
		{
			thoughtDefinition.label = "A prisoner was executed";
			thoughtDefinition.description = "A prisoner was executed in cold blood. This colony seems more evil by the day.";
			thoughtDefinition.baseHappinessEffect = -8f;
			thoughtDefinition.baseFearEffect = 40f;
			thoughtDefinition.duration = 200000;
			thoughtDefinition.stackLimit = 5;
		}
		if (thType == ThoughtType.WitnessedDeath)
		{
			thoughtDefinition.label = "Witnessed someone's death";
			thoughtDefinition.description = "I saw someone die. They were alive one moment, and dead the next.";
			thoughtDefinition.baseHappinessEffect = -10f;
			thoughtDefinition.baseFearEffect = 20f;
			thoughtDefinition.duration = 100000;
			thoughtDefinition.stackedEffectMultiplier = 0.4f;
			thoughtDefinition.stackLimit = 20;
		}
		if (thType == ThoughtType.PrisonerFriendlyChat)
		{
			thoughtDefinition.label = "Convinced by warden";
			thoughtDefinition.description = "That warden and I had a talk about the colony. It sounds like a good place...";
			thoughtDefinition.baseHappinessEffect = 2.6f;
			thoughtDefinition.duration = 120000;
			thoughtDefinition.stackedEffectMultiplier = 0.98f;
			thoughtDefinition.stackLimit = 40;
		}
		if (thType == ThoughtType.PrisonerBeatenMild)
		{
			thoughtDefinition.label = "Beaten by warden";
			thoughtDefinition.description = "That warden just beat me up. He didn't break anything but, damn, it hurts.";
			thoughtDefinition.baseHappinessEffect = -3f;
			thoughtDefinition.baseFearEffect = 3f;
			thoughtDefinition.duration = 100000;
			thoughtDefinition.stackedEffectMultiplier = 0.95f;
			thoughtDefinition.stackLimit = 20;
		}
		if (thType == ThoughtType.PrisonerBeatenVicious)
		{
			thoughtDefinition.label = "Beaten viciously by warden";
			thoughtDefinition.description = "That warden just layed into me with extreme viciousness. They're not holding back. I could die from this so easily.";
			thoughtDefinition.baseHappinessEffect = -6f;
			thoughtDefinition.baseFearEffect = 6f;
			thoughtDefinition.duration = 100000;
			thoughtDefinition.stackedEffectMultiplier = 0.95f;
			thoughtDefinition.stackLimit = 20;
		}
		if (thType == ThoughtType.SocialTalk)
		{
			thoughtDefinition.label = "Had social chat";
			thoughtDefinition.description = "I just had a good chat with someone.";
			thoughtDefinition.baseHappinessEffect = 4f;
			thoughtDefinition.duration = 20000;
			thoughtDefinition.stackLimit = 3;
			thoughtDefinition.stackedEffectMultiplier = 0.5f;
		}
		return thoughtDefinition;
	}
}
