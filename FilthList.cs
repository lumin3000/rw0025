using System.Collections.Generic;

public static class FilthList
{
	public static HashSet<Filth> allFilth = new HashSet<Filth>();

	public static void Notify_FilthSpawned(Filth f)
	{
		allFilth.Add(f);
	}

	public static void Notify_FilthDestroyed(Filth f)
	{
		allFilth.Remove(f);
	}
}
