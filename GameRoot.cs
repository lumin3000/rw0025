using UnityEngine;

public class GameRoot : MonoBehaviour
{
	public Map curMap;

	public TickManager tickManager;

	public DestroyManager destroyManager;

	public ResourceManager resourceManager;

	public RealTime realTime;

	private void Start()
	{
		GlobalInit.GlobalInitIfNotAlreadyDone();
		Prefs.ApplyPrefs();
		realTime = new RealTime();
		Find.ResetFind();
		if (MapInitParams.mapToLoad != string.Empty)
		{
			MapIniter_LoadFromFile.InitMapFromFile(MapInitParams.mapToLoad);
		}
		else
		{
			MapIniter_NewGame.InitNewGeneratedMap();
		}
		MapInitParams.Notify_MapInited();
	}

	public void GameReset()
	{
		curMap = new Map();
		tickManager = new TickManager();
		destroyManager = new DestroyManager();
		resourceManager = new ResourceManager();
		Find.ResetFind();
		Find.CameraMap.ResetCamera();
	}

	private void Update()
	{
		if (Time.frameCount == 3)
		{
			Prefs.ApplyPrefs();
		}
		SkyManager.UpdateSkylight();
		WindManager.UpdateWindVector();
		DragSliderManager.DragSlidersUpdate();
		realTime.Update();
		BasicTrainingSignaller.BasicTrainingUpdate();
		tickManager.TickManagerUpdate();
		curMap.MapUpdate();
		SoundLooperManager.LoopersUpdate();
		SoundQueue.ResolveSounds();
		LongEventHandler.LongEventsUpdate();
	}

	private void OnGUI()
	{
		if (Game.GMode == GameMode.Gameplay)
		{
			Find.UIMapRoot.UIRootOnGUI();
		}
	}
}
