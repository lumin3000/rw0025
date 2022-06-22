using System.Collections.Generic;
using UnityEngine;

public static class SoundQueue
{
	private static List<QueuedSound> SoundList = new List<QueuedSound>();

	public static void QueueSound(QueuedSound q)
	{
		SoundList.Add(q);
	}

	public static void ResolveSounds()
	{
		List<AudioClip> list = new List<AudioClip>();
		foreach (QueuedSound sound in SoundList)
		{
			if (!list.Contains(sound.clip))
			{
				sound.ExecuteSound();
				list.Add(sound.clip);
			}
		}
		SoundList.Clear();
	}
}
