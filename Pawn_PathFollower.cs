using System.Collections.Generic;
using UnityEngine;

public class Pawn_PathFollower : Saveable
{
	private const int MaxCheckAheadNodes = 20;

	private const int ManualDoorInteractTicks = 50;

	protected Pawn pawn;

	public bool moving;

	public IntVec3 nextSquare;

	private IntVec3 lastSquare;

	public int ticksUntilMove;

	public int totalMoveDuration = 1;

	private TargetPack destination;

	private bool adjacentIsOK;

	public PawnPath curPath;

	public IntVec3 lastPathedTargetPosition;

	private List<PathFinderNode> Nodes
	{
		get
		{
			if (curPath == null)
			{
				return null;
			}
			return curPath.nodeList;
		}
	}

	public Pawn_PathFollower(Pawn newPawn)
	{
		pawn = newPawn;
	}

	public void ExposeData()
	{
		Scribe.LookField(ref moving, "Moving", forceSave: true);
		Scribe.LookField(ref nextSquare, "NextSquare");
		Scribe.LookField(ref ticksUntilMove, "TicksUntilMove");
		Scribe.LookField(ref totalMoveDuration, "TotalMoveDuration");
		if (moving)
		{
			if (destination != null && destination.thing != null && destination.thing.destroyed)
			{
				Debug.LogError(string.Concat("Saved while ", pawn, " was moving towards destroyed thing ", destination.thing, " with job ", pawn.jobs.CurJob));
			}
			Scribe.LookSaveable(ref destination, "Destination");
		}
		if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
		{
			if (moving)
			{
				StartPathTowards(destination);
			}
			Find.PawnDestinationManager.ReserveDestinationFor(pawn, destination.Loc);
		}
	}

	public void Notify_Teleported_Int()
	{
		if (Scribe.mode == LoadSaveMode.None)
		{
			StopDead();
			nextSquare = pawn.Position;
		}
	}

	public void StartPathTowards(TargetPack newDest)
	{
		StartPathTowards(newDest, adjacentIsOK: false);
	}

	public void StartPathTowards(TargetPack newDest, bool autoReserve, bool adjacentIsOK)
	{
		if (autoReserve)
		{
			if (newDest.HasThing)
			{
				Debug.LogWarning("Cannot use autoReserve when pathing to a Thing. We don't know what the destination square is yet, and it could change.");
			}
			else if (adjacentIsOK)
			{
				Debug.LogWarning("Cannot use autoReserve when pathing with adjacentIsOK. We don't know what the destination square is yet, and it could change.");
			}
			else
			{
				Find.PawnDestinationManager.ReserveDestinationFor(pawn, newDest.Loc);
			}
		}
		StartPathTowards(newDest, adjacentIsOK);
	}

