public abstract class Building_Turret : Building
{
	private const float SightRadiusTurret = 13.4f;

	protected StunHandler stunner;

	public abstract Thing CurrentTarget { get; }

	public Building_Turret()
	{
		stunner = new StunHandler(this);
	}

	public override void Tick()
	{
		base.Tick();
		stunner.StunHandlerTick();
		if (base.Team == TeamType.Colonist)
		{
			Find.FogGrid.ClearFogCircle(base.Position, 13.4f);
		}
	}

	protected override void ApplyDamage(DamageInfo dinfo)
	{
		stunner.Notify_DamageApplied(dinfo);
		base.ApplyDamage(dinfo);
	}
}
