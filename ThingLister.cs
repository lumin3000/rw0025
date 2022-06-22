using System.Collections.Generic;

public class ThingLister
{
	public List<Thing> spawnedThings = new List<Thing>();

	public List<Thing> spawnedHaulables = new List<Thing>();

	public List<Thing> spawnedCultivatedPlants = new List<Thing>();

	public List<Thing> spawnedEdibles = new List<Thing>();

	public List<Thing> spawnedGUIOverlayThings = new List<Thing>();

	public List<Thing> spawnedCorpses = new List<Thing>();

	public List<Fire> spawnedFires = new List<Fire>();

	public void RegisterThingSpawned(Thing t)
	{
		spawnedThings.Add(t);
		if (t.def.alwaysHaulable)
		{
			spawnedHaulables.Add(t);
		}
		if (t.def.IsPlant && !t.def.plant.wild)
		{
			spawnedCultivatedPlants.Add(t);
		}
		if (t.def.Edible)
		{
			spawnedEdibles.Add(t);
		}
		if (t.def.drawGUIOverlay)
		{
			spawnedGUIOverlayThings.Add(t);
		}
		if (t.def.eType == EntityType.Corpse)
		{
			spawnedCorpses.Add(t);
		}
		Fire fire = t as Fire;
		if (fire != null)
		{
			spawnedFires.Add(fire);
		}
	}

	public void DeRegisterThingSpawned(Thing t)
	{
		spawnedThings.Remove(t);
		if (t.def.alwaysHaulable)
		{
			spawnedHaulables.Remove(t);
		}
		if (t.def.IsPlant && !t.def.plant.wild)
		{
			spawnedCultivatedPlants.Remove(t);
		}
		if (t.def.Edible)
		{
			spawnedEdibles.Remove(t);
		}
		if (t.def.drawGUIOverlay)
		{
			spawnedGUIOverlayThings.Remove(t);
		}
		if (t.def.eType == EntityType.Corpse)
		{
			spawnedCorpses.Remove(t);
		}
		Fire fire = t as Fire;
		if (fire != null)
		{
			spawnedFires.Remove(fire);
		}
	}
}
