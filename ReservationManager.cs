using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ReservationManager : Saveable
{
	public List<ThingReservation> reservations = new List<ThingReservation>();

	private static readonly Material DebugReservedThingIcon = MaterialPool.MatFrom("UI/Overlays/ReservedForWork", MatBases.Cutout);

	public void ExposeData()
	{
		Scribe.LookList(ref reservations, "Reservations");
	}

	public Pawn ReserverOf(TargetPack target, ReservationType itype)
	{
		return (from r in reservations
			where r.target.SameAs(target) && r.interaction == itype
			select r.claimant).FirstOrDefault();
	}

	public Pawn ReserverOf(TargetPack target)
	{
		return (from r in reservations
			where r.target.SameAs(target)
			select r.claimant).FirstOrDefault();
	}

	public bool CanReserveFor(Pawn worker, TargetPack target, ReservationType itype)
	{
		foreach (ThingReservation reservation in reservations)
		{
			if (reservation.target.SameAs(target) && reservation.interaction == itype)
			{
				return false;
			}
		}
		if (!worker.CanReach(target, adjacentIsOK: true))
		{
			return false;
		}
		if (target.HasThing)
		{
			bool flag = false;
			if (target.thing.def.passability == Traversability.Standable)
			{
				foreach (IntVec3 item in Gen.SquaresOccupiedBy(target.thing))
				{
					if (item.IsGoodWorkSpotFor(worker))
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag && worker.CanGetAdjacentTo(target.thing))
			{
				flag = true;
			}
			if (!flag)
			{
				return false;
			}
		}
		return true;
	}

	public void ReserveFor(Pawn worker, TargetPack target, ReservationType itype)
	{
		if (target.ThingDestroyed)
		{
			Debug.LogWarning(string.Concat(worker, " tried to reserve destroyed thing ", target, " for ", itype));
		}
		else if (!CanReserveFor(worker, target, itype))
		{
			if (worker.CanReach(target, adjacentIsOK: true))
			{
				Debug.LogWarning(string.Concat("Tried to reserve thing ", target, " for ", worker, " doing work ", itype, " that was already reserved by ", ReserverOf(target, itype), "."));
			}
		}
		else
		{
			ThingReservation thingReservation = new ThingReservation();
			thingReservation.claimant = worker;
			thingReservation.target = target;
			thingReservation.interaction = itype;
			reservations.Add(thingReservation);
		}
	}

	public bool TryReserveFor(Pawn worker, TargetPack target, ReservationType itype)
	{
		if (!CanReserveFor(worker, target, itype))
		{
			return false;
		}
		ThingReservation thingReservation = new ThingReservation();
		thingReservation.claimant = worker;
		thingReservation.target = target;
		thingReservation.interaction = itype;
		reservations.Add(thingReservation);
		return true;
	}

	public void UnReserve(TargetPack target, ReservationType itype)
	{
		if (target.ThingDestroyed)
		{
			Debug.LogWarning(string.Concat("Unreserving destroyed thing ", target, " for ", itype));
		}
		ThingReservation thingReservation = null;
		foreach (ThingReservation reservation in reservations)
		{
			if (reservation.target.SameAs(target) && reservation.interaction == itype)
			{
				thingReservation = reservation;
				break;
			}
		}
		if (thingReservation == null && !target.ThingDestroyed)
		{
			Debug.LogWarning(string.Concat("Tried to unreserve thing ", target, " for ", itype, " that wasn't reserved."));
		}
		else
		{
			reservations.Remove(thingReservation);
		}
	}

	public void UnReserveAllForThing(TargetPack target)
	{
		reservations.RemoveAll((ThingReservation r) => r.target == target);
	}

	public void UnReserveAllForPawn(Pawn worker)
	{
		reservations.RemoveAll((ThingReservation r) => r.claimant == worker);
	}

	public void DrawWorkReservations()
	{
		foreach (ThingReservation reservation in reservations)
		{
			if (reservation.target.thing != null)
			{
				Thing thing = reservation.target.thing;
				Vector3 s = new Vector3(thing.RotatedSize.x, 1f, thing.RotatedSize.z);
				Matrix4x4 matrix = default(Matrix4x4);
				matrix.SetTRS(thing.DrawPos + Vector3.up * 0.1f, Quaternion.identity, s);
				Graphics.DrawMesh(MeshPool.plane10, matrix, DebugReservedThingIcon, 0);
				GenRender.DrawLineBetween(reservation.claimant.DrawPos, reservation.target.thing.DrawPos);
			}
			else
			{
				Graphics.DrawMesh(MeshPool.plane10, reservation.target.Loc.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays), Quaternion.identity, DebugReservedThingIcon, 0);
				GenRender.DrawLineBetween(reservation.claimant.DrawPos, reservation.target.Loc.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays));
			}
		}
	}
}
