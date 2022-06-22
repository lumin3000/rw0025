public class Building_TurretGun : Building_Turret
{
	protected const int BurstCooldownTicksMax = 300;

	public const string GunName = "Gun_L-15 LMG";

	protected Equipment gun;

	protected TurretTop top;

	protected CompPowerTrader powerComp;

	protected Thing target;

	protected int burstCooldownTicksLeft;

	public override Thing CurrentTarget => target;

	public override void SpawnSetup()
	{
		base.SpawnSetup();
		powerComp = GetComp<CompPowerTrader>();
		gun = (Equipment)ThingMaker.MakeThing("Gun_L-15 LMG");
		gun.InitVerb();
		gun.verb.owner = this;
		top = new TurretTop(this);
	}

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe.LookField(ref burstCooldownTicksLeft, "BurstCooldownTicksLeft");
	}

	public override void Tick()
	{
		base.Tick();
		gun.verb.VerbTick();
		if (!stunner.Stunned && powerComp.PowerOn && gun.verb.state != VerbState.Bursting)
		{
			if (burstCooldownTicksLeft > 0)
			{
				burstCooldownTicksLeft--;
			}
			if (burstCooldownTicksLeft <= 0)
			{
				TryStartShoot();
			}
			top.TurretTopTick();
		}
	}

	public override void Draw()
	{
		top.DrawTurret();
		base.Draw();
	}

	public override void DrawSelectedExtras()
	{
		ThingDefinition thingDefinition = ThingDefDatabase.ThingDefNamed("Gun_L-15 LMG");
		GenRender.RenderRadiusRing(base.Position, thingDefinition.verbDef.range);
	}

	protected void TryStartShoot()
	{
		GenScan.CloseToThingValidator validator = delegate(Thing t)
		{
			Pawn pawn = t as Pawn;
			if (pawn != null)
			{
				if (!pawn.raceDef.humanoid)
				{
					return false;
				}
				if (pawn.inventory != null && pawn.inventory.Has(EntityType.DoorKey))
				{
					return false;
				}
			}
			return true;
		};
		target = this.ClosestReachableEnemyTarget(validator, gun.def.verbDef.range, needsLOStoDynamic: true, needsLOStoStatic: true);
		if (target != null && gun.verb.TryStartCastOn(new TargetPack(target)))
		{
			gun.verb.castCompleteCallback = BurstComplete;
		}
	}

	protected void BurstComplete()
	{
		burstCooldownTicksLeft = 300;
	}

	public override string GetInspectString()
	{
		return "Installed: " + gun.Label + "\n" + base.GetInspectString();
	}
}
