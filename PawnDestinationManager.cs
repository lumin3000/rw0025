using System;
using System.Collections.Generic;
using UnityEngine;

public class PawnDestinationManager
{
	private Dictionary<TeamType, Dictionary<Pawn, IntVec3>> ReservedDestinations = new Dictionary<TeamType, Dictionary<Pawn, IntVec3>>();

	private readonly Material DestinationMat;

	private readonly Material DestinationSelectionMat;

	public PawnDestinationManager()
	{
		Reset();
		DestinationMat = MaterialPool.MatFrom("UI/Overlays/ReservedDestinationPNG");
		DestinationSelectionMat = MaterialPool.MatFrom("UI/Overlays/ReservedDestinationSelectionPNG");
	}

	public void Reset()
	{
		ReservedDestinations.Clear();
		foreach (int value in Enum.GetValues(typeof(TeamType)))
		{
			ReservedDestinations.Add((TeamType)value, new Dictionary<Pawn, IntVec3>());
		}
	}

	public void DrawDestinations()
	{
		foreach (KeyValuePair<Pawn, IntVec3> item in ReservedDestinations[TeamType.Colonist])
		{
			if (!(item.Key.Position == item.Value))
			{
				IntVec3 value = item.Value;
				Vector3 s = new Vector3(1f, 1f, 1f);
				Matrix4x4 matrix = default(Matrix4x4);
				matrix.SetTRS(value.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays), Quaternion.identity, s);
				Graphics.DrawMesh(MeshPool.plane10, matrix, DestinationMat, 0);
				if (Find.Selector.IsSelected(item.Key))
				{
					Graphics.DrawMesh(MeshPool.plane10, matrix, DestinationSelectionMat, 0);
				}
			}
		}
	}

	public void ReserveDestinationFor(Pawn p, IntVec3 Loc)
	{
		Pawn pawn = ReserverOfDestinationForTeam(Loc, p.Team);
		if (pawn == null || pawn == p)
		{
			if (!ReservedDestinations[p.Team].ContainsKey(p))
			{
				ReservedDestinations[p.Team].Add(p, Loc);
			}
			else
			{
				ReservedDestinations[p.Team][p] = Loc;
			}
		}
	}

	public IntVec3 DestinationReservedFor(Pawn p)
	{
		if (ReservedDestinations[p.Team].ContainsKey(p))
		{
			return ReservedDestinations[p.Team][p];
		}
		return default(IntVec3);
	}

	public bool DestinationIsReserved(IntVec3 Loc)
	{
		foreach (KeyValuePair<TeamType, Dictionary<Pawn, IntVec3>> reservedDestination in ReservedDestinations)
		{
			foreach (KeyValuePair<Pawn, IntVec3> item in reservedDestination.Value)
			{
				if (item.Value == Loc)
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool DestinationIsReserved(IntVec3 Loc, Pawn PawnSearchingFor)
	{
		foreach (KeyValuePair<Pawn, IntVec3> item in ReservedDestinations[PawnSearchingFor.Team])
		{
			if (item.Value == Loc && item.Key != PawnSearchingFor)
			{
				return true;
			}
		}
		return false;
	}

	public Pawn ReserverOfDestinationForTeam(IntVec3 Loc, TeamType Team)
	{
		foreach (KeyValuePair<Pawn, IntVec3> item in ReservedDestinations[Team])
		{
			if (item.Value == Loc)
			{
				return item.Key;
			}
		}
		return null;
	}

	public void UnreserveAllFor(Pawn p)
	{
		ReservedDestinations[p.Team].Remove(p);
	}

	public void RemovePawnFromSystem(Pawn p)
	{
		if (ReservedDestinations[p.Team].ContainsKey(p))
		{
			ReservedDestinations[p.Team].Remove(p);
		}
	}
}
