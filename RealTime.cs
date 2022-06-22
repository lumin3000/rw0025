using UnityEngine;

public class RealTime : Saveable
{
	private float lastRealTime;

	public float realDeltaTime;

	public MoteList moteList = new MoteList();

	public float timeUnpaused;

	public void ExposeData()
	{
		Scribe.LookField(ref timeUnpaused, "TimeUnpaused");
	}

	public void Update()
	{
		realDeltaTime = Time.realtimeSinceStartup - lastRealTime;
		lastRealTime = Time.realtimeSinceStartup;
		moteList.MoteListUpdate();
		if (Game.GMode == GameMode.Gameplay && !Find.TickManager.Paused)
		{
			timeUnpaused += Time.deltaTime;
		}
	}
}
