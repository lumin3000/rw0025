using System.Collections.Generic;
using UnityEngine;

public static class ThingDefsHardcoded
{
	private static ThingDefinition NewBaseBuildingDef()
	{
		ThingDefinition thingDefinition = new ThingDefinition();
		thingDefinition.category = EntityCategory.Building;
		thingDefinition.bulletHitSoundFolder = "Metal";
		thingDefinition.selectable = true;
		thingDefinition.addToMapMesh = true;
		return thingDefinition;
	}

	public static IEnumerable<ThingDefinition> Definitions_Buildings()
	{
		ThingDefinition d10 = NewBaseBuildingDef();
		d10.eType = EntityType.Building_CommsConsole;
		d10.label = "Comms console";
		d10.thingClass = typeof(Building_CommsConsole);
		d10.texturePath = "Icons/Building/CommsConsole";
		d10.alwaysDraw = true;
		d10.overDraw = true;
		d10.altitudeLayer = AltitudeLayer.BuildingTall;
		d10.passability = Traversability.Impassable;
		d10.castEdgeShadows = true;
		d10.canBeSeenOver = false;
		d10.maxHealth = 250;
		d10.flammability = 1f;
		d10.desc = "Communicate with nearby ships here for negotiation and trade.";
		d10.size = new IntVec2(3, 2);
		d10.workToBuild = 400f;
		d10.costList.Add(new ResourceCost(EntityType.Metal, 120));
		d10.buildingWantsAir = true;
		d10.interactionSquareOffset = new IntVec3(0, 0, 2);
		d10.hasInteractionSquare = true;
		d10.leavings.Add(new LeavingRecord(EntityType.DebrisSlag, 3));
		d10.filthLeavings.Add("SlagRubble", 2);
		d10.leaveResources = true;
		d10.surfaceNeeded = SurfaceType.Heavy;
		d10.designationCategory = DesignationCategory.Building;
		d10.compSetupList.Add(CompSetup.PowerTrader);
		d10.basePowerConsumption = 200f;
		d10.staticSunShadowHeight = 0.5f;
		d10.constructionEffects = typeof(EffectMaker_ConstructMetal);
		d10.startElectricalFires = true;
		yield return d10;
		d10 = NewBaseBuildingDef();
		d10.eType = EntityType.Building_NutrientDispenser;
		d10.label = "Nutrient paste dispenser";
		d10.thingClass = typeof(Building_NutrientDispenser);
		d10.texturePath = "Icons/Building/NutrientDispenser";
		d10.alwaysDraw = true;
		d10.altitudeLayer = AltitudeLayer.BuildingTall;
		d10.passability = Traversability.Impassable;
		d10.castEdgeShadows = true;
		d10.canBeSeenOver = false;
		d10.maxHealth = 350;
		d10.flammability = 1f;
		d10.desc = "Synthesizes nutrient paste from organic feedstocks.";
		d10.compSetupList.Add(CompSetup.PowerTrader);
		d10.basePowerConsumption = 200f;
		d10.size = new IntVec2(3, 4);
		d10.workToBuild = 600f;
		d10.costList.Add(new ResourceCost(EntityType.Metal, 90));
		d10.buildingWantsAir = true;
		d10.overDraw = true;
		d10.interactionSquareOffset = new IntVec3(0, 0, 3);
		d10.hasInteractionSquare = true;
		d10.leavings.Add(new LeavingRecord(EntityType.DebrisSlag, 6));
		d10.filthLeavings.Add("SlagRubble", 3);
		d10.leaveResources = true;
		d10.surfaceNeeded = SurfaceType.Heavy;
		d10.designationCategory = DesignationCategory.Building;
		d10.staticSunShadowHeight = 0.75f;
		d10.constructionEffects = typeof(EffectMaker_ConstructMetal);
		d10.startElectricalFires = true;
		yield return d10;
		d10 = NewBaseBuildingDef();
		d10.eType = EntityType.Building_Grave;
		d10.label = "Grave";
		d10.thingClass = typeof(Building_Grave);
		d10.texturePath = "Icons/Building/GraveEmpty";
		d10.altitudeLayer = AltitudeLayer.FloorEmplacement;
		d10.overDraw = true;
		d10.alwaysDraw = true;
		d10.useStandardHealth = false;
		d10.desc = "Place the dead in graves to give them a decent final resting place.";
		d10.size = new IntVec2(1, 2);
		d10.workToBuild = 200f;
		d10.costList.Add(new ResourceCost(EntityType.Metal, 0));
		d10.passability = Traversability.Standable;
		d10.storables.Add(StoreType.Corpse);
		d10.maxNumStoreSlots = 1;
		d10.inspectorTabs.Add(new ITab_Building_Storage());
		d10.surfaceNeeded = SurfaceType.Diggable;
		d10.designationCategory = DesignationCategory.Building;
		d10.constructionEffects = typeof(EffectMaker_ConstructDig);
		d10.bulletHitSoundFolder = "Ground";
		d10.constructionEffects = typeof(EffectMaker_ConstructMetal);
		yield return d10;
		d10 = NewBaseBuildingDef();
		d10.eType = EntityType.Building_GibbetCage;
		d10.label = "Gibbet cage";
		d10.thingClass = typeof(Building_GibbetCage);
		d10.texturePath = "Icons/Building/GibbetCage";
		d10.alwaysDraw = true;
		d10.altitudeLayer = AltitudeLayer.BuildingTall;
		d10.overDraw = false;
		d10.useStandardHealth = true;
		d10.maxHealth = 120;
		d10.desc = "Place corpses in gibbet cages for display. These inspire disgust and fear.";
		d10.size = new IntVec2(1, 1);
		d10.workToBuild = 200f;
		d10.costList.Add(new ResourceCost(EntityType.Metal, 15));
		d10.filthLeavings.Add("SlagRubble", 2);
		d10.passability = Traversability.Impassable;
		d10.castEdgeShadows = true;
		d10.storables.Add(StoreType.Corpse);
		d10.maxNumStoreSlots = 1;
		d10.inspectorTabs.Add(new ITab_Building_Storage());
		d10.researchPrerequisite = ResearchType.FearTech1;
		d10.surfaceNeeded = SurfaceType.Light;
		d10.designationCategory = DesignationCategory.Building;
		d10.staticSunShadowHeight = 0.5f;
		d10.constructionEffects = typeof(EffectMaker_ConstructMetal);
		yield return d10;
		d10 = NewBaseBuildingDef();
		d10.eType = EntityType.Building_PowerPlant;
		d10.label = "Solar generator";
		d10.definitionName = "SolarGenerator";
		d10.thingClass = typeof(Building_PowerPlantSolar);
		d10.texturePath = "Icons/Building/SolarCollector";
		d10.alwaysDraw = true;
		d10.altitudeLayer = AltitudeLayer.Waist;
		d10.passability = Traversability.Impassable;
		d10.coverPercent = 0.5f;
		d10.castEdgeShadows = true;
		d10.maxHealth = 300;
		d10.flammability = 1f;
		d10.tickerType = TickerType.Normal;
		d10.desc = "Produces electricity from sunlight. Does not work in the dark.";
		d10.size = new IntVec2(4, 4);
		d10.workToBuild = 500f;
		d10.costList.Add(new ResourceCost(EntityType.Metal, 80));
		d10.overDraw = false;
		d10.compSetupList.Add(CompSetup.PowerTrader);
		d10.leavings.Add(new LeavingRecord(EntityType.DebrisSlag, 8));
		d10.filthLeavings.Add("SlagRubble", 2);
		d10.leaveResources = true;
		d10.surfaceNeeded = SurfaceType.Light;
		d10.designationCategory = DesignationCategory.Building;
		d10.staticSunShadowHeight = 0.2f;
		d10.transmitsPower = true;
		d10.basePowerConsumption = -1f;
		d10.constructionEffects = typeof(EffectMaker_ConstructMetal);
		yield return d10;
		d10 = NewBaseBuildingDef();
		d10.eType = EntityType.Building_PowerPlantGeothermal;
		d10.label = "Geothermal generator";
		d10.definitionName = "GeothermalGenerator";
		d10.thingClass = typeof(Building_PowerPlantSteam);
		d10.texturePath = "Icons/Building/GeothermalPlant";
		d10.alwaysDraw = true;
		d10.altitudeLayer = AltitudeLayer.BuildingTall;
		d10.passability = Traversability.Impassable;
		d10.castEdgeShadows = true;
		d10.canBeSeenOver = false;
		d10.blockLight = true;
		d10.maxHealth = 500;
		d10.flammability = 1f;
		d10.tickerType = TickerType.Normal;
		d10.desc = "Produces electricity from geothermal steam geysers. Must be placed on a geyser.";
		d10.size = new IntVec2(6, 6);
		d10.workToBuild = 1000f;
		d10.costList.Add(new ResourceCost(EntityType.Metal, 250));
		d10.overDraw = false;
		d10.compSetupList.Add(CompSetup.PowerTrader);
		d10.leavings.Add(new LeavingRecord(EntityType.DebrisSlag, 16));
		d10.filthLeavings.Add("SlagRubble", 3);
		d10.leaveResources = true;
		d10.surfaceNeeded = SurfaceType.Heavy;
		d10.designationCategory = DesignationCategory.Building;
		d10.staticSunShadowHeight = 1f;
		d10.transmitsPower = true;
		d10.basePowerConsumption = -1f;
		d10.placementRestrictions.Add(PlacementRestriction.OnSteamGeyser);
		d10.constructionEffects = typeof(EffectMaker_ConstructMetal);
		yield return d10;
		d10 = NewBaseBuildingDef();
		d10.eType = EntityType.Building_Battery;
		d10.label = "Battery";
		d10.thingClass = typeof(Building_Battery);
		d10.texturePath = "Icons/Building/Battery";
		d10.alwaysDraw = true;
		d10.altitudeLayer = AltitudeLayer.BuildingTall;
		d10.passability = Traversability.Impassable;
		d10.castEdgeShadows = true;
		d10.canBeSeenOver = false;
		d10.tickerType = TickerType.Normal;
		d10.maxHealth = 100;
		d10.flammability = 2f;
		d10.desc = "Stores electricity when there is excess power and yields it when there is not. Warning - charged batteries tend to explode when heated.";
		d10.size = new IntVec2(1, 2);
		d10.workToBuild = 200f;
		d10.costList.Add(new ResourceCost(EntityType.Metal, 50));
		d10.overDraw = false;
		d10.compSetupList.Add(CompSetup.PowerBattery);
		d10.leavings.Add(new LeavingRecord(EntityType.DebrisSlag, 1));
		d10.filthLeavings.Add("SlagRubble", 2);
		d10.leaveResources = true;
		d10.surfaceNeeded = SurfaceType.Light;
		d10.designationCategory = DesignationCategory.Building;
		d10.staticSunShadowHeight = 0.5f;
		d10.transmitsPower = true;
		d10.constructionEffects = typeof(EffectMaker_ConstructMetal);
		d10.startElectricalFires = true;
		yield return d10;
		d10 = NewBaseBuildingDef();
		d10.eType = EntityType.Building_AirTank;
		d10.label = "Air tank";
		d10.thingClass = typeof(Building_AirTank);
		d10.texturePath = "Icons/Building/AirTank";
		d10.alwaysDraw = true;
		d10.altitudeLayer = AltitudeLayer.BuildingTall;
		d10.compSetupList.Add(CompSetup.Explosive_49);
		d10.passability = Traversability.Impassable;
		d10.castEdgeShadows = true;
		d10.canBeSeenOver = false;
		d10.maxHealth = 200;
		d10.flammability = 1f;
		d10.desc = "Stores air.";
		d10.size = new IntVec2(4, 2);
		d10.workToBuild = 300f;
		d10.costList.Add(new ResourceCost(EntityType.Metal, 45));
		d10.leavings.Add(new LeavingRecord(EntityType.DebrisSlag, 3));
		d10.filthLeavings.Add("SlagRubble", 2);
		d10.leaveResources = true;
		d10.surfaceNeeded = SurfaceType.Heavy;
		d10.staticSunShadowHeight = 0.75f;
		d10.constructionEffects = typeof(EffectMaker_ConstructMetal);
		yield return d10;
		d10 = NewBaseBuildingDef();
		d10.eType = EntityType.Building_AirMiner;
		d10.label = "Air miner";
		d10.thingClass = typeof(Building_AirMiner);
		d10.texturePath = "Icons/Building/AirMiner";
		d10.alwaysDraw = true;
		d10.altitudeLayer = AltitudeLayer.BuildingTall;
		d10.passability = Traversability.Impassable;
		d10.castEdgeShadows = true;
		d10.compSetupList.Add(CompSetup.PowerTrader);
		d10.basePowerConsumption = 250f;
		d10.canBeSeenOver = false;
		d10.blockLight = true;
		d10.maxHealth = 350;
		d10.flammability = 1f;
		d10.tickerType = TickerType.Normal;
		d10.desc = "Captures gases from the atmosphere and refines them into breathable air. Air miners produce air at a slow, steady rate.";
		d10.size = new IntVec2(6, 4);
		d10.workToBuild = 800f;
		d10.costList.Add(new ResourceCost(EntityType.Metal, 100));
		d10.leavings.Add(new LeavingRecord(EntityType.DebrisSlag, 6));
		d10.filthLeavings.Add("SlagRubble", 3);
		d10.leaveResources = true;
		d10.surfaceNeeded = SurfaceType.Heavy;
		d10.staticSunShadowHeight = 0.75f;
		d10.transmitsPower = true;
		d10.constructionEffects = typeof(EffectMaker_ConstructMetal);
		yield return d10;
		d10 = NewBaseBuildingDef();
		d10.eType = EntityType.Building_AirOutlet;
		d10.label = "Air outlet";
		d10.thingClass = typeof(Building_AirOutlet);
		d10.texturePath = "Icons/Building/AirOutlet";
		d10.alwaysDraw = true;
		d10.altitudeLayer = AltitudeLayer.FloorEmplacement;
		d10.maxHealth = 90;
		d10.flammability = 1f;
		d10.tickerType = TickerType.Normal;
		d10.desc = "Releases air into a room.";
		d10.size = new IntVec2(1, 1);
		d10.workToBuild = 100f;
		d10.costList.Add(new ResourceCost(EntityType.Metal, 15));
		d10.filthLeavings.Add("SlagRubble", 1);
		d10.surfaceNeeded = SurfaceType.Light;
		d10.constructionEffects = typeof(EffectMaker_ConstructMetal);
		yield return d10;
	}

