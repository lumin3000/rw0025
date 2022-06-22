using UnityEngine;

public abstract class SoundLooper
{
	protected GameObject looperObj;

	public AudioSource looperSource;

	protected SoundLooperMaintenanceType maintenanceType;

	protected long lastMaintainTickOrFrame;

	public float Volume
	{
		get
		{
			return looperSource.volume;
		}
		set
		{
			if (value < 0f)
			{
				value = 0f;
			}
			looperSource.volume = value;
		}
	}

	public SoundLooper(AudioClip LooperClip, float Volume, SoundLooperMaintenanceType MaintenanceType)
		: this(LooperClip, Volume)
	{
		maintenanceType = MaintenanceType;
		Maintain();
	}

	public SoundLooper(AudioClip LooperClip, float Volume)
	{
		looperObj = GetGameObjectToAttachTo();
		looperSource = looperObj.AddComponent<AudioSource>();
		looperSource.name = "SoundLooperSource";
		looperSource.dopplerLevel = 0f;
		looperSource.clip = LooperClip;
		looperSource.loop = true;
		looperSource.volume = Volume;
		looperSource.Play();
		SoundLooperManager.Register(this);
	}

	public void Maintain()
	{
		if (maintenanceType == SoundLooperMaintenanceType.PerTick)
		{
			lastMaintainTickOrFrame = Find.TickManager.tickCount;
		}
		else if (maintenanceType == SoundLooperMaintenanceType.PerFrame)
		{
			lastMaintainTickOrFrame = Time.frameCount;
		}
	}

	public void LooperUpdate()
	{
		if (maintenanceType == SoundLooperMaintenanceType.PerTick)
		{
			if (Find.TickManager.tickCount > lastMaintainTickOrFrame + 1)
			{
				Cleanup();
			}
		}
		else if (maintenanceType == SoundLooperMaintenanceType.PerFrame && Time.frameCount > lastMaintainTickOrFrame + 1)
		{
			Cleanup();
		}
	}

	protected abstract GameObject GetGameObjectToAttachTo();

	public virtual void Cleanup()
	{
		SoundLooperManager.Deregister(this);
	}
}
