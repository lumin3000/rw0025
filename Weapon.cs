public abstract class Weapon : Building
{
	public float CooldownTicksLeft;

	public bool CoolingDown => CooldownTicksLeft > 0f;

	public bool MachStopped => base.HealthState >= HealthState.HeavyDamage;

	public override void SpawnSetup()
	{
		base.SpawnSetup();
		if (def.mach_AutoFire)
		{
		}
	}

	public override void Tick()
	{
		base.Tick();
		if (CooldownTicksLeft > 0f && !MachStopped)
		{
			float num = 1f;
			CooldownTicksLeft -= num;
		}
	}

	public virtual void Beginning_TacticalMode()
	{
		if (def.mach_CooldownOnTacticalStart)
		{
			ResetCooldown();
		}
	}

	public virtual void Beginning_WorldMode()
	{
		CooldownTicksLeft = 0f;
	}

	protected void ResetCooldown()
	{
		CooldownTicksLeft = def.mach_CooldownTicks;
	}

	public bool TryCastAt(TargetPack Target)
	{
		CastAt(Target);
		return true;
	}

	protected virtual void CastAt(TargetPack Target)
	{
	}

	public override void DrawGUIOverlay()
	{
		base.DrawGUIOverlay();
		if (CoolingDown)
		{
			OverlayDrawer.DrawOverlayBar(this, 1f - CooldownTicksLeft / (float)def.mach_CooldownTicks, GenUI.GreenTex);
		}
	}
}