	public static IEnumerable<ThingDefinition> Definitions_BuildingsAreas()
	{
		ThingDefinition d3 = NewBaseBuildingDef();
		d3.eType = EntityType.Area_Stockpile;
		d3.label = "Stockpile area";
		d3.thingClass = typeof(Building);
		d3.texturePath = "Icons/Building/AreaStockpile";
		d3.alwaysDraw = true;
		d3.altitudeLayer = AltitudeLayer.FloorEmplacement;
		d3.canBeSeenOver = true;
		d3.useStandardHealth = false;
		d3.desc = "Colonists drop resources here.";
		d3.size = new IntVec2(6, 6);
		d3.workToBuild = 0f;
		d3.costList.Add(new ResourceCost(EntityType.Metal, 200));
		d3.overDraw = true;
		d3.designationCategory = DesignationCategory.Area;
		d3.staticSunShadowHeight = 0.1f;
		yield return d3;
		d3 = NewBaseBuildingDef();
		d3.eType = EntityType.Area_Dump;
		d3.label = "Dumping area";
		d3.thingClass = typeof(Building_Storage);
		d3.texturePath = "Icons/Building/AreaDump";
		d3.alwaysDraw = true;
		d3.altitudeLayer = AltitudeLayer.FloorEmplacement;
		d3.useStandardHealth = false;
		d3.desc = "Designates an area where colonists will dump unwanted items.";
		d3.size = new IntVec2(4, 4);
		d3.overDraw = true;
		d3.workToBuild = 0f;
		d3.storables.Add(StoreType.Corpse);
		d3.storables.Add(StoreType.Debris);
		d3.storables.Add(StoreType.Meal);
		d3.inspectorTabs.Add(new ITab_Building_Storage());
		d3.designationCategory = DesignationCategory.Area;
		d3.constructionEffects = typeof(EffectMaker_ConstructDig);
		yield return d3;
		d3 = NewBaseBuildingDef();
		d3.eType = EntityType.Area_Growing;
		d3.label = "Growing area";
		d3.thingClass = typeof(Building_PlantGrower);
		d3.texturePath = "Icons/Building/AreaGrowing";
		d3.alwaysDraw = true;
		d3.altitudeLayer = AltitudeLayer.FloorEmplacement;
		d3.passability = Traversability.Standable;
		d3.useStandardHealth = false;
		d3.desc = "Designates an area where colonists will plant crops. Usually grows enough for one or two people.";
		d3.size = new IntVec2(4, 6);
		d3.workToBuild = 0f;
		d3.overDraw = true;
		d3.buildingWantsAir = false;
		d3.plantToGrowDefName = "PlantPotato";
		d3.compSetupList.Add(CompSetup.Forbiddable);
		d3.surfaceNeeded = SurfaceType.GrowSoil;
		d3.designationCategory = DesignationCategory.Area;
		d3.constructionEffects = typeof(EffectMaker_ConstructDig);
		d3.bulletHitSoundFolder = "Ground";
		d3.supportsPlants = true;
		yield return d3;
	}

