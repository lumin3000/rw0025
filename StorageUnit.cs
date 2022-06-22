using System;
using System.Collections.Generic;

public class StorageUnit
{
	private List<int> resourceAmounts = new List<int>();

	public int TotalCapacity = 100;

	public StorageUnit()
	{
		resourceAmounts = new List<int>();
		int length = Enum.GetValues(typeof(EntityType)).Length;
		for (int i = 0; i < length; i++)
		{
			resourceAmounts.Add(0);
		}
	}

	public int AmountOf(EntityType resType)
	{
		return resourceAmounts[(int)resType];
	}

	public void Gain(EntityType resType, int Amount)
	{
		List<int> list;
		List<int> list2 = (list = resourceAmounts);
		int index;
		int index2 = (index = (int)resType);
		index = list[index];
		list2[index2] = index + Amount;
	}

	public int AvailableCapacity()
	{
		int num = TotalCapacity;
		foreach (int resourceAmount in resourceAmounts)
		{
			num -= resourceAmount;
		}
		return num;
	}
}
