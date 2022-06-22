using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceManager
{
	protected List<int> resourceAmounts = new List<int>();

	public int Money
	{
		get
		{
			return TotalAmountOf(EntityType.Money);
		}
		set
		{
			SetAmount(EntityType.Money, value);
		}
	}

	public int Food
	{
		get
		{
			return TotalAmountOf(EntityType.Food);
		}
		set
		{
			SetAmount(EntityType.Food, value);
		}
	}

	public int Metal
	{
		get
		{
			return TotalAmountOf(EntityType.Metal);
		}
		set
		{
			SetAmount(EntityType.Metal, value);
		}
	}

	public int Uranium
	{
		get
		{
			return TotalAmountOf(EntityType.Uranium);
		}
		set
		{
			SetAmount(EntityType.Uranium, value);
		}
	}

	public int Medicine
	{
		get
		{
			return TotalAmountOf(EntityType.Medicine);
		}
		set
		{
			SetAmount(EntityType.Medicine, value);
		}
	}

	public int Shells
	{
		get
		{
			return TotalAmountOf(EntityType.Shells);
		}
		set
		{
			SetAmount(EntityType.Shells, value);
		}
	}

	public int Missiles
	{
		get
		{
			return TotalAmountOf(EntityType.Missiles);
		}
		set
		{
			SetAmount(EntityType.Missiles, value);
		}
	}

	public ResourceManager()
	{
		resourceAmounts = new List<int>();
		int length = Enum.GetValues(typeof(EntityType)).Length;
		for (int i = 0; i < length; i++)
		{
			resourceAmounts.Add(0);
		}
	}

	public void Expose()
	{
		Scribe.EnterNode("Resources");
		foreach (ThingDefinition item in ThingDefDatabase.AllThingDefinitions.Where((ThingDefinition def) => def.isResource))
		{
			int value = TotalAmountOf(item.eType);
			Scribe.LookField(ref value, item.label);
			resourceAmounts[(int)item.eType] = value;
		}
		Scribe.ExitNode();
	}

	public int TotalAmountOf(EntityType rType)
	{
		return resourceAmounts[(int)rType];
	}

	public static Rect ResourceRect()
	{
		return new Rect(Screen.width / 2 - 180, 0f, 500f, 300f);
	}

	public void Gain(EntityType resType, int amount)
	{
		SetAmount(resType, TotalAmountOf(resType) + amount);
	}

	public void SetAmount(EntityType resType, int amount)
	{
		resourceAmounts[(int)resType] = amount;
		if (amount < 0)
		{
			Debug.LogError(string.Concat("Just set amount of ", resType, " to ", amount, ", which is less than 0."));
		}
	}

	public void DayPassed()
	{
	}
}
