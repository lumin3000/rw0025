using System.Collections.Generic;
using UnityEngine;

internal class MapSectionLayer_Terrain : MapSectionLayer
{
	private static readonly TerrainDefinition UnderwallDef = TerrainDefDatabase.TerrainWithLabel("Underwall");

	private static readonly Color32 ColorWhite = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	private static readonly Color32 ColorClear = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);

	public MapSectionLayer_Terrain(MapSection section)
		: base(section)
	{
		relevantChangeTypes.Add(MapChangeType.Terrain);
	}

	public override void DrawLayer()
	{
		if (DebugSettings.drawTerrain)
		{
			base.DrawLayer();
		}
	}

	public override void RegenerateMesh()
	{
		if (!DebugSettings.drawTerrain)
		{
			return;
		}
		layerMats.Clear();
		Grids grids = Find.Grids;
		TerrainGrid terrainGrid = Find.TerrainGrid;
		IntRect mapRect = section.MapRect;
		foreach (IntVec3 item in mapRect)
		{
			if (!layerMats.Contains(terrainGrid.TerrainAt(item).drawMat))
			{
				layerMats.Add(terrainGrid.TerrainAt(item).drawMat);
			}
		}
		List<Vector3> list = new List<Vector3>();
		list.Capacity = mapRect.Area * 4;
		List<Color32> list2 = new List<Color32>();
		list2.Capacity = mapRect.Area * 4;
		List<List<int>> list3 = new List<List<int>>();
		for (int i = 0; i < layerMats.Count; i++)
		{
			List<int> list4 = new List<int>();
			list4.Capacity = mapRect.Area * 4;
			list3.Add(new List<int>());
		}
		TerrainDefinition[] array = new TerrainDefinition[8];
		HashSet<TerrainDefinition> hashSet = new HashSet<TerrainDefinition>();
		bool[] array2 = new bool[8];
		foreach (IntVec3 item2 in mapRect)
		{
			hashSet.Clear();
			TerrainDefinition terrainDefinition = terrainGrid.TerrainAt(item2);
			int count = list.Count;
			list.Add(new Vector3(item2.x, 0f, item2.z));
			list2.Add(ColorWhite);
			list.Add(new Vector3(item2.x, 0f, item2.z + 1));
			list2.Add(ColorWhite);
			list.Add(new Vector3(item2.x + 1, 0f, item2.z + 1));
			list2.Add(ColorWhite);
			list.Add(new Vector3(item2.x + 1, 0f, item2.z));
			list2.Add(ColorWhite);
			int index = layerMats.IndexOf(terrainDefinition.drawMat);
			list3[index].Add(count);
			list3[index].Add(count + 1);
			list3[index].Add(count + 2);
			list3[index].Add(count);
			list3[index].Add(count + 2);
			list3[index].Add(count + 3);
			for (int j = 0; j < 8; j++)
			{
				IntVec3 intVec = item2 + Gen.AdjacentSquaresAroundBottom[j];
				if (!intVec.InBounds())
				{
					array[j] = terrainDefinition;
					continue;
				}
				TerrainDefinition terrainDefinition2 = terrainGrid.TerrainAt(intVec);
				Thing thing = grids.BlockerAt(intVec);
				if (thing != null && thing.def.fillsSquare)
				{
					terrainDefinition2 = UnderwallDef;
				}
				array[j] = terrainDefinition2;
				if (terrainDefinition2 != terrainDefinition && terrainDefinition2.edgeType != 0 && terrainDefinition2.renderPrecedence >= terrainDefinition.renderPrecedence && !hashSet.Contains(terrainDefinition2))
				{
					hashSet.Add(terrainDefinition2);
				}
			}
			foreach (TerrainDefinition item3 in hashSet)
			{
				count = list.Count;
				list.Add(new Vector3((float)item2.x + 0.5f, 0f, item2.z));
				list.Add(new Vector3(item2.x, 0f, item2.z));
				list.Add(new Vector3(item2.x, 0f, (float)item2.z + 0.5f));
				list.Add(new Vector3(item2.x, 0f, item2.z + 1));
				list.Add(new Vector3((float)item2.x + 0.5f, 0f, item2.z + 1));
				list.Add(new Vector3(item2.x + 1, 0f, item2.z + 1));
				list.Add(new Vector3(item2.x + 1, 0f, (float)item2.z + 0.5f));
				list.Add(new Vector3(item2.x + 1, 0f, item2.z));
				list.Add(new Vector3((float)item2.x + 0.5f, 0f, (float)item2.z + 0.5f));
				for (int k = 0; k < 8; k++)
				{
					array2[k] = false;
				}
				for (int l = 0; l < 8; l++)
				{
					if (l % 2 == 0)
					{
						if (array[l] == item3)
						{
							int num = l - 1;
							if (num < 0)
							{
								num += 8;
							}
							array2[num] = true;
							array2[l] = true;
							array2[(l + 1) % 8] = true;
						}
					}
					else if (array[l] == item3)
					{
						array2[l] = true;
					}
				}
				for (int m = 0; m < 8; m++)
				{
					if (array2[m])
					{
						list2.Add(ColorWhite);
					}
					else
					{
						list2.Add(ColorClear);
					}
				}
				list2.Add(ColorClear);
				int num2 = layerMats.IndexOf(item3.drawMat);
				if (num2 == -1)
				{
					List<int> list5 = new List<int>();
					list5.Capacity = mapRect.Area * 4;
					list3.Add(new List<int>());
					layerMats.Add(item3.drawMat);
					num2 = layerMats.Count - 1;
				}
				List<int> list6 = list3[num2];
				for (int n = 0; n < 8; n++)
				{
					list6.Add(count + n);
					list6.Add(count + (n + 1) % 8);
					list6.Add(count + 8);
				}
			}
		}
		ResetLayerMesh();
		layerMesh.vertices = list.ToArray();
		layerMesh.colors32 = list2.ToArray();
		layerMesh.subMeshCount = layerMats.Count;
		for (int num3 = 0; num3 < layerMesh.subMeshCount; num3++)
		{
			layerMesh.SetTriangles(list3[num3].ToArray(), num3);
		}
	}
}
