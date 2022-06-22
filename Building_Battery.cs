using System.Linq;
using UnityEngine;

public class Building_Battery : Building
{
	private const float MinEnergyToExplode = 500f;

	private const float EnergyToLoseWhenExplode = 400f;

	private const float ExplodeChancePerDamage = 0.05f;

	private int ticksToExplode;

	private SoundLooper wickLooper;

	private static readonly Vector2 BarSize = new Vector2(1.3f, 0.4f);

	private static readonly Material BarFilledMat = GenRender.SolidColorMaterial(new Color(0.9f, 0.85f, 0.2f));

	private static readonly Material BarUnfilledMat = GenRender.SolidColorMaterial(new Color(0.3f, 0.3f, 0.3f));

	private static readonly AudioClip WickLoopSound = Res.LoadSound("Hiss/HissSmall");

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe.LookField(ref ticksToExplode, "TicksToExplode", 0);
	}

	public override void Draw()
	{
		base.Draw();
		CompPowerBattery comp = GetComp<CompPowerBattery>();
		GenRender.FillableBarRequest fillableBarRequest = new GenRender.FillableBarRequest();
		fillableBarRequest.Center = DrawPos + Vector3.up * 0.1f;
		fillableBarRequest.BarSize = BarSize;
		fillableBarRequest.FillPercent = comp.storedEnergy / comp.storedEnergyMax;
		fillableBarRequest.FilledMat = BarFilledMat;
		fillableBarRequest.UnfilledMat = BarUnfilledMat;
		fillableBarRequest.Margin = 0.15f;
		IntRot intRot = rotation;
		intRot.Rotate(RotationDirection.Clockwise);
		fillableBarRequest.Rotation = intRot;
		GenRender.RenderFillableBar(fillableBarRequest);
		if (ticksToExplode > 0)
		{
			OverlayDrawer.DrawOverlay(this, OverlayTypes.BurningWick);
		}
	}

	public override void Tick()
	{
		base.Tick();
		if (ticksToExplode > 0)
		{
			wickLooper.Maintain();
			ticksToExplode--;
			if (ticksToExplode == 0)
			{
				IntVec3 loc = Gen.SquaresOccupiedBy(this).ToList().RandomElement();
				float radius = Random.Range(0.5f, 1f) * 3f;
				Explosion.DoExplosion(loc, radius, DamageType.Flame);
				GetComp<CompPowerBattery>().storedEnergy -= 400f;
			}
		}
	}

	protected override void ApplyDamage(DamageInfo dinfo)
	{
		base.ApplyDamage(dinfo);
		if (ticksToExplode == 0 && dinfo.type == DamageType.Flame && Random.value < 0.05f && GetComp<CompPowerBattery>().storedEnergy > 500f)
		{
			ticksToExplode = Random.Range(70, 150);
			wickLooper = new SoundLooperThing(this, WickLoopSound, 0.2f, SoundLooperMaintenanceType.PerTick);
		}
	}
}
