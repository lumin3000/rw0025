using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class MapSectionLayer_LightingOverlay : MapSectionLayer
{
	private const byte RoofedAreaMinSkyCover = 150;

	private static Color32[,,] glowGrid;

	private static EntityType[,] roofGrid;

	private int firstCenterIdx;

	private IntRect sectRect;

	private static readonly IntVec3[] CheckSquareOffsets = new IntVec3[4]
	{
		new IntVec3(0, 0, -1),
		new IntVec3(-1, 0, -1),
		new IntVec3(-1, 0, 0),
		new IntVec3(0, 0, 0)
	};

	public MapSectionLayer_LightingOverlay(MapSection section)
		: base(section)
	{
		layerMats = new List<Material>();
		layerMats.Add(MatBases.LightOverlay);
		relevantChangeTypes.Add(MapChangeType.GroundGlow);
	}

	public override void DrawLayer()
	{
		if (DebugSettings.drawLightingOverlay)
		{
			base.DrawLayer();
		}
	}

	private void MakeBaseMesh()
	{
		ResetLayerMesh();
		glowGrid = Find.GlowGrid.glowGrid;
		roofGrid = Find.RoofGrid.roofGrid;
		sectRect = new IntRect(section.botLeft.x, section.botLeft.z, 17, 17);
		sectRect.ClipInsideMap();
		int num = (sectRect.Width + 1) * (sectRect.Height + 1) + sectRect.Area;
		float y = Altitudes.AltitudeFor(AltitudeLayer.LightingOverlay);
		Vector3[] array = new Vector3[num];
		int num2 = 0;
		for (int i = sectRect.minZ; i <= sectRect.maxZ + 1; i++)
		{
			for (int j = sectRect.minX; j <= sectRect.maxX + 1; j++)
			{
				ref Vector3 reference = ref array[num2];
				reference = new Vector3(j, y, i);
				num2++;
			}
		}
		firstCenterIdx = num2;
		for (int k = sectRect.minZ; k <= sectRect.maxZ; k++)
		{
			for (int l = sectRect.minX; l <= sectRect.maxX; l++)
			{
				ref Vector3 reference2 = ref array[num2];
				reference2 = new Vector3((float)l + 0.5f, y, (float)k + 0.5f);
				num2++;
			}
		}
		layerMesh.vertices = array;
		int[] array2 = new int[sectRect.Area * 4 * 3];
		int num3 = 0;
		for (int m = sectRect.minZ; m <= sectRect.maxZ; m++)
		{
			for (int n = sectRect.minX; n <= sectRect.maxX; n++)
			{
				CalculateVertexIndices(n, m, out var botLeft, out var topLeft, out var topRight, out var botRight, out var center);
				array2[num3] = botLeft;
				array2[num3 + 1] = center;
				array2[num3 + 2] = botRight;
				array2[num3 + 3] = botLeft;
				array2[num3 + 4] = topLeft;
				array2[num3 + 5] = center;
				array2[num3 + 6] = topLeft;
				array2[num3 + 7] = topRight;
				array2[num3 + 8] = center;
				array2[num3 + 9] = topRight;
				array2[num3 + 10] = botRight;
				array2[num3 + 11] = center;
				num3 += 12;
			}
		}
		layerMesh.triangles = array2;
	}

	private void CalculateVertexIndices(int worldX, int worldZ, out int botLeft, out int topLeft, out int topRight, out int botRight, out int center)
	{
		int num = worldX - sectRect.minX;
		int num2 = worldZ - sectRect.minZ;
		botLeft = num2 * (sectRect.Width + 1) + num;
		topLeft = (num2 + 1) * (sectRect.Width + 1) + num;
		topRight = (num2 + 1) * (sectRect.Width + 1) + (num + 1);
		botRight = num2 * (sectRect.Width + 1) + (num + 1);
		center = firstCenterIdx + (num2 * sectRect.Width + num);
	}

	public override void RegenerateMesh()
	{
		if (DebugSettings.drawLightingOverlay)
		{
			if (layerMesh == null || layerMesh.vertexCount == 0)
			{
				MakeBaseMesh();
			}
			UpdateVertexColors();
		}
	}

	private void UpdateVertexColors()
	{
		Color32[] array = new Color32[layerMesh.vertexCount];
		int maxX = sectRect.maxX;
		int maxZ = sectRect.maxZ;
		bool[] array2 = new bool[4];
		Thing[,,] blockerGrid = Find.Grids.blockerGrid;
		for (int i = sectRect.minX; i <= maxX + 1; i++)
		{
			for (int j = sectRect.minZ; j <= maxZ + 1; j++)
			{
				CalculateVertexIndices(i, j, out var botLeft, out var _, out var _, out var _, out var _);
				IntVec3 intVec = new IntVec3(i, 0, j);
				bool flag = false;
				for (int k = 0; k < 4; k++)
				{
					IntVec3 sq = intVec + CheckSquareOffsets[k];
					if (!sq.InBounds())
					{
						array2[k] = true;
						continue;
					}
					Thing thing = blockerGrid[sq.x, 0, sq.z];
					if (roofGrid[sq.x, sq.z] != 0 && (thing == null || !thing.def.holdsRoof))
					{
						flag = true;
					}
					if (thing != null && thing.def.blockLight)
					{
						array2[k] = true;
					}
					else
					{
						array2[k] = false;
					}
				}
				ColorInt colorInt = new ColorInt(0, 0, 0, 0);
				int num = 0;
				if (!array2[0])
				{
					colorInt += glowGrid[i, 0, j - 1].AsColorInt();
					num++;
				}
				if (!array2[1])
				{
					colorInt += glowGrid[i - 1, 0, j - 1].AsColorInt();
					num++;
				}
				if (!array2[2])
				{
					colorInt += glowGrid[i - 1, 0, j].AsColorInt();
					num++;
				}
				if (!array2[3])
				{
					colorInt += glowGrid[i, 0, j].AsColorInt();
					num++;
				}
				if (num > 0)
				{
					colorInt /= (float)num;
					ref Color32 reference = ref array[botLeft];
					reference = colorInt.ToColor32;
				}
				else
				{
					ref Color32 reference2 = ref array[botLeft];
					reference2 = new Color32(0, 0, 0, 0);
				}
				if (flag && array[botLeft].a < 150)
				{
					array[botLeft].a = 150;
				}
			}
		}
		for (int l = sectRect.minX; l <= maxX; l++)
		{
			for (int m = sectRect.minZ; m <= maxZ; m++)
			{
				CalculateVertexIndices(l, m, out var botLeft2, out var topLeft2, out var topRight2, out var botRight2, out var center2);
				ColorInt colorInt2 = default(ColorInt);
				colorInt2 += array[botLeft2];
				colorInt2 += array[topLeft2];
				colorInt2 += array[topRight2];
				colorInt2 += array[botRight2];
				ref Color32 reference3 = ref array[center2];
				reference3 = (colorInt2 / 4f).ToColor32;
				Thing thing = blockerGrid[l, 0, m];
				if (roofGrid[l, m] != 0 && (thing == null || !thing.def.holdsRoof) && array[center2].a < 150)
				{
					array[center2].a = 150;
				}
			}
		}
		layerMesh.colors32 = array;
	}

	public string GlowReportAt(IntVec3 sq)
	{
		Color32[] colors = layerMesh.colors32;
		CalculateVertexIndices(sq.x, sq.z, out var botLeft, out var topLeft, out var topRight, out var botRight, out var center);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("BL=" + colors[botLeft]);
		stringBuilder.Append("\nTL=" + colors[topLeft]);
		stringBuilder.Append("\nTR=" + colors[topRight]);
		stringBuilder.Append("\nBR=" + colors[botRight]);
		stringBuilder.Append("\nCenter=" + colors[center]);
		return stringBuilder.ToString();
	}
}
