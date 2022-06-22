using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class Thing : Entity, Saveable
{
	public ThingDefinition def;

	public int thingIDNumber = -1;

	private IntVec3 positionInt = new IntVec3(0, 0, 0);

	public IntRot rotation = default(IntRot);

	public int health;

	public List<AttachableThing> attachList = new List<AttachableThing>();

	public Pawn carrier;

	public int stackCount = 1;

	protected Material randomizedDrawMat;

	private float HealthThresh_HeavyDamage = 0.5f;

	public HealthState HealthState
	{
		get
		{
			if (!def.useStandardHealth)
			{
				return HealthState.Full;
			}
			if (health >= def.maxHealth)
			{
				return HealthState.Full;
			}
			if ((float)health > (float)def.maxHealth * HealthThresh_HeavyDamage)
			{
				return HealthState.LightDamage;
			}
			if (health > 1)
			{
				return HealthState.HeavyDamage;
			}
			return HealthState.Dead;
		}
	}

	public IntVec3 Position
	{
		get
		{
			return positionInt;
		}
		set
		{
			if (Find.Grids == null)
			{
				return;
			}
			Find.Grids.DeRegisterInGrids(this);
			positionInt = value;
			Find.Grids.RegisterInGrids(this);
			foreach (AttachableThing attach in attachList)
			{
				attach.Position = value;
			}
		}
	}

	public EntityType TType => def.eType;

	public string ThingID
	{
		get
		{
			if (def.HasThingIDNumber)
			{
				return def.definitionName + thingIDNumber;
			}
			return def.definitionName;
		}
		set
		{
			thingIDNumber = IDNumberFromThingID(value);
		}
	}

	public IntVec2 RotatedSize
	{
		get
		{
			if (!rotation.IsHorizontal)
			{
				return def.size;
			}
			return new IntVec2(def.size.z, def.size.x);
		}
	}

	public override string Label
	{
		get
		{
			if (stackCount == 1)
			{
				return def.label;
			}
			return def.label + " x" + stackCount;
		}
	}

	public virtual bool Selectable
	{
		get
		{
			if (!def.selectable || !spawnedInWorld)
			{
				return false;
			}
			if (def.size.x == 1 && def.size.z == 1)
			{
				return !Position.IsFogged();
			}
			foreach (IntVec3 item in Gen.SquaresOccupiedBy(this))
			{
				if (!item.IsFogged())
				{
					return true;
				}
			}
			return false;
		}
	}

	public virtual Mesh DrawMesh
	{
		get
		{
			if (!def.overDraw)
			{
				return MeshPool.plane10;
			}
			return MeshPool.plane30;
		}
	}

	public virtual Vector3 DrawPos => this.TrueCenter();

	public virtual Material DrawMat
	{
		get
		{
			if (randomizedDrawMat != null)
			{
				return randomizedDrawMat;
			}
			return def.drawMat;
		}
	}

	public virtual Vector3 ScreenPos
	{
		get
		{
			Vector3 result = Find.CameraMap.camera.WorldToScreenPoint(DrawPos);
			result.y = (float)Screen.height - result.y;
			return result;
		}
	}

	public Thing()
	{
		LoadedThingLookup.RegisterThingConstructed(this);
	}

	public static int IDNumberFromThingID(string thingID)
	{
		string value = Regex.Match(thingID, "\\d+$").Value;
		int result = 0;
		try
		{
			result = Convert.ToInt32(value);
			return result;
		}
		catch (Exception ex)
		{
			Debug.LogError("Could not convert id number from thingID=" + thingID + ", numString=" + value + " Exception=" + ex.ToString());
			return result;
		}
	}

	public override void SpawnSetup()
	{
		spawnedInWorld = true;
		if (Find.TickManager != null)
		{
			Find.TickManager.RegisterAllTickabilityFor(this);
		}
		Find.Map.thingLister.RegisterThingSpawned(this);
		if (def.addToMapMesh)
		{
			foreach (IntVec3 item in Gen.SquaresOccupiedBy(this))
			{
				Find.Map.mapDrawer.MapChanged(item, MapChangeType.Things);
			}
		}
		if (def.DrawEveryFrame)
		{
			Find.DrawManager.RegisterDrawable(this);
		}
		if (def.hasTooltip)
		{
			Find.TooltipGiverList.RegisterTooltipGiver(this);
		}
		if (def.passability == Traversability.Impassable)
		{
			Find.ReachabilityRegions.Notify_ImpassableThingAdded(this);
		}
		if (def.pathCost != 0)
		{
			Find.PathGrid.RecalculatePathCostUnder(this);
		}
	}

	public override void DeSpawn()
	{
		if (!spawnedInWorld)
		{
			Debug.LogWarning("Despawning " + Label + " that is not spawned in the world.");
		}
		Find.Map.thingLister.DeRegisterThingSpawned(this);
		Find.Grids.DeRegisterInGrids(this);
		if (def.hasTooltip)
		{
			Find.TooltipGiverList.DeregisterTooltipGiver(this);
		}
		if (def.addToMapMesh)
		{
			foreach (IntVec3 item in Gen.SquaresOccupiedBy(this))
			{
				Find.Map.mapDrawer.MapChanged(item, MapChangeType.Things);
			}
		}
		if (def.DrawEveryFrame)
		{
			Find.DrawManager.DeRegisterDrawable(this);
		}
		if (def.passability == Traversability.Impassable)
		{
			Find.ReachabilityRegions.Notify_ImpassableThingRemoved(this);
		}
		if (def.pathCost != 0)
		{
			Find.PathGrid.RecalculatePathCostUnder(this);
		}
		Find.TickManager.DeRegisterAllTickabilityFor(this);
		spawnedInWorld = false;
	}

	public virtual void Destroy()
	{
		if (!def.destroyable)
		{
			Debug.LogWarning("Tried to destroy non-destroyable thing " + this);
		}
		else
		{
			if (destroyed)
			{
				return;
			}
			destroyed = true;
			Find.DestroyManager.AddToDestroyList(this);
			if (def.designateHaulable || def.mineable)
			{
				Find.DesignationManager.RemoveAllDesignationsAt(Position);
			}
			foreach (AttachableThing item in attachList.ListFullCopy())
			{
				item.Destroy();
			}
			if (Find.MapUI != null)
			{
				Find.Selector.Deselect(this);
			}
			if (carrier != null)
			{
				carrier.carryHands.carriedThing = null;
			}
			Find.ReservationManager.UnReserveAllForThing(this);
		}
	}

	public override void DestroyFinalize()
	{
		base.DestroyFinalize();
	}

	public virtual void Killed(DamageInfo Damage)
	{
		Destroy();
		foreach (KeyValuePair<string, int> filthLeaving in def.filthLeavings)
		{
			foreach (IntVec3 item in Gen.SquaresOccupiedBy(this))
			{
				FilthUtility.AddFilthAt(item, filthLeaving.Key, filthLeaving.Value);
			}
		}
	}

	public void ResolveRandomizedIcon()
	{
		if (def.textureFolderPath != string.Empty && randomizedDrawMat == null)
		{
			randomizedDrawMat = def.RandomDrawMat;
		}
	}

	public virtual void ExposeData()
	{
		string value = string.Empty;
		if (def != null)
		{
			value = def.definitionName;
		}
		Scribe.LookField(ref value, "Def");
		def = ThingDefDatabase.ThingDefNamed(value);
		if (def.HasThingIDNumber)
		{
			string value2 = ThingID;
			Scribe.LookField(ref value2, "ID");
			ThingID = value2;
		}
		Scribe.LookField(ref positionInt, "Pos");
		Scribe.LookField(ref rotation, "Rot", IntRot.north);
		TeamType value3 = base.Team;
		Scribe.LookField(ref value3, "Team", TeamType.Neutral);
		canDoUnsafeTeamChange = true;
		base.Team = value3;
		if (def.useStandardHealth)
		{
			Scribe.LookField(ref health, "Health");
		}
		if (def.stackLimit > 1)
		{
			Scribe.LookField(ref stackCount, "StackCount", forceSave: true);
		}
		if (Scribe.mode == LoadSaveMode.PostLoadInit)
		{
			ResolveRandomizedIcon();
		}
	}

	public void SetRawPosition(IntVec3 newPos)
	{
		positionInt = newPos;
	}

	public virtual void Draw()
	{
		DrawAt(DrawPos);
	}

	public virtual void DrawAt(Vector3 drawLoc)
	{
		Graphics.DrawMesh(DrawMesh, drawLoc, rotation.AsQuat, DrawMat, 0);
		if (def.sunShadowMesh != null && !Find.RoofGrid.SquareIsRoofed(drawLoc.ToIntVec3()))
		{
			Vector3 position = drawLoc + def.sunShadowOffset;
			position.y = Altitudes.AltitudeFor(AltitudeLayer.Shadows);
			Graphics.DrawMesh(def.sunShadowMesh, position, Quaternion.identity, MatBases.SunShadowFade, 0);
		}
	}

	public virtual void DrawGUIOverlay()
	{
		GenWorldUI.DrawThingLabelFor(this, "Error", Color.white);
	}

	public virtual IEnumerable<MapMeshPiece> EmitMapMeshPieces()
	{
		Vector3 trueCenter = this.TrueCenter();
		Material drawMat = DrawMat;
		if (drawMat != null && drawMat != MatsSimple.BadMaterial)
		{
			Vector2 planeSize = def.size.ToVector2();
			if (def.overDraw)
			{
				planeSize.x += 2f;
				planeSize.y += 2f;
			}
			yield return new MapMeshPiece_Plane(trueCenter, planeSize, drawMat, rotation.AsAngle);
		}
		if (def.sunShadowMesh != null)
		{
			Vector3 shadowCenter = trueCenter;
			shadowCenter.y = Altitudes.AltitudeFor(AltitudeLayer.Shadows);
			yield return new MapMeshPiece_Mesh(shadowCenter, def.sunShadowMesh, MatBases.SunShadowFade);
		}
	}

	public virtual void DrawSelectedExtras()
	{
	}

	public virtual string GetInspectString()
	{
		return string.Empty;
	}

	public virtual IEnumerable<Command> GetCommandOptions()
	{
		yield break;
	}

	public virtual IEnumerable<FloatMenuChoice> GetFloatMenuChoicesFor(Pawn myPawn)
	{
		yield break;
	}

	public void TakeDamage(DamageInfo d)
	{
		if (!destroyed)
		{
			ApplyDamage(d);
		}
	}

	protected virtual void ApplyDamage(DamageInfo d)
	{
		if (d.type == DamageType.Flame)
		{
			this.TryIgnite(UnityEngine.Random.Range(0.15f, 0.25f));
		}
		if (def.useStandardHealth && d.type != DamageType.Healing && d.type != DamageType.Stun)
		{
			health -= d.Amount;
			if (health <= 0)
			{
				Killed(d);
			}
		}
	}

	public Thing GetAttachment(EntityType AttachType)
	{
		foreach (AttachableThing attach in attachList)
		{
			if (attach.def.eType == AttachType)
			{
				return attach;
			}
		}
		return null;
	}

	public bool HasAttachment(EntityType AttachType)
	{
		return GetAttachment(AttachType) != null;
	}

	public virtual Thing SplitOff(int CountToDrop)
	{
		if (CountToDrop >= stackCount)
		{
			Debug.LogWarning(string.Concat("Splitting off ", this, " but taking the whole thing anyway."));
			DeSpawn();
			return this;
		}
		Thing thing = ThingMaker.MakeThing(def);
		thing.stackCount = CountToDrop;
		stackCount -= CountToDrop;
		return thing;
	}

	public virtual TooltipDef GetTooltip()
	{
		string text = Label;
		if (def.useStandardHealth)
		{
			string text2 = text;
			text = text2 + "\nHealth :  " + health + " / " + def.maxHealth;
		}
		foreach (AttachableThing attach in attachList)
		{
			text += attach.InfoStringAddon;
		}
		return new TooltipDef(text, Label);
	}

	public virtual bool BlocksPawn(Pawn p)
	{
		return def.passability == Traversability.Impassable;
	}

	public override string ToString()
	{
		return ThingID;
	}

	public override int GetHashCode()
	{
		return thingIDNumber * thingIDNumber * thingIDNumber * thingIDNumber * thingIDNumber * thingIDNumber * thingIDNumber;
	}
}
