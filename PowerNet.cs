using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class PowerNet
{
	private const int MaxRestartTryInterval = 200;

	private const int MinRestartTryInterval = 30;

	private const int ShutdownInterval = 20;

	private const float MinStoredEnergyToTurnOn = 5f;

	public bool hasPowerSource;

	public List<Building> connectors = new List<Building>();

	public List<Building> transmitters = new List<Building>();

	public List<CompPowerTrader> powerComps = new List<CompPowerTrader>();

	public List<CompPowerBattery> batteryComps = new List<CompPowerBattery>();

	private float debugLastCreatedEnergy;

	private float debugLastRawStoredEnergy;

	private float debugLastApparentStoredEnergy;

	public PowerNet(IEnumerable<Building> newTransmitters)
	{
		foreach (Building newTransmitter in newTransmitters)
		{
			transmitters.Add(newTransmitter);
			newTransmitter.powerNet = this;
			RegisterComponentsOf(newTransmitter);
			foreach (Building connectee in newTransmitter.connectees)
			{
				RegisterConnector(connectee);
			}
		}
		hasPowerSource = false;
		foreach (Building transmitter in transmitters)
		{
			if (transmitter.def.IsPowerSource)
			{
				hasPowerSource = true;
				break;
			}
		}
	}

	public void RegisterConnector(Building b)
	{
		connectors.Add(b);
		RegisterComponentsOf(b);
	}

	public void DeregisterConnector(Building b)
	{
		connectors.Remove(b);
		DeregisterComponentsOf(b);
	}

	private void RegisterComponentsOf(Building b)
	{
		CompPowerTrader comp = b.GetComp<CompPowerTrader>();
		if (comp != null)
		{
			powerComps.Add(comp);
		}
		CompPowerBattery comp2 = b.GetComp<CompPowerBattery>();
		if (comp2 != null)
		{
			batteryComps.Add(comp2);
		}
	}

	private void DeregisterComponentsOf(Building b)
	{
		CompPowerTrader comp = b.GetComp<CompPowerTrader>();
		if (comp != null)
		{
			powerComps.Remove(comp);
		}
		CompPowerBattery comp2 = b.GetComp<CompPowerBattery>();
		if (comp2 != null)
		{
			batteryComps.Remove(comp2);
		}
	}

	public float CurrentEnergyGainRate()
	{
		if (DebugSettings.unlimitedPower)
		{
			return 9999999f;
		}
		return powerComps.Where((CompPowerTrader c) => c.PowerOn).Sum((CompPowerTrader c) => c.EnergyPerTick);
	}

	public float CurrentStoredEnergy()
	{
		return batteryComps.Sum((CompPowerBattery c) => c.storedEnergy);
	}

	public void PowerNetTick()
	{
		float num = CurrentEnergyGainRate();
		float num2 = CurrentStoredEnergy();
		if (num2 + num >= 0f && !Find.MapConditionManager.ConditionIsActive(MapConditionType.SolarFlare))
		{
			float num3 = ((batteryComps.Count <= 0) ? num2 : (num2 - 5f));
			if (Debug.isDebugBuild)
			{
				debugLastApparentStoredEnergy = num3;
				debugLastCreatedEnergy = num;
				debugLastRawStoredEnergy = num2;
			}
			if (num3 + num >= 0f)
			{
				List<CompPowerTrader> list = powerComps.Where((CompPowerTrader part) => !part.PowerOn && part.DesirePowerOn).ToList();
				if (list.Count > 0)
				{
					int num4 = 200 / list.Count;
					if (num4 < 30)
					{
						num4 = 30;
					}
					if (Find.TickManager.tickCount % num4 == 0)
					{
						CompPowerTrader compPowerTrader = list.RandomElement();
						if (num + num2 + compPowerTrader.EnergyPerTick >= 0f)
						{
							compPowerTrader.PowerOn = true;
							num += compPowerTrader.EnergyPerTick;
						}
					}
				}
			}
			if (num > 0f)
			{
				StoreEnergy(num);
			}
			else
			{
				DrawEnergy(0f - num);
			}
		}
		else if (Find.TickManager.tickCount % 20 == 0)
		{
			List<CompPowerTrader> list2 = powerComps.Where((CompPowerTrader part) => part.PowerOn && part.EnergyPerTick < 0f).ToList();
			if (list2.Count > 0)
			{
				list2.RandomElement().PowerOn = false;
			}
		}
	}

	private void StoreEnergy(float extra)
	{
		List<CompPowerBattery> list = batteryComps.Where((CompPowerBattery b) => b.AmountCanAccept > 0.01f).ToList();
		float val = extra / (float)list.Count;
		foreach (CompPowerBattery item in list)
		{
			float num = Math.Min(val, item.AmountCanAccept);
			item.StorePower(num);
			extra -= num;
			if (extra < 0.01f)
			{
				break;
			}
		}
	}

	private void DrawEnergy(float debt)
	{
		List<CompPowerBattery> list = batteryComps.Where((CompPowerBattery b) => b.storedEnergy > float.Epsilon).ToList();
		float a = debt / (float)list.Count;
		int num = 0;
		while (debt > float.Epsilon)
		{
			foreach (CompPowerBattery item in list)
			{
				float num2 = Mathf.Min(a, item.storedEnergy);
				item.DrawPower(num2);
				debt -= num2;
				if (debt < float.Epsilon)
				{
					return;
				}
			}
			num++;
			if (num > 10)
			{
				break;
			}
		}
		if (debt > 0f)
		{
			Debug.LogWarning("Drew energy from a PowerNet that didn't have it.");
		}
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("POWERNET:");
		stringBuilder.AppendLine("  Created energy: " + debugLastCreatedEnergy);
		stringBuilder.AppendLine("  Raw stored energy: " + debugLastRawStoredEnergy);
		stringBuilder.AppendLine("  Apparent stored energy: " + debugLastApparentStoredEnergy);
		stringBuilder.AppendLine("  Transmitters: ");
		foreach (Building transmitter in transmitters)
		{
			stringBuilder.AppendLine("      " + transmitter);
		}
		stringBuilder.AppendLine("  Connectors: ");
		foreach (Building connector in connectors)
		{
			stringBuilder.AppendLine("      " + connector);
		}
		return stringBuilder.ToString();
	}
}
