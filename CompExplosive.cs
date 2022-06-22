using System;
using UnityEngine;

public class CompExplosive : ThingComp
{
	public float explosiveRadius;

	public DamageType explosiveDamType;

	public float startWickHealthPercent = 0.2f;

	public int wickTicksMin = 150;

	public int wickTicksMax = 150;

	public float wickScale = 2.4f;

	protected bool wickStarted;

	protected int wickTicksLeft;

	protected SoundLooper wickSoundLooper;

	private static readonly AudioClip WickStartSound = Res.LoadSound("Hiss/MetalHit");

	private static readonly AudioClip WickLoopSound = Res.LoadSound("Hiss/HissSmall");

	private bool detonated;

	protected int StartWickThreshold => (int)Math.Round(startWickHealthPercent * (float)parent.def.maxHealth);

	public override void CompTick()
	{
		if (wickStarted)
		{
			wickSoundLooper.Maintain();
			wickTicksLeft--;
			if (wickTicksLeft <= 0)
			{
				Detonate();
			}
		}
	}

	public override void CompDraw()
	{
		if (wickStarted)
		{
			OverlayDrawer.DrawOverlay(parent, OverlayTypes.BurningWick);
		}
	}

	public override void CompApplyDamage(DamageInfo dinfo)
	{
		if (parent.health <= 0)
		{
			Detonate();
		}
		else if (wickStarted && dinfo.type == DamageType.Stun)
		{
			StopWick();
		}
		else if (!wickStarted && parent.health <= StartWickThreshold)
		{
			StartWick();
		}
	}

	public override void CompKilled(DamageInfo dam)
	{
		Detonate();
	}

	public void StartWick()
	{
		if (wickStarted)
		{
			Debug.LogWarning("Started wick twice on " + parent);
			return;
		}
		wickStarted = true;
		wickTicksLeft = UnityEngine.Random.Range(wickTicksMin, wickTicksMax);
		GenSound.PlaySoundAt(parent.Position, WickStartSound, 0.4f);
		wickSoundLooper = new SoundLooperThing(parent, WickLoopSound, 0.3f, SoundLooperMaintenanceType.PerTick);
	}

	public void StopWick()
	{
		wickStarted = false;
	}

	protected void Detonate()
	{
		if (!detonated)
		{
			detonated = true;
			if (!parent.destroyed)
			{
				parent.Killed(new DamageInfo(explosiveDamType, 50));
			}
			Explosion.DoExplosion(parent.Position, explosiveRadius, explosiveDamType);
		}
	}
}
