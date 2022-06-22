using System.Linq;
using UnityEngine;

public class QueuedIncident : Saveable
{
	public IncidentDefinition def;

	public int occurTick;

	public IncidentParms parms = new IncidentParms();

	public QueuedIncident()
	{
	}

	public QueuedIncident(IncidentDefinition incident)
	{
		def = incident;
		int num = Mathf.RoundToInt(Random.Range(0.05f, 0.95f) * 5000f);
		occurTick = Find.TickManager.tickCount + num;
	}

	public void ExposeData()
	{
		int saveKey = 0;
		if (Scribe.mode == LoadSaveMode.Saving)
		{
			saveKey = def.uniqueSaveKey;
		}
		Scribe.LookField(ref saveKey, "DefinitionIndex");
		if (Scribe.mode == LoadSaveMode.LoadingVars)
		{
			def = IncidentDatabase.allIncidentDefs.Where((IncidentDefinition d) => d.uniqueSaveKey == saveKey).FirstOrDefault();
			if (def == null)
			{
				Debug.Log("Failed loading from key " + saveKey);
			}
		}
		Scribe.LookField(ref occurTick, "OccurTick");
		Scribe.LookSaveable(ref parms, "Parms");
	}

	public override string ToString()
	{
		string text = occurTick / 20000 + "d " + occurTick % 20000 + "t";
		string text2 = def.ToString();
		text2 = text2.Substring(9);
		text2.PadRight(17);
		string text3 = text2 + " at " + text;
		if (parms != null)
		{
			text3 = text3 + " " + parms.ToString();
		}
		return text3;
	}
}
