using System;
using System.Collections.Generic;
using UnityEngine;

public class AirNetwork
{
	public List<Building_AirNetworkable> airBuildings = new List<Building_AirNetworkable>();

	private int consumeTickStartInd;

	public float TotalAir
	{
		get
		{
			float num = 0f;
			foreach (Building_AirNetworkable airBuilding in airBuildings)
			{
				num += airBuilding.StoredAir;
			}
			return num;
		}
	}

	public float TotalAirMax
	{
		get
		{
			float num = 0f;
			foreach (Building_AirNetworkable airBuilding in airBuildings)
			{
				num += airBuilding.StoredAirMax;
			}
			return num;
		}
	}

	public void AirNetTick()
	{
		consumeTickStartInd++;
		if (consumeTickStartInd >= airBuildings.Count)
		{
			consumeTickStartInd = 0;
		}
		for (int i = 0; i < airBuildings.Count; i++)
		{
			int index = (i + consumeTickStartInd) % airBuildings.Count;
			airBuildings[index].Tick_ConsumeAir();
		}
	}

	public void LoseAir(float amount)
	{
		foreach (Building_AirNetworkable airBuilding in airBuildings)
		{
			float num = Math.Min(amount, airBuilding.StoredAir);
			airBuilding.StoredAir -= num;
			amount -= num;
			if (amount <= 0f)
			{
				return;
			}
		}
		Debug.LogError("Took air from the network that wasn't there");
	}

	public void GainAir(float Amount)
	{
		foreach (Building_AirNetworkable airBuilding in airBuildings)
		{
			float num = Math.Min(Amount, airBuilding.StoredAirMax - airBuilding.StoredAir);
			airBuilding.StoredAir += num;
			Amount -= num;
			if (Amount <= 0f)
			{
				break;
			}
		}
	}
}
