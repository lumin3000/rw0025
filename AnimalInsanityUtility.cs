public static class AnimalInsanityUtility
{
	public static float PointsPerAnimal(RaceDefinition animalDef)
	{
		float num = 10f;
		num += (float)animalDef.meleeDamage * 1.8f + (float)animalDef.baseMaxHealth * 0.23f;
		if (animalDef.raceName == "Boomrat")
		{
			num += 30f;
		}
		return num;
	}
}
