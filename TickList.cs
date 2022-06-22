using System;
using System.Collections.Generic;
using UnityEngine;

public class TickList
{
	private TickerType tickType;

	private List<List<Thing>> thingLists = new List<List<Thing>>();

	private List<Thing> thingsToRegister = new List<Thing>();

	private List<Thing> thingsToDeregister = new List<Thing>();

	private int Interval => tickType switch
	{
		TickerType.Normal => 1, 
		TickerType.Rare => 250, 
		_ => -1, 
	};

	public TickList(TickerType tickType)
	{
		this.tickType = tickType;
		for (int i = 0; i < Interval; i++)
		{
			thingLists.Add(new List<Thing>());
		}
	}

	public void RegisterThing(Thing t)
	{
		thingsToRegister.Add(t);
	}

	public void DeregisterThing(Thing t)
	{
		thingsToDeregister.Add(t);
	}

	public void SingleTick()
	{
		foreach (Thing item in thingsToRegister)
		{
			BucketOf(item).Add(item);
		}
		thingsToRegister.Clear();
		foreach (Thing item2 in thingsToDeregister)
		{
			BucketOf(item2).Remove(item2);
		}
		thingsToDeregister.Clear();
		if (DebugSettings.fastEcology)
		{
			foreach (List<Thing> thingList in thingLists)
			{
				foreach (Thing item3 in thingList)
				{
					if (item3.def.IsPlant)
					{
						item3.TickRare();
					}
				}
			}
		}
		List<Thing> list = thingLists[Find.TickManager.tickCount % Interval];
		List<Thing> list2 = new List<Thing>();
		foreach (Thing item4 in list)
		{
			try
			{
				if (tickType == TickerType.Normal)
				{
					item4.Tick();
				}
				else
				{
					item4.TickRare();
				}
			}
			catch (Exception ex)
			{
				if (Debug.isDebugBuild)
				{
					throw ex;
				}
				list2.Add(item4);
			}
		}
		if (!Debug.isDebugBuild)
		{
			return;
		}
		foreach (Thing item5 in list2)
		{
			item5.Destroy();
		}
	}

	private List<Thing> BucketOf(Thing t)
	{
		int num = t.GetHashCode();
		if (num < 0)
		{
			num *= -1;
		}
		int index = num % Interval;
		return thingLists[index];
	}
}
