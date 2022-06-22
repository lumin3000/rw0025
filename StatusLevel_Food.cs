using System.Text;
using UnityEngine;

public class StatusLevel_Food : StatusLevel
{
	private const float BaseFoodFallRate = 0.004f;

	private const int TicksBetweenStarveDamage = 3000;

	private const int StarvationDamAmount = 5;

	private int TicksToNextStarveDamage;

	public override string Label => "Food";

	public override bool ShouldTrySatisfy => CurHungerLevel >= HungerLevel.Hungry;

	public bool Starving => CurHungerLevel == HungerLevel.Starving;

	public bool UrgentlyHungry => CurHungerLevel == HungerLevel.UrgentlyHungry;

	public bool Hungry => CurHungerLevel == HungerLevel.Hungry;

	public HungerLevel CurHungerLevel
	{
		get
		{
			RaceDefinition raceDef = pawn.raceDef;
			if (base.curLevel < 0.01f)
			{
				return HungerLevel.Starving;
			}
			if (base.curLevel < raceDef.hungerThreshold * 0.66666f)
			{
				return HungerLevel.UrgentlyHungry;
			}
			if (base.curLevel < raceDef.hungerThreshold)
			{
				return HungerLevel.Hungry;
			}
			return HungerLevel.Fed;
		}
	}

	public float FoodFallRate => CurHungerLevel switch
	{
		HungerLevel.Fed => 0.004f, 
		HungerLevel.Hungry => 0.002f, 
		HungerLevel.UrgentlyHungry => 0.001f, 
		HungerLevel.Starving => 0.0006f, 
		_ => 999f, 
	};

	public override int RateOfChange => -1;

	public StatusLevel_Food(Pawn pawn)
		: base(pawn)
	{
		if (pawn.raceDef.humanoid)
		{
			base.curLevel = 80f;
		}
		else
		{
			base.curLevel = Random.Range(50, 90);
		}
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe.LookField(ref TicksToNextStarveDamage, "TicksToNextStarveDamage");
	}

	public override void StatusLevelTick()
	{
		base.curLevel -= FoodFallRate;
		if (Starving)
		{
			TicksToNextStarveDamage--;
			if (TicksToNextStarveDamage <= 0)
			{
				pawn.TakeDamage(new DamageInfo(DamageType.Starvation, 5));
				TicksToNextStarveDamage = 3000;
			}
		}
	}

	public void Notify_ThingEaten(Thing eatenThing)
	{
		if (!eatenThing.def.Edible)
		{
			Debug.LogWarning(string.Concat(pawn, " ate ", eatenThing, " which is not defined as edible."));
		}
		Edible edible = eatenThing as Edible;
		if (edible == null)
		{
			Debug.LogError(string.Concat(pawn, " ate ", eatenThing, " which does not have Edible interface."));
			return;
		}
		edible.Notify_Eaten();
		if (pawn.psychology != null && eatenThing.def.food.eatenThoughtType != 0)
		{
			pawn.psychology.thoughts.GainThought(eatenThing.def.food.eatenThoughtType);
		}
		base.curLevel += edible.CurNutrition;
		if (base.curLevel > 100f)
		{
			base.curLevel = 100f;
		}
	}

	public override TooltipDef GetTooltipDef()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(base.TooltipBase);
		stringBuilder.AppendLine();
		stringBuilder.AppendLine();
		stringBuilder.Append("Food is how satisfied someone is with their meals.");
		stringBuilder.AppendLine();
		stringBuilder.AppendLine();
		stringBuilder.Append("Food resets when someone eats. After a while, it will begin falling until they eat again. At zero food, a person will take starvation damage.");
		return new TooltipDef(stringBuilder.ToString(), 11721);
	}
}
