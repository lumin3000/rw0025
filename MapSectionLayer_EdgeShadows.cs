using System;
using System.Collections.Generic;
using UnityEngine;

internal class MapSectionLayer_EdgeShadows : MapSectionLayer
{
	private const float InDist = 0.45f;

	private const byte ShadowBrightness = 195;

	private static Thing[,,] blockerGrid;

	private static readonly Color32 Shadowed = new Color32(195, 195, 195, byte.MaxValue);

	private static readonly Color32 Lit = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

	public MapSectionLayer_EdgeShadows(MapSection section)
		: base(section)
	{
		layerMats = new List<Material>();
		layerMats.Add(MatBases.EdgeShadow);
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
		if (!DebugSettings.drawShadows)
		{
			return;
		}
		blockerGrid = Find.Grids.blockerGrid;
		float y = Altitudes.AltitudeFor(AltitudeLayer.Shadows);
		IntRect intRect = new IntRect(section.botLeft.x, section.botLeft.z, 17, 17);
		intRect.ClipInsideMap();
		List<Vector3> vertsList = new List<Vector3>();
		List<Color32> colorsList = new List<Color32>();
		List<int> trianglesList = new List<int>();
		vertsList.Capacity = intRect.Area * 4;
		colorsList.Capacity = intRect.Area * 4;
		trianglesList.Capacity = intRect.Area * 8;
		bool[] array = new bool[4];
		bool[] array2 = new bool[4];
		bool[] array3 = new bool[4];
		float num = 0f;
		float num2 = 0f;
		for (int i = intRect.minX; i <= intRect.maxX; i++)
		{
			for (int j = intRect.minZ; j <= intRect.maxZ; j++)
			{
				Thing thing = blockerGrid[i, 0, j];
				if (thing != null && thing.def.castEdgeShadows)
				{
					vertsList.Add(new Vector3(i, y, j));
					colorsList.Add(Shadowed);
					vertsList.Add(new Vector3(i, y, j + 1));
					colorsList.Add(Shadowed);
					vertsList.Add(new Vector3(i + 1, y, j + 1));
					colorsList.Add(Shadowed);
					vertsList.Add(new Vector3(i + 1, y, j));
					colorsList.Add(Shadowed);
					trianglesList.Add(vertsList.Count - 4);
					trianglesList.Add(vertsList.Count - 3);
					trianglesList.Add(vertsList.Count - 2);
					trianglesList.Add(vertsList.Count - 4);
					trianglesList.Add(vertsList.Count - 2);
					trianglesList.Add(vertsList.Count - 1);
					continue;
				}
				array[0] = false;
				array[1] = false;
				array[2] = false;
				array[3] = false;
				array2[0] = false;
				array2[1] = false;
				array2[2] = false;
				array2[3] = false;
				array3[0] = false;
				array3[1] = false;
				array3[2] = false;
				array3[3] = false;
				IntVec3 intVec = new IntVec3(i, 0, j);
				IntVec3[] cardinalDirectionsAround = Gen.CardinalDirectionsAround;
				for (int k = 0; k < 4; k++)
				{
					IntVec3 sq = intVec + cardinalDirectionsAround[k];
					if (sq.InBounds())
					{
						thing = blockerGrid[sq.x, 0, sq.z];
						if (thing != null && thing.def.castEdgeShadows)
						{
							array2[k] = true;
							array[(k + 3) % 4] = true;
							array[k] = true;
						}
					}
				}
				IntVec3[] cornerDirectionsAround = Gen.CornerDirectionsAround;
				for (int l = 0; l < 4; l++)
				{
					if (array[l])
					{
						continue;
					}
					IntVec3 sq = intVec + cornerDirectionsAround[l];
					if (sq.InBounds())
					{
						thing = blockerGrid[sq.x, 0, sq.z];
						if (thing != null && thing.def.castEdgeShadows)
						{
							array[l] = true;
							array3[l] = true;
						}
					}
				}
				Action<int> action = delegate(int idx)
				{
					trianglesList.Add(vertsList.Count - 2);
					trianglesList.Add(idx);
					trianglesList.Add(vertsList.Count - 1);
					trianglesList.Add(vertsList.Count - 1);
					trianglesList.Add(idx);
					trianglesList.Add(idx + 1);
				};
				Action action2 = delegate
				{
					colorsList.Add(Shadowed);
					colorsList.Add(Lit);
					colorsList.Add(Lit);
					trianglesList.Add(vertsList.Count - 3);
					trianglesList.Add(vertsList.Count - 2);
					trianglesList.Add(vertsList.Count - 1);
				};
				int count = vertsList.Count;
				if (array[0])
				{
					if (array2[0] || array2[1])
					{
						num = (num2 = 0f);
						if (array2[0])
						{
							num2 = 0.45f;
						}
						if (array2[1])
						{
							num = 0.45f;
						}
						vertsList.Add(new Vector3(i, y, j));
						colorsList.Add(Shadowed);
						vertsList.Add(new Vector3((float)i + num, y, (float)j + num2));
						colorsList.Add(Lit);
						if (array[1] && !array3[1])
						{
							action(vertsList.Count);
						}
					}
					else
					{
						vertsList.Add(new Vector3(i, y, j));
						vertsList.Add(new Vector3(i, y, (float)j + 0.45f));
						vertsList.Add(new Vector3((float)i + 0.45f, y, j));
						action2();
					}
				}
				if (array[1])
				{
					if (array2[1] || array2[2])
					{
						num = (num2 = 0f);
						if (array2[1])
						{
							num = 0.45f;
						}
						if (array2[2])
						{
							num2 = -0.45f;
						}
						vertsList.Add(new Vector3(i, y, j + 1));
						colorsList.Add(Shadowed);
						vertsList.Add(new Vector3((float)i + num, y, (float)(j + 1) + num2));
						colorsList.Add(Lit);
						if (array[2] && !array3[2])
						{
							action(vertsList.Count);
						}
					}
					else
					{
						vertsList.Add(new Vector3(i, y, j + 1));
						vertsList.Add(new Vector3((float)i + 0.45f, y, j + 1));
						vertsList.Add(new Vector3(i, y, (float)(j + 1) - 0.45f));
						action2();
					}
				}
				if (array[2])
				{
					if (array2[2] || array2[3])
					{
						num = (num2 = 0f);
						if (array2[2])
						{
							num2 = -0.45f;
						}
						if (array2[3])
						{
							num = -0.45f;
						}
						vertsList.Add(new Vector3(i + 1, y, j + 1));
						colorsList.Add(Shadowed);
						vertsList.Add(new Vector3((float)(i + 1) + num, y, (float)(j + 1) + num2));
						colorsList.Add(Lit);
						if (array[3] && !array3[3])
						{
							action(vertsList.Count);
						}
					}
					else
					{
						vertsList.Add(new Vector3(i + 1, y, j + 1));
						vertsList.Add(new Vector3(i + 1, y, (float)(j + 1) - 0.45f));
						vertsList.Add(new Vector3((float)(i + 1) - 0.45f, y, j + 1));
						action2();
					}
				}
				if (!array[3])
				{
					continue;
				}
				if (array2[3] || array2[0])
				{
					num = (num2 = 0f);
					if (array2[3])
					{
						num = -0.45f;
					}
					if (array2[0])
					{
						num2 = 0.45f;
					}
					vertsList.Add(new Vector3(i + 1, y, j));
					colorsList.Add(Shadowed);
					vertsList.Add(new Vector3((float)(i + 1) + num, y, (float)j + num2));
					colorsList.Add(Lit);
					if (array[0] && !array3[0])
					{
						action(count);
					}
				}
				else
				{
					vertsList.Add(new Vector3(i + 1, y, j));
					vertsList.Add(new Vector3((float)(i + 1) - 0.45f, y, j));
					vertsList.Add(new Vector3(i + 1, y, (float)j + 0.45f));
					action2();
				}
			}
		}
		ResetLayerMesh();
		layerMesh.vertices = vertsList.ToArray();
		layerMesh.colors32 = colorsList.ToArray();
		layerMesh.triangles = trianglesList.ToArray();
	}
}
