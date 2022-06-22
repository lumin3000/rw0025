using System.Collections.Generic;
using UnityEngine;

internal class MapSectionLayer_SunShadows : MapSectionLayer
{
	private static Thing[,,] blockerGrid;

	private static readonly Color32 LowVertexColor = new Color32(0, 0, 0, 0);

	public MapSectionLayer_SunShadows(MapSection section)
		: base(section)
	{
		layerMats = new List<Material>();
		layerMats.Add(MatBases.SunShadow);
		relevantChangeTypes.Add(MapChangeType.Blockers);
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
		blockerGrid = Find.Grids.blockerGrid;
		float y = Altitudes.AltitudeFor(AltitudeLayer.Shadows);
		IntRect intRect = new IntRect(section.botLeft.x, section.botLeft.z, 17, 17);
		intRect.ClipInsideMap();
		List<Vector3> list = new List<Vector3>();
		List<Color32> list2 = new List<Color32>();
		List<int> list3 = new List<int>();
		list.Capacity = intRect.Area * 2;
		list3.Capacity = intRect.Area * 4;
		for (int i = intRect.minX; i <= intRect.maxX; i++)
		{
			for (int j = intRect.minZ; j <= intRect.maxZ; j++)
			{
				Thing thing = blockerGrid[i, 0, j];
				if (thing == null || !(thing.def.staticSunShadowHeight > 0f))
				{
					continue;
				}
				float staticSunShadowHeight = thing.def.staticSunShadowHeight;
				Color32 item = new Color32(0, 0, 0, (byte)(255f * staticSunShadowHeight));
				int count = list.Count;
				list.Add(new Vector3(i, y, j));
				list2.Add(LowVertexColor);
				list.Add(new Vector3(i, y, j + 1));
				list2.Add(LowVertexColor);
				list.Add(new Vector3(i + 1, y, j + 1));
				list2.Add(LowVertexColor);
				list.Add(new Vector3(i + 1, y, j));
				list2.Add(LowVertexColor);
				list3.Add(list.Count - 4);
				list3.Add(list.Count - 3);
				list3.Add(list.Count - 2);
				list3.Add(list.Count - 4);
				list3.Add(list.Count - 2);
				list3.Add(list.Count - 1);
				if (i > 0)
				{
					thing = blockerGrid[i - 1, 0, j];
					if (thing == null || thing.def.staticSunShadowHeight < staticSunShadowHeight)
					{
						int count2 = list.Count;
						list.Add(new Vector3(i, y, j));
						list2.Add(item);
						list.Add(new Vector3(i, y, j + 1));
						list2.Add(item);
						list3.Add(count + 1);
						list3.Add(count);
						list3.Add(count2);
						list3.Add(count2);
						list3.Add(count2 + 1);
						list3.Add(count + 1);
					}
				}
				if (i < Find.Map.Size.x - 1)
				{
					thing = blockerGrid[i + 1, 0, j];
					if (thing == null || thing.def.staticSunShadowHeight < staticSunShadowHeight)
					{
						int count3 = list.Count;
						list.Add(new Vector3(i + 1, y, j + 1));
						list2.Add(item);
						list.Add(new Vector3(i + 1, y, j));
						list2.Add(item);
						list3.Add(count + 2);
						list3.Add(count3);
						list3.Add(count3 + 1);
						list3.Add(count3 + 1);
						list3.Add(count + 3);
						list3.Add(count + 2);
					}
				}
				if (j > 0)
				{
					thing = blockerGrid[i, 0, j - 1];
					if (thing == null || thing.def.staticSunShadowHeight < staticSunShadowHeight)
					{
						int count4 = list.Count;
						list.Add(new Vector3(i, y, j));
						list2.Add(item);
						list.Add(new Vector3(i + 1, y, j));
						list2.Add(item);
						list3.Add(count);
						list3.Add(count + 3);
						list3.Add(count4);
						list3.Add(count + 3);
						list3.Add(count4 + 1);
						list3.Add(count4);
					}
				}
			}
		}
		ResetLayerMesh();
		layerMesh.vertices = list.ToArray();
		layerMesh.colors32 = list2.ToArray();
		layerMesh.triangles = list3.ToArray();
	}
}
