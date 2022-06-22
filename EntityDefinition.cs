using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class EntityDefinition
{
	public EntityType eType;

	public EntityCategory category;

	public string label = string.Empty;

	public string definitionName = string.Empty;

	public string desc = string.Empty;

	public IntVec2 size = new IntVec2(1, 1);

	public float flammability;

	public string texturePath = string.Empty;

	public string menuIconPath = string.Empty;

	public string textureFolderPath = string.Empty;

	public AltitudeLayer altitudeLayer = AltitudeLayer.MetaOverlays;

	public Material baseMaterial = MatBases.Cutout;

	public bool overDraw;

	public int pathCost;

	public float fertility = -1f;

	public float workToBuild = 1f;

	public List<ResourceCost> costList = new List<ResourceCost>();

	public BeautyCategory beauty = BeautyCategory.Neutral;

	public SurfaceType surfaceNeeded;

	public List<EntityType> buildingPrerequisites = new List<EntityType>();

	public ResearchType researchPrerequisite;

	public int placingDraggableDimensions;

	public Type constructionEffects;

	public Type repairEffects = typeof(EffectMaker_Repair);

	public Traversability passability;

	public bool buildingWantsAir;

	public bool rotatable = true;

	public List<PlacementRestriction> placementRestrictions = new List<PlacementRestriction>();

	public Action placingDisplayMethod;

	public IntVec3 interactionSquareOffset = new IntVec3(0, 0, 0);

	public bool hasInteractionSquare;

	public bool neverBuildFloorsOver;

	public DesignationCategory designationCategory;

	public Dictionary<string, int> filthLeavings = new Dictionary<string, int>();

	public Material drawMat = MatsSimple.BadMaterial;

	public List<Material> folderDrawMats;

	public Texture2D uiIcon = GenUI.MissingContentTex;

	public float altitude => Altitudes.AltitudeFor(altitudeLayer);

	public Material RandomDrawMat => folderDrawMats.RandomElement();

	public bool Flammable => flammability > 0.001f;

	public virtual void PostLoad()
	{
		if (definitionName == string.Empty && label != string.Empty)
		{
			string text = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(label.ToLower());
			definitionName = text.Replace(" ", string.Empty);
		}
		if (definitionName == string.Empty)
		{
			definitionName = eType.ToString();
		}
		if (label == string.Empty)
		{
			label = definitionName;
		}
		if (texturePath != string.Empty)
		{
			drawMat = MaterialPool.MatFrom(texturePath, baseMaterial);
		}
		if (textureFolderPath != string.Empty)
		{
			folderDrawMats = MaterialPool.MatsFromFolder(textureFolderPath, baseMaterial).ToList();
		}
		if (menuIconPath != string.Empty)
		{
			uiIcon = Res.LoadTexture(menuIconPath);
		}
		else if (drawMat != null && drawMat != MatsSimple.BadMaterial)
		{
			uiIcon = (Texture2D)drawMat.mainTexture;
		}
	}

	public override string ToString()
	{
		return definitionName;
	}

	public override int GetHashCode()
	{
		return definitionName.GetHashCode();
	}
}
