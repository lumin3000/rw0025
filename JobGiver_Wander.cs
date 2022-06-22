using System;
using UnityEngine;

public abstract class JobGiver_Wander : ThinkNode_JobGiver
{
	protected float radius;

	protected Predicate<IntVec3> wanderDestValidator;

	protected IntRange ticksBetweenWandersRange;

	protected MoveSpeed moveSpeed = MoveSpeed.Walk;

	private bool nextOrderIsWait = true;

	protected override Job TryGiveTerminalJob()
	{
		nextOrderIsWait = !nextOrderIsWait;
		if (nextOrderIsWait)
		{
			Job job = new Job(JobType.Wait);
			job.TimeLimit = ticksBetweenWandersRange.RandomInRange;
			return job;
		}
		IntVec3 intVec = RandomWanderPosition();
		Find.PawnDestinationManager.ReserveDestinationFor(pawn, intVec);
		Job job2 = new Job(JobType.Goto, new TargetPack(intVec));
		job2.moveSpeed = moveSpeed;
		return job2;
	}

	protected abstract IntVec3 GetWanderRoot();

	private IntVec3 RandomWanderPosition()
	{
		//Discarded unreachable code: IL_01e4
		IntVec3 wanderRoot = GetWanderRoot();
		bool flag = Debug.isDebugBuild && DebugSettings.drawDestSearch;
		if (flag)
		{
			Find.DebugDrawer.MakeDebugSquare(wanderRoot, "ROOT", 60, 100);
		}
		int num = 0;
		int max = Gen.NumSquaresInRadius(radius);
		IntVec3 intVec;
		while (true)
		{
			num++;
			if (num >= 20)
			{
				return wanderRoot;
			}
			intVec = wanderRoot + Gen.RadialPattern[UnityEngine.Random.Range(0, max)];
			if (!intVec.Standable())
			{
				if (flag)
				{
					Find.DebugDrawer.MakeDebugSquare(intVec, "ST", 25, 100);
				}
				continue;
			}
			if (!pawn.CanReach(intVec, adjacentIsOK: false))
			{
				if (flag)
				{
					Find.DebugDrawer.MakeDebugSquare(intVec, "RCH", 60, 100);
				}
				continue;
			}
			if (!pawn.inventory.Has(EntityType.DoorKey) && pawn.ContainingRoom() != intVec.ContainingRoom())
			{
				if (flag)
				{
					Find.DebugDrawer.MakeDebugSquare(intVec, "ROOM", 68, 100);
				}
				continue;
			}
			if (Find.PawnDestinationManager.DestinationIsReserved(intVec, pawn))
			{
				if (flag)
				{
					Find.DebugDrawer.MakeDebugSquare(intVec, "RSVD", 75, 100);
				}
				continue;
			}
			if (Find.PathGrid.PerceivedPathCostAt(intVec) > 20)
			{
				if (flag)
				{
					Find.DebugDrawer.MakeDebugSquare(intVec, "PC", 40, 100);
				}
				continue;
			}
			if (wanderDestValidator == null || wanderDestValidator(intVec))
			{
				break;
			}
			if (flag)
			{
				Find.DebugDrawer.MakeDebugSquare(intVec, "VALD", 15, 100);
			}
		}
		if (flag)
		{
			Find.DebugDrawer.MakeDebugSquare(intVec, "GO", 90, 100);
		}
		return intVec;
	}
}
