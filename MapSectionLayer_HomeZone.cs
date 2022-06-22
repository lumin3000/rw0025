using System.Collections.Generic;
using UnityEngine;

internal class MapSectionLayer_HomeZone : MapSectionLayer
{
	private static Material CleanGridOverlayMat = GenRender.SolidColorMaterial(new Color(0.3f, 0.3f, 0.9f, 0.4f));

	public MapSectionLayer_HomeZone(MapSection section)
		: base(section)
	{
		CleanGridOverlayMat.renderQueue = 3600;
		layerMats.Add(CleanGridOverlayMat);
		relevantChangeTypes.Add(MapChangeType.HomeZone);
	}

	public override void DrawLayer()
	{
		if (OverlayDrawHandler.ShouldDrawHomeZone)
		{
			base.DrawLayer();
		}
	}

	public override void RegenerateMesh()
	{
		if (!DebugSettings.drawWorldOverlays)
		{
			return;
		}
		bool[,] homeGrid = Find.HomeZoneGrid.homeGrid;
		IntRect intRect = new IntRect(section.botLeft.x, section.botLeft.z, 17, 17);
		intRect.ClipInsideMap();
		List<Vector3> list = new List<Vector3>();
		List<int> list2 = new List<int>();
		list.Capacity = intRect.Area * 2;
		list2.Capacity = intRect.Area * 4;
		float y = Altitudes.AltitudeFor(AltitudeLayer.WorldDataOverlay);
		for (int i = intRect.minX; i <= intRect.maxX; i++)
		{
			for (int j = intRect.minZ; j <= intRect.maxZ; j++)
			{
				if (homeGrid[i, j])
				{
					list.Add(new Vector3(i, y, j));
					list.Add(new Vector3(i, y, j + 1));
					list.Add(new Vector3(i + 1, y, j + 1));
					list.Add(new Vector3(i + 1, y, j));
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
