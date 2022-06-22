using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UI_Tutor : Saveable
{
	public List<TutorItem> unusedItems = new List<TutorItem>();

	private List<MapPointer> activePointers = new List<MapPointer>();

	public TutorNote activeNote;

	public float activeNoteStartRealTime = -1f;

	public UI_Tutor()
	{
		foreach (Type item in typeof(TutorItem).AllLeafSubclasses())
		{
			unusedItems.Add((TutorItem)Activator.CreateInstance(item));
		}
	}

	public void ExposeData()
	{
		List<string> unusedItemNames = unusedItems.Select((TutorItem item) => item.GetType().Name).ToList();
		Scribe.LookList(ref unusedItemNames, "UnusedItemNamed");
		unusedItems.RemoveAll((TutorItem item) => !unusedItemNames.Contains(item.GetType().Name));
		Scribe.LookList(ref activePointers, "ActivePointers");
		Scribe.LookSaveable(ref activeNote, "ActiveNote");
	}

	public TutorItem ItemOfType(Type itemType)
	{
		return unusedItems.Where((TutorItem item) => item.GetType() == itemType).FirstOrDefault();
	}

	public void StartShow(TutorItem item)
	{
		if (!Prefs.TutorialEnabled)
		{
			return;
		}
		if (!unusedItems.Contains(item))
		{
			Debug.LogWarning(string.Concat("Starting showing ", item, " which is not in the unused tutor items list."));
		}
		else
		{
			unusedItems.Remove(item);
		}
		if (item.Completed)
		{
			Debug.LogWarning(string.Concat("Started looking at completed tutor item ", item, "."));
			unusedItems.Remove(item);
			return;
		}
		MapPointer mapPointer = item as MapPointer;
		if (mapPointer != null)
		{
			activePointers.Add(mapPointer);
		}
		TutorNote tutorNote = item as TutorNote;
		if (tutorNote != null)
		{
			GenSound.PlaySoundOnCamera(UISounds.TutorMessageAppear, 0.2f);
			if (activeNote != null)
			{
				tutorNote.doFadeIn = false;
			}
			activeNote = tutorNote;
			activeNoteStartRealTime = Time.time;
		}
	}

	public void TutorOnGUI()
	{
		if (!Prefs.TutorialEnabled)
		{
			activePointers.Clear();
			activeNote = null;
			return;
		}
		activePointers.RemoveAll((MapPointer po) => po.Completed);
		foreach (MapPointer activePointer in activePointers)
		{
			activePointer.TutorItemOnGUI();
		}
		if (activeNote != null)
		{
			if (activeNote.Completed)
			{
				activeNote = null;
			}
			else
			{
				activeNote.TutorItemOnGUI();
			}
		}
	}

	public void Signal(TutorSignal signal)
	{
		Signal(signal, null);
	}

	public void Signal(TutorSignal signal, Thing subject)
	{
		if (MapInitParams.StartedDebug || activeNote != null)
		{
			return;
		}
		foreach (TutorItem unusedItem in unusedItems)
		{
			if (unusedItem.ShowOnSignal(signal))
			{
				StartShow(unusedItem);
				break;
			}
		}
	}
}