	public static IEnumerable<ThingDefinition> Definitions_Furniture()
	{
		ThingDefinition d8 = NewBaseBuildingDef();
		d8.eType = EntityType.Building_HydroponicsTable;
		d8.label = "Hydroponics table";
		d8.thingClass = typeof(Building_PlantGrower);
		d8.texturePath = "Icons/Building/HydroponicsTable";
		d8.alwaysDraw = true;
		d8.altitudeLayer = AltitudeLayer.Waist;
		d8.passability = Traversability.Impassable;
		d8.castEdgeShadows = true;
		d8.maxHealth = 180;
		d8.flammability = 1f;
		d8.desc = "For growing food. Hydroponics tables grow a few plants quickly.";
		d8.size = new IntVec2(1, 4);
		d8.workToBuild = 600f;
		d8.costList.Add(new ResourceCost(EntityType.Metal, 25));
		d8.overDraw = true;
		d8.plantToGrowDefName = "PlantPotatoHydro";
		d8.plantsDestroyWithMe = true;
		d8.fertility = 2.75f;
		d8.compSetupList.Add(CompSetup.Forbiddable);
		d8.leavings.Add(new LeavingRecord(EntityType.DebrisSlag, 2));
		d8.filthLeavings.Add("SlagRubble", 1);
		d8.leaveResources = true;
		d8.researchPrerequisite = ResearchType.Hydroponics;
		d8.surfaceNeeded = SurfaceType.Heavy;
		d8.designationCategory = DesignationCategory.Furniture;
		d8.staticSunShadowHeight = 0.2f;
		d8.bulletHitSoundFolder = "Ground";
		d8.supportsPlants = true;
		d8.constructionEffects = typeof(EffectMaker_ConstructMetal);
		yield return d8;
		d8 = NewBaseBuildingDef();
		d8.eType = EntityType.Building_PlantPot;
		d8.label = "Plant pot";
		d8.thingClass = typeof(Building_PlantGrower);
		d8.texturePath = "Icons/Building/PlantPot";
		d8.alwaysDraw = true;
		d8.altitudeLayer = AltitudeLayer.Waist;
		d8.passability = Traversability.Impassable;
		d8.maxHealth = 100;
		d8.flammability = 1f;
		d8.desc = "Growers plant decorative flowers here.";
		d8.size = new IntVec2(1, 1);
		d8.workToBuild = 100f;
		d8.costList.Add(new ResourceCost(EntityType.Metal, 10));
		d8.filthLeavings.Add("SlagRubble", 1);
		d8.overDraw = false;
		d8.plantToGrowDefName = "PlantDaylily";
		d8.fertility = 1f;
		d8.surfaceNeeded = SurfaceType.Light;
		d8.designationCategory = DesignationCategory.Furniture;
		d8.sunShadowMesh = MeshPool.shadow0306;
		d8.plantsDestroyWithMe = true;
		d8.supportsPlants = true;
		d8.constructionEffects = typeof(EffectMaker_ConstructMetal);
		d8.rotatable = false;
		yield return d8;
		d8 = NewBaseBuildingDef();
		d8.eType = EntityType.Building_ResearchBench;
		d8.label = "Research bench";
		d8.thingClass = typeof(Building_ResearchBench);
		d8.texturePath = "Icons/Building/ResearchBench";
		d8.alwaysDraw = true;
		d8.altitudeLayer = AltitudeLayer.Waist;
		d8.passability = Traversability.Impassable;
		d8.castEdgeShadows = true;
		d8.maxHealth = 250;
		d8.flammability = 1f;
		d8.desc = "Researchers work here to discover new things.";
		d8.size = new IntVec2(2, 5);
		d8.workToBuild = 250f;
		d8.costList.Add(new ResourceCost(EntityType.Metal, 30));
		d8.overDraw = true;
		d8.buildingWantsAir = true;
		d8.coverPercent = 0.4f;
		d8.leavings.Add(new LeavingRecord(EntityType.DebrisSlag, 4));
		d8.filthLeavings.Add("SlagRubble", 2);
		d8.leaveResources = true;
		d8.interactionSquareOffset = new IntVec3(2, 0, 0);
		d8.hasInteractionSquare = true;
		d8.surfaceNeeded = SurfaceType.Heavy;
		d8.designationCategory = DesignationCategory.Furniture;
		d8.staticSunShadowHeight = 0.2f;
		d8.constructionEffects = typeof(EffectMaker_ConstructMetal);
		yield return d8;
		d8 = NewBaseBuildingDef();
		d8.eType = EntityType.Building_EquipmentRack;
		d8.label = "Equipment rack";
		d8.thingClass = typeof(Building_EquipmentRack);
		d8.texturePath = "Icons/Building/EquipmentRack";
		d8.alwaysDraw = true;
		d8.altitudeLayer = AltitudeLayer.FloorEmplacement;
		d8.passability = Traversability.Impassable;
		d8.castEdgeShadows = true;
		d8.maxHealth = 100;
		d8.flammability = 1f;
		d8.desc = "Haulers carry equipment here for storage.";
		d8.size = new IntVec2(2, 1);
		d8.workToBuild = 175f;
		d8.costList.Add(new ResourceCost(EntityType.Metal, 30));
		d8.overDraw = true;
		d8.leavings.Add(new LeavingRecord(EntityType.DebrisSlag, 1));
		d8.filthLeavings.Add("SlagRubble", 2);
		d8.storables.Add(StoreType.Equipment);
		d8.surfaceNeeded = SurfaceType.Light;
		d8.designationCategory = DesignationCategory.Furniture;
		d8.staticSunShadowHeight = 0.5f;
		d8.constructionEffects = typeof(EffectMaker_ConstructMetal);
		yield return d8;
		d8 = NewBaseBuildingDef();
		d8.eType = EntityType.Building_DropBeacon;
		d8.label = "Drop beacon";
		d8.thingClass = typeof(Building);
		d8.texturePath = "Icons/Building/DropBeacon";
		d8.alwaysDraw = true;
		d8.altitudeLayer = AltitudeLayer.Waist;
		d8.maxHealth = 75;
		d8.desc = "Orbital traders will drop goods near this.";
		d8.size = new IntVec2(1, 1);
		d8.workToBuild = 40f;
		d8.flammability = 1f;
		d8.costList.Add(new ResourceCost(EntityType.Metal, 15));
		d8.filthLeavings.Add("SlagRubble", 1);
		d8.overDraw = false;
		d8.pathCost = 30;
		d8.restEffectiveness = 0.8f;
		d8.buildingWantsAir = true;
		d8.surfaceNeeded = SurfaceType.Light;
		d8.designationCategory = DesignationCategory.Furniture;
		d8.sunShadowMesh = MeshPool.shadow0306;
		d8.constructionEffects = typeof(EffectMaker_ConstructMetal);
		d8.rotatable = false;
		yield return d8;
		d8 = NewBaseBuildingDef();
		d8.eType = EntityType.Building_Chair;
		d8.label = "Chair";
		d8.thingClass = typeof(Building_Chair);
		d8.texturePath = "Icons/Building/Chair";
		d8.alwaysDraw = true;
		d8.altitudeLayer = AltitudeLayer.Waist;
		d8.maxHealth = 75;
		d8.desc = "People sit here.";
		d8.size = new IntVec2(1, 1);
		d8.workToBuild = 150f;
		d8.flammability = 1f;
		d8.costList.Add(new ResourceCost(EntityType.Metal, 15));
		d8.filthLeavings.Add("SlagRubble", 1);
		d8.overDraw = false;
		d8.pathCost = 30;
		d8.restEffectiveness = 0.8f;
		d8.buildingWantsAir = true;
		d8.surfaceNeeded = SurfaceType.Light;
		d8.designationCategory = DesignationCategory.Furniture;
		d8.sunShadowMesh = MeshPool.shadow0306;
		d8.constructionEffects = typeof(EffectMaker_ConstructMetal);
		yield return d8;
		d8 = NewBaseBuildingDef();
		d8.eType = EntityType.Building_TableShort;
		d8.label = "Table (short)";
		d8.thingClass = typeof(Building);
		d8.texturePath = "Icons/Building/TableShort";
		d8.alwaysDraw = true;
		d8.altitudeLayer = AltitudeLayer.Waist;
		d8.maxHealth = 100;
		d8.flammability = 1f;
		d8.desc = "People eat off tables when chairs are placed facing them.";
		d8.size = new IntVec2(2, 2);
		d8.workToBuild = 125f;
		d8.costList.Add(new ResourceCost(EntityType.Metal, 20));
		d8.overDraw = false;
		d8.passability = Traversability.PassThroughOnly;
		d8.castEdgeShadows = true;
		d8.pathCost = 60;
		d8.coverPercent = 0.4f;
		d8.buildingWantsAir = true;
		d8.leavings.Add(new LeavingRecord(EntityType.DebrisSlag, 1));
		d8.filthLeavings.Add("SlagRubble", 1);
		d8.leaveResources = true;
		d8.surfaceNeeded = SurfaceType.Light;
		d8.designationCategory = DesignationCategory.Furniture;
		d8.staticSunShadowHeight = 0.2f;
		d8.actAsTable = true;
		d8.constructionEffects = typeof(EffectMaker_ConstructMetal);
		yield return d8;
		d8 = NewBaseBuildingDef();
		d8.eType = EntityType.Building_TableLong;
		d8.label = "Table (long)";
		d8.thingClass = typeof(Building);
		d8.texturePath = "Icons/Building/TableLong";
		d8.alwaysDraw = true;
		d8.altitudeLayer = AltitudeLayer.Waist;
		d8.maxHealth = 150;
		d8.flammability = 1f;
		d8.desc = "People eat off tables when chairs are placed facing them.";
		d8.size = new IntVec2(2, 4);
		d8.workToBuild = 250f;
		d8.costList.Add(new ResourceCost(EntityType.Metal, 35));
		d8.overDraw = false;
		d8.passability = Traversability.PassThroughOnly;
		d8.castEdgeShadows = true;
		d8.pathCost = 60;
		d8.coverPercent = 0.4f;
		d8.buildingWantsAir = true;
		d8.leavings.Add(new LeavingRecord(EntityType.DebrisSlag, 2));
		d8.filthLeavings.Add("SlagRubble", 1);
		d8.leaveResources = true;
		d8.surfaceNeeded = SurfaceType.Light;
		d8.designationCategory = DesignationCategory.Furniture;
		d8.staticSunShadowHeight = 0.2f;
		d8.actAsTable = true;
		d8.constructionEffects = typeof(EffectMaker_ConstructMetal);
		yield return d8;
		yield return new ThingDefinition
		{
			eType = EntityType.Building_Lamp,
			label = "Standing lamp",
			definitionName = "StandingLamp",
			thingClass = typeof(Building_Glower),
			category = EntityCategory.Building,
			texturePath = "Icons/Building/LampStanding",
			altitudeLayer = AltitudeLayer.BuildingTall,
			addToMapMesh = true,
			alwaysDraw = true,
			passability = Traversability.PassThroughOnly,
			maxHealth = 50,
			flammability = 1f,
			selectable = true,
			desc = "Standing lamp that lights an area. Consumes 150 W of power",
			size = new IntVec2(1, 1),
			workToBuild = 35f,
			costList = 
			{
				new ResourceCost(EntityType.Metal, 5)
			},
			buildingWantsAir = false,
			bulletHitSoundFolder = "Metal",
			leaveResources = false,
			compSetupList = 
			{
				CompSetup.Glower_Medium,
				CompSetup.PowerTrader
			},
			surfaceNeeded = SurfaceType.Light,
			designationCategory = DesignationCategory.Furniture,
			powerOnSound = Res.LoadSound("Building/PowerOnSmall"),
			powerOffSound = Res.LoadSound("Building/PowerOffSmall"),
			basePowerConsumption = 150f,
			sunShadowMesh = MeshPool.shadow0306,
			sunShadowOffset = new Vector3(0f, 0f, -0.1f),
			constructionEffects = typeof(EffectMaker_ConstructMetal),
			rotatable = false
		};
		yield return new ThingDefinition
		{
			eType = EntityType.Building_Lamp,
			label = "Sun lamp",
			definitionName = "SunLamp",
			thingClass = typeof(Building_Glower),
			category = EntityCategory.Building,
			texturePath = "Icons/Building/LampSun",
			altitudeLayer = AltitudeLayer.BuildingTall,
			addToMapMesh = true,
			alwaysDraw = true,
			passability = Traversability.PassThroughOnly,
			maxHealth = 50,
			flammability = 1f,
			selectable = true,
			desc = "Lights an area brightly enough to grow crops. Consumes 800 W of power.",
			size = new IntVec2(1, 1),
			workToBuild = 100f,
			costList = 
			{
				new ResourceCost(EntityType.Metal, 20)
			},
			buildingWantsAir = false,
			bulletHitSoundFolder = "Metal",
			leaveResources = false,
			compSetupList = 
			{
				CompSetup.Glower_Overlight,
				CompSetup.PowerTrader
			},
			surfaceNeeded = SurfaceType.Light,
			designationCategory = DesignationCategory.Furniture,
			powerOnSound = Res.LoadSound("Building/PowerOnSmall"),
			powerOffSound = Res.LoadSound("Building/PowerOffSmall"),
			basePowerConsumption = 600f,
			sunShadowMesh = MeshPool.shadow0306,
			sunShadowOffset = new Vector3(0f, 0f, -0.1f),
			constructionEffects = typeof(EffectMaker_ConstructMetal),
			rotatable = false,
			startElectricalFires = true
		};
	}

	public static IEnumerable<ThingDefinition> Definitions_BuildingsNatural()
	{
		yield return new ThingDefinition
		{
			eType = EntityType.Rock,
			label = "Rock",
			thingClass = typeof(Mineable),
			linkDrawer = LinkDrawers.cornerFiller,
			category = EntityCategory.Building,
			texturePath = "Icons/Linked/Rock_Atlas",
			linkFlags = (LinkFlags.Rock | LinkFlags.MapEdge),
			altitudeLayer = AltitudeLayer.BuildingTall,
			passability = Traversability.Impassable,
			castEdgeShadows = true,
			canBeSeenOver = false,
			blockLight = true,
			maxHealth = 240,
			selectable = true,
			neverMultiSelect = true,
			desc = "Divides and encloses rooms. Mine to remove.",
			isBarrier = true,
			rotatable = false,
			addToMapMesh = true,
			naturalBuilding = true,
			mineable = true,
			saveCompressible = true,
			makeFog = true,
			leaveTerrain = TerrainDefDatabase.TerrainWithLabel("Rough stone"),
			filthLeavings = { { "RockRubble", 2 } },
			fillsSquare = true,
			holdsRoof = true,
			staticSunShadowHeight = 1f
		};
		yield return new ThingDefinition
		{
			eType = EntityType.Mineral,
			label = "Mineral",
			thingClass = typeof(Mineable),
			linkDrawer = LinkDrawers.cornerFiller,
			category = EntityCategory.Building,
			texturePath = "Icons/Linked/Mineral_Atlas",
			linkFlags = (LinkFlags.Minerals | LinkFlags.MapEdge),
			altitudeLayer = AltitudeLayer.BuildingTall,
			passability = Traversability.Impassable,
			castEdgeShadows = true,
			canBeSeenOver = false,
			blockLight = true,
			maxHealth = 1000,
			selectable = true,
			neverMultiSelect = true,
			desc = "Mine this for metal.",
			isBarrier = true,
			rotatable = false,
			hasTooltip = false,
			addToMapMesh = true,
			mineableResource = EntityType.Metal,
			naturalBuilding = true,
			mineable = true,
			saveCompressible = true,
			makeFog = true,
			leaveTerrain = TerrainDefDatabase.TerrainWithLabel("Rough stone"),
			filthLeavings = { { "RockRubble", 2 } },
			fillsSquare = true,
			holdsRoof = true,
			staticSunShadowHeight = 1f
		};
		yield return new ThingDefinition
		{
			eType = EntityType.SteamGeyser,
			label = "Steam geyser",
			definitionName = "SteamGeyser",
			thingClass = typeof(Building_SteamGeyser),
			category = EntityCategory.Building,
			texturePath = "Icons/Building/SteamGeyser",
			baseMaterial = MatBases.Transparent,
			altitudeLayer = AltitudeLayer.Floor,
			destroyable = false,
			useStandardHealth = false,
			tickerType = TickerType.Normal,
			selectable = true,
			desc = "Natural steam source.",
			size = new IntVec2(2, 2),
			overDraw = true,
			beauty = BeautyCategory.Gorgeous,
			neverBuildFloorsOver = true
		};
	}

