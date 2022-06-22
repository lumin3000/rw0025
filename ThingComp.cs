using System.Collections.Generic;
using UnityEngine;

public abstract class ThingComp
{
	public ThingWithComponents parent;

	public CompSetup setup;

	public virtual bool GetShouldDrawCompUI()
	{
		return false;
	}

	public virtual void CompUIDraw(Rect drect)
	{
	}

	public virtual void CompExposeData()
	{
	}

	public virtual void CompSpawnSetup()
	{
	}

	public virtual void CompDeSpawn()
	{
	}

	public virtual void CompKilled(DamageInfo dam)
	{
	}

	public virtual void CompDestroy()
	{
	}

	public virtual void CompDestroyFinalize()
	{
	}

	public virtual void CompTick()
	{
	}

	public virtual void CompApplyDamage(DamageInfo dinfo)
	{
	}

	public virtual void CompDraw()
	{
	}

	public virtual IEnumerable<MapMeshPiece> CompEmitMapMeshPieces()
	{
		yield break;
	}

	public virtual IEnumerable<Command> CompCommands()
	{
		yield break;
	}

	public virtual string CompInspectString()
	{
		return string.Empty;
	}
}
