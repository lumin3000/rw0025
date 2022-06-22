using System;
using System.Collections.Generic;
using System.Text;

public static class GridSaveUtility
{
	public class LoadedGridByte
	{
		public byte val;

		public IntVec3 pos;
	}

	private const int NewlineInterval = 100;

	public static void ExposeBoolGrid(ref bool[,] grid, string label)
	{
		string value = string.Empty;
		int num = (int)Math.Ceiling((float)(Find.Map.Size.x * Find.Map.Size.z) / 6f);
		byte[] array = new byte[num];
		if (Scribe.mode == LoadSaveMode.Saving)
		{
			int num2 = 0;
			byte b = 1;
			for (int i = 0; i < Find.Map.Size.z; i++)
			{
				for (int j = 0; j < Find.Map.Size.x; j++)
				{
					if (grid[j, i])
					{
						array[num2] |= b;
					}
					b = (byte)(b * 2);
					if (b > 32)
					{
						b = 1;
						num2++;
					}
				}
			}
			value = Convert.ToBase64String(array);
			value = AddLineBreaksTo(value);
		}
		Scribe.LookField(ref value, label);
		if (Scribe.mode != LoadSaveMode.LoadingVars)
		{
			return;
		}
		value.Replace("\n", string.Empty);
		array = Convert.FromBase64String(value);
		int num3 = 0;
		byte b2 = 1;
		for (int k = 0; k < Find.Map.Size.z; k++)
		{
			for (int l = 0; l < Find.Map.Size.x; l++)
			{
				if (grid == null)
				{
					grid = new bool[Find.Map.Size.x, Find.Map.Size.z];
				}
				grid[l, k] = (array[num3] & b2) != 0;
				b2 = (byte)(b2 * 2);
				if (b2 > 32)
				{
					b2 = 1;
					num3++;
				}
			}
		}
	}

	public static string CompressedStringForByteGrid(Func<IntVec3, byte> byteGetter)
	{
		int numSquaresOnMap = Find.Map.info.NumSquaresOnMap;
		byte[] array = new byte[numSquaresOnMap];
		IntVec3 arg = new IntVec3(0, 0, 0);
		for (int i = 0; i < numSquaresOnMap; i++)
		{
			array[i] = byteGetter(arg);
			arg.x++;
			if (arg.x >= Find.Map.Size.x)
			{
				arg.x = 0;
				arg.z++;
			}
		}
		string compressedString = Convert.ToBase64String(array);
		return AddLineBreaksTo(compressedString);
	}

	public static IEnumerable<LoadedGridByte> ThingsFromThingTypeGrid(string compressedString)
	{
		compressedString.Replace("\n", string.Empty);
		byte[] typesMap = Convert.FromBase64String(compressedString);
		IntVec3 curSq = new IntVec3(0, 0, 0);
		for (int i = 0; i < typesMap.Length; i++)
		{
			yield return new LoadedGridByte
			{
				val = typesMap[i],
				pos = curSq
			};
			curSq.x++;
			if (curSq.x >= Find.Map.Size.x)
			{
				curSq.x = 0;
				curSq.z++;
			}
		}
	}

	public static string AddLineBreaksTo(string compressedString)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine();
		for (int i = 0; i < compressedString.Length; i++)
		{
			stringBuilder.Append(compressedString[i]);
			if (i % 100 == 0)
			{
				stringBuilder.AppendLine();
			}
		}
		stringBuilder.AppendLine();
		return stringBuilder.ToString();
	}
}
