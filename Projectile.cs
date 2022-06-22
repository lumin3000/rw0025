using System;
using UnityEngine;

public abstract class Projectile : ThingWithComponents
{
	protected Vector3 origin;

	protected Vector3 destination;

	protected Thing assignedTarget;

	public bool canFreeIntercept;

	protected bool landed;

	protected int ticksToImpact;

	public override Vector3 DrawPos => ExactPosition;

	protected int StartingTicksToImpact => (int)Math.Round((origin - destination).magnitude / (def.projectile_Speed / 100f));

	protected IntVec3 DestinationSquare => new IntVec3(destination);

	public virtual Vector3 ExactPosition
	{
		get
		{
			Vector3 vector = (destination - origin) * (1f - (float)ticksToImpact / (float)StartingTicksToImpact);
			return origin + vector + Vector3.up * def.altitude;
		}
	}

	public virtual Quaternion ExactRotation => Quaternion.LookRotation(destination - origin);

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe.LookField(ref origin, "Origin");
		Scribe.LookField(ref destination, "Destination");
		Scribe.LookThingRef(ref assignedTarget, "AssignedTarget", this);
		Scribe.LookField(ref canFreeIntercept, "CanFreeIntercept");
		Scribe.LookField(ref landed, "Landed");
		Scribe.LookField(ref ticksToImpact, "TicksToImpact");
	}

	public void Launch(TargetPack targ)
	{
		Launch(base.Position.ToVector3Shifted(), targ);
	}

	public void Launch(Vector3 newOrigin, TargetPack targ)
	{
		origin = newOrigin;
		if (targ.thing != null)
		{
			assignedTarget = targ.thing;
		}
		destination = targ.Loc.ToVector3Shifted() + new Vector3(UnityEngine.Random.Range(-0.3f, 0.3f), 0f, UnityEngine.Random.Range(-0.3f, 0.3f));
		ticksToImpact = StartingTicksToImpact;
	}

	public override void Tick()
	{
		base.Tick();
		if (landed)
		{
			return;
		}
		ticksToImpact--;
		if (!ExactPosition.InBounds())
		{
			ticksToImpact++;
			base.Position = ExactPosition.ToIntVec3();
			Destroy();
			return;
		}
		base.Position = ExactPosition.ToIntVec3();
		if (ticksToImpact <= 0)
		{
			if (DestinationSquare.InBounds())
			{
				base.Position = DestinationSquare;
			}
			ImpactSomething();
		}
		else
		{
			if (!def.projectile_ImpactWorld || !canFreeIntercept || ticksToImpact >= StartingTicksToImpact / 2)
			{
				return;
			}
			foreach (Thing item in Find.Grids.ThingsAt(base.Position))
			{
				if (!item.def.canBeSeenOver)
				{
					ticksToImpact++;
					Impact(item);
					break;
				}
				if (item.def.category != EntityCategory.Pawn)
				{
					continue;
				}
				Pawn pawn = (Pawn)item;
				float num;
				if (pawn.Incapacitated)
				{
					num = 0.05f;
				}
				else
				{
					float num2 = (ExactPosition - origin).MagnitudeHorizontal();
					num = ((num2 < 4f) ? 0f : ((!(num2 < 6f)) ? 0.8f : 0.4f));
				}
				if (UnityEngine.Random.value < num)
				{
					Pawn pawn2 = Find.Grids.ThingAt(base.Position, EntityType.Pawn) as Pawn;
					if (pawn2 != null)
					{
						Impact(pawn2);
						break;
					}
				}
			}
		}
	}

	public override void Draw()
	{
		Graphics.DrawMesh(MeshPool.plane10, DrawPos, ExactRotation, def.drawMat, 0);
		Comps_Draw();
	}

	private void ImpactSomething()
	{
		if (assignedTarget != null)
		{
			Pawn pawn = assignedTarget as Pawn;
			if (pawn != null && pawn.Incapacitated && (origin - destination).MagnitudeHorizontalSquared() > 5f && UnityEngine.Random.value < 0.2f)
			{
				Impact(null);
			}
			else
			{
				Impact(assignedTarget);
			}
			return;
		}
		Thing thing = Find.Grids.ThingAt(base.Position, EntityType.Pawn);
		if (thing != null)
		{
			Impact(thing);
			return;
		}
		foreach (Thing item in Find.Grids.ThingsAt(base.Position))
		{
			if (item.def.coverPercent > 0f || item.def.passability != 0)
			{
				Impact(item);
				return;
			}
		}
		Impact(null);
	}

	protected virtual void Impact(Thing hitThing)
	{
		Destroy();
	}

	public void ForceInstantImpact()
	{
		if (!DestinationSquare.InBounds())
		{
			Destroy();
			return;
		}
		ticksToImpact = 0;
		base.Position = DestinationSquare;
		ImpactSomething();
	}
}
