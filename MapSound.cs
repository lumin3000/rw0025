using UnityEngine;

public class MapSound
{
	protected SoundLooperCamera AmbienceLooper;

	protected float AmbienceVolume = 0.16f;

	protected readonly AudioClip DriveLoopClip = (AudioClip)Resources.Load("Sounds/Map/DriveAmbienceA");

	protected readonly AudioClip StationaryLoopClip = (AudioClip)Resources.Load("Sounds/Map/StationaryAmbienceA");

	public MapSound()
	{
		StartAmbience(MapAmbienceType.Stationary);
	}

	public void StartAmbience(MapAmbienceType AmbienceType)
	{
	}
}
