using System.Collections.Generic;
using UnityEngine;

public static class PowerConnectionMaker
{
	private const float ConnectRadius = 5.9f;

	private static readonly int ConnectSearchNumSquares;

	static PowerConnectionMaker()
	{
		ConnectSearchNumSquares = Gen.NumSquaresInRadius(5.9f);
	}

	public static void Notify_PostTransmitterSpawned(Building newTransmitter)
	{
		foreach (Building item in PotentialConnectorsForTransmitter(newTransmitter))
		{
			if (item.connectedToTransmitter == null)
			{
				item.ConnectToTransmitter(newTransmitter);
			}
		}
	}

	public static void Notify_PostTransmitterDespawned(Building deadTransmitter)
	{
		foreach (Building connectee in deadTransmitter.connectees)
		{
			connectee.connectedToTransmitter = null;
			CompPowerTrader comp = connectee.GetComp<CompPowerTrader>();
			if (comp != null)
			{
				comp.PowerOn = false;
			}
			TryConnectNewTransmitterFor(connectee);
		}
	}

	public static void Notify_PostConnectorSpawned(Building c)
	{
		if (c.connectedToTransmitter == null)
		{
			TryConnectNewTransmitterFor(c);
		}
	}

	public static void Notify_PostConnectorDespawned(Building c)
	{
		if (c.connectedToTransmitter != null)
		{
			if (c.ConnectedToNet != null)
			{
				c.ConnectedToNet.DeregisterConnector(c);
			}
			c.connectedToTransmitter.connectees.Remove(c);
		}
	}

	private static void TryConnectNewTransmitterFor(Building c)
	{
		Building building = BestTransmitterForConnector(c.Position);
		if (building != null)
		{
			c.ConnectToTransmitter(building);
		}
		else
		{
			c.connectedToTransmitter = null;
		}
	}

	private static void ConnectToTransmitter(this Building c, Building b)
	{
		if (c.connectedToTransmitter != null)
		{
			Debug.LogWarning(string.Concat(c, " is already connected to ", c.connectedToTransmitter, ", will not connect to ", b));
			return;
		}
		if (b == null)
		{
			Debug.LogWarning(string.Concat(c, " should not connect to null transmitter using ConnectToTransmitter."));
			c.connectedToTransmitter = null;
			return;
		}
		c.connectedToTransmitter = b;
		c.connectedToTransmitter.connectees.Add(c);
		PowerNet connectedToNet = c.ConnectedToNet;
		if (connectedToNet != null)
		{
			connectedToNet.RegisterConnector(c);
			return;
		}
		Debug.LogWarning(string.Concat("Null connected net for ", c, " with transmitter ", b));
	}

	public static Building BestTransmitterForConnector(IntVec3 connectorLoc)
	{
		for (int i = 0; i < ConnectSearchNumSquares; i++)
		{
			IntVec3 sq = connectorLoc + Gen.RadialPattern[i];
			foreach (Thing item in Find.Grids.ThingsAt(sq))
			{
				if (item.def.transmitsPower && !item.destroyed)
				{
					return (Building)item;
				}
			}
		}
		return null;
	}

	private static IEnumerable<Building> PotentialConnectorsForTransmitter(Building b)
	{
		for (int i = 0; i < ConnectSearchNumSquares; i++)
		{
			IntVec3 sq = b.Position + Gen.RadialPattern[i];
			foreach (Thing t in Find.Grids.ThingsAt(sq))
			{
				if (t.def.ConnectToPower)
				{
					yield return (Building)t;
				}
			}
		}
	}
}
