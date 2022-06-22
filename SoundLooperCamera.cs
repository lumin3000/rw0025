using UnityEngine;

public class SoundLooperCamera : SoundLooper
{
	public SoundLooperCamera(AudioClip LooperClip, float Volume, SoundLooperMaintenanceType MaintenanceType)
		: base(LooperClip, Volume, MaintenanceType)
	{
	}

	public SoundLooperCamera(AudioClip LooperClip, float Volume)
		: base(LooperClip, Volume)
	{
	}

	protected override GameObject GetGameObjectToAttachTo()
	{
		return Find.AudioListenerDummy;
	}

	public override void Cleanup()
	{
		base.Cleanup();
		Object.Destroy(looperSource);
	}
}
