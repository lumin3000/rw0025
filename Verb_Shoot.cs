using System.Text;

public class Verb_Shoot : Verb_LaunchProjectile
{
	public override string InfoTextFull
	{
		get
		{
			VerbDefinition verbDef = equipment.def.verbDef;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(verbDef.description);
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			stringBuilder.Append("Damage: " + verbDef.projDef.projectile_DamageAmountBase);
			stringBuilder.AppendLine();
			stringBuilder.Append("Range: " + verbDef.range);
			stringBuilder.AppendLine();
			stringBuilder.Append("Accuracy: " + verbDef.accuracy);
			stringBuilder.AppendLine();
			if (verbDef.burstShotCount > 1)
			{
				stringBuilder.Append("Automatic fire. Shots per burst: " + verbDef.burstShotCount);
			}
			else
			{
				stringBuilder.Append("Single-shot");
			}
			stringBuilder.AppendLine();
			stringBuilder.Append("Aim time: " + verbDef.warmupTicks.TicksInSecondsString());
			return stringBuilder.ToString();
		}
	}

	protected override int ShotsPerBurst => equipment.def.verbDef.burstShotCount;

	protected override bool TryShotSpecialEffect()
	{
		if (base.TryShotSpecialEffect())
		{
			MoteMaker.ThrowFlash(owner.Position, "ShotFlash", 9f);
			return true;
		}
		return false;
	}
}
