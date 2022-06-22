using System.Collections.Generic;

public class JobDriver_Equip : JobDriverToil
{
	public JobDriver_Equip(Pawn pawn)
		: base(pawn)
	{
	}

	public override JobReport GetReport()
	{
		return new JobReport("Equipping " + base.TargetThingA.Label + ".", null);
	}

	protected override IEnumerable<Toil> NewToilList()
	{
		yield return new Toil(delegate
		{
			Toils_General.GetToPickup(pawn, base.TargetThingA);
		}, ToilCompleteMode.PatherArrival)
		{
			tickFailCondition = () => !ToilTools.CanInteractStandard(pawn, base.TargetThingA) || base.TargetThingA.carrier != null
		};
		yield return new Toil(delegate
		{
			((Equipment)base.CurJob.targetA.thing).TakenAndEquippedBy(pawn);
		}, ToilCompleteMode.Immediate);
	}
}
