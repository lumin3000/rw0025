using System.Collections.Generic;

public class MoteList
{
	private List<Mote> allMotes = new List<Mote>();

	public void MoteSpawned(Mote newMote)
	{
		allMotes.Add(newMote);
	}

	public void MoteDestroyed(Mote oldMote)
	{
		allMotes.Remove(oldMote);
	}

	public void MoteListUpdate()
	{
		for (int num = allMotes.Count - 1; num >= 0; num--)
		{
			allMotes[num].RealtimeUpdate();
		}
	}
}
