public class Incident_CropBlight : IncidentDefinition
{
	public Incident_CropBlight()
	{
		uniqueSaveKey = 871047;
		chance = 2.5f;
		global = true;
		favorability = IncidentFavorability.Bad;
	}

	public override bool TryExecute(IncidentParms parms)
	{
		bool flag = false;
		foreach (Plant spawnedCultivatedPlant in Find.Map.thingLister.spawnedCultivatedPlants)
		{
			if (spawnedCultivatedPlant.LifeStage == PlantLifeStage.Growing || spawnedCultivatedPlant.LifeStage == PlantLifeStage.Mature)
			{
				flag = true;
			}
		}
		if (!flag)
		{
			return false;
		}
		foreach (Plant item in Find.Map.thingLister.spawnedCultivatedPlants.ListFullCopy())
		{
			item.CropBlighted();
		}
		Find.LetterStack.ReceiveLetter("A mysterious blight has destroyed your crops.");
		return true;
	}
}
