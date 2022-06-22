internal class Building_Glower : Building
{
	public override void SpawnSetup()
	{
		base.SpawnSetup();
		CompPowerTrader comp = GetComp<CompPowerTrader>();
		comp.PowerStartedCallback = delegate
		{
			GetComp<CompGlower>().GlowOn = true;
		};
		comp.PowerStoppedCallback = delegate
		{
			GetComp<CompGlower>().GlowOn = false;
		};
	}
}