	public static IEnumerable<ThingDefinition> Definitions_BuildingsSecurity()
	{
		ThingDefinition d2 = NewBaseBuildingDef();
		d2.eType = EntityType.Building_TurretGun;
		d2.label = "Auto-turret";
		d2.thingClass = typeof(Building_TurretGun);
		d2.texturePath = "Icons/Building/TurretGun";
		d2.alwaysDraw = true;
		d2.altitudeLayer = AltitudeLayer.Waist;
		d2.maxHealth = 140;
		d2.tickerType = TickerType.Normal;
		d2.compSetupList.Add(CompSetup.Explosive_39);
		d2.compSetupList.Add(CompSetup.Forbiddable);
		d2.compSetupList.Add(CompSetup.PowerTrader);
		d2.basePowerConsumption = 350f;
		d2.desc = "Automatically fires a machine gun at nearby enemies. Will explode when badly damaged.";
		d2.flammability = 1f;
		d2.size = new IntVec2(2, 2);
		d2.passability = Traversability.Impassable;
		d2.castEdgeShadows = true;
		d2.canBeSeenOver = true;
		d2.costList.Add(new ResourceCost(EntityType.Metal, 160));
		d2.workToBuild = 1000f;
		d2.overDraw = true;
		d2.combatTargetBuilding = true;
		d2.beauty = BeautyCategory.Ugly;
		d2.placingDisplayMethod = delegate
		{
			ThingDefinition thingDefinition = ThingDefDatabase.ThingDefNamed("Gun_L-15 LMG");
			GenRender.RenderRadiusRing(Gen.MouseWorldSquare(), thingDefinition.verbDef.range);
		};
		d2.leavings.Add(new LeavingRecord(EntityType.DebrisSlag, 2));
		d2.filthLeavings.Add("SlagRubble", 1);
		d2.leaveResources = true;
		d2.surfaceNeeded = SurfaceType.Light;
		d2.designationCategory = DesignationCategory.Security;
		d2.constructionEffects = typeof(EffectMaker_ConstructMetal);
		yield return d2;
		d2 = NewBaseBuildingDef();
		d2.eType = EntityType.Building_BlastingCharge;
		d2.label = "Blasting charge";
		d2.thingClass = typeof(Building_BlastingCharge);
		d2.texturePath = "Icons/Building/BlastingCharge";
		d2.alwaysDraw = true;
		d2.altitudeLayer = AltitudeLayer.Waist;
		d2.passability = Traversability.PassThroughOnly;
		d2.useStandardHealth = true;
		d2.maxHealth = 50;
		d2.flammability = 1f;
		d2.selectable = true;
		d2.tickerType = TickerType.Normal;
		d2.desc = "Detonates on command.";
		d2.compSetupList.Add(CompSetup.Explosive_39);
		d2.size = new IntVec2(1, 1);
		d2.workToBuild = 30f;
		d2.costList.Add(new ResourceCost(EntityType.Metal, 25));
		ThingDefinition blastChargeDef = d2;
		d2.placingDisplayMethod = delegate
		{
			CompExplosive compExplosive = (CompExplosive)ThingCompMaker.MakeThingComp(blastChargeDef.compSetupList[0]);
			Explosion.DisplayPredictedExplosiveRadius(Gen.MouseWorldSquare(), compExplosive.explosiveRadius);
		};
		d2.researchPrerequisite = ResearchType.BlastingCharges;
		d2.surfaceNeeded = SurfaceType.Light;
		d2.designationCategory = DesignationCategory.Security;
		d2.constructionEffects = typeof(EffectMaker_ConstructMetal);
		yield return d2;
	}

	public static IEnumerable<ThingDefinition> Definitions_BuildingsStructure()
	{
		yield return new ThingDefinition
		{
			eType = EntityType.Wall,
			label = "Wall",
			thingClass = typeof(Building),
			linkDrawer = LinkDrawers.cornerFiller,
			category = EntityCategory.Building,
			texturePath = "Icons/Linked/Wall_Atlas",
			blueprintTexturePath = "Icons/Linked/Wall_Blueprint_Atlas",
			menuIconPath = "Icons/Linked/Wall_MenuIcon",
			linkFlags = (LinkFlags.Wall | LinkFlags.Rock | LinkFlags.Minerals),
			addToMapMesh = true,
			alwaysDraw = true,
			altitudeLayer = AltitudeLayer.BuildingTall,
			passability = Traversability.Impassable,
			castEdgeShadows = true,
			canBeSeenOver = false,
			blockLight = true,
			maxHealth = 280,
			flammability = 1f,
			desc = "Divides and encloses rooms.",
			placingDraggableDimensions = 1,
			workToBuild = 75f,
			isBarrier = true,
			costList = 
			{
				new ResourceCost(EntityType.Metal, 3)
			},
			tickerType = TickerType.Normal,
			rotatable = false,
			selectable = true,
			neverMultiSelect = true,
			bulletHitSoundFolder = "Metal",
			leavings = 
			{
				new LeavingRecord(EntityType.DebrisSlag, 1)
			},
			filthLeavings = { { "SlagRubble", 2 } },
			surfaceNeeded = SurfaceType.Heavy,
			fillsSquare = true,
			holdsRoof = true,
			designationCategory = DesignationCategory.Structure,
			staticSunShadowHeight = 1f,
			transmitsPower = true,
			constructionEffects = typeof(EffectMaker_ConstructMetal)
		};
		yield return new ThingDefinition
		{
			eType = EntityType.Door,
			label = "Door",
			thingClass = typeof(Building_Door),
			blueprintClass = typeof(Blueprint_Door),
			category = EntityCategory.Building,
			drawMat = null,
			alwaysDraw = true,
			blueprintTexturePath = "Icons/Building/Door_Blueprint",
			menuIconPath = "Icons/Building/Door_MenuIcon",
			altitudeLayer = AltitudeLayer.DoorMoveable,
			canBeSeenOver = false,
			blockLight = true,
			useStandardHealth = true,
			maxHealth = 240,
			flammability = 1f,
			selectable = true,
			tickerType = TickerType.Normal,
			desc = "Seals rooms apart.",
			size = new IntVec2(1, 1),
			workToBuild = 150f,
			isBarrier = true,
			costList = 
			{
				new ResourceCost(EntityType.Metal, 25)
			},
			placementRestrictions = { PlacementRestriction.NotAdjacentToDoor },
			rotatable = false,
			bulletHitSoundFolder = "Metal",
			surfaceNeeded = SurfaceType.Heavy,
			designationCategory = DesignationCategory.Structure,
			compSetupList = { CompSetup.PowerTrader },
			basePowerConsumption = 50f,
			holdsRoof = true,
			staticSunShadowHeight = 1f,
			transmitsPower = true,
			constructionEffects = typeof(EffectMaker_ConstructMetal)
		};
		ThingDefinition d = new ThingDefinition();
		d.eType = EntityType.Building_PowerConduit;
		d.label = "Power conduit";
		d.thingClass = typeof(Building);
		d.linkDrawer = LinkDrawers.transmitter;
		d.linkFlags = LinkFlags.PowerConduit;
		d.category = EntityCategory.Building;
		d.texturePath = "Icons/Linked/PowerConduit_Atlas";
		d.blueprintTexturePath = "Icons/Linked/PowerConduit_Blueprint_Atlas";
		d.menuIconPath = "Icons/Linked/PowerConduit_MenuIcon";
		d.addToMapMesh = true;
		d.alwaysDraw = true;
		d.flammability = 1f;
		d.linkFlags = LinkFlags.PowerConduit;
		d.altitudeLayer = AltitudeLayer.FloorEmplacement;
		d.passability = Traversability.Standable;
		d.maxHealth = 80;
		d.desc = "Transmits power.";
		d.placingDraggableDimensions = 1;
		d.workToBuild = 10f;
		d.costList.Add(new ResourceCost(EntityType.Metal, 1));
		d.rotatable = false;
		d.selectable = true;
		d.neverMultiSelect = true;
		d.bulletHitSoundFolder = "Metal";
		d.surfaceNeeded = SurfaceType.Light;
		d.designationCategory = DesignationCategory.Structure;
		d.transmitsPower = true;
		d.constructionEffects = typeof(EffectMaker_ConstructDig);
		yield return d;
		yield return new ThingDefinition
		{
			eType = EntityType.Sandbags,
			label = "Sandbags",
			thingClass = typeof(Building),
			linkDrawer = LinkDrawers.basic,
			category = EntityCategory.Building,
			texturePath = "Icons/Linked/Sandbags_Atlas",
			blueprintTexturePath = "Icons/Linked/Sandbags_Blueprint_Atlas",
			menuIconPath = "Icons/Linked/Sandbags_MenuIcon",
			addToMapMesh = true,
			alwaysDraw = true,
			linkFlags = LinkFlags.Sandbags,
			altitudeLayer = AltitudeLayer.Waist,
			pathCost = 60,
			passability = Traversability.PassThroughOnly,
			castEdgeShadows = true,
			coverPercent = 0.6f,
			maxHealth = 300,
			desc = "Cover from gunfire.",
			placingDraggableDimensions = 1,
			workToBuild = 50f,
			costList = 
			{
				new ResourceCost(EntityType.Metal, 3)
			},
			filthLeavings = { { "SandbagRubble", 2 } },
			rotatable = false,
			selectable = true,
			neverMultiSelect = true,
			beauty = BeautyCategory.UglyTiny,
			surfaceNeeded = SurfaceType.Light,
			designationCategory = DesignationCategory.Structure,
			staticSunShadowHeight = 0.2f,
			constructionEffects = typeof(EffectMaker_ConstructDig),
			repairEffects = typeof(EffectMaker_ConstructDig)
		};
	}

	private static ThingDefinition NewBaseEquipmentDefinition()
	{
		ThingDefinition thingDefinition = new ThingDefinition();
		thingDefinition.eType = EntityType.Equipment;
		thingDefinition.invType = InventoryType.Secondary;
		thingDefinition.label = "Equipment lacks label";
		thingDefinition.thingClass = typeof(Equipment);
		thingDefinition.category = EntityCategory.SmallObject;
		thingDefinition.useStandardHealth = true;
		thingDefinition.selectable = true;
		thingDefinition.maxHealth = 100;
		thingDefinition.altitudeLayer = AltitudeLayer.OverWaist;
		thingDefinition.desc = "Equipment lacks desc.";
		thingDefinition.compSetupList.Add(CompSetup.Forbiddable);
		thingDefinition.alwaysHaulable = true;
		thingDefinition.storeType = StoreType.Equipment;
		thingDefinition.tickerType = TickerType.Never;
		return thingDefinition;
	}

	public static IEnumerable<ThingDefinition> Definitions_SpecialWeapons()
	{
		ThingDefinition p2 = NewBaseProjectileDefinition();
		p2.eType = EntityType.Proj_GrenadeFrag;
		p2.label = "Grenade";
		p2.thingClass = typeof(Projectile_Explosive);
		p2.texturePath = "Icons/Projectile/Grenade";
		p2.projectile_Speed = 12f;
		p2.projectile_ImpactWorld = true;
		p2.projectile_ExplosionRadius = 1.9f;
		p2.projectile_DamageType = DamageType.Bomb;
		p2.projectile_ExplosionDelay = 100;
		p2.projectile_RandomMissRadius = 1f;
		yield return p2;
		ThingDefinition d2 = NewBaseEquipmentDefinition();
		d2.definitionName = "Weapon_GrenadeFrag";
		d2.invType = InventoryType.Primary;
		d2.label = "Frag grenades";
		d2.texturePath = "Icons/SmallObject/Equipment/Grenades";
		d2.interactSound = Res.LoadSound("Verb/GrenadePin");
		d2.basePrice = 1000;
		d2.verbDef = new VerbDefinition();
		d2.verbDef.id = VerbID.Nonnative;
		d2.verbDef.verbType = typeof(Verb_LaunchProjectile);
		d2.verbDef.label = "Frag grenades";
		d2.verbDef.description = "Thrown explosive.";
		d2.verbDef.range = 12.9f;
		d2.verbDef.warmupTicks = 108;
		d2.verbDef.cooldownTicks = 120;
		d2.verbDef.noiseRadius = 4f;
		d2.verbDef.isBuildingDestroyer = true;
		d2.verbDef.soundCast = Res.LoadSound("Verb/GrenadePin");
		d2.verbDef.targetParams.canTargetLocations = true;
		d2.verbDef.targetParams.targetTeams.Add(TeamType.Raider);
		d2.verbDef.targetParams.targetTeams.Add(TeamType.Psychotic);
		d2.verbDef.targetParams.canTargetPawns = true;
		d2.verbDef.targetParams.canTargetBuildings = true;
		d2.verbDef.targetParams.worldObjectTargetsMustBeAutoAttackable = true;
		d2.verbDef.hasStandardCommand = true;
		d2.verbDef.projDef = p2;
		yield return d2;
		p2 = NewBaseProjectileDefinition();
		p2.eType = EntityType.Proj_GrenadeMolotov;
		p2.label = "Molotov cocktail";
		p2.thingClass = typeof(Projectile_ExplosiveMolotov);
		p2.texturePath = "Icons/Projectile/Molotov";
		p2.projectile_Speed = 12f;
		p2.projectile_ImpactWorld = true;
		p2.projectile_ExplosionRadius = 1.1f;
		p2.projectile_DamageType = DamageType.Flame;
		p2.projectile_RandomMissRadius = 2.9f;
		yield return p2;
		d2 = NewBaseEquipmentDefinition();
		d2.definitionName = "Weapon_GrenadeMolotov";
		d2.invType = InventoryType.Primary;
		d2.label = "Molotov cocktails";
		d2.texturePath = "Icons/SmallObject/Equipment/Molotov";
		d2.interactSound = Res.LoadSound("Verb/GrenadePin");
		d2.basePrice = 800;
		d2.verbDef = new VerbDefinition();
		d2.verbDef.id = VerbID.Nonnative;
		d2.verbDef.verbType = typeof(Verb_LaunchProjectile);
		d2.verbDef.label = "Molotov cocktails";
		d2.verbDef.description = "Thrown explosive.";
		d2.verbDef.range = 12.9f;
		d2.verbDef.warmupTicks = 108;
		d2.verbDef.cooldownTicks = 120;
		d2.verbDef.noiseRadius = 4f;
		d2.verbDef.isBuildingDestroyer = true;
		d2.verbDef.soundCast = Res.LoadSound("Verb/GrenadePin");
		d2.verbDef.targetParams.canTargetLocations = true;
		d2.verbDef.targetParams.targetTeams.Add(TeamType.Raider);
		d2.verbDef.targetParams.targetTeams.Add(TeamType.Psychotic);
		d2.verbDef.targetParams.canTargetPawns = true;
		d2.verbDef.targetParams.canTargetBuildings = true;
		d2.verbDef.targetParams.worldObjectTargetsMustBeAutoAttackable = true;
		d2.verbDef.hasStandardCommand = true;
		d2.verbDef.projDef = p2;
		yield return d2;
	}

