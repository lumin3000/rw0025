using System.Collections.Generic;

public static class SoundLooperManager
{
	private static List<SoundLooper> LooperList = new List<SoundLooper>();

	public static void Register(SoundLooper newLooper)
	{
		LooperList.Add(newLooper);
	}

	public static void Deregister(SoundLooper oldLooper)
	{
		LooperList.Remove(oldLooper);
	}

	public static void LoopersUpdate()
	{
		foreach (SoundLooper item in LooperList.ListFullCopy())
		{
			item.LooperUpdate();
		}
	}
}
