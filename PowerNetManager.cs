using System.Collections.Generic;
using System.Linq;

public static class PowerNetManager
{
	private static List<PowerNet> allNets;

	private static List<Building> newTransmitters;

	private static List<Building> destroyedTransmitters;

	private static List<Building> newConnectors;

	private static List<Building> oldConnectors;

	public static void ResetStaticData()
	{
		allNets = new List<PowerNet>();
		newTransmitters = new List<Building>();
		destroyedTransmitters = new List<Building>();
		newConnectors = new List<Building>();
		oldConnectors = new List<Building>();
	}

	public static void Notify_TransmitterSpawned(Building newTransmitter)
	{
		newTransmitters.Add(newTransmitter);
		NotifyDrawersForWireUpdate(newTransmitter.Position);
	}

	public static void Notify_TransmitterDespawned(Building oldTransmitter)
	{
		destroyedTransmitters.Add(oldTransmitter);
		NotifyDrawersForWireUpdate(oldTransmitter.Position);
	}

	public static void Notify_ConnectorSpawned(Building newCon)
	{
		newConnectors.Add(newCon);
		NotifyDrawersForWireUpdate(newCon.Position);
	}

	public static void Notify_ConnectorDespawned(Building oldCon)
	{
		oldConnectors.Add(oldCon);
		NotifyDrawersForWireUpdate(oldCon.Position);
	}

	private static void NotifyDrawersForWireUpdate(IntVec3 root)
	{
		Find.MapDrawer.MapChanged(root, MapChangeType.Things);
	}

	public static void RegisterPowerNet(PowerNet newNet)
	{
		allNets.Add(newNet);
		PowerNetGrid.Notify_PowerNetCreated(newNet);
		PowerNetMaker.UpdateVisualLinkagesFor(newNet);
	}

	public static void DeletePowerNet(PowerNet oldNet)
	{
		allNets.Remove(oldNet);
		PowerNetGrid.Notify_PowerNetDeleted(oldNet);
	}

	public static void PowerNetsTick()
	{
		foreach (PowerNet allNet in allNets)
		{
			allNet.PowerNetTick();
		}
	}

	public static void UpdatePowerNetsAndConnections_First()
	{
		foreach (Building newTransmitter in newTransmitters)
		{
			foreach (IntVec3 item in Gen.AdjacentSquaresCardinal(newTransmitter))
			{
				if (item.InBounds())
				{
					PowerNet powerNet = PowerNetGrid.TransmittedPowerNetAt(item);
					if (powerNet != null)
					{
						DeletePowerNet(powerNet);
					}
				}
			}
		}
		foreach (Building destroyedTransmitter in destroyedTransmitters)
		{
			PowerNet powerNet2 = PowerNetGrid.TransmittedPowerNetAt(destroyedTransmitter.Position);
			if (powerNet2 != null)
			{
				DeletePowerNet(powerNet2);
			}
		}
		foreach (Building newTransmitter2 in newTransmitters)
		{
			if (PowerNetGrid.TransmittedPowerNetAt(newTransmitter2.Position) == null)
			{
				RegisterPowerNet(PowerNetMaker.NewPowerNetStartingFrom(newTransmitter2));
			}
		}
		foreach (Building destroyedTransmitter2 in destroyedTransmitters)
		{
			foreach (IntVec3 item2 in Gen.AdjacentSquaresCardinal(destroyedTransmitter2))
			{
				if (item2.InBounds() && PowerNetGrid.TransmittedPowerNetAt(item2) == null)
				{
					Building building = (Building)(from b in Find.Grids.ThingsAt(item2)
						where b.def.transmitsPower
						select b).FirstOrDefault();
					if (building != null)
					{
						RegisterPowerNet(PowerNetMaker.NewPowerNetStartingFrom(building));
					}
				}
			}
		}
		foreach (Building newConnector in newConnectors)
		{
			PowerConnectionMaker.Notify_PostConnectorSpawned(newConnector);
		}
		foreach (Building oldConnector in oldConnectors)
		{
			PowerConnectionMaker.Notify_PostConnectorDespawned(oldConnector);
		}
		foreach (Building newTransmitter3 in newTransmitters)
		{
			PowerConnectionMaker.Notify_PostTransmitterSpawned(newTransmitter3);
		}
		foreach (Building destroyedTransmitter3 in destroyedTransmitters)
		{
			PowerConnectionMaker.Notify_PostTransmitterDespawned(destroyedTransmitter3);
		}
		newTransmitters.Clear();
		destroyedTransmitters.Clear();
		newConnectors.Clear();
		oldConnectors.Clear();
		if (DebugSettings.drawReportPower)
		{
			DrawDebugPowerNets();
		}
	}

	private static void DrawDebugPowerNets()
	{
		int num = 0;
		foreach (PowerNet allNet in allNets)
		{
			foreach (Building item in allNet.transmitters.Concat(allNet.connectors))
			{
				foreach (IntVec3 item2 in Gen.SquaresOccupiedBy(item))
				{
					DebugRender.RenderSquare(item2, num);
				}
			}
			num++;
		}
	}
}