	private static ThingDefinition NewBaseFilthDef()
	{
		ThingDefinition thingDefinition = new ThingDefinition();
		thingDefinition.label = "Unspecified filth";
		thingDefinition.thingClass = typeof(Filth);
		thingDefinition.category = EntityCategory.Filth;
		thingDefinition.altitudeLayer = AltitudeLayer.Filth;
		thingDefinition.useStandardHealth = false;
		thingDefinition.addToMapMesh = true;
		thingDefinition.baseMaterial = MatBases.Transparent;
		return thingDefinition;
	}

	public static IEnumerable<ThingDefinition> Definitions_Filth()
	{
		ThingDefinition d6 = NewBaseFilthDef();
		d6.label = "Blood";
		d6.textureFolderPath = "Icons/Filth/Blood";
		d6.cleaningWorkToReduceThickness = 150f;
		d6.canBePickedUp = true;
		yield return d6;
		d6 = NewBaseFilthDef();
		d6.definitionName = "FilthDirt";
		d6.label = "Dirt";
		d6.textureFolderPath = "Icons/Filth/Dirt";
		d6.beauty = BeautyCategory.UglyTiny;
		d6.cleaningWorkToReduceThickness = 75f;
		d6.isTerrainSourceFilth = true;
		d6.canBePickedUp = true;
		yield return d6;
		d6 = NewBaseFilthDef();
		d6.definitionName = "FilthSand";
		d6.label = "Sand";
		d6.textureFolderPath = "Icons/Filth/Sand";
		d6.beauty = BeautyCategory.UglyTiny;
		d6.cleaningWorkToReduceThickness = 75f;
		d6.isTerrainSourceFilth = true;
		d6.canBePickedUp = true;
		yield return d6;
		d6 = NewBaseFilthDef();
		d6.definitionName = "RockRubble";
		d6.label = "Rock rubble";
		d6.textureFolderPath = "Icons/Filth/RubbleRock";
		d6.beauty = BeautyCategory.Ugly;
		d6.cleaningWorkToReduceThickness = 75f;
		yield return d6;
		d6 = NewBaseFilthDef();
		d6.definitionName = "SlagRubble";
		d6.label = "Slag rubble";
		d6.textureFolderPath = "Icons/Filth/RubbleSlag";
		d6.beauty = BeautyCategory.Ugly;
		d6.cleaningWorkToReduceThickness = 75f;
		yield return d6;
		d6 = NewBaseFilthDef();
		d6.label = "Scattered sandbags";
		d6.definitionName = "SandbagRubble";
		d6.textureFolderPath = "Icons/Filth/RubbleSandbags";
		d6.beauty = BeautyCategory.Ugly;
		d6.cleaningWorkToReduceThickness = 75f;
		yield return d6;
	}

	private static ThingDefinition NewBaseBulletDefinition()
	{
		ThingDefinition thingDefinition = NewBaseProjectileDefinition();
		thingDefinition.thingClass = typeof(Bullet);
		thingDefinition.label = "Unspecified bullet";
		thingDefinition.projectile_ImpactWorld = true;
		thingDefinition.projectile_DamageType = DamageType.Bullet;
		thingDefinition.baseMaterial = MatBases.MotePostLight;
		return thingDefinition;
	}

	private static ThingDefinition NewBaseGunDefinition()
	{
		ThingDefinition thingDefinition = NewBaseEquipmentDefinition();
		thingDefinition.label = "Gun";
		thingDefinition.desc = "Shoots bullets.";
		thingDefinition.invType = InventoryType.Primary;
		thingDefinition.isGun = true;
		return thingDefinition;
	}

	private static VerbDefinition NewBaseGunshotVerbDefinition()
	{
		VerbDefinition verbDefinition = new VerbDefinition();
		verbDefinition.id = VerbID.Nonnative;
		verbDefinition.verbType = typeof(Verb_Shoot);
		verbDefinition.cooldownTicks = 30;
		verbDefinition.label = "VerbGun";
		verbDefinition.description = "Verb fires bullets.";
		verbDefinition.hasStandardCommand = true;
		verbDefinition.targetParams.targetTeams = TargetingParameters.AllTeams;
		verbDefinition.targetParams.canTargetPawns = true;
		verbDefinition.targetParams.canTargetBuildings = true;
		verbDefinition.targetParams.worldObjectTargetsMustBeAutoAttackable = true;
		verbDefinition.canMiss = true;
		return verbDefinition;
	}

