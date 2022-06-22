using System;
using System.Collections.Generic;
using UnityEngine;

public class ThingDefinition : EntityDefinition
{
	public Type thingClass;

	public TickerType tickerType;

	public bool selectable;

	public bool neverMultiSelect;

	public bool canBeSeenOver = true;

	public float coverPercent;

	public bool useStandardHealth = true;

	public int maxHealth = 1;

	public List<CompSetup> compSetupList = new List<CompSetup>();

	public bool isAutoAttackableWorldObject;

	public LinkFlags linkFlags;

	public bool hasTooltip;

	public int stackLimit = 1;

	public List<ITab> inspectorTabs = new List<ITab>();

	public bool seeThroughFog;

	public string bulletHitSoundFolder = "Ground";

	public bool alwaysHaulable;

	public bool designateHaulable;

	public bool drawGUIOverlay;

	public StoreType storeType;

	public AudioClip dropSound = Res.LoadSound("Interaction/Haul/StandardDrop");

	public string pickupSoundFolder = "Interaction/Haul/StandardPickup";

	public bool isDebris;

	public bool saveCompressible;

	public bool destroyable = true;

	public bool isSaveable = true;

	public Type blueprintClass = typeof(Blueprint);

	public string blueprintTexturePath = string.Empty;

	public LinkDrawer linkDrawer;

	public bool isBarrier;

	public bool blockLight;

	public bool addToMapMesh;

	public bool alwaysDraw;

	public EntityType mineableResource;

	public bool naturalBuilding;

	public bool combatTargetBuilding;

	public int bed_HealTickInterval;

	public float restEffectiveness;

	public bool bed_ShowSleeperBody;

	public Material iconMat_ForPrisoner;

	public bool leaveResources;

	public List<LeavingRecord> leavings = new List<LeavingRecord>();

	public TerrainDefinition leaveTerrain;

	public bool randomizeRotationOnSpawn;

	public bool mineable;

	public List<StoreType> storables = new List<StoreType>();

	public int maxNumStoreSlots = 9999;

	public bool makeFog;

	public bool fillsSquare;

	public bool holdsRoof;

	public bool castEdgeShadows;

	public float staticSunShadowHeight;

	public Mesh sunShadowMesh;

	public Vector3 sunShadowOffset = Vector3.zero;

	public bool transmitsPower;

	public bool plantsDestroyWithMe;

	public string plantToGrowDefName = string.Empty;

	public bool supportsPlants;

	public bool actAsTable;

	public bool startElectricalFires;

	public float basePowerConsumption;

	public AudioClip powerOnSound;

	public AudioClip powerOffSound;

	public int meal_ticksBeforeSpoil;

	public FoodProperties food;

	public float cleaningWorkToReduceThickness = 75f;

	public bool isTerrainSourceFilth;

	public bool canBePickedUp;

	public int basePrice;

	public bool purchasable;

	public AudioClip interactSound;

	public bool isResource;

	public VerbDefinition verbDef;

	public InventoryType invType;

	public bool isGun;

	public EntityDefinition entityDefToBuild;

	public MoteProperties mote;

	public PlantProperties plant;

	public ThingDefinition seed_PlantDefToMake;

	public int mach_CooldownTicks;

	public bool mach_CooldownOnTacticalStart = true;

	public float mach_MinRange;

	public float mach_MaxRange = float.MaxValue;

	public EntityType mach_AmmoType;

	public bool mach_AutoFire;

	public Material mach_CastTargetMat;

	public Color mach_CastTargetNumberColor = Color.white;

	public float projectile_Speed = 1f;

	public bool projectile_ImpactWorld = true;

	public float projectile_ExplosionRadius;

	public int projectile_ExplosionDelay;

	public DamageType projectile_DamageType;

	public int projectile_DamageAmountBase = 1;

	public float projectile_RandomMissRadius;

	public float bomb_ExplosionRadius;

