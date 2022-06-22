using UnityEngine;

public class MapSectionLayer_FogOfWar : MapSectionLayer
{
	private const byte FogBrightness = 35;

	private bool[,] fogGrid;

	public MapSectionLayer_FogOfWar(MapSection section)
		: base(section)
	{
		layerMats.Add(MatBases.FogOfWar);
		relevantChangeTypes.Add(MapChangeType.FogOfWar);
	}

	public override void DrawLayer()
	{
		if (DebugSettings.drawFog)
		{
			base.DrawLayer();
		}
	}

	private void MakeBaseMesh()
	{
		layerMesh.Clear();
		fogGrid = Find.FogGrid.fogGrid;
		IntRect intRect = new IntRect(section.botLeft.x, section.botLeft.z, 17, 17);
		intRect.ClipInsideMap();
		int num = intRect.Area * 9;
		float y = Altitudes.AltitudeFor(AltitudeLayer.FogOfWar);
		Vector3[] array = new Vector3[num];
		int num2 = 0;
		for (int i = intRect.minX; i <= intRect.maxX; i++)
		{
			for (int j = intRect.minZ; j <= intRect.maxZ; j++)
			{
				ref Vector3 reference = ref array[num2];
				reference = new Vector3(i, y, j);
				ref Vector3 reference2 = ref array[num2 + 1];
				reference2 = new Vector3(i, y, (float)j + 0.5f);
				ref Vector3 reference3 = ref array[num2 + 2];
				reference3 = new Vector3(i, y, j + 1);
				ref Vector3 reference4 = ref array[num2 + 3];
				reference4 = new Vector3((float)i + 0.5f, y, j + 1);
				ref Vector3 reference5 = ref array[num2 + 4];
				reference5 = new Vector3(i + 1, y, j + 1);
				ref Vector3 reference6 = ref array[num2 + 5];
				reference6 = new Vector3(i + 1, y, (float)j + 0.5f);
				ref Vector3 reference7 = ref array[num2 + 6];
				reference7 = new Vector3(i + 1, y, j);
				ref Vector3 reference8 = ref array[num2 + 7];
				reference8 = new Vector3((float)i + 0.5f, y, j);
				ref Vector3 reference9 = ref array[num2 + 8];
				reference9 = new Vector3((float)i + 0.5f, y, (float)j + 0.5f);
				num2 += 9;
			}
		}
		for (int k = 0; k < array.Length; k++)
		{
			ref Vector3 reference10 = ref array[k];
			reference10.z = reference10.z;
		}
		layerMesh.vertices = array;
		int[] array2 = new int[intRect.Area * 8 * 3];
		int num3 = 0;
		int num4 = 0;
		while (num3 < array2.Length)
		{
			array2[num3++] = num4 + 7;
			array2[num3++] = num4;
			array2[num3++] = num4 + 1;
			array2[num3++] = num4 + 1;
			array2[num3++] = num4 + 2;
			array2[num3++] = num4 + 3;
			array2[num3++] = num4 + 3;
			array2[num3++] = num4 + 4;
			array2[num3++] = num4 + 5;
			array2[num3++] = num4 + 5;
			array2[num3++] = num4 + 6;
			array2[num3++] = num4 + 7;
			array2[num3++] = num4 + 7;
			array2[num3++] = num4 + 1;
			array2[num3++] = num4 + 8;
			array2[num3++] = num4 + 1;
			array2[num3++] = num4 + 3;
			array2[num3++] = num4 + 8;
			array2[num3++] = num4 + 3;
			array2[num3++] = num4 + 5;
			array2[num3++] = num4 + 8;
			array2[num3++] = num4 + 5;
			array2[num3++] = num4 + 7;
			array2[num3++] = num4 + 8;
			num4 += 9;
		}
		layerMesh.triangles = array2;
	}

	public override void RegenerateMesh()
	{
		if (!DebugSettings.drawFog)
		{
			return;
		}
		if (layerMesh.vertexCount == 0)
		{
			MakeBaseMesh();
		}
		IntRect mapRect = section.MapRect;
		int num = Find.Map.Size.z - 1;
		int num2 = Find.Map.Size.x - 1;
		bool[] array = new bool[9];
		Color32[] array2 = new Color32[layerMesh.vertexCount];
		int num3 = 0;
		bool flag = false;
		for (int i = mapRect.minX; i <= mapRect.maxX; i++)
		{
			for (int j = mapRect.minZ; j <= mapRect.maxZ; j++)
			{
				if (fogGrid[i, j])
				{
					for (int k = 0; k < 9; k++)
					{
						array[k] = true;
					}
				}
				else
				{
					for (int l = 0; l < 9; l++)
					{
						array[l] = false;
					}
					if (j < num && fogGrid[i, j + 1])
					{
						array[2] = true;
						array[3] = true;
						array[4] = true;
					}
					if (j > 0 && fogGrid[i, j - 1])
					{
						array[6] = true;
						array[7] = true;
						array[0] = true;
					}
					if (i < num2 && fogGrid[i + 1, j])
					{
						array[4] = true;
						array[5] = true;
						array[6] = true;
					}
					if (i > 0 && fogGrid[i - 1, j])
					{
						array[0] = true;
						array[1] = true;
						array[2] = true;
					}
					if (j > 0 && i > 0 && fogGrid[i - 1, j - 1])
					{
						array[0] = true;
					}
					if (j < num && i > 0 && fogGrid[i - 1, j + 1])
					{
						array[2] = true;
					}
					if (j < num && i < num2 && fogGrid[i + 1, j + 1])
					{
						array[4] = true;
					}
					if (j > 0 && i < num2 && fogGrid[i + 1, j - 1])
					{
						array[6] = true;
					}
				}
				for (int m = 0; m < 9; m++)
				{
					array2[num3 + m].r = byte.MaxValue;
					array2[num3 + m].g = byte.MaxValue;
					array2[num3 + m].b = byte.MaxValue;
					if (array[m])
					{
						array2[num3 + m].a = byte.MaxValue;
						flag = true;
					}
					else
					{
						array2[num3 + m].a = 0;
					}
				}
				num3 += 9;
			}
		}
		if (!flag)
		{
			layerMesh.Clear();
		}
		else
		{
			layerMesh.colors32 = array2;
		}
	}
}
