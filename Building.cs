using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Building : ThingWithComponents
{
	private const float OxyLeakRate_LightDamage = 0.005f;

	private const float OxyLeakRate_HeavyDamage = 0.01f;

	private const float OxyLeakRate_Dead = 0.05f;

	private const float OxyLeakRate_Open = 0.125f;

	private const float GasGraphicChanceMultipler = 8f;

	private const int LeavingStackBaseSize = 30;

	private const int LeavingMinStackSize = 15;

	private const int LeavingStackVariance = 5;

	private const float LeavingResourcePercent = 0.3f;

	private bool doLeavings;

	public PowerNet powerNet;

	public Building connectedToTransmitter;

	public List<Building> connectees = new List<Building>();

	public PowerNet ConnectedToNet
	{
		get
		{
			if (connectedToTransmitter == null)
			{
				return null;
			}
			return PowerNetGrid.TransmittedPowerNetAt(connectedToTransmitter.Position);
		}
	}

	public override Mesh DrawMesh
	{
		get
		{
			IntVec2 size = def.size;
			if (def.overDraw)
			{
				size.x += 2;
				size.z += 2;
			}
			return MeshPool.gridPlanes[size.x, size.z];
		}
	}

	public IntVec3 InteractionSquare => InteractionSquareWhenAt(def, base.Position, rotation);

	public int RepairUrgency
	{
		get
		{
			if (health < def.maxHealth && def.useStandardHealth)
			{
				if (def.isBarrier)
				{
					if (base.HealthState == HealthState.LightDamage)
					{
						return 2;
					}
					int num = 0;
					foreach (IntVec3 item in base.Position.AdjacentSquares8Way())
					{
						if (!Find.Grids.HasBarrierAt(item) && Find.Grids.GetRoomAt(item) == null)
						{
							num = 10;
							break;
						}
					}
					if (base.HealthState == HealthState.Dead)
					{
						return 10 + num;
					}
					if (base.HealthState == HealthState.HeavyDamage)
					{
						return 5 + num;
					}
				}
				return 1;
			}
			return 0;
		}
	}

	public override void SpawnSetup()
	{
		base.SpawnSetup();
		Find.BuildingManager.RegisterBuilding(this);
		if (def.isBarrier)
		{
			Find.RoomManager.BarrierSpawned(this);
		}
		if (def.transmitsPower)
		{
			PowerNetManager.Notify_TransmitterSpawned(this);
		}
		if (def.ConnectToPower)
		{
			PowerNetManager.Notify_ConnectorSpawned(this);
		}
		if (def.linkDrawer != null)
		{
			LinkGrid.Notify_LinkerCreatedOrDestroyed(this);
			Find.MapDrawer.MapChanged(base.Position, MapChangeType.Things, regenAdjacentSquares: true, regenAdjacentSections: false);
		}
		if (def.fillsSquare)
		{
			Find.MapDrawer.MapChanged(base.Position, MapChangeType.Terrain);
		}
		if (def.transmitsPower || def.ConnectToPower)
		{
			Find.MapDrawer.MapChanged(base.Position, MapChangeType.PowerGrid, regenAdjacentSquares: true, regenAdjacentSections: false);
		}
	}

	public override void Killed(DamageInfo d)
	{
		doLeavings = true;
		base.Killed(d);
	}

	public override void Destroy()
	{
		base.Destroy();
		Find.BuildingManager.DeRegisterBuilding(this);
		if (def.isBarrier)
		{
			Find.RoomManager.BarrierRemovedAt(base.Position);
		}
		if (def.transmitsPower)
		{
			PowerNetManager.Notify_TransmitterDespawned(this);
		}
		if (def.ConnectToPower)
		{
			PowerNetManager.Notify_ConnectorDespawned(this);
		}
		if (def.linkDrawer != null)
		{
			LinkGrid.Notify_LinkerCreatedOrDestroyed(this);
			Find.MapDrawer.MapChanged(base.Position, MapChangeType.Things, regenAdjacentSquares: true, regenAdjacentSections: false);
		}
		if (def.transmitsPower || def.ConnectToPower)
		{
			Find.MapDrawer.MapChanged(base.Position, MapChangeType.PowerGrid, regenAdjacentSquares: true, regenAdjacentSections: false);
		}
	}

	public override void DestroyFinalize()
	{
		base.DestroyFinalize();
		DropLeavings();
		if (def.makeFog)
		{
			Find.FogGrid.Notify_FogBlockerDestroyed(base.Position);
		}
		if (def.fillsSquare)
		{
			RoofCollapseChecker.Notify_RoofHolderDestroyed(this);
		}
		if (def.leaveTerrain == null || !Find.Map.initialized)
		{
			return;
		}
		foreach (IntVec3 item in Gen.SquaresOccupiedBy(this))
		{
			Find.TerrainGrid.SetTerrain(item, def.leaveTerrain);
		}
	}

	public override void Draw()
	{
		if (!def.addToMapMesh)
		{
			if (def.linkDrawer != null)
			{
				Debug.LogWarning("Linkdrawers don't support realtime drawing.");
			}
			else
			{
				base.Draw();
			}
		}
		if (def.buildingWantsAir && !this.HasAir())
		{
			OverlayDrawer.DrawOverlay(this, OverlayTypes.NeedsO2);
		}
		if (health < def.maxHealth && def.useStandardHealth)
		{
			OverlayDrawer.DrawOverlay(this, OverlayTypes.Damaged);
		}
		Comps_Draw();
	}

	public override IEnumerable<MapMeshPiece> EmitMapMeshPieces()
	{
		if (connectedToTransmitter != null)
		{
			yield return PowerNetGraphics.WirePieceConnecting(this, connectedToTransmitter, forPowerOverlay: false);
		}
		if (def.linkDrawer != null)
		{
			foreach (MapMeshPiece mapMeshPiece in def.linkDrawer.GetMapMeshPieces(this))
			{
				yield return mapMeshPiece;
			}
			yield break;
		}
		foreach (MapMeshPiece item in base.EmitMapMeshPieces())
		{
			yield return item;
		}
	}

	public IEnumerable<MapMeshPiece> EmitMapMeshPiecesForPowerGrid()
	{
		if (def.transmitsPower)
		{
			foreach (IntVec3 sq in Gen.SquaresOccupiedBy(this))
			{
				foreach (MapMeshPiece mapMeshPiece in LinkDrawers.transmitterOverlay.GetMapMeshPieces(this, sq))
				{
					yield return mapMeshPiece;
				}
			}
		}
		if (def.ConnectToPower)
		{
			yield return PowerNetGraphics.OverlayConnectorBaseFor(this);
		}
		if (connectedToTransmitter != null)
		{
			yield return PowerNetGraphics.WirePieceConnecting(this, connectedToTransmitter, forPowerOverlay: true);
		}
	}

	public static IntVec3 InteractionSquareWhenAt(EntityDefinition tDef, IntVec3 loc, IntRot rot)
	{
		IntVec3 intVec = tDef.interactionSquareOffset.RotatedBy(rot);
		return loc + intVec;
	}

	public override void Tick()
	{
		base.Tick();
		if (!def.isBarrier || DebugSettings.worldBreathable)
		{
			return;
		}
		float num = 0f;
		if (base.HealthState == HealthState.LightDamage)
		{
			num = 0.005f;
		}
		else if (base.HealthState == HealthState.HeavyDamage)
		{
			num = 0.01f;
		}
		else if (base.HealthState == HealthState.Dead)
		{
			num = 0.05f;
		}
		if (def.eType == EntityType.Door && ((Building_Door)this).isOpen)
		{
			num = 0.125f;
		}
		if (!(num > 0f))
		{
			return;
		}
		List<IntVec3> list = new List<IntVec3>();
		foreach (IntVec3 item in base.Position.AdjacentSquares8Way())
		{
			if (!Find.Grids.HasBarrierAt(item))
			{
				list.Add(item);
			}
		}
		list = Gen.CombineGroupedSquares(list);
		float num2 = 0f;
		List<Room> list2 = new List<Room>();
		List<IntVec3> list3 = new List<IntVec3>();
		foreach (IntVec3 item2 in list)
		{
			num2 += item2.AirPressure();
			Room roomAt = Find.Grids.GetRoomAt(item2);
			if (roomAt != null)
			{
				list2.Add(roomAt);
			}
			else
			{
				list3.Add(item2);
			}
		}
		float num3 = num2 / (float)list.Count;
		foreach (Room item3 in list2)
		{
			if (item3.airTight)
			{
				float num4 = num3 - item3.AirPressure;
				item3.Air += num4 * num;
			}
		}
		foreach (IntVec3 item4 in list3)
		{
			float num5 = 0f;
			num5 = num * 8f;
			num5 *= num3;
			if (UnityEngine.Random.value < num5)
			{
				MoteMaker.ThrowAirPuff(base.Position, item4, highSpeed: true);
			}
		}
	}

	private void DropLeavings()
	{
		if (!Find.Map.initialized || !doLeavings)
		{
			return;
		}
		List<LeavingRecord> list = def.leavings.ListFullCopy();
		if (def.leaveResources)
		{
			foreach (ResourceCost cost in def.costList)
			{
				list.Add(new LeavingRecord(cost.rType, (int)Math.Round((float)cost.Amount * 0.3f)));
			}
		}
		if (list.Count == 0)
		{
			return;
		}
		List<IntVec3> list2 = Gen.SquaresOccupiedBy(this).ToList();
		foreach (LeavingRecord item in list)
		{
			if (item.ThingDefToSpawn == null)
			{
				Debug.LogWarning(string.Concat(this, " cannot drop leavings: got null ThingDefToSpawn from record ", item));
				continue;
			}
			List<int> list3 = new List<int>();
			if (item.ThingDefToSpawn.stackLimit == 1)
			{
				for (int i = 0; i < item.count; i++)
				{
					list3.Add(1);
				}
			}
			else
			{
				int num = (int)Math.Ceiling((float)item.count / 30f);
				int num2 = (int)Math.Round((float)item.count / (float)num);
				int num3 = item.count;
				while (num3 > 0)
				{
					int num4 = num2;
					num4 += UnityEngine.Random.Range(-5, 5);
					if (num4 > num3)
					{
						num4 = num3;
					}
					if (num4 > 0)
					{
						if (num4 + 15 >= num3)
						{
							num4 = num3;
						}
						list3.Add(num4);
						num3 -= num4;
					}
				}
			}
			foreach (int item2 in list3)
			{
				if (list2.Count <= 0)
				{
					Debug.LogError(string.Concat(this, " can't spawn all it's debris. Not enough occupied spaces."));
					break;
				}
				IntVec3 intVec = list2.RandomElement();
				list2.Remove(intVec);
				Thing thing = ThingMaker.MakeThing(item.ThingDefToSpawn);
				thing.stackCount = item2;
				ThingMaker.Spawn(thing, intVec);
				ThingWithComponents thingWithComponents = thing as ThingWithComponents;
				if (thingWithComponents != null && thingWithComponents.GetComp<CompForbiddable>() != null)
				{
					thingWithComponents.GetComp<CompForbiddable>().forbidden = true;
				}
			}
		}
	}
}
