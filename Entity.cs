using UnityEngine;

public abstract class Entity
{
	public bool destroyed;

	public bool spawnedInWorld;

	protected bool canDoUnsafeTeamChange = true;

	protected TeamType TeamInt;

	public TeamType Team
	{
		get
		{
			return TeamInt;
		}
		set
		{
			if (!canDoUnsafeTeamChange)
			{
				Debug.LogWarning(string.Concat("Setting team twice on ", this, ". Make a special team change handler method."));
			}
			TeamInt = value;
			canDoUnsafeTeamChange = false;
		}
	}

	public abstract string Label { get; }

	public virtual string LabelShort => Label;

	public virtual string LabelMouseover => Label;

	public abstract void SpawnSetup();

	public abstract void DeSpawn();

	public virtual void Tick()
	{
		Debug.LogError(string.Concat(this, " does not implement Tick."));
	}

	public virtual void TickRare()
	{
		Debug.LogError(string.Concat(this, " does not implement TickRare."));
	}

	public virtual void DestroyFinalize()
	{
		if (spawnedInWorld)
		{
			DeSpawn();
		}
	}

	public override string ToString()
	{
		return Label;
	}
}
