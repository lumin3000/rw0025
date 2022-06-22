using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapSection
{
	public const int Size = 17;

	public IntVec2 botLeft;

	public MapChangeType changesThisFrame = MapChangeType.None;

	private List<MapSectionLayer> sectionLayers = new List<MapSectionLayer>();

	private bool foundRect;

	private IntRect calculatedRect;

	public IntRect MapRect
	{
		get
		{
			if (!foundRect)
			{
				calculatedRect = new IntRect(botLeft.x, botLeft.z, 17, 17);
				calculatedRect.ClipInsideMap();
				foundRect = true;
			}
			return calculatedRect;
		}
	}

	public MapSection(IntVec2 SectCoords)
	{
		botLeft = SectCoords * 17;
		foreach (Type item in typeof(MapSectionLayer).AllLeafSubclasses())
		{
			sectionLayers.Add((MapSectionLayer)Activator.CreateInstance(item, this));
		}
	}

	public void DrawSection()
	{
		foreach (MapSectionLayer sectionLayer in sectionLayers)
		{
			sectionLayer.DrawLayer();
		}
		if (DebugSettings.drawSectionEdges)
		{
			GenRender.DrawLineBetween(botLeft.ToVector3, botLeft.ToVector3 + new Vector3(0f, 0f, 17f));
			GenRender.DrawLineBetween(botLeft.ToVector3, botLeft.ToVector3 + new Vector3(17f, 0f, 0f));
		}
	}

	public void RegenerateAllLayers()
	{
		foreach (MapSectionLayer sectionLayer in sectionLayers)
		{
			sectionLayer.RegenerateMesh();
		}
	}

	public void RegenerateLayers(MapChangeType changeType)
	{
		foreach (MapSectionLayer sectionLayer in sectionLayers)
		{
			if (sectionLayer.relevantChangeTypes.Contains(changeType))
			{
				sectionLayer.RegenerateMesh();
			}
		}
	}

	public MapSectionLayer GetLayer(Type type)
	{
		return sectionLayers.Where((MapSectionLayer sect) => sect.GetType() == type).FirstOrDefault();
	}
}
