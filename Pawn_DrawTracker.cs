using UnityEngine;

public class Pawn_DrawTracker : Saveable
{
	private const float MeleeJitterDistance = 0.5f;

	private Pawn pawn;

	public PawnTweener tweener;

	private JitterHandler jitterer;

	private PawnLeaner leaner;

	public PawnRotator rotator;

	public PawnRenderer renderer;

	public PawnUIOverlay ui;

	public Vector3 DrawPos
	{
		get
		{
			Vector3 tweenedPos = tweener.TweenedPos;
			tweenedPos += jitterer.CurrentJitterOffset;
			tweenedPos += leaner.LeanOffset;
			tweenedPos.y = pawn.def.altitude;
			return tweenedPos;
		}
	}

	public Pawn_DrawTracker(Pawn newPawn)
	{
		pawn = newPawn;
		tweener = new PawnTweener(pawn);
		jitterer = new JitterHandler();
		leaner = new PawnLeaner(pawn);
		rotator = new PawnRotator(pawn);
		renderer = new PawnRenderer(pawn);
		ui = new PawnUIOverlay(pawn);
	}

	public void DrawTrackerTick()
	{
		jitterer.JitterHandlerTick();
		tweener.TweenerTick();
		leaner.LeanerTick();
		rotator.PawnRotatorTick();
		renderer.RendererTick();
	}

	public void ExposeData()
	{
		Scribe.LookSaveable(ref renderer, "Renderer", pawn);
	}

	public void Notify_Spawned()
	{
		tweener.Notify_Spawned();
	}

	public void Notify_WarmingCastAlongLine(ShootLine newShootLine, IntVec3 ShootPosition)
	{
		leaner.Notify_WarmingCastAlongLine(newShootLine, ShootPosition);
	}

	public void Notify_DamageApplied(DamageInfo dinfo)
	{
		if (!pawn.destroyed)
		{
			jitterer.Notify_DamageApplied(dinfo);
			renderer.Notify_DamageApplied(dinfo);
		}
	}

	public void Notify_MeleeAttackOn(Thing Target)
	{
		if (Target.Position != pawn.Position)
		{
			jitterer.AddOffset(0.5f, (Target.Position - pawn.Position).AngleFlat);
		}
	}

	public void Notify_DebugAffected()
	{
		for (int i = 0; i < 10; i++)
		{
			MoteMaker.ThrowAirPuffUp(pawn.DrawPos, AltitudeLayer.PawnState);
		}
		jitterer.AddOffset(0.3f, Random.Range(0, 360));
	}
}
