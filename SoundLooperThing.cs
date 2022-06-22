using UnityEngine;

public class SoundLooperThing : SoundLooper
{
	public SoundLooperThing(TargetPack target, AudioClip LooperClip, float Volume)
		: this(target, LooperClip, Volume, SoundLooperMaintenanceType.None)
	{
	}

	public SoundLooperThing(TargetPack target, AudioClip LooperClip, float Volume, SoundLooperMaintenanceType MaintenanceType)
		: base(LooperClip, Volume, MaintenanceType)
	{
		if (target.thing == null || !target.thing.destroyed)
		{
			Vector3 position = target.Loc.ToVector3Shifted();
			position.y = 0f;
			looperObj.transform.position = position;
		}
	}

	protected override GameObject GetGameObjectToAttachTo()
	{
		return new GameObject("SoundLooperObj");
	}

	public override void Cleanup()
	{
		base.Cleanup();
		Object.Destroy(looperObj);
	}
}