	public static IEnumerable<ThingDefinition> Definitions_Guns()
	{
		ThingDefinition b10 = NewBaseBulletDefinition();
		b10.definitionName = "Bullet_Pistol";
		b10.label = "Pistol bullet";
		b10.projectile_DamageAmountBase = 10;
		b10.texturePath = "Icons/Projectile/Bullet_Small";
		b10.projectile_Speed = 55f;
		yield return b10;
		ThingDefinition g10 = NewBaseGunDefinition();
		g10.definitionName = "Gun_Pistol";
		g10.label = "Pistol";
		g10.desc = "Ancient pattern automatic pistol. Weak and short range, but quick.";
		g10.texturePath = "Icons/SmallObject/Equipment/Pistol";
		g10.interactSound = Res.LoadSound("Interface/WeaponHandling/HandleWeapon_SmallA");
		g10.purchasable = true;
		g10.basePrice = 250;
		g10.verbDef = NewBaseGunshotVerbDefinition();
		g10.verbDef.projDef = b10;
		g10.verbDef.accuracy = 4f;
		g10.verbDef.warmupTicks = 68;
		g10.verbDef.range = 24f;
		g10.verbDef.soundCast = Res.LoadSound("Guns/GunshotB");
		yield return g10;
		b10 = NewBaseBulletDefinition();
		b10.definitionName = "Bullet_Pump Shotgun";
		b10.label = "Shotgun blast";
		b10.projectile_DamageAmountBase = 20;
		b10.texturePath = "Icons/Projectile/Bullet_Shotgun";
		b10.projectile_Speed = 55f;
		yield return b10;
		g10 = NewBaseGunDefinition();
		g10.definitionName = "Gun_Pump Shotgun";
		g10.label = "Pump Shotgun";
		g10.desc = "Ancient design. Deadly, but short range.";
		g10.texturePath = "Icons/SmallObject/Equipment/Shotgun";
		g10.interactSound = Res.LoadSound("Interface/WeaponHandling/HandleWeapon_BigALow");
		g10.purchasable = true;
		g10.basePrice = 700;
		g10.verbDef = NewBaseGunshotVerbDefinition();
		g10.verbDef.projDef = b10;
		g10.verbDef.warmupTicks = 108;
		g10.verbDef.range = 16f;
		g10.verbDef.accuracy = 7f;
		g10.verbDef.soundCast = Res.LoadSound("Guns/GunshotE");
		yield return g10;
		b10 = NewBaseBulletDefinition();
		b10.definitionName = "Bullet_Lee-Enfield";
		b10.label = "Rifle bullet";
		b10.projectile_DamageAmountBase = 18;
		b10.projectile_Speed = 70f;
		b10.texturePath = "Icons/Projectile/Bullet_big";
		yield return b10;
		g10 = NewBaseGunDefinition();
		g10.definitionName = "Gun_Lee-Enfield";
		g10.label = "Lee-Enfield";
		g10.desc = "Ancient bolt-action rifle. Probably pulled from a basement somewhere. Good range, Good power, low rate of fire.";
		g10.texturePath = "Icons/SmallObject/Equipment/LeeEnfield";
		g10.interactSound = Res.LoadSound("Interface/WeaponHandling/HandleWeaponA");
		g10.purchasable = true;
		g10.basePrice = 750;
		g10.verbDef = NewBaseGunshotVerbDefinition();
		g10.verbDef.projDef = b10;
		g10.verbDef.warmupTicks = 182;
		g10.verbDef.range = 37f;
		g10.verbDef.accuracy = 9f;
		g10.verbDef.soundCast = Res.LoadSound("Guns/GunshotC");
		yield return g10;
		b10 = NewBaseBulletDefinition();
		b10.definitionName = "Bullet_M-16Rifle";
		b10.label = "M-16 bullet";
		b10.projectile_DamageAmountBase = 7;
		b10.projectile_Speed = 70f;
		b10.texturePath = "Icons/Projectile/Bullet_Small";
		yield return b10;
		g10 = NewBaseGunDefinition();
		g10.definitionName = "Gun_M-16Rifle";
		g10.label = "M-16";
		g10.desc = "Ancient pattern military weapon. Three-round burst. Good range, low power, high rate of fire.";
		g10.texturePath = "Icons/SmallObject/Equipment/M-16";
		g10.interactSound = Res.LoadSound("Interface/WeaponHandling/HandleWeaponB");
		g10.purchasable = true;
		g10.basePrice = 1300;
		g10.verbDef = NewBaseGunshotVerbDefinition();
		g10.verbDef.projDef = b10;
		g10.verbDef.warmupTicks = 108;
		g10.verbDef.range = 32f;
		g10.verbDef.accuracy = 7f;
		g10.verbDef.burstShotCount = 3;
		g10.verbDef.ticksBetweenBurstShots = 7;
		g10.verbDef.soundCast = Res.LoadSound("Guns/GunshotD");
		yield return g10;
		b10 = NewBaseBulletDefinition();
		b10.definitionName = "Bullet_M-24Rifle";
		b10.label = "M-24 bullet";
		b10.projectile_DamageAmountBase = 45;
		b10.projectile_Speed = 100f;
		b10.texturePath = "Icons/Projectile/Bullet_Big";
		yield return b10;
		g10 = NewBaseGunDefinition();
		g10.definitionName = "Gun_M-24Rifle";
		g10.label = "M-24";
		g10.desc = "Ancient pattern military sniper rifle. Bolt action. Long range, great accuracy and power.";
		g10.texturePath = "Icons/SmallObject/Equipment/M-24";
		g10.interactSound = Res.LoadSound("Interface/WeaponHandling/HandleWeaponA");
		g10.purchasable = true;
		g10.basePrice = 1500;
		g10.verbDef = NewBaseGunshotVerbDefinition();
		g10.verbDef.projDef = b10;
		g10.verbDef.warmupTicks = 182;
		g10.verbDef.range = 45f;
		g10.verbDef.accuracy = 9f;
		g10.verbDef.soundCast = Res.LoadSound("Guns/GunshotC");
		yield return g10;
		b10 = NewBaseBulletDefinition();
		b10.definitionName = "Bullet_Uzi";
		b10.label = "Uzi bullet";
		b10.projectile_DamageAmountBase = 5;
		b10.projectile_Speed = 55f;
		b10.texturePath = "Icons/Projectile/Bullet_Small";
		yield return b10;
		g10 = NewBaseGunDefinition();
		g10.definitionName = "Gun_Uzi";
		g10.label = "Uzi";
		g10.desc = "Ancient pattern submachine gun. Short range, low power, high rate of fire.";
		g10.texturePath = "Icons/SmallObject/Equipment/Uzi";
		g10.interactSound = Res.LoadSound("Interface/WeaponHandling/HandleWeaponA");
		g10.purchasable = true;
		g10.basePrice = 800;
		g10.verbDef = NewBaseGunshotVerbDefinition();
		g10.verbDef.projDef = b10;
		g10.verbDef.warmupTicks = 108;
		g10.verbDef.range = 24f;
		g10.verbDef.accuracy = 3f;
		g10.verbDef.burstShotCount = 4;
		g10.verbDef.ticksBetweenBurstShots = 7;
		g10.verbDef.soundCast = Res.LoadSound("Guns/GunshotD");
		yield return g10;
		b10 = NewBaseBulletDefinition();
		b10.definitionName = "Bullet_Minigun";
		b10.label = "Minigun bullet";
		b10.projectile_DamageAmountBase = 5;
		b10.projectile_Speed = 70f;
		b10.texturePath = "Icons/Projectile/Bullet_Small";
		yield return b10;
		g10 = NewBaseGunDefinition();
		g10.definitionName = "Gun_Minigun";
		g10.label = "Minigun";
		g10.desc = "Not intended to be held and used by humans.";
		g10.texturePath = "Icons/SmallObject/Equipment/Default";
		g10.interactSound = Res.LoadSound("Interface/WeaponHandling/HandleWeaponA");
		g10.verbDef = NewBaseGunshotVerbDefinition();
		g10.verbDef.projDef = b10;
		g10.verbDef.warmupTicks = 108;
		g10.verbDef.range = 30f;
		g10.verbDef.accuracy = 2f;
		g10.verbDef.burstShotCount = 15;
		g10.verbDef.ticksBetweenBurstShots = 5;
		g10.verbDef.soundCast = Res.LoadSound("Guns/GunShotMinigun");
		yield return g10;
		b10 = NewBaseBulletDefinition();
		b10.definitionName = "Bullet_L-15 LMG";
		b10.label = "L-15 bullet";
		b10.projectile_DamageAmountBase = 7;
		b10.projectile_Speed = 70f;
		b10.texturePath = "Icons/Projectile/Bullet_Small";
		yield return b10;
		g10 = NewBaseGunDefinition();
		g10.definitionName = "Gun_L-15 LMG";
		g10.label = "L-15 LMG";
		g10.desc = "Light machine gun.";
		g10.texturePath = "Icons/SmallObject/Equipment/Default";
		g10.interactSound = Res.LoadSound("Interface/WeaponHandling/HandleWeaponB");
		g10.verbDef = NewBaseGunshotVerbDefinition();
		g10.verbDef.projDef = b10;
		g10.verbDef.warmupTicks = 108;
		g10.verbDef.range = 25.9f;
		g10.verbDef.accuracy = 3f;
		g10.verbDef.ticksBetweenBurstShots = 8;
		g10.verbDef.burstShotCount = 3;
		g10.verbDef.soundCast = Res.LoadSound("Guns/GunShotMinigun");
		yield return g10;
		b10 = NewBaseBulletDefinition();
		b10.definitionName = "Bullet_T-9 Incendiary Launcher";
		b10.label = "T-9 Incendiary bolt";
		b10.projectile_DamageAmountBase = 4;
		b10.projectile_Speed = 40f;
		b10.texturePath = "Icons/Projectile/Charge_Small";
		b10.thingClass = typeof(BulletIncendiary);
		yield return b10;
		g10 = NewBaseGunDefinition();
		g10.definitionName = "Gun_T-9 Incendiary Launcher";
		g10.label = "T-9 Incendiary Launcher";
		g10.desc = "Incendiary bolt launcher. Starts fires.";
		g10.texturePath = "Icons/SmallObject/Equipment/T-9";
		g10.interactSound = Res.LoadSound("Interface/WeaponHandling/HandleWeaponB");
		g10.purchasable = true;
		g10.basePrice = 1800;
		g10.verbDef = NewBaseGunshotVerbDefinition();
		g10.verbDef.projDef = b10;
		g10.verbDef.warmupTicks = 182;
		g10.verbDef.range = 24f;
		g10.verbDef.accuracy = 1f;
		g10.verbDef.burstShotCount = 1;
		g10.verbDef.soundCast = Res.LoadSound("Guns/GunshotE");
		yield return g10;
		b10 = NewBaseBulletDefinition();
		b10.definitionName = "Bullet_R-4 charge rifle";
		b10.label = "Charge shot";
		b10.projectile_DamageAmountBase = 14;
		b10.projectile_Speed = 50f;
		b10.texturePath = "Icons/Projectile/Charge_Small";
		yield return b10;
		g10 = NewBaseGunDefinition();
		g10.definitionName = "Gun_R-4 charge rifle";
		g10.label = "R-4 charge rifle";
		g10.desc = "Charged-shot energy/projectile rifle.";
		g10.texturePath = "Icons/SmallObject/Equipment/R-4";
		g10.interactSound = Res.LoadSound("Interface/WeaponHandling/HandleWeaponB");
		g10.purchasable = true;
		g10.basePrice = 2000;
		g10.verbDef = NewBaseGunshotVerbDefinition();
		g10.verbDef.projDef = b10;
		g10.verbDef.warmupTicks = 108;
		g10.verbDef.range = 24f;
		g10.verbDef.accuracy = 6f;
		g10.verbDef.ticksBetweenBurstShots = 12;
		g10.verbDef.burstShotCount = 3;
		g10.verbDef.soundCast = Res.LoadSound("Guns/ChargeShotA");
		yield return g10;
	}

	private static ThingDefinition BaseMealDefinition()
	{
		ThingDefinition thingDefinition = new ThingDefinition();
		thingDefinition.eType = EntityType.Meal;
		thingDefinition.label = "Meal lacks label";
		thingDefinition.thingClass = typeof(Meal);
		thingDefinition.category = EntityCategory.SmallObject;
		thingDefinition.useStandardHealth = true;
		thingDefinition.selectable = true;
		thingDefinition.maxHealth = 50;
		thingDefinition.altitudeLayer = AltitudeLayer.OverWaist;
		thingDefinition.stackLimit = 1;
		thingDefinition.tickerType = TickerType.Rare;
		thingDefinition.desc = "Meal lacks desc.";
		thingDefinition.flammability = 1f;
		thingDefinition.storeType = StoreType.Meal;
		thingDefinition.designateHaulable = true;
		thingDefinition.compSetupList.Add(CompSetup.Forbiddable);
		return thingDefinition;
	}

	public static IEnumerable<ThingDefinition> Definitions_Meals()
	{
		ThingDefinition d = BaseMealDefinition();
		d.definitionName = "MealNutrientPaste";
		d.label = "Nutrient paste meal";
		d.desc = "A synthetic mixture of protein, carbohydrates, and vitamins, amino acids and minerals. Everything the body needs, and absolutely disgusting.";
		d.texturePath = "Icons/SmallObject/Meal/NutrientPaste";
		d.meal_ticksBeforeSpoil = 80000;
		d.food = new FoodProperties(FoodCategory.Prepared, 80f);
		d.food.eatenThoughtType = ThoughtType.AteNutrientPaste;
		yield return d;
	}

	private static ThingDefinition NewBasePlantDef()
	{
		ThingDefinition thingDefinition = new ThingDefinition();
		thingDefinition.eType = EntityType.Plant;
		thingDefinition.thingClass = typeof(Plant);
		thingDefinition.category = EntityCategory.SmallObject;
		thingDefinition.altitudeLayer = AltitudeLayer.FloorDeco;
		thingDefinition.flammability = 1f;
		thingDefinition.useStandardHealth = true;
		thingDefinition.maxHealth = 75;
		thingDefinition.tickerType = TickerType.Rare;
		thingDefinition.selectable = true;
		thingDefinition.sunShadowMesh = null;
		thingDefinition.selectable = false;
		thingDefinition.neverMultiSelect = true;
		thingDefinition.addToMapMesh = true;
		thingDefinition.baseMaterial = MatBases.PlantCutout;
		thingDefinition.plant = new PlantProperties();
		thingDefinition.plant.minGlowToGrow = PsychGlow.Overlit;
		thingDefinition.plant.SeedShootMinGrowthPercent = 0.6f;
		return thingDefinition;
	}

