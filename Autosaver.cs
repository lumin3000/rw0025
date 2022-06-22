using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Autosaver
{
	private const int TicksBetweenSaves = 20000;

	private const int NumAutosaves = 5;

	private int ticksUntilSave = 20000;

	public void AutosaverTick()
	{
		if (!Find.GameEnder.gameEnding)
		{
			ticksUntilSave--;
			if (ticksUntilSave <= 0)
			{
				ticksUntilSave = 20000;
				LongEventHandler.QueueLongEvent(DoMemoryCleanup, "Memory cleanup");
				LongEventHandler.QueueLongEvent(DoAutosave, "Autosaving");
			}
		}
	}

	private void DoAutosave()
	{
		string mapName = NewAutosaveFileName();
		MapSaveLoad.SaveToFile(Find.Map, mapName);
	}

	private void DoMemoryCleanup()
	{
		Resources.UnloadUnusedAssets();
	}

	private string NewAutosaveFileName()
	{
		string text = (from name in AutoSaveNames()
			where !MapFiles.HaveMapNamed(name)
			select name).FirstOrDefault();
		if (text != null)
		{
			return text;
		}
		return (from name in AutoSaveNames()
			orderby MapFiles.FileInfoForMap(name).LastWriteTime
			select name).First();
	}

	private IEnumerable<string> AutoSaveNames()
	{
		for (int i = 1; i <= 5; i++)
		{
			yield return "Autosave-" + i;
		}
	}
}
