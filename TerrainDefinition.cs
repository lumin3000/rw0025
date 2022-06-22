using System.Collections.Generic;
using UnityEngine;

public class TerrainDefinition : EntityDefinition
{
	public enum TerrainEdgeType
	{
		Hard,
		Fade
	}

	public TerrainEdgeType edgeType;

	public int renderPrecedence;

	public List<SurfaceType> surfacesSupported = new List<SurfaceType>();

	public ScatterType scatterType;

	public bool takeFootprints;

	public string terrainFilthName = string.Empty;

	public bool acceptTerrainSourceFilth = true;

	public ThingDefinition TerrainFilthDef
	{
		get
		{
			if (terrainFilthName == string.Empty)
			{
				return null;
			}
			return ThingDefDatabase.ThingDefNamed(terrainFilthName);
		}
	}

	public byte UniqueSaveKey => (byte)renderPrecedence;

	public ThingDefinition BlueprintDefinition => ThingDefDatabase.ThingDefNamed(label + ConstructionUtility.BlueprintDefNameSuffix);

	public ThingDefinition FrameDefinition => ThingDefDatabase.ThingDefNamed(label + ConstructionUtility.BuildingFrameDefNameSuffix);

	public override void PostLoad()
	{
		base.PostLoad();
		category = EntityCategory.Terrain;
		placingDraggableDimensions = 2;
		if (terrainFilthName != string.Empty)
		{
			acceptTerrainSourceFilth = false;
		}
		definitionName = "Terrain_" + label.Replace(" ", string.Empty);
		Material baseMat = null;
		if (edgeType == TerrainEdgeType.Hard)
		{
			baseMat = MatBases.TerrainHard;
		}
		if (edgeType == TerrainEdgeType.Fade)
		{
			baseMat = MatBases.TerrainFade;
		}
		drawMat = MaterialPool.MatFrom(texturePath, baseMat);
		drawMat.renderQueue = 2000 + renderPrecedence;
		if (fertility < 0f)
		{
			Debug.LogWarning(string.Concat("Terrain definition ", this, " has no fertility value set."));
			fertility = 0f;
		}
	}

	public override string ToString()
	{
		return "TerrainDef:(" + label + ")";
	}
}