	public static IEnumerable<ThingDefinition> Definitions_Plants()
	{
		ThingDefinition d10 = NewBasePlantDef();
		d10.definitionName = "PlantPotato";
		d10.label = "Potato plant";
		d10.maxHealth = 85;
		d10.desc = "A simple, highly nutritious tuber.";
		d10.beauty = BeautyCategory.NiceTiny;
		d10.textureFolderPath = "Icons/Plant/PotatoPlant";
		d10.selectable = true;
		d10.pathCost = 10;
		d10.food = new FoodProperties(FoodCategory.Unharvested, 40f);
		d10.plant.maxFoodYield = 6f;
		d10.plant.wild = false;
		d10.plant.minFertility = 0.7f;
		d10.plant.topWindExposure = 0.1f;
		d10.plant.growthPer20kTicks = 0.48f;
		d10.plant.lifeSpan = 200000;
		d10.plant.fertilityFactorGrowthRate = 1f;
		d10.plant.destroyOnHarvest = true;
		d10.plant.sizeRange = new FloatRange(0.3f, 1.1f);
		yield return d10;
		d10 = NewBasePlantDef();
		d10.definitionName = "PlantPotatoHydro";
		d10.label = "Accele-potato plant";
		d10.maxHealth = 85;
		d10.desc = "A simple, highly nutritious tuber. This variant is genetically engineered to grow at extreme speed, but can only survive in a nutrient-enriched hydroponic solution.";
		d10.beauty = BeautyCategory.NiceTiny;
		d10.textureFolderPath = "Icons/Plant/PotatoPlant";
		d10.altitudeLayer = AltitudeLayer.OverWaist;
		d10.selectable = true;
		d10.pathCost = 10;
		d10.food = new FoodProperties(FoodCategory.Unharvested, 40f);
		d10.plant.wild = false;
		d10.plant.maxFoodYield = 6f;
		d10.plant.topWindExposure = 0.1f;
		d10.plant.growthPer20kTicks = 0.48f;
		d10.plant.lifeSpan = 200000;
		d10.plant.fertilityFactorGrowthRate = 1f;
		d10.plant.destroyOnHarvest = true;
		d10.plant.sizeRange = new FloatRange(0.7f, 1f);
		yield return d10;
		d10 = NewBasePlantDef();
		d10.definitionName = "PlantDaylily";
		d10.label = "Daylily";
		d10.maxHealth = 85;
		d10.desc = "A beautiful cultivated flower.";
		d10.beauty = BeautyCategory.Gorgeous;
		d10.textureFolderPath = "Icons/Plant/Daylily";
		d10.altitudeLayer = AltitudeLayer.OverWaist;
		d10.selectable = true;
		d10.sunShadowMesh = MeshPool.shadow0306;
		d10.food = new FoodProperties(FoodCategory.Unharvested, 20f);
		d10.plant.wild = false;
		d10.plant.minGlowToGrow = PsychGlow.Lit;
		d10.plant.rotDamagePerTick = 0.002f;
		d10.plant.maxFoodYield = 0f;
		d10.plant.topWindExposure = 0.1f;
		d10.plant.growthPer20kTicks = 1f;
		d10.plant.lifeSpan = 60000;
		d10.plant.fertilityFactorGrowthRate = 1f;
		d10.plant.destroyOnHarvest = true;
		yield return d10;
		d10 = NewBasePlantDef();
		d10.definitionName = "PlantAgave";
		d10.label = "Agave";
		d10.maxHealth = 120;
		d10.desc = "Large-leafed desert plant with edible flowers and stalks.";
		d10.beauty = BeautyCategory.Neutral;
		d10.textureFolderPath = "Icons/Plant/Agave";
		d10.selectable = true;
		d10.sunShadowMesh = MeshPool.shadow0604short;
		d10.coverPercent = 0.25f;
		d10.pathCost = 30;
		d10.food = new FoodProperties(FoodCategory.Unharvested, 40f);
		d10.plant.maxFoodYield = 7f;
		d10.plant.wild = true;
		d10.plant.minFertility = 0.3f;
		d10.plant.wildCommonality = 0.3f;
		d10.plant.sizeRange = new FloatRange(0.7f, 1.2f);
		d10.plant.growthPer20kTicks = 0.45f;
		d10.plant.topWindExposure = 0.3f;
		d10.plant.SeedShootRadius = 3f;
		d10.plant.SeedEmitAveragePer20kTicks = 0.4f;
		d10.plant.lifeSpan = 500000;
		d10.plant.fertilityFactorGrowthRate = 0.5f;
		d10.plant.generateClusterSizeRange = new IntRange(3, 10);
		yield return d10;
		d10 = NewBasePlantDef();
		d10.definitionName = "PlantRaspberry";
		d10.label = "Raspberry bush";
		d10.maxHealth = 120;
		d10.desc = "Bushy plant with delicious red berries.";
		d10.beauty = BeautyCategory.Neutral;
		d10.textureFolderPath = "Icons/Plant/Raspberry";
		d10.selectable = true;
		d10.sunShadowMesh = MeshPool.shadow0604short;
		d10.coverPercent = 0.25f;
		d10.pathCost = 30;
		d10.food = new FoodProperties(FoodCategory.Unharvested, 40f);
		d10.plant.maxFoodYield = 7f;
		d10.plant.wild = true;
		d10.plant.minFertility = 0.5f;
		d10.plant.wildCommonality = 0.3f;
		d10.plant.sizeRange = new FloatRange(0.7f, 1.2f);
		d10.plant.growthPer20kTicks = 0.45f;
		d10.plant.topWindExposure = 0.3f;
		d10.plant.SeedShootRadius = 3f;
		d10.plant.SeedEmitAveragePer20kTicks = 0.4f;
		d10.plant.lifeSpan = 500000;
		d10.plant.fertilityFactorGrowthRate = 0.5f;
		d10.plant.generateClusterSizeRange = new IntRange(3, 10);
		yield return d10;
		d10 = NewBasePlantDef();
		d10.definitionName = "PlantBush";
		d10.label = "Bush";
		d10.maxHealth = 120;
		d10.desc = "Short shrub. Does not typically talk.";
		d10.beauty = BeautyCategory.Neutral;
		d10.textureFolderPath = "Icons/Plant/Bush";
		d10.selectable = true;
		d10.sunShadowMesh = MeshPool.shadow0604short;
		d10.coverPercent = 0.25f;
		d10.pathCost = 30;
		d10.food = new FoodProperties(FoodCategory.Unharvested, 30f);
		d10.plant.wild = true;
		d10.plant.minFertility = 0.4f;
		d10.plant.wildCommonality = 0.7f;
		d10.plant.sizeRange = new FloatRange(0.7f, 1.2f);
		d10.plant.growthPer20kTicks = 0.3f;
		d10.plant.topWindExposure = 0.3f;
		d10.plant.SeedShootRadius = 3f;
		d10.plant.SeedEmitAveragePer20kTicks = 0.35f;
		d10.plant.lifeSpan = 500000;
		d10.plant.fertilityFactorGrowthRate = 0.5f;
		d10.plant.generateClusterSizeRange = new IntRange(2, 6);
		yield return d10;
		d10 = NewBasePlantDef();
		d10.definitionName = "PlantPovertyGrass";
		d10.label = "Poverty grass";
		d10.maxHealth = 85;
		d10.desc = "Hardy wild grass. Grows anywhere there is a little light and minimally fertile ground.";
		d10.beauty = BeautyCategory.NiceTiny;
		d10.textureFolderPath = "Icons/Plant/PovertyGrass";
		d10.food = new FoodProperties(FoodCategory.Unharvested, 15f);
		d10.plant.wild = true;
		d10.plant.minFertility = 0.05f;
		d10.plant.wildCommonality = 7f;
		d10.plant.maxMeshCount = 9;
		d10.plant.sizeRange = new FloatRange(0.4f, 0.6f);
		d10.plant.growthPer20kTicks = 0.4f;
		d10.plant.topWindExposure = 0.4f;
		d10.plant.SeedShootRadius = 8f;
		d10.plant.SeedEmitAveragePer20kTicks = 0.9f;
		d10.plant.lifeSpan = 300000;
		d10.plant.fertilityFactorGrowthRate = 0.6f;
		d10.plant.fertilityFactorPlantChance = 0.6f;
		yield return d10;
		d10 = NewBasePlantDef();
		d10.definitionName = "PlantDandelion";
		d10.label = "Dandelions";
		d10.maxHealth = 85;
		d10.desc = "Common small flower. Though it is often considered a weed because of its fecundity, it is pleasant to look at";
		d10.beauty = BeautyCategory.NiceTiny;
		d10.textureFolderPath = "Icons/Plant/Dandelion";
		d10.food = new FoodProperties(FoodCategory.Unharvested, 15f);
		d10.plant.wild = true;
		d10.plant.minFertility = 0.05f;
		d10.plant.wildCommonality = 0.8f;
		d10.plant.maxMeshCount = 32;
		d10.plant.sizeRange = new FloatRange(0.3f, 0.4f);
		d10.plant.growthPer20kTicks = 0.4f;
		d10.plant.topWindExposure = 0.3f;
		d10.plant.SeedShootRadius = 2.5f;
		d10.plant.SeedEmitAveragePer20kTicks = 1f;
		d10.plant.lifeSpan = 240000;
		d10.plant.fertilityFactorGrowthRate = 1f;
		d10.plant.fertilityFactorPlantChance = 1f;
		d10.plant.generateClusterSizeRange = new IntRange(4, 14);
		yield return d10;
		d10 = NewBasePlantDef();
		d10.definitionName = "PlantSaguaroCactus";
		d10.label = "Saguaro cactus";
		d10.maxHealth = 200;
		d10.desc = "Large cactus native to arid Earth environments.";
		d10.beauty = BeautyCategory.Neutral;
		d10.textureFolderPath = "Icons/Plant/SaguaroCactus";
		d10.altitudeLayer = AltitudeLayer.BuildingTall;
		d10.selectable = true;
		d10.coverPercent = 0.35f;
		d10.sunShadowMesh = MeshPool.shadow0306;
		d10.passability = Traversability.Impassable;
		d10.plant.sizeRange = new FloatRange(1.5f, 2f);
		d10.plant.wildCommonality = 0.3f;
		d10.plant.wild = true;
		d10.plant.minFertility = 0.05f;
		d10.plant.growthPer20kTicks = 0.14f;
		d10.plant.topWindExposure = 0.08f;
		d10.plant.SeedShootRadius = 15f;
		d10.plant.SeedEmitAveragePer20kTicks = 0.05f;
		d10.plant.lifeSpan = 800000;
		d10.plant.fertilityFactorGrowthRate = 0f;
		yield return d10;
		d10 = NewBasePlantDef();
		d10.definitionName = "PlantPincushionCactus";
		d10.label = "Pincushion cactus";
		d10.maxHealth = 150;
		d10.desc = "Short cactus, so named because it resembles a pincushion.";
		d10.beauty = BeautyCategory.Neutral;
		d10.textureFolderPath = "Icons/Plant/PincushionCactus";
		d10.selectable = true;
		d10.pathCost = 30;
		d10.plant.wild = true;
		d10.plant.minFertility = 0.05f;
		d10.plant.wildCommonality = 0.3f;
		d10.plant.sizeRange = new FloatRange(0.45f, 0.6f);
		d10.plant.growthPer20kTicks = 0.3f;
		d10.plant.topWindExposure = 0f;
		d10.plant.SeedShootRadius = 4f;
		d10.plant.SeedEmitAveragePer20kTicks = 0.1f;
		d10.plant.lifeSpan = 800000;
		d10.plant.fertilityFactorGrowthRate = 0f;
		d10.plant.generateClusterSizeRange = new IntRange(3, 5);
		d10.plant.maxMeshCount = 4;
		yield return d10;
	}

	public static IEnumerable<ThingDefinition> Definitions_Projectiles()
	{
		ThingDefinition d6 = new ThingDefinition();
		d6 = NewBaseProjectileDefinition();
		d6.eType = EntityType.Proj_Spark;
		d6.label = "Spark";
		d6.thingClass = typeof(Spark);
		d6.texturePath = "Icons/Projectile/Spark";
		d6.projectile_Speed = 1.5f;
		d6.projectile_ImpactWorld = true;
		yield return d6;
		d6 = NewBaseProjectileDefinition();
		d6.eType = EntityType.Proj_ThrownTorch;
		d6.label = "Thrown torch";
		d6.thingClass = typeof(Spark);
		d6.texturePath = "Icons/Projectile/Spark";
		d6.useStandardHealth = false;
		d6.projectile_Speed = 6f;
		d6.projectile_ImpactWorld = false;
		yield return d6;
		d6 = NewBaseProjectileDefinition();
		d6.eType = EntityType.Proj_WaterSplash;
		d6.label = "Water splash";
		d6.thingClass = typeof(WaterSplash);
		d6.texturePath = "Icons/Projectile/WaterSplash";
		d6.projectile_Speed = 7f;
		d6.projectile_ImpactWorld = false;
		yield return d6;
		d6 = NewBaseProjectileDefinition();
		d6.eType = EntityType.Proj_StunShot;
		d6.label = "Stun shot";
		d6.thingClass = typeof(Projectile_Explosive);
		d6.texturePath = "Icons/Projectile/Spark";
		d6.projectile_Speed = 40f;
		d6.projectile_ImpactWorld = true;
		d6.projectile_ExplosionRadius = 1.9f;
		d6.projectile_DamageType = DamageType.Stun;
		yield return d6;
		d6 = NewBaseProjectileDefinition();
		d6.eType = EntityType.Proj_GrenadeFallen;
		d6.label = "Fallen Grenade";
		d6.thingClass = typeof(Projectile_Explosive);
		d6.texturePath = "Icons/Projectile/Grenade";
		d6.projectile_Speed = 13f;
		d6.projectile_ImpactWorld = true;
		d6.projectile_ExplosionRadius = 1.9f;
		d6.projectile_DamageType = DamageType.Bomb;
		d6.projectile_ExplosionDelay = 999999;
		d6.compSetupList.Add(CompSetup.Explosive_FallenGrenade);
		d6.altitudeLayer = AltitudeLayer.SmallObjectCritical;
		d6.hasTooltip = true;
		d6.desc = "Will detonate if damaged.";
		yield return d6;
	}

