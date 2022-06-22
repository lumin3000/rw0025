using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public static class Gen
{
	private const int RadialPatternCount = 5000;

	public static int[,] EightWayRoundPattern;

	public static IntVec3[] ManualRadialPattern;

	public static IntVec3[] RadialPattern;

	private static float[] RadialPatternRadii;

	public static IntVec3[] CardinalDirections;

	public static IntVec3[] CardinalDirectionsAround;

	public static IntVec3[] CornerDirections;

	public static IntVec3[] CornerDirectionsAround;

	public static IntVec3[] AdjacentSquares;

	public static IntVec3[] AdjacentSquaresAndInside;

	public static IntVec3[] AdjacentSquaresAround;

	public static IntVec3[] AdjacentSquaresAroundBottom;

	public static float[] SquareRootsByTenths;

	public static IEnumerable<IntVec3> AdjacentSquares8WayRandomized
	{
		get
		{
			List<int> selectorList = new List<int>();
			for (int j = 0; j < 8; j++)
			{
				selectorList.Add(j);
			}
			selectorList.Shuffle();
			for (int i = 0; i < 8; i++)
			{
				yield return AdjacentSquares[selectorList[i]];
			}
		}
	}

	static Gen()
	{
		EightWayRoundPattern = new int[8, 2]
		{
			{ 0, -1 },
			{ 0, 1 },
			{ 1, 0 },
			{ -1, 0 },
			{ 1, -1 },
			{ 1, 1 },
			{ -1, 1 },
			{ -1, -1 }
		};
		ManualRadialPattern = new IntVec3[49];
		RadialPattern = new IntVec3[5000];
		RadialPatternRadii = new float[5000];
		CardinalDirections = new IntVec3[4];
		CardinalDirectionsAround = new IntVec3[4];
		CornerDirections = new IntVec3[4];
		CornerDirectionsAround = new IntVec3[4];
		AdjacentSquares = new IntVec3[8];
		AdjacentSquaresAndInside = new IntVec3[9];
		AdjacentSquaresAround = new IntVec3[8];
		AdjacentSquaresAroundBottom = new IntVec3[9];
		SquareRootsByTenths = new float[25000];
		SetupAdjacencyTables();
		SetupManualRadialPattern();
		SetupRadialPattern();
		SetupSquareRootLookupTable();
	}

	private static void SetupAdjacencyTables()
	{
		ref IntVec3 reference = ref CardinalDirections[0];
		reference = new IntVec3(0, 0, 1);
		ref IntVec3 reference2 = ref CardinalDirections[1];
		reference2 = new IntVec3(1, 0, 0);
		ref IntVec3 reference3 = ref CardinalDirections[2];
		reference3 = new IntVec3(0, 0, -1);
		ref IntVec3 reference4 = ref CardinalDirections[3];
		reference4 = new IntVec3(-1, 0, 0);
		ref IntVec3 reference5 = ref CardinalDirectionsAround[0];
		reference5 = new IntVec3(0, 0, -1);
		ref IntVec3 reference6 = ref CardinalDirectionsAround[1];
		reference6 = new IntVec3(-1, 0, 0);
		ref IntVec3 reference7 = ref CardinalDirectionsAround[2];
		reference7 = new IntVec3(0, 0, 1);
		ref IntVec3 reference8 = ref CardinalDirectionsAround[3];
		reference8 = new IntVec3(1, 0, 0);
		ref IntVec3 reference9 = ref CornerDirections[0];
		reference9 = new IntVec3(-1, 0, -1);
		ref IntVec3 reference10 = ref CornerDirections[1];
		reference10 = new IntVec3(-1, 0, 1);
		ref IntVec3 reference11 = ref CornerDirections[2];
		reference11 = new IntVec3(1, 0, 1);
		ref IntVec3 reference12 = ref CornerDirections[3];
		reference12 = new IntVec3(1, 0, -1);
		ref IntVec3 reference13 = ref CornerDirectionsAround[0];
		reference13 = new IntVec3(-1, 0, -1);
		ref IntVec3 reference14 = ref CornerDirectionsAround[1];
		reference14 = new IntVec3(-1, 0, 1);
		ref IntVec3 reference15 = ref CornerDirectionsAround[2];
		reference15 = new IntVec3(1, 0, 1);
		ref IntVec3 reference16 = ref CornerDirectionsAround[3];
		reference16 = new IntVec3(1, 0, -1);
		ref IntVec3 reference17 = ref AdjacentSquares[0];
		reference17 = new IntVec3(0, 0, 1);
		ref IntVec3 reference18 = ref AdjacentSquares[1];
		reference18 = new IntVec3(1, 0, 0);
		ref IntVec3 reference19 = ref AdjacentSquares[2];
		reference19 = new IntVec3(0, 0, -1);
		ref IntVec3 reference20 = ref AdjacentSquares[3];
		reference20 = new IntVec3(-1, 0, 0);
		ref IntVec3 reference21 = ref AdjacentSquares[4];
		reference21 = new IntVec3(1, 0, -1);
		ref IntVec3 reference22 = ref AdjacentSquares[5];
		reference22 = new IntVec3(1, 0, 1);
		ref IntVec3 reference23 = ref AdjacentSquares[6];
		reference23 = new IntVec3(-1, 0, 1);
		ref IntVec3 reference24 = ref AdjacentSquares[7];
		reference24 = new IntVec3(-1, 0, -1);
		ref IntVec3 reference25 = ref AdjacentSquaresAndInside[0];
		reference25 = new IntVec3(0, 0, 1);
		ref IntVec3 reference26 = ref AdjacentSquaresAndInside[1];
		reference26 = new IntVec3(1, 0, 0);
		ref IntVec3 reference27 = ref AdjacentSquaresAndInside[2];
		reference27 = new IntVec3(0, 0, -1);
		ref IntVec3 reference28 = ref AdjacentSquaresAndInside[3];
		reference28 = new IntVec3(-1, 0, 0);
		ref IntVec3 reference29 = ref AdjacentSquaresAndInside[4];
		reference29 = new IntVec3(1, 0, -1);
		ref IntVec3 reference30 = ref AdjacentSquaresAndInside[5];
		reference30 = new IntVec3(1, 0, 1);
		ref IntVec3 reference31 = ref AdjacentSquaresAndInside[6];
		reference31 = new IntVec3(-1, 0, 1);
		ref IntVec3 reference32 = ref AdjacentSquaresAndInside[7];
		reference32 = new IntVec3(-1, 0, -1);
		ref IntVec3 reference33 = ref AdjacentSquaresAndInside[8];
		reference33 = new IntVec3(0, 0, 0);
		ref IntVec3 reference34 = ref AdjacentSquaresAround[0];
		reference34 = new IntVec3(0, 0, 1);
		ref IntVec3 reference35 = ref AdjacentSquaresAround[1];
		reference35 = new IntVec3(1, 0, 1);
		ref IntVec3 reference36 = ref AdjacentSquaresAround[2];
		reference36 = new IntVec3(1, 0, 0);
		ref IntVec3 reference37 = ref AdjacentSquaresAround[3];
		reference37 = new IntVec3(1, 0, -1);
		ref IntVec3 reference38 = ref AdjacentSquaresAround[4];
		reference38 = new IntVec3(0, 0, -1);
		ref IntVec3 reference39 = ref AdjacentSquaresAround[5];
		reference39 = new IntVec3(-1, 0, -1);
		ref IntVec3 reference40 = ref AdjacentSquaresAround[6];
		reference40 = new IntVec3(-1, 0, 0);
		ref IntVec3 reference41 = ref AdjacentSquaresAround[7];
		reference41 = new IntVec3(-1, 0, 1);
		ref IntVec3 reference42 = ref AdjacentSquaresAroundBottom[0];
		reference42 = new IntVec3(0, 0, -1);
		ref IntVec3 reference43 = ref AdjacentSquaresAroundBottom[1];
		reference43 = new IntVec3(-1, 0, -1);
		ref IntVec3 reference44 = ref AdjacentSquaresAroundBottom[2];
		reference44 = new IntVec3(-1, 0, 0);
		ref IntVec3 reference45 = ref AdjacentSquaresAroundBottom[3];
		reference45 = new IntVec3(-1, 0, 1);
		ref IntVec3 reference46 = ref AdjacentSquaresAroundBottom[4];
		reference46 = new IntVec3(0, 0, 1);
		ref IntVec3 reference47 = ref AdjacentSquaresAroundBottom[5];
		reference47 = new IntVec3(1, 0, 1);
		ref IntVec3 reference48 = ref AdjacentSquaresAroundBottom[6];
		reference48 = new IntVec3(1, 0, 0);
		ref IntVec3 reference49 = ref AdjacentSquaresAroundBottom[7];
		reference49 = new IntVec3(1, 0, -1);
		ref IntVec3 reference50 = ref AdjacentSquaresAroundBottom[8];
		reference50 = new IntVec3(0, 0, 0);
	}

	private static void SetupManualRadialPattern()
	{
		ref IntVec3 reference = ref ManualRadialPattern[0];
		reference = new IntVec3(0, 0, 0);
		ref IntVec3 reference2 = ref ManualRadialPattern[1];
		reference2 = new IntVec3(0, 0, -1);
		ref IntVec3 reference3 = ref ManualRadialPattern[2];
		reference3 = new IntVec3(1, 0, 0);
		ref IntVec3 reference4 = ref ManualRadialPattern[3];
		reference4 = new IntVec3(0, 0, 1);
		ref IntVec3 reference5 = ref ManualRadialPattern[4];
		reference5 = new IntVec3(-1, 0, 0);
		ref IntVec3 reference6 = ref ManualRadialPattern[5];
		reference6 = new IntVec3(1, 0, -1);
		ref IntVec3 reference7 = ref ManualRadialPattern[6];
		reference7 = new IntVec3(1, 0, 1);
		ref IntVec3 reference8 = ref ManualRadialPattern[7];
		reference8 = new IntVec3(-1, 0, 1);
		ref IntVec3 reference9 = ref ManualRadialPattern[8];
		reference9 = new IntVec3(-1, 0, -1);
		ref IntVec3 reference10 = ref ManualRadialPattern[9];
		reference10 = new IntVec3(2, 0, 0);
		ref IntVec3 reference11 = ref ManualRadialPattern[10];
		reference11 = new IntVec3(-2, 0, 0);
		ref IntVec3 reference12 = ref ManualRadialPattern[11];
		reference12 = new IntVec3(0, 0, 2);
		ref IntVec3 reference13 = ref ManualRadialPattern[12];
		reference13 = new IntVec3(0, 0, -2);
		ref IntVec3 reference14 = ref ManualRadialPattern[13];
		reference14 = new IntVec3(2, 0, 1);
		ref IntVec3 reference15 = ref ManualRadialPattern[14];
		reference15 = new IntVec3(2, 0, -1);
		ref IntVec3 reference16 = ref ManualRadialPattern[15];
		reference16 = new IntVec3(-2, 0, 1);
		ref IntVec3 reference17 = ref ManualRadialPattern[16];
		reference17 = new IntVec3(-2, 0, -1);
		ref IntVec3 reference18 = ref ManualRadialPattern[17];
		reference18 = new IntVec3(-1, 0, 2);
		ref IntVec3 reference19 = ref ManualRadialPattern[18];
		reference19 = new IntVec3(1, 0, 2);
		ref IntVec3 reference20 = ref ManualRadialPattern[19];
		reference20 = new IntVec3(-1, 0, -2);
		ref IntVec3 reference21 = ref ManualRadialPattern[20];
		reference21 = new IntVec3(1, 0, -2);
		ref IntVec3 reference22 = ref ManualRadialPattern[21];
		reference22 = new IntVec3(2, 0, 2);
		ref IntVec3 reference23 = ref ManualRadialPattern[22];
		reference23 = new IntVec3(-2, 0, -2);
		ref IntVec3 reference24 = ref ManualRadialPattern[23];
		reference24 = new IntVec3(2, 0, -2);
		ref IntVec3 reference25 = ref ManualRadialPattern[24];
		reference25 = new IntVec3(-2, 0, 2);
		ref IntVec3 reference26 = ref ManualRadialPattern[25];
		reference26 = new IntVec3(3, 0, 0);
		ref IntVec3 reference27 = ref ManualRadialPattern[26];
		reference27 = new IntVec3(0, 0, 3);
		ref IntVec3 reference28 = ref ManualRadialPattern[27];
		reference28 = new IntVec3(-3, 0, 0);
		ref IntVec3 reference29 = ref ManualRadialPattern[28];
		reference29 = new IntVec3(0, 0, -3);
		ref IntVec3 reference30 = ref ManualRadialPattern[29];
		reference30 = new IntVec3(3, 0, 1);
		ref IntVec3 reference31 = ref ManualRadialPattern[30];
		reference31 = new IntVec3(-3, 0, -1);
		ref IntVec3 reference32 = ref ManualRadialPattern[31];
		reference32 = new IntVec3(1, 0, 3);
		ref IntVec3 reference33 = ref ManualRadialPattern[32];
		reference33 = new IntVec3(-1, 0, -3);
		ref IntVec3 reference34 = ref ManualRadialPattern[33];
		reference34 = new IntVec3(-3, 0, 1);
		ref IntVec3 reference35 = ref ManualRadialPattern[34];
		reference35 = new IntVec3(3, 0, -1);
		ref IntVec3 reference36 = ref ManualRadialPattern[35];
		reference36 = new IntVec3(-1, 0, 3);
		ref IntVec3 reference37 = ref ManualRadialPattern[36];
		reference37 = new IntVec3(1, 0, -3);
		ref IntVec3 reference38 = ref ManualRadialPattern[37];
		reference38 = new IntVec3(3, 0, 2);
		ref IntVec3 reference39 = ref ManualRadialPattern[38];
		reference39 = new IntVec3(-3, 0, -2);
		ref IntVec3 reference40 = ref ManualRadialPattern[39];
		reference40 = new IntVec3(2, 0, 3);
		ref IntVec3 reference41 = ref ManualRadialPattern[40];
		reference41 = new IntVec3(-2, 0, -3);
		ref IntVec3 reference42 = ref ManualRadialPattern[41];
		reference42 = new IntVec3(-3, 0, 2);
		ref IntVec3 reference43 = ref ManualRadialPattern[42];
		reference43 = new IntVec3(3, 0, -2);
		ref IntVec3 reference44 = ref ManualRadialPattern[43];
		reference44 = new IntVec3(-2, 0, 3);
		ref IntVec3 reference45 = ref ManualRadialPattern[44];
		reference45 = new IntVec3(2, 0, -3);
		ref IntVec3 reference46 = ref ManualRadialPattern[45];
		reference46 = new IntVec3(3, 0, 3);
		ref IntVec3 reference47 = ref ManualRadialPattern[46];
		reference47 = new IntVec3(3, 0, -3);
		ref IntVec3 reference48 = ref ManualRadialPattern[47];
		reference48 = new IntVec3(-3, 0, 3);
		ref IntVec3 reference49 = ref ManualRadialPattern[48];
		reference49 = new IntVec3(-3, 0, -3);
	}

	private static void SetupRadialPattern()
	{
		List<IntVec3> list = new List<IntVec3>();
		for (int i = -40; i < 40; i++)
		{
			for (int j = -40; j < 40; j++)
			{
				list.Add(new IntVec3(i, 0, j));
			}
		}
		list.Sort(delegate(IntVec3 A, IntVec3 B)
		{
			float lengthHorizontalSquared = A.LengthHorizontalSquared;
			float lengthHorizontalSquared2 = B.LengthHorizontalSquared;
			if (lengthHorizontalSquared < lengthHorizontalSquared2)
			{
				return -1;
			}
			return (lengthHorizontalSquared != lengthHorizontalSquared2) ? 1 : 0;
		});
		for (int k = 0; k < 5000; k++)
		{
			ref IntVec3 reference = ref RadialPattern[k];
			reference = list[k];
			RadialPatternRadii[k] = list[k].LengthHorizontal;
		}
	}

	private static void SetupSquareRootLookupTable()
	{
		for (int i = 0; i < SquareRootsByTenths.Length; i++)
		{
			SquareRootsByTenths[i] = (float)Math.Sqrt((float)i * 0.1f);
		}
	}

	public static int NumSquaresToFillForRadius_ManualRadialPattern(int radius)
	{
		switch (radius)
		{
		case 0:
			return 1;
		case 1:
			return 9;
		case 2:
			return 21;
		case 3:
			return 37;
		default:
			Debug.LogError("NumSquares radius error");
			return 0;
		}
	}

	public static int NumSquaresInRadius(float radius)
	{
		for (int i = 0; i < 5000; i++)
		{
			if (RadialPatternRadii[i] > radius + float.Epsilon)
			{
				return i;
			}
		}
		Debug.LogError("Not enough squares to get to radius " + radius + ". Max is " + RadialPatternRadii[RadialPatternRadii.Length - 1]);
		return 5000;
	}

	public static float RadiusOfNumSquares(int numSquares)
	{
		return RadialPatternRadii[numSquares];
	}

	public static List<IntVec3> CombineGroupedSquares(List<IntVec3> LocList)
	{
		List<IntVec3> list = new List<IntVec3>();
		while (LocList.Count > 0)
		{
			IntVec3 item = LocList[0];
			List<IntVec3> list2 = new List<IntVec3>();
			list2.Add(item);
			int count;
			do
			{
				count = list2.Count;
				foreach (IntVec3 item2 in list2.ListFullCopy())
				{
					foreach (IntVec3 item3 in item2.AdjacentSquares8Way())
					{
						if (LocList.Contains(item3) && !list2.Contains(item3))
						{
							list2.Add(item3);
						}
					}
				}
			}
			while (list2.Count != count);
			list.Add(item);
			foreach (IntVec3 item4 in list2)
			{
				LocList.Remove(item4);
			}
		}
		return list;
	}

	public static bool WithinBoxRadius(IntVec3 A, IntVec3 B, float BoxRad)
	{
		int num = A.x - B.x;
		int num2 = A.z - B.z;
		if ((float)num > BoxRad || (float)num < 0f - BoxRad || (float)num2 > BoxRad || (float)num2 < 0f - BoxRad)
		{
			return false;
		}
		return true;
	}

	public static bool IsOnEdgeOfMap(this IntVec3 pos)
	{
		return pos.x == 0 || pos.x == Find.Map.Size.x - 1 || pos.z == 0 || pos.z == Find.Map.Size.z - 1;
	}

	public static IntVec3 NeighborAtAngle(float angle)
	{
		if (angle < 0f || angle > 360f)
		{
			Debug.LogError("Angle out of range: + " + angle);
		}
		if (angle < 22.5f || angle > 337.5f)
		{
			return AdjacentSquaresAround[0];
		}
		return AdjacentSquaresAround[(int)Math.Floor((angle - 22.5f) / 45f) + 1];
	}

	public static float AngleReversed(float angle)
	{
		if (angle < 0f || angle > 360f)
		{
			Debug.LogError("Angle out of range: + " + angle);
		}
		angle += 180f;
		if (angle > 360f)
		{
			angle -= 360f;
		}
		return angle;
	}

	public static Vector3 TrueCenter(this Thing t)
	{
		return TrueCenter(t.Position, t.rotation, t.def.size, t.def.altitude);
	}

	public static Vector3 TrueCenter(IntVec3 loc, IntRot rotation, IntVec2 thingSize, float altitude)
	{
		Vector3 result = loc.ToVector3ShiftedWithAltitude(altitude);
		if (thingSize.x != 1 || thingSize.z != 1)
		{
			if (rotation.IsHorizontal)
			{
				int x = thingSize.x;
				thingSize.x = thingSize.z;
				thingSize.z = x;
			}
			switch (rotation.AsInt)
			{
			case 0:
				if (thingSize.x % 2 == 0)
				{
					result.x += 0.5f;
				}
				if (thingSize.z % 2 == 0)
				{
					result.z += 0.5f;
				}
				break;
			case 1:
				if (thingSize.x % 2 == 0)
				{
					result.x += 0.5f;
				}
				if (thingSize.z % 2 == 0)
				{
					result.z -= 0.5f;
				}
				break;
			case 2:
				if (thingSize.x % 2 == 0)
				{
					result.x -= 0.5f;
				}
				if (thingSize.z % 2 == 0)
				{
					result.z -= 0.5f;
				}
				break;
			case 3:
				if (thingSize.x % 2 == 0)
				{
					result.x -= 0.5f;
				}
				if (thingSize.z % 2 == 0)
				{
					result.z += 0.5f;
				}
				break;
			}
		}
		return result;
	}

	public static IntVec3 RandomAdjSquare8Way(this IntVec3 root)
	{
		return root + AdjacentSquares[UnityEngine.Random.Range(0, 8)];
	}

	public static IntVec3 RandomAdjSquareCardinal(this IntVec3 root)
	{
		return root + CardinalDirections[UnityEngine.Random.Range(0, 4)];
	}

	private static void AdjustForRotation(ref IntVec3 Loc, ref IntVec2 ThingSize, IntRot Rotation)
	{
		if (Rotation.IsHorizontal)
		{
			int x = ThingSize.x;
			ThingSize.x = ThingSize.z;
			ThingSize.z = x;
		}
		switch (Rotation.AsInt)
		{
		case 0:
			break;
		case 1:
			if (ThingSize.z % 2 == 0)
			{
				Loc.z--;
			}
			break;
		case 2:
			if (ThingSize.x % 2 == 0)
			{
				Loc.x--;
			}
			if (ThingSize.z % 2 == 0)
			{
				Loc.z--;
			}
			break;
		case 3:
			if (ThingSize.x % 2 == 0)
			{
				Loc.x--;
			}
			break;
		}
	}

	public static IEnumerable<IntVec3> SquaresOccupiedBy(Thing t)
	{
		return SquaresOccupiedBy(t.Position, t.rotation, t.def.size);
	}

	public static IEnumerable<IntVec3> SquaresOccupiedBy(IntVec3 ThingCent, IntRot ThingRot, IntVec2 ThingSize)
	{
		AdjustForRotation(ref ThingCent, ref ThingSize, ThingRot);
		int minX = ThingCent.x - (ThingSize.x - 1) / 2;
		int minZ = ThingCent.z - (ThingSize.z - 1) / 2;
		int maxX = minX + ThingSize.x - 1;
		int maxZ = minZ + ThingSize.z - 1;
		for (int i = minX; i <= maxX; i++)
		{
			for (int j = minZ; j <= maxZ; j++)
			{
				yield return new IntVec3(i, 0, j);
			}
		}
	}

	public static IEnumerable<IntVec3> AdjacentSquaresCardinal(Thing t)
	{
		return AdjacentSquaresCardinal(t.Position, t.rotation, t.def.size);
	}

	public static IEnumerable<IntVec3> AdjacentSquaresCardinal(IntVec3 ThingCent, IntRot ThingRot, IntVec2 ThingSize)
	{
		AdjustForRotation(ref ThingCent, ref ThingSize, ThingRot);
		int minX = ThingCent.x - (ThingSize.x - 1) / 2 - 1;
		int minZ = ThingCent.z - (ThingSize.z - 1) / 2 - 1;
		int maxX = minX + ThingSize.x + 1;
		int maxZ = minZ + ThingSize.z + 1;
		for (int i = minX; i <= maxX; i++)
		{
			for (int j = minZ; j <= maxZ; j++)
			{
				if ((i == minX || i == maxX || j == minZ || j == maxZ) && (i != minX || j != minZ) && (i != minX || j != maxZ) && (i != maxX || j != maxZ) && (i != maxX || j != minZ))
				{
					yield return new IntVec3(i, 0, j);
				}
			}
		}
	}

	public static IEnumerable<IntVec3> AdjacentSquaresAlongEdge(IntVec3 thingCent, IntRot thingRot, IntVec2 thingSize, LinkDirections dir)
	{
		AdjustForRotation(ref thingCent, ref thingSize, thingRot);
		int minX = thingCent.x - (thingSize.x - 1) / 2 - 1;
		int minZ = thingCent.z - (thingSize.z - 1) / 2 - 1;
		int maxX = minX + thingSize.x + 1;
		int maxZ = minZ + thingSize.z + 1;
		if (dir == LinkDirections.Down)
		{
			for (int x2 = minX; x2 <= maxX; x2++)
			{
				yield return new IntVec3(x2, thingCent.y, minZ - 1);
			}
		}
		if (dir == LinkDirections.Up)
		{
			for (int x = minX; x <= maxX; x++)
			{
				yield return new IntVec3(x, thingCent.y, maxZ + 1);
			}
		}
		if (dir == LinkDirections.Left)
		{
			for (int z2 = minZ; z2 <= maxZ; z2++)
			{
				yield return new IntVec3(minX - 1, thingCent.y, z2);
			}
		}
		if (dir == LinkDirections.Right)
		{
			for (int z = minZ; z <= maxZ; z++)
			{
				yield return new IntVec3(maxX + 1, thingCent.y, z);
			}
		}
	}

	public static IEnumerable<IntVec3> AdjacentSquares8Way(Thing t)
	{
		return AdjacentSquares8Way(t.Position, t.rotation, t.def.size);
	}

	public static IEnumerable<IntVec3> AdjacentSquares8Way(IntVec3 ThingCent, IntRot ThingRot, IntVec2 ThingSize)
	{
		AdjustForRotation(ref ThingCent, ref ThingSize, ThingRot);
		int MinX = ThingCent.x - (ThingSize.x - 1) / 2 - 1;
		int MinZ = ThingCent.z - (ThingSize.z - 1) / 2 - 1;
		int MaxX = MinX + ThingSize.x + 1;
		int MaxZ = MinZ + ThingSize.z + 1;
		for (int i = MinX; i <= MaxX; i++)
		{
			for (int j = MinZ; j <= MaxZ; j++)
			{
				if (i == MinX || i == MaxX || j == MinZ || j == MaxZ)
				{
					yield return new IntVec3(i, 0, j);
				}
			}
		}
	}

	public static bool AdjacentTo8Way(this IntVec3 loc, Thing t)
	{
		return loc.AdjacentTo8Way(t.Position, t.rotation, t.def.size);
	}

	public static bool AdjacentTo8Way(this IntVec3 loc, IntVec3 ThingCent, IntRot ThingRot, IntVec2 ThingSize)
	{
		foreach (IntVec3 item in AdjacentSquares8Way(ThingCent, ThingRot, ThingSize))
		{
			if (item == loc)
			{
				return true;
			}
		}
		return false;
	}

	public static bool AdjacentTo8WayOrInside(this IntVec3 loc, Thing t)
	{
		return loc.AdjacentTo8WayOrInside(t.Position, t.rotation, t.def.size);
	}

	public static bool AdjacentTo8WayOrInside(this IntVec3 Loc, IntVec3 ThingCent, IntRot ThingRot, IntVec2 ThingSize)
	{
		AdjustForRotation(ref ThingCent, ref ThingSize, ThingRot);
		int num = ThingCent.x - (ThingSize.x - 1) / 2 - 1;
		int num2 = ThingCent.z - (ThingSize.z - 1) / 2 - 1;
		int num3 = num + ThingSize.x + 1;
		int num4 = num2 + ThingSize.z + 1;
		if (Loc.x >= num && Loc.x <= num3 && Loc.z >= num2 && Loc.z <= num4)
		{
			return true;
		}
		return false;
	}

	public static bool IsInside(this IntVec3 Loc, Thing t)
	{
		return Loc.IsInside(t.Position, t.rotation, t.def.size);
	}

	public static bool IsInside(this IntVec3 Loc, IntVec3 ThingCent, IntRot ThingRot, IntVec2 ThingSize)
	{
		AdjustForRotation(ref ThingCent, ref ThingSize, ThingRot);
		int num = ThingCent.x - (ThingSize.x - 1) / 2;
		int num2 = ThingCent.z - (ThingSize.z - 1) / 2;
		int num3 = num + ThingSize.x;
		int num4 = num2 + ThingSize.z;
		if (Loc.x >= num && Loc.x <= num3 && Loc.z >= num2 && Loc.z <= num4)
		{
			return true;
		}
		return false;
	}

	public static Vector3 RandomHorizontalVector(float max)
	{
		return new Vector3(UnityEngine.Random.Range(0f - max, max), 0f, UnityEngine.Random.Range(0f - max, max));
	}

	public static int GetBitCountOf(long lValue)
	{
		int num = 0;
		while (lValue != 0L)
		{
			lValue &= lValue - 1;
			num++;
		}
		return num;
	}

	public static IEnumerable<T> InRandomOrder<T>(this IEnumerable<T> source)
	{
		List<T> inList = source.ToList();
		int inListPos2 = 0;
		while (inList.Count > 0)
		{
			inListPos2 = UnityEngine.Random.Range(0, inList.Count);
			yield return inList[inListPos2];
			inList.RemoveAt(inListPos2);
		}
	}

	public static int RandomRoundToInt(float f)
	{
		int num = Mathf.FloorToInt(f);
		if (UnityEngine.Random.value < f % 1f)
		{
			num++;
		}
		return num;
	}

	public static T RandomElementByWeight<T>(this IEnumerable<T> sourceList, Func<T, float> weightSelector)
	{
		float num = sourceList.Sum(weightSelector);
		float num2 = UnityEngine.Random.value * num;
		float num3 = 0f;
		foreach (T source in sourceList)
		{
			num3 += weightSelector(source);
			if (num3 >= num2)
			{
				return source;
			}
		}
		return default(T);
	}

	public static T RandomElement<T>(this IEnumerable<T> sourceList)
	{
		return sourceList.ToList().RandomElement();
	}

	public static T RandomEnumValue<T>()
	{
		return Enum.GetValues(typeof(T)).Cast<T>().ToList()
			.RandomElement();
	}

	public static bool StacksWith(this Thing A, Thing B)
	{
		if (A.def != B.def)
		{
			return false;
		}
		if (A.def.stackLimit == 1 || B.def.stackLimit == 1)
		{
			return false;
		}
		ThingResource thingResource = A as ThingResource;
		ThingResource thingResource2 = B as ThingResource;
		if (thingResource != null || thingResource2 != null)
		{
			if (thingResource == null || thingResource2 == null)
			{
				return false;
			}
			if (thingResource.def.eType != thingResource2.def.eType)
			{
				return false;
			}
		}
		return true;
	}

	public static string SplitCamelCase(string Str)
	{
		return Regex.Replace(Str, "(?<a>(?<!^)((?:[A-Z][a-z])|(?:(?<!^[A-Z]+)[A-Z0-9]+(?:(?=[A-Z][a-z])|$))|(?:[0-9]+)))", " ${a}");
	}

	public static IEnumerable<Type> AllSubclasses(this Type baseType)
	{
		return from type in baseType.Assembly.GetTypes()
			where type.IsSubclassOf(baseType)
			select type;
	}

	public static IEnumerable<Type> AllLeafSubclasses(this Type baseType)
	{
		return from type in baseType.AllSubclasses()
			where !type.AllSubclasses().Any()
			select type;
	}

	public static Vector3 ScreenToWorldPoint(Vector2 screenLoc)
	{
		Ray ray = Find.CameraCurrent.ScreenPointToRay(screenLoc);
		Vector3 result = new Vector3(ray.origin.x, 0f, ray.origin.z);
		return result;
	}

	public static Vector3 InvertedWorldToScreen(Vector3 worldLoc)
	{
		return Find.CameraMap.InvertedWorldToScreenPoint(worldLoc);
	}

	public static Vector3 MouseWorldPosVector3()
	{
		return ScreenToWorldPoint(Input.mousePosition);
	}

	public static IntVec3 MouseWorldSquare()
	{
		return ScreenToWorldPoint(Input.mousePosition).ToIntVec3();
	}

	public static Vector2 RotatedBy(this Vector2 v, float angle)
	{
		Vector3 vector = new Vector3(v.x, 0f, v.y);
		vector = Quaternion.AngleAxis(angle, Vector3.up) * vector;
		return new Vector2(vector.x, vector.z);
	}

	public static Vector3 RotatedBy(this Vector3 v3, float angle)
	{
		return Quaternion.AngleAxis(angle, Vector3.up) * v3;
	}

	public static IntVec3 RotatedBy(this IntVec3 Orig, IntRot Rot)
	{
		return Rot.AsInt switch
		{
			0 => Orig, 
			1 => new IntVec3(Orig.z, Orig.y, -Orig.x), 
			2 => new IntVec3(-Orig.x, Orig.y, -Orig.z), 
			3 => new IntVec3(-Orig.z, Orig.y, Orig.x), 
			_ => Orig, 
		};
	}

	public static AudioSource PlayAudioClip(AudioClip clip, float volume, Vector3 Pos)
	{
		GameObject gameObject = new GameObject("aud");
		gameObject.transform.position = Pos;
		AudioSource audioSource = gameObject.AddComponent<AudioSource>();
		audioSource.clip = clip;
		audioSource.volume = volume;
		audioSource.Play();
		UnityEngine.Object.Destroy(gameObject, clip.length);
		return audioSource;
	}

	public static int ManhattanDistanceFlat(IntVec3 A, IntVec3 B)
	{
		return Math.Abs(A.x - B.x) + Math.Abs(A.z - B.z);
	}

	public static float AngleFlat(this Vector3 v)
	{
		if (v.x == 0f && v.z == 0f)
		{
			return 0f;
		}
		return Quaternion.LookRotation(v).eulerAngles.y;
	}

	public static Quaternion ToQuat(this float Ang)
	{
		return Quaternion.AngleAxis(Ang, Vector3.up);
	}

	public static int Clamp(int value, int min, int max)
	{
		return (value < min) ? min : ((value <= max) ? value : max);
	}
}
