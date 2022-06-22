using System.Collections.Generic;
using UnityEngine;

internal class MapSectionLayer_IndoorMask : MapSectionLayer
{
	public MapSectionLayer_IndoorMask(MapSection section)
		: base(section)
	{
		layerMats = new List<Material>();
		layerMats.Add(MatBases.IndoorMask);
		relevantChangeTypes.Add(MapChangeType.Roofs);
		relevantChangeTypes.Add(MapChangeType.FogOfWar);
	}

	public override void DrawLayer()
	{
		if (DebugSettings.drawShadows)
		{
			base.DrawLayer();
		}
	}

	public override void RegenerateMesh()
	{
		if (!DebugSettings.drawShadows || !MatBases.SunShadow.shader.isSupported)
		{
			return;
		}
		EntityType[,] roofGrid = Find.RoofGrid.roofGrid;
		Thing[,,] blockerGrid = Find.Grids.blockerGrid;
		IntRect intRect = new IntRect(section.botLeft.x, section.botLeft.z, 17, 17);
		intRect.ClipInsideMap();
		List<Vector3> list = new List<Vector3>();
		List<int> list2 = new List<int>();
		list.Capacity = intRect.Area * 2;
		list2.Capacity = intRect.Area * 4;
		float y = Altitudes.AltitudeFor(AltitudeLayer.MetaOverlays);
		for (int i = intRect.minX; i <= intRect.maxX; i++)
		{
			for (int j = intRect.minZ; j <= intRect.maxZ; j++)
			{
				if (roofGrid[i, j] != 0 && !Find.FogGrid.IsFogged(new IntVec3(i, 0, j)))
				{
					Thing thing = blockerGrid[i, 0, j];
					float num = ((thing == null || (thing.def.passability != Traversability.Impassable && thing.def.eType != EntityType.Door)) ? 0.16f : 0f);
					list.Add(new Vector3((float)i - num, y, (float)j - num));
					list.Add(new Vector3((float)i - num, y, (float)(j + 1) + num));
					list.Add(new Vector3((float)(i + 1) + num, y, (float)(j + 1) + num));
					list.Add(new Vector3((float)(i + 1) + num, y, (float)j - num));
					list2.Add(list.Count - 4);
					list2.Add(list.Count - 3);
					list2.Add(list.Count - 2);
					list2.Add(list.Count - 4);
					list2.Add(list.Count - 2);
					list2.Add(list.Count - 1);
				}
			}
		}
		ResetLayerMesh();
		layerMesh.vertices = list.ToArray();
		layerMesh.triangles = list2.ToArray();
	}
}
