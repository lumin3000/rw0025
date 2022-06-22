public class Genner_Sanctity
{
	protected const float SacrosanctZoneRadiusInner = 12f;

	protected const float SacrosanctZoneRadiusOuter = 35f;

	protected float[,] SanctityMap;

	public float SanctityAt(IntVec3 Loc)
	{
		return SanctityMap[Loc.x, Loc.z];
	}

	public void GenerateGenZones()
	{
		SanctityMap = new float[Find.Map.Size.x, Find.Map.Size.z];
		for (int i = 0; i < Find.Map.Size.x; i++)
		{
			for (int j = 0; j < Find.Map.Size.z; j++)
			{
				SanctityMap[i, j] = 0f;
			}
		}
		IntVec3 center = Find.Map.Center;
		for (int k = 0; k < Gen.NumSquaresInRadius(35f); k++)
		{
			IntVec3 intVec = center + Gen.RadialPattern[k];
			float lengthHorizontal = (intVec - center).LengthHorizontal;
			float num = 1f;
			if (lengthHorizontal > 12f)
			{
				num = 1f - (lengthHorizontal - 12f) / 23f;
			}
			SanctityMap[intVec.x, intVec.z] = num;
		}
	}
}
