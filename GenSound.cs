using System;
using System.Collections.Generic;
using UnityEngine;

public static class GenSound
{
	private const float VolumeMuliplier = 2f;

	public const float SoundAltitude = 0f;

	public const AudioRolloffMode WorldRolloffMode = AudioRolloffMode.Linear;

	public const float WorldSoundMinDistance = 25f;

	public const float WorldSoundMaxDistance = 70f;

	private static Dictionary<SoundSlot, float> allowedPlayTimes;

	private static Dictionary<string, string> LastRandomClipsInFolders;

	static GenSound()
	{
		allowedPlayTimes = new Dictionary<SoundSlot, float>();
		LastRandomClipsInFolders = new Dictionary<string, string>();
		foreach (int value in Enum.GetValues(typeof(SoundSlot)))
		{
			if (value != 0)
			{
				allowedPlayTimes.Add((SoundSlot)value, 0f);
			}
		}
	}

	public static void PlaySoundOnCamera(string soundPath, float volume)
	{
		PlaySoundOnCamera(soundPath, volume, SoundSlot.None);
	}

	public static void PlaySoundOnCamera(AudioClip clip, float volume)
	{
		PlaySoundOnCamera(clip, volume, SoundSlot.None);
	}

	public static void PlaySoundOnCamera(string soundPath, float volume, SoundSlot slot)
	{
		AudioClip audioClip = Res.LoadSound(soundPath);
		if (audioClip == null)
		{
			Debug.Log("Could not find sound " + soundPath);
		}
		else
		{
			PlaySoundOnCamera(audioClip, volume, slot);
		}
	}

	public static void PlaySoundOnCamera(AudioClip clip, float volume, SoundSlot slot)
	{
		if (clip == null)
		{
			Debug.LogWarning("Played null sound on camera.");
			return;
		}
		if (slot != 0)
		{
			if (Time.realtimeSinceStartup < allowedPlayTimes[slot])
			{
				return;
			}
			allowedPlayTimes[slot] = Time.realtimeSinceStartup + clip.length;
		}
		volume *= 2f;
		volume *= AudioListener.volume;
		SoundQueue.QueueSound(new QueuedSound(Find.CameraAudioSource, clip, volume));
	}

	public static void PlaySoundAt(IntVec3 Location, string ClipPath, float Volume)
	{
		PlaySoundAt(Location, (AudioClip)Resources.Load("Sounds/" + ClipPath), Volume);
	}

	public static void PlaySoundAt(IntVec3 Location, AudioClip Clip)
	{
		PlaySoundAt(Location, Clip, 1f);
	}

	public static void PlaySoundAt(IntVec3 Location, AudioClip Clip, float Volume)
	{
		if (Clip == null)
		{
			Debug.LogWarning("Played null sound in world.");
			return;
		}
		Volume *= 2f;
		SoundQueue.QueueSound(new QueuedSound(Location, Clip, Volume));
	}

	public static AudioClip RandomClipInFolder(string FolderPath, bool NoRepeat)
	{
		//Discarded unreachable code: IL_0049
		if (!NoRepeat)
		{
			return RandomClipInFolder(FolderPath);
		}
		AudioClip audioClip;
		do
		{
			audioClip = RandomClipInFolder(FolderPath);
		}
		while (LastRandomClipsInFolders.ContainsKey(FolderPath) && audioClip.name == LastRandomClipsInFolders[FolderPath]);
		if (!LastRandomClipsInFolders.ContainsKey(FolderPath))
		{
			LastRandomClipsInFolders.Add(FolderPath, audioClip.name);
		}
		else
		{
			LastRandomClipsInFolders[FolderPath] = audioClip.name;
		}
		return audioClip;
	}

	public static AudioClip RandomClipInFolder(string FolderPath)
	{
		UnityEngine.Object[] array = Resources.LoadAll("Sounds/" + FolderPath, typeof(AudioClip));
		if (array.Length == 0)
		{
			Debug.LogWarning("Asked for RandomClipInFolder for an empty or nonexistent folder " + FolderPath);
			return null;
		}
		int num = UnityEngine.Random.Range(0, array.Length);
		return (AudioClip)array[num];
	}
}
