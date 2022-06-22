using System.Collections.Generic;
using UnityEngine;

public static class ConstructionUtility
{
	public static readonly string BlueprintDefNameSuffix = "_Blueprint";

	public static readonly string BuildingFrameDefNameSuffix = "_BuildingFrame";

	private static readonly Material TerrainBlueprintIconMat = MaterialPool.MatFrom("Icons/Special/TerrainBlueprint", MatBases.MetaOverlay);

	public static IEnumerable<ThingDefinition> ConstructionRelatedThingDefinitions()
	{
		foreach (ThingDefinition buildingDef in ThingDefDatabase.AllThingDefinitions.ListFullCopy())
		{
			if (buildingDef.category == EntityCategory.Building)
			{
				ThingDefinition blueDef2 = ThingDefBases.NewBaseDefinitionFrom(EntityType.Blueprint);
				blueDef2.addToMapMesh = buildingDef.addToMapMesh;
				blueDef2.definitionName = buildingDef.definitionName + BlueprintDefNameSuffix;
				blueDef2.label = buildingDef.label + " (blueprint)";
				blueDef2.entityDefToBuild = buildingDef;
				blueDef2.size = buildingDef.size;
				blueDef2.thingClass = buildingDef.blueprintClass;
				blueDef2.linkFlags = buildingDef.linkFlags;
				blueDef2.linkDrawer = buildingDef.linkDrawer;
				blueDef2.overDraw = buildingDef.overDraw;
				blueDef2.constructionEffects = null;
				if (buildingDef.blueprintMat != null)
				{
					blueDef2.drawMat = buildingDef.blueprintMat;
				}
				else
				{
					blueDef2.texturePath = buildingDef.texturePath;
					blueDef2.baseMaterial = MatBases.Blueprint;
				}
				blueDef2.constructionEffects = null;
				yield return blueDef2;
				ThingDefinition frameDef2 = ThingDefBases.NewBaseDefinitionFrom(EntityType.BuildingFrame);
				frameDef2.definitionName = buildingDef.definitionName + BuildingFrameDefNameSuffix;
				frameDef2.label = buildingDef.label + " (building)";
				frameDef2.entityDefToBuild = buildingDef;
				frameDef2.size = buildingDef.size;
				frameDef2.maxHealth = buildingDef.maxHealth;
				frameDef2.canBeSeenOver = buildingDef.canBeSeenOver;
				frameDef2.blockLight = buildingDef.blockLight;
				frameDef2.coverPercent = buildingDef.coverPercent;
				frameDef2.desc = buildingDef.desc;
				frameDef2.passability = buildingDef.passability;
				frameDef2.selectable = buildingDef.selectable;
				frameDef2.isBarrier = false;
				frameDef2.constructionEffects = buildingDef.constructionEffects;
				frameDef2.transmitsPower = buildingDef.transmitsPower;
				yield return frameDef2;
			}
		}
		foreach (TerrainDefinition terrDef in TerrainDefDatabase.allTerrainDefs)
		{
			ThingDefinition blueDef = ThingDefBases.NewBaseDefinitionFrom(EntityType.Blueprint);
			blueDef.definitionName = terrDef.definitionName + BlueprintDefNameSuffix;
			blueDef.label = terrDef.label + " (blueprint)";
			blueDef.entityDefToBuild = terrDef;
			blueDef.drawMat = TerrainBlueprintIconMat;
			ThingDefDatabase.AddDefinitionToDatabase(blueDef);
			ThingDefinition frameDef = ThingDefBases.NewBaseDefinitionFrom(EntityType.BuildingFrame);
			frameDef.definitionName = terrDef.definitionName + BuildingFrameDefNameSuffix;
			frameDef.label = terrDef.label + " (building)";
			frameDef.entityDefToBuild = terrDef;
			frameDef.useStandardHealth = false;
			frameDef.canBeSeenOver = true;
			frameDef.blockLight = false;
			frameDef.coverPercent = 0f;
			frameDef.desc = "Terrain building in progress.";
			frameDef.passability = Traversability.Standable;
			frameDef.selectable = true;
			frameDef.isBarrier = false;
			frameDef.constructionEffects = terrDef.constructionEffects;
			ThingDefDatabase.AddDefinitionToDatabase(frameDef);
		}
	}

	public static ThingDefinition BlueprintDefinitionOf(EntityDefinition sourceDef)
	{
		string defName = sourceDef.definitionName + BlueprintDefNameSuffix;
		return ThingDefDatabase.ThingDefNamed(defName);
	}

	public static void PlaceBlueprintOf(EntityDefinition sourceDef, IntVec3 loc, IntRot rotation)
	{
		Blueprint blueprint = (Blueprint)ThingMaker.MakeThing(BlueprintDefinitionOf(sourceDef));
		blueprint.Team = TeamType.Colonist;
		ThingMaker.Spawn(blueprint, loc, rotation);
	}

	public static BuildingFrame PlaceBuildingFrameOf(EntityDefinition sourceDef, IntVec3 loc, IntRot rotation)
	{
		ThingMaker.OverwriteExistingThings(loc, rotation, sourceDef, reclaimResources: true);
		ThingDefinition def = ThingDefDatabase.ThingDefNamed(sourceDef.definitionName + BuildingFrameDefNameSuffix);
		BuildingFrame buildingFrame = (BuildingFrame)ThingMaker.MakeThing(def);
		buildingFrame.Team = TeamType.Colonist;
		ThingMaker.Spawn(buildingFrame, loc, rotation);
		return buildingFrame;
	}

	public static bool BuildingCanGoOnTerrain(EntityDefinition buildingDef, IntVec3 loc, IntRot rot)
	{
		if (buildingDef.surfaceNeeded == SurfaceType.Any)
		{
			return true;
		}
		foreach (IntVec3 item in Gen.SquaresOccupiedBy(loc, rot, buildingDef.size))
		{
			if (!item.InBounds())
			{
				return false;
			}
			TerrainDefinition terrainDefinition = Find.TerrainGrid.TerrainAt(item);
			if (!terrainDefinition.surfacesSupported.Contains(buildingDef.surfaceNeeded))
			{
				return false;
			}
		}
		return true;
	}
}
