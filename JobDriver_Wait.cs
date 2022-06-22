using System.Collections.Generic;
using System.Linq;

public class JobDriver_Wait : JobDriver
{
	public JobDriver_Wait(Pawn pawn)
		: base(pawn)
	{
	}

	public override JobReport GetReport()
	{
		return new JobReport("Standing.", null);
	}

	public override void DriverStart()
	{
		Find.PawnDestinationManager.ReserveDestinationFor(pawn, pawn.Position);
		pawn.pather.StopDead();
	}

	public override void DriverTick()
	{
		if (base.pawn.story != null && base.pawn.story.WorkIsDisabled(WorkType.Soldier))
		{
			return;
		}
		List<IntVec3> list = Gen.AdjacentSquares8Way(base.pawn).ToList();
		list.Shuffle();
		list.Add(base.pawn.Position);
		foreach (IntVec3 item in list)
		{
			foreach (Thing item2 in Find.Grids.ThingsAt(item))
			{
				Pawn pawn = item2 as Pawn;
				if (pawn != null && base.pawn.Team.IsHostileToTeam(pawn.Team) && !pawn.Incapacitated)
				{
					base.pawn.natives.TryMeleeAttack(pawn);
					return;
				}
			}
		}
		if (base.pawn.equipment.Primary != null)
		{
			Thing thing = base.pawn.ClosestReachableEnemyTarget(null, base.pawn.equipment.Primary.verb.VerbDef.range + 2f, needsLOStoDynamic: true, needsLOStoStatic: true);
			if (thing != null)
			{
				base.pawn.equipment.TryStartAttack(thing);
			}
		}
	}
}
