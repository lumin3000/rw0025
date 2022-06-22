using System;
using System.Collections.Generic;
using UnityEngine;

public static class BasicTrainingSignaller
{
	private const float BasicTrainingStartTime = 10f;

	private const float BasicTrainingInterval = 80f;

	private static bool gameStartGiven;

	private static float lastTutorEventTime;

	private static List<Type> basicTrainingItems;

	static BasicTrainingSignaller()
	{
		gameStartGiven = false;
		lastTutorEventTime = 0f;
		basicTrainingItems = new List<Type>();
		basicTrainingItems.Add(typeof(TutorNote_IntroA));
		basicTrainingItems.Add(typeof(MapPointer_MineMinerals));
		basicTrainingItems.Add(typeof(TutorNote_Pause));
		basicTrainingItems.Add(typeof(MapPointer_Unforbid_Food));
		basicTrainingItems.Add(typeof(MapPointer_Unforbid_Metal));
		basicTrainingItems.Add(typeof(MapPointer_TakeGun));
		basicTrainingItems.Add(typeof(TutorNote_WorkSettingsA));
		basicTrainingItems.Add(typeof(TutorNote_Codex));
	}

	public static void BasicTrainingUpdate()
	{
		if (!(Time.timeSinceLevelLoad < 1f) && !MapInitParams.StartedDebug && Prefs.TutorialEnabled)
		{
			if (Find.RealTime.timeUnpaused > 10f && !gameStartGiven && Scribe.mode == LoadSaveMode.None)
			{
				Find.Tutor.Signal(TutorSignal.GameStarted);
				gameStartGiven = true;
			}
			if (Find.Tutor.activeNote != null)
			{
				lastTutorEventTime = Find.RealTime.timeUnpaused;
			}
			if (Find.RealTime.timeUnpaused - lastTutorEventTime > 80f)
			{
				ShowNextBasicTrainingItem();
			}
		}
	}

	public static void ShowNextBasicTrainingItem()
	{
		TutorItem tutorItem;
		do
		{
			if (basicTrainingItems.Count == 0)
			{
				return;
			}
			tutorItem = Find.Tutor.ItemOfType(basicTrainingItems[0]);
			basicTrainingItems.RemoveAt(0);
		}
		while (tutorItem == null || tutorItem.Completed);
		lastTutorEventTime = Find.RealTime.timeUnpaused;
		Find.Tutor.StartShow(tutorItem);
	}
}
