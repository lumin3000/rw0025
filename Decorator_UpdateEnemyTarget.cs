internal class Decorator_UpdateEnemyTarget : ThinkNode
{
	private const float TargetAcquireRadius = 56f;

	private const float TargetKeepRadius = 65f;

	private Thing EnemyTarget
	{
		get
		{
			return pawn.MindState.enemyTarget;
		}
		set
		{
			pawn.MindState.enemyTarget = value;
		}
	}

	public override JobPackage TryIssueJobPackage()
	{
		if (EnemyTarget != null)
		{
			if (EnemyTarget.destroyed || !base.pawn.CanReach(EnemyTarget, adjacentIsOK: true) || (base.pawn.Position - EnemyTarget.Position).LengthHorizontalSquared > 4225f)
			{
				EnemyTarget = null;
			}
			Pawn pawn = EnemyTarget as Pawn;
			if (pawn != null && pawn.Incapacitated)
			{
				EnemyTarget = null;
			}
		}
		if (EnemyTarget == null)
		{
			EnemyTarget = base.pawn.ClosestReachableEnemyTarget(null, 56f, needsLOStoDynamic: true, needsLOStoStatic: false);
			if (EnemyTarget != null)
			{
				Find.AIKingManager.KingOf(base.pawn)?.Notify_PawnAcquiredTarget(base.pawn, EnemyTarget);
			}
		}
		else
		{
			Thing thing = base.pawn.ClosestReachableEnemyTarget(null, 56f, needsLOStoDynamic: true, needsLOStoStatic: true);
			if (thing != null)
			{
				EnemyTarget = thing;
			}
		}
		return null;
	}
}