	public DamageType bomb_DamageType = DamageType.Bomb;

	public Material blueprintMat;

	public bool ListOnReadout => category != EntityCategory.Mote;

	public bool EverHaulable => alwaysHaulable || designateHaulable;

	public bool Storeable => storeType != StoreType.Undefined;

	public bool HasThingIDNumber => true;

	public bool Edible => food != null;

	public ThingDefinition ThingDefToBuild => (ThingDefinition)entityDefToBuild;

	public bool IsPlant => plant != null;

	public bool IsBlocker
	{
		get
		{
			if (category == EntityCategory.Building && passability != 0)
			{
				return true;
			}
			if (!canBeSeenOver)
			{
				return true;
			}
			if (coverPercent > 0.001f)
			{
				return true;
			}
			if (blockLight)
			{
				return true;
			}
			return false;
		}
	}

	public CoverValue Coverage
	{
		get
		{
			if (coverPercent < 0.01f)
			{
				return CoverValue.None;
			}
			return CoverValue.Partial;
		}
	}

	public bool ConnectToPower
	{
		get
		{
			if (transmitsPower)
			{
				return false;
			}
			return compSetupList.Contains(CompSetup.PowerBattery) || compSetupList.Contains(CompSetup.PowerTrader);
		}
	}

	public bool IsPowerSource
	{
		get
		{
			if (eType == EntityType.Building_Battery)
			{
				return true;
			}
			return basePowerConsumption < 0f;
		}
	}

	public bool DrawEveryFrame => !addToMapMesh || alwaysDraw;

	public ThingDefinition PlantDefinitionToGrow => ThingDefDatabase.ThingDefNamed(plantToGrowDefName);

	public bool BlockBuilding
	{
		get
		{
			if (IsPlant)
			{
				return false;
			}
			return category == EntityCategory.Pawn || category == EntityCategory.SmallObject;
		}
	}

	public bool BlockPlanting
	{
		get
		{
			if (supportsPlants)
			{
				return false;
			}
			if (IsPlant)
			{
				return true;
			}
			if (category == EntityCategory.Building)
			{
				return true;
			}
			if (IsBlocker)
			{
				return true;
			}
			return false;
		}
	}

	public override void PostLoad()
	{
		base.PostLoad();
		if (blueprintTexturePath != string.Empty)
		{
			blueprintMat = MaterialPool.MatFrom(blueprintTexturePath, MatBases.MetaOverlay);
		}
		if (food != null)
		{
			food.PostLoad();
		}
	}

	public void ErrorCheck()
	{
		if (char.IsNumber(definitionName[definitionName.Length - 1]))
		{
			Debug.LogError("Definition name " + definitionName + " ends with a numeral, which is not allowed.");
		}
		foreach (ThingDefinition allThingDefinition in ThingDefDatabase.AllThingDefinitions)
		{
			if (allThingDefinition != this && allThingDefinition.definitionName == definitionName)
			{
				Debug.LogWarning("There is more than one definition named " + definitionName);
			}
		}
		if (compSetupList.Count > 0 && !typeof(ThingWithComponents).IsAssignableFrom(thingClass))
		{
			Debug.LogError(definitionName + " - ThingDefinition has components but it's not a ThingWithComponents");
			return;
		}
		if (ConnectToPower && !addToMapMesh && eType != EntityType.BuildingFrame)
		{
			Debug.LogWarning(string.Concat(this, " connects to power but does not add to map mesh. Will not create wire meshes."));
		}
		if (!useStandardHealth && base.Flammable && eType != EntityType.Pawn)
		{
			Debug.LogWarning(string.Concat(this, " is flammable but does not use standard health."));
		}
		if (startElectricalFires && !compSetupList.Contains(CompSetup.PowerTrader) && !compSetupList.Contains(CompSetup.PowerBattery))
		{
			Debug.LogWarning(string.Concat(this, " starts electrical fires but has no power component."));
		}
	}
}
