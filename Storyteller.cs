using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public abstract class Storyteller : Saveable
{
	public string name = "Unnamed";

	public string description = "No description.";

	public string quotation = "No quotation.";

	public Texture2D portrait = Res.LoadTexture("UI/HeroArt/Storytellers/Default");

	public StoryWatcher watcher = new StoryWatcher();

	public AnimalSpawner animalSpawner = new AnimalSpawner();

	public IncidentQueue incidentQueue = new IncidentQueue();

	public WeatherDecider weatherDecider = new WeatherDecider();

	public StoryState storyState = new StoryState();

	public IncidentMaker incidentMaker;

	public StoryIntender_Population intenderPopulation = new StoryIntender_Population();

	public string DebugReadout
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Storyteller : " + name);
			stringBuilder.AppendLine(intenderPopulation.DebugReadout);
			stringBuilder.AppendLine("Event queue:");
			stringBuilder.AppendLine(incidentQueue.DebugQueueReadout);
			stringBuilder.AppendLine(watcher.DebugReadout);
			return stringBuilder.ToString();
		}
	}

	public void ExposeData()
	{
		Scribe.LookSaveable(ref animalSpawner, "AnimalSpawner");
		Scribe.LookSaveable(ref incidentQueue, "IncidentQueue");
		Scribe.LookSaveable(ref weatherDecider, "WeatherDecider");
		Scribe.LookSaveable(ref storyState, "StoryState");
		Scribe.LookSaveable(ref intenderPopulation, "IntenderPopulation");
	}

	public virtual void StorytellerTick()
	{
		watcher.StoryWatcherTick();
		animalSpawner.AnimalSpawnerTick();
		incidentQueue.IncidentManagerTick();
		weatherDecider.WeatherDeciderTick();
		incidentMaker.IncidentMakerTick();
	}

	public virtual void PrintTestStoryEvents()
	{
		throw new NotImplementedException();
	}

	public virtual Weather NextWeather()
	{
		List<Weather> list = WeatherDecider.AllWeathers.ToList();
		if (!Find.WeatherManager.curWeather.repeatable)
		{
			list.RemoveAll((Weather we) => we.GetType() == Find.WeatherManager.curWeather.GetType());
		}
		if (DateHandler.DaysPassed < 5)
		{
			list.RemoveAll((Weather we) => we.favorability < IncidentFavorability.Neutral);
		}
		if (watcher.watcherFire.LargeFireDangerPresent)
		{
			list.RemoveAll((Weather we) => we.rainRate < 0.001f);
		}
		return list.RandomElementByWeight((Weather w) => w.commonality);
	}
}
