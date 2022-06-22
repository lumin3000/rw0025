using UnityEngine;

public class IncidentDefinition
{
	public float chance;

	public bool global;

	public IncidentFavorability favorability = IncidentFavorability.Neutral;

	public IncidentThreatLevel threatLevel;

	public IncidentPopulationEffect populationEffect = IncidentPopulationEffect.None;

	public int minRefireInterval;

	public bool pointsScaleable;

	public int uniqueSaveKey;

	public virtual bool TryExecute(IncidentParms parms)
	{
		Debug.LogError("Unimplemented incident " + this);
		return false;
	}
}
