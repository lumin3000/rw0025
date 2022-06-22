using UnityEngine;

public class Jetter : Thing
{
	private enum JetterState
	{
		Resting,
		WickBurning,
		Jetting
	}

	private const int TicksBeforeBeginAccelerate = 25;

	private const int TicksBetweenMoves = 3;

	private JetterState JState;

	private int WickTicksLeft;

	private int TicksUntilMove;

	protected SoundLooper WickSoundLooper;

	protected SoundLooper JetSoundLooper;

	public override void Tick()
	{
		if (JState == JetterState.WickBurning)
		{
			OverlayDrawer.DrawOverlay(this, OverlayTypes.BurningWick);
			WickTicksLeft--;
			if (WickTicksLeft == 0)
			{
				StartJetting();
			}
		}
		else if (JState == JetterState.Jetting)
		{
			TicksUntilMove--;
			if (TicksUntilMove <= 0)
			{
				MoveJetter();
				TicksUntilMove = 3;
			}
		}
	}

	protected override void ApplyDamage(DamageInfo dinfo)
	{
		if (dinfo.type.HarmsHealth() && JState == JetterState.Resting)
		{
			StartWick();
		}
	}

	protected void StartWick()
	{
		JState = JetterState.WickBurning;
		WickTicksLeft = 25;
		GenSound.PlaySoundAt(base.Position, "Hiss/MetalHit", 1f);
		WickSoundLooper = new SoundLooperThing(this, (AudioClip)Resources.Load("Sounds/Hiss/HissSmall"), 0.5f);
	}

	protected void StartJetting()
	{
		JState = JetterState.Jetting;
		TicksUntilMove = 3;
		WickSoundLooper.Cleanup();
		WickSoundLooper = null;
		AudioClip looperClip = (AudioClip)Resources.Load("Sounds/Hiss/HissJet");
		JetSoundLooper = new SoundLooperThing(this, looperClip, 1f);
	}

	protected void MoveJetter()
	{
		IntVec3 intVec = base.Position + rotation.FacingSquare;
		if (!intVec.Walkable() || Find.Grids.SquareContains(intVec, EntityType.Pawn) || Find.Grids.BlockerAt(intVec) != null)
		{
			Destroy();
			Explosion.DoExplosion(base.Position, 2.9f, DamageType.Bomb);
		}
		else
		{
			base.Position = intVec;
		}
	}

	public override void Destroy()
	{
		base.Destroy();
		if (WickSoundLooper != null)
		{
			WickSoundLooper.Cleanup();
			WickSoundLooper = null;
		}
		if (JetSoundLooper != null)
		{
			JetSoundLooper.Cleanup();
			JetSoundLooper = null;
		}
	}
}