	private void StartPathTowards(TargetPack newDest, bool adjacentIsOK)
	{
		if (!pawn.Position.Walkable())
		{
			bool flag = false;
			IntVec3[] radialPattern = Gen.RadialPattern;
			foreach (IntVec3 intVec in radialPattern)
			{
				if ((pawn.Position + intVec).Walkable())
				{
					pawn.Position += intVec;
					moving = false;
					nextSquare = pawn.Position;
					StartPathTowards(newDest);
					Debug.LogWarning(string.Concat(pawn, " was at an unwalkable position and has been teleported to ", pawn.Position));
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				pawn.Destroy();
				Debug.LogWarning(string.Concat(pawn, " was at an unwalkable position. Could not find walkable position. Destroyed."));
				return;
			}
		}
		if (!pawn.CanReach(newDest, adjacentIsOK))
		{
			Debug.LogWarning(string.Concat(pawn.ThingID, " (", pawn.Label, ") at ", pawn.Position, " tried to path to unreachable dest ", newDest, ". This should have been checked before trying to path. (adjacentIsOK=", adjacentIsOK, ")"));
			PatherArrived();
			return;
		}
		if (!nextSquare.Walkable())
		{
			nextSquare = pawn.Position;
		}
		this.adjacentIsOK = adjacentIsOK;
		destination = newDest;
		if (!destination.HasThing && Find.PawnDestinationManager.DestinationReservedFor(pawn) != destination.Loc)
		{
			Find.PawnDestinationManager.UnreserveAllFor(pawn);
		}
		if (AtDestinationPosition())
		{
			PatherArrived();
		}
		else if (pawn.Incapacitated)
		{
			Debug.LogWarning(pawn.Label + " tried to path while incapacitated. This should never happen.");
		}
		else if (!moving || curPath == null || destination != newDest)
		{
			curPath = null;
			moving = true;
		}
	}

	public void StopDead()
	{
		moving = false;
		curPath = null;
		nextSquare = pawn.Position;
	}

	private void PatherArrived()
	{
		StopDead();
		if (pawn.jobs.CurJob == null)
		{
			Debug.LogWarning(string.Concat(pawn, " got PatherArrived arrived without having a job. Destination=", destination, ", lastSquare=", lastSquare, " nextSquare=", nextSquare));
		}
		else
		{
			pawn.jobs.CurJobDriver.Notify_PatherArrived();
		}
	}

	private void PatherFailed()
	{
		StopDead();
		pawn.jobs.CurJobDriver.Notify_PatherFailed();
	}

	public void PatherTick()
	{
		if (ticksUntilMove > 0)
		{
			ticksUntilMove--;
		}
		else if (moving)
		{
			TryEnterNextPathSquare();
		}
	}

	public Thing ThingBlockingNextPathSquare()
	{
		Thing thing = Find.Grids.BlockerAt(nextSquare);
		if (thing != null && thing.BlocksPawn(pawn))
		{
			return thing;
		}
		return null;
	}

	public Building_Door NextSquareDoorToOpen()
	{
		Building_Door building_Door = Find.Grids.ThingAt<Building_Door>(nextSquare);
		if (building_Door != null && !building_Door.powerComp.PowerOn && !building_Door.isOpen && building_Door.WillOpenFor(pawn))
		{
			return building_Door;
		}
		return null;
	}

	private void TryEnterNextPathSquare()
	{
		Thing thing = ThingBlockingNextPathSquare();
		if (thing != null)
		{
			if (!pawn.Team.IsHostileToTeam(thing.Team))
			{
				Debug.LogWarning(string.Concat(pawn, " had path to ", destination, " blocked by ", thing));
				PatherFailed();
			}
			else
			{
				pawn.natives.TryMeleeAttack(thing);
				ticksUntilMove = 5;
			}
			return;
		}
		Building_Door building_Door = NextSquareDoorToOpen();
		if (building_Door != null)
		{
			pawn.stances.SetStance(new Stance_Cooldown(50, building_Door.Position, null));
			building_Door.StartManualOpenBy(pawn);
			return;
		}
		lastSquare = pawn.Position;
		pawn.Position = nextSquare;
		pawn.filth.Notify_EnteredNewSquare();
		Building_Door building_Door2 = Find.Grids.ThingAt<Building_Door>(lastSquare);
		if (building_Door2 != null && !building_Door2.powerComp.PowerOn && !building_Door2.CloseBlocked && !pawn.Team.IsHostileToTeam(building_Door2.Team) && pawn.raceDef.CanUseTechnology)
		{
			pawn.stances.SetStance(new Stance_Cooldown(50, building_Door2.Position, null));
			building_Door2.StartManualCloseBy(pawn);
		}
		else if (!NeedNewPath() || TrySetNewPath())
		{
			if (AtDestinationPosition())
			{
				PatherArrived();
			}
			else if (curPath.nodeList.Count == 0)
			{
				Debug.LogWarning(string.Concat(pawn, " ran out of path nodes. Force-arriving."));
				PatherArrived();
			}
			else
			{
				SetupMoveIntoNextSquare();
			}
		}
	}

	private void SetupMoveIntoNextSquare()
	{
		if (curPath.nodeList.Count < 2)
		{
			Debug.LogWarning(string.Concat(pawn, " at ", pawn.Position, " ran out of path nodes while pathing to ", destination, "."));
			PatherFailed();
			return;
		}
		nextSquare = curPath.nodeList[1].Position;
		Find.Grids.ThingAt<Building_Door>(nextSquare)?.Notify_PawnApproaching(pawn);
		int num = ((nextSquare.x != pawn.Position.x && nextSquare.z != pawn.Position.z) ? pawn.TicksPerMoveDiagonal : pawn.TicksPerMoveCardinal);
		num += PathGrid.CalculatedCostAt(nextSquare, perceived: false);
		if (pawn.jobs.CurJob == null)
		{
			if (pawn.carrier == null && !pawn.Incapacitated)
			{
				Debug.LogWarning(string.Concat(pawn, " has no job and isn't being carried or incapped!"));
			}
		}
		else
		{
			switch (pawn.jobs.CurJob.moveSpeed)
			{
			case MoveSpeed.Amble:
				num *= 3;
				break;
			case MoveSpeed.Walk:
				num *= 2;
				break;
			case MoveSpeed.Jog:
				num *= 1;
				break;
			case MoveSpeed.Sprint:
				num = Mathf.RoundToInt((float)num * 0.75f);
				break;
			}
		}
		totalMoveDuration = num;
		ticksUntilMove = num;
		Nodes.RemoveAt(0);
	}

	private bool TrySetNewPath()
	{
		PawnPath pawnPath = GenerateNewPath();
		if (!pawnPath.found)
		{
			PatherFailed();
			return false;
		}
		curPath = pawnPath;
		return true;
	}

	private PawnPath GenerateNewPath()
	{
		lastPathedTargetPosition = destination.Loc;
		PathRequest pathRequest = new PathRequest();
		pathRequest.pawn = pawn;
		pathRequest.start = pawn.Position;
		pathRequest.dest = destination;
		pathRequest.pathParams = pawn.PathParams;
		pathRequest.adjacentIsOK = adjacentIsOK;
		PawnPath pawnPath = PathSolver.FindPath(pathRequest);
		pawnPath.pathingPawn = pawn;
		return pawnPath;
	}

	private bool AtDestinationPosition()
	{
		if (pawn.Position == destination.Loc)
		{
			return true;
		}
		if (adjacentIsOK && pawn.Position.AdjacentTo8Way(destination.Loc))
		{
			return true;
		}
		if (destination.thing != null && pawn.Position.AdjacentTo8Way(destination.thing))
		{
			return true;
		}
		return false;
	}

	private bool NeedNewPath()
	{
		if (curPath == null || !curPath.found || Nodes.Count == 0)
		{
			return true;
		}
		if (lastPathedTargetPosition != destination.Loc)
		{
			float lengthHorizontalSquared = (pawn.Position - destination.Loc).LengthHorizontalSquared;
			float num = ((lengthHorizontalSquared > 900f) ? 10f : ((lengthHorizontalSquared > 289f) ? 5f : ((lengthHorizontalSquared > 100f) ? 3f : ((!(lengthHorizontalSquared > 49f)) ? 0.5f : 2f))));
			if ((lastPathedTargetPosition - destination.Loc).LengthHorizontalSquared > num * num)
			{
				return true;
			}
		}
		for (int i = 0; i < 20 && i < Nodes.Count; i++)
		{
			if (!Nodes[i].Position.Walkable())
			{
				return true;
			}
		}
		return false;
	}

	public void PatherDraw()
	{
		if (DebugSettings.drawPaths && curPath != null && pawn.Team != TeamType.Colonist && Find.Selector.IsSelected(pawn))
		{
			curPath.DrawPath();
		}
	}
}