	public static ThingDefinition NewBaseProjectileDefinition()
	{
		ThingDefinition thingDefinition = new ThingDefinition();
		thingDefinition.category = EntityCategory.Projectile;
		thingDefinition.tickerType = TickerType.Normal;
		thingDefinition.altitudeLayer = AltitudeLayer.Projectile;
		thingDefinition.baseMaterial = MatBases.Transparent;
		thingDefinition.useStandardHealth = false;
		thingDefinition.neverMultiSelect = true;
		return thingDefinition;
	}

	private static ThingDefinition ResourceDefBase()
	{
		ThingDefinition thingDefinition = new ThingDefinition();
		thingDefinition.thingClass = typeof(ThingResource);
		thingDefinition.label = "Unspecified resource";
		thingDefinition.category = EntityCategory.SmallObject;
		thingDefinition.isResource = true;
		thingDefinition.useStandardHealth = true;
		thingDefinition.selectable = true;
		thingDefinition.maxHealth = 100;
		thingDefinition.altitudeLayer = AltitudeLayer.OverWaist;
		thingDefinition.stackLimit = 99999;
		thingDefinition.desc = "Resource lacks desc.";
		thingDefinition.compSetupList.Add(CompSetup.Forbiddable);
		thingDefinition.beauty = BeautyCategory.Ugly;
		thingDefinition.alwaysHaulable = true;
		thingDefinition.drawGUIOverlay = true;
		thingDefinition.rotatable = false;
		return thingDefinition;
	}

	public static IEnumerable<ThingDefinition> Definitions_Resources()
	{
		ThingDefinition def7 = ResourceDefBase();
		def7.eType = EntityType.Money;
		def7.label = "Money";
		def7.desc = "For trading.";
		def7.texturePath = "UI/Resources/MoneyIcon";
		def7.interactSound = Res.LoadSound("Interaction/Drop/MetalDrop");
		yield return def7;
		def7 = ResourceDefBase();
		def7.eType = EntityType.Food;
		def7.label = "Food";
		def7.desc = "Unrefined foodstuffs.";
		def7.texturePath = "UI/Resources/FoodIcon";
		def7.interactSound = Res.LoadSound("Interaction/Drop/FoodDrop");
		def7.basePrice = 10;
		def7.food = new FoodProperties(FoodCategory.Harvested, 5f);
		def7.food.eatenThoughtType = ThoughtType.AteRawFood;
		yield return def7;
		def7 = ResourceDefBase();
		def7.eType = EntityType.Metal;
		def7.label = "Metal";
		def7.desc = "For building structures.";
		def7.texturePath = "UI/Resources/MetalIcon";
		def7.interactSound = Res.LoadSound("Interaction/Drop/MetalDrop");
		def7.basePrice = 10;
		def7.useStandardHealth = false;
		yield return def7;
		def7 = ResourceDefBase();
		def7.eType = EntityType.Medicine;
		def7.label = "Medicine";
		def7.desc = "Medical staff use these supplies to heal the wounded.";
		def7.texturePath = "UI/Resources/MedicineIcon";
		def7.interactSound = Res.LoadSound("Interaction/Drop/MedicineDrop");
		def7.basePrice = 40;
		yield return def7;
		def7 = ResourceDefBase();
		def7.eType = EntityType.Uranium;
		def7.label = "Uranium";
		def7.desc = "Powers reactors.";
		def7.texturePath = "UI/Resources/UraniumIcon";
		def7.interactSound = Res.LoadSound("Interaction/Drop/MetalDrop");
		def7.basePrice = 20;
		yield return def7;
		def7 = ResourceDefBase();
		def7.eType = EntityType.Shells;
		def7.label = "Shells";
		def7.desc = "Fired from cannons.";
		def7.texturePath = "UI/Resources/ShellIcon";
		def7.interactSound = Res.LoadSound("Interaction/Drop/MetalDrop");
		def7.basePrice = 10;
		yield return def7;
		def7 = ResourceDefBase();
		def7.eType = EntityType.Missiles;
		def7.label = "Missiles";
		def7.desc = "Launched from missile launchers.";
		def7.texturePath = "UI/Resources/MissileIcon";
		def7.interactSound = Res.LoadSound("Interaction/Drop/MetalDrop");
		def7.basePrice = 100;
		yield return def7;
	}

	public static IEnumerable<ThingDefinition> Definitions_Various()
	{
		ThingDefinition d = new ThingDefinition();
		yield return new ThingDefinition
		{
			eType = EntityType.Pawn,
			thingClass = typeof(Pawn),
			category = EntityCategory.Pawn,
			selectable = true,
			tickerType = TickerType.Normal,
			altitudeLayer = AltitudeLayer.Pawn,
			useStandardHealth = false,
			flammability = 1f,
			hasTooltip = true,
			bulletHitSoundFolder = "Flesh",
			inspectorTabs = 
			{
				(ITab)new ITab_Pawn_Thoughts(),
				(ITab)new ITab_Pawn_Needs(),
				(ITab)new ITab_Pawn_Prisoner(),
				(ITab)new ITab_Pawn_Character()
			},
			drawGUIOverlay = true
		};
		yield return new ThingDefinition
		{
			eType = EntityType.Corpse,
			thingClass = typeof(Corpse),
			category = EntityCategory.SmallObject,
			selectable = true,
			tickerType = TickerType.Rare,
			altitudeLayer = AltitudeLayer.SmallObject,
			maxHealth = 100,
			flammability = 1f,
			bulletHitSoundFolder = "Flesh",
			beauty = BeautyCategory.Hideous,
			alwaysHaulable = true,
			storeType = StoreType.Corpse,
			compSetupList = { CompSetup.Forbiddable },
			dropSound = Res.LoadSound("Interaction/Haul/CorpseDrop")
		};
		yield return new ThingDefinition
		{
			eType = EntityType.DebrisRock,
			label = "Rock debris",
			thingClass = typeof(Building),
			category = EntityCategory.SmallObject,
			textureFolderPath = "Icons/Building/DebrisRock",
			altitudeLayer = AltitudeLayer.Waist,
			passability = Traversability.PassThroughOnly,
			coverPercent = 0.4f,
			maxHealth = 300,
			selectable = true,
			neverMultiSelect = true,
			pathCost = 60,
			overDraw = false,
			addToMapMesh = true,
			naturalBuilding = true,
			randomizeRotationOnSpawn = true,
			designateHaulable = true,
			storeType = StoreType.Debris,
			dropSound = Res.LoadSound("Interaction/Haul/JunkDrop"),
			isDebris = true,
			saveCompressible = true
		};
		yield return new ThingDefinition
		{
			eType = EntityType.DebrisSlag,
			label = "Slag debris",
			thingClass = typeof(Building),
			category = EntityCategory.SmallObject,
			textureFolderPath = "Icons/Building/DebrisSlag",
			altitudeLayer = AltitudeLayer.Waist,
			passability = Traversability.PassThroughOnly,
			coverPercent = 0.4f,
			maxHealth = 300,
			selectable = true,
			neverMultiSelect = true,
			pathCost = 60,
			overDraw = false,
			addToMapMesh = true,
			naturalBuilding = true,
			randomizeRotationOnSpawn = true,
			designateHaulable = true,
			storeType = StoreType.Debris,
			dropSound = Res.LoadSound("Interaction/Haul/JunkDrop"),
			isDebris = true,
			saveCompressible = true
		};
		yield return new ThingDefinition
		{
			eType = EntityType.DoorKey,
			label = "Door key",
			thingClass = typeof(ThingWithComponents),
			category = EntityCategory.SmallObject,
			texturePath = "Icons/SmallObject/DoorKey",
			altitudeLayer = AltitudeLayer.Waist,
			useStandardHealth = true,
			maxHealth = 30,
			compSetupList = { CompSetup.Forbiddable },
			selectable = true,
			desc = "Doors will open for someone carrying this key."
		};
		yield return new ThingDefinition
		{
			eType = EntityType.LiquidFuel,
			label = "Liquid fuel",
			thingClass = typeof(LiquidFuel),
			category = EntityCategory.SmallObject,
			texturePath = "Icons/SmallObject/LiquidFuel",
			baseMaterial = MatBases.Transparent,
			altitudeLayer = AltitudeLayer.FloorDeco,
			flammability = 4f,
			useStandardHealth = true,
			maxHealth = 150,
			tickerType = TickerType.Normal,
			desc = "Burns on the ground."
		};
		yield return new ThingDefinition
		{
			eType = EntityType.ExplosiveBarrel,
			label = "Oil Can",
			thingClass = typeof(ThingWithComponents),
			category = EntityCategory.SmallObject,
			texturePath = "Icons/SmallObject/CanOil",
			altitudeLayer = AltitudeLayer.Waist,
			flammability = 4f,
			useStandardHealth = true,
			maxHealth = 100,
			tickerType = TickerType.Normal,
			compSetupList = { CompSetup.Explosive_49 },
			selectable = true,
			desc = "Explodes when damaged.",
			coverPercent = 0.5f,
			isAutoAttackableWorldObject = true,
			bulletHitSoundFolder = "Metal"
		};
		yield return new ThingDefinition
		{
			eType = EntityType.PressurizedCan,
			label = "Pressurized Canister",
			thingClass = typeof(Jetter),
			category = EntityCategory.SmallObject,
			texturePath = "Icons/SmallObject/CanPressurized",
			altitudeLayer = AltitudeLayer.Waist,
			flammability = 4f,
			maxHealth = 100,
			tickerType = TickerType.Normal,
			selectable = true,
			desc = "Jets and explodes when damaged.",
			coverPercent = 0.5f,
			isAutoAttackableWorldObject = true,
			bulletHitSoundFolder = "Metal"
		};
		yield return new ThingDefinition
		{
			eType = EntityType.ElectricalBox,
			label = "Electrical Box",
			thingClass = typeof(ThingWithComponents),
			category = EntityCategory.SmallObject,
			texturePath = "Icons/SmallObject/CanOil",
			altitudeLayer = AltitudeLayer.Waist,
			compSetupList = 
			{
				CompSetup.Glower_Medium,
				CompSetup.Explosive_19Flame
			},
			maxHealth = 100,
			selectable = true,
			flammability = 4f,
			beauty = BeautyCategory.Ugly,
			tickerType = TickerType.Normal,
			desc = "Explodes and ignites the surrounding area when damaged.",
			isAutoAttackableWorldObject = true,
			bulletHitSoundFolder = "Metal"
		};
		yield return new ThingDefinition
		{
			eType = EntityType.DropPodIncoming,
			label = "Drop pod (incoming)",
			thingClass = typeof(DropPodIncoming),
			category = EntityCategory.Special,
			tickerType = TickerType.Normal,
			texturePath = "Icons/Special/DropPod",
			overDraw = true,
			altitudeLayer = AltitudeLayer.MetaOverlays,
			useStandardHealth = false,
			baseMaterial = MatBases.CutoutFlying
		};
		yield return new ThingDefinition
		{
			eType = EntityType.DropPod,
			label = "Drop pod",
			thingClass = typeof(DropPod),
			category = EntityCategory.Building,
			tickerType = TickerType.Normal,
			texturePath = "Icons/Special/DropPod",
			overDraw = true,
			altitudeLayer = AltitudeLayer.BuildingTall,
			useStandardHealth = true,
			maxHealth = 500,
			selectable = true,
			sunShadowMesh = MeshPool.shadow1006
		};
		yield return new ThingDefinition
		{
			eType = EntityType.Fire,
			label = "Fire",
			thingClass = typeof(Fire),
			category = EntityCategory.Attachment,
			tickerType = TickerType.Normal,
			textureFolderPath = "Icons/Special/Fire",
			baseMaterial = MatBases.MotePostLight,
			altitudeLayer = AltitudeLayer.PawnState,
			useStandardHealth = false,
			beauty = BeautyCategory.Horrifying
		};
	}
}
