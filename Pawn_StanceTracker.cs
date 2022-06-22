public class Pawn_StanceTracker
{
	public Pawn pawn;

	public Stance curStance = new Stance_Mobile();

	public StunHandler stunner;

	public bool FullBodyBusy
	{
		get
		{
			if (stunner.Stunned)
			{
				return true;
			}
			return curStance.StanceBusy();
		}
	}

	public Pawn_StanceTracker(Pawn newPawn)
	{
		pawn = newPawn;
		stunner = new StunHandler(pawn);
	}

	public void StanceTrackerTick()
	{
		stunner.StunHandlerTick();
		if (!stunner.Stunned)
		{
			curStance.StanceTick();
		}
	}

	public void StanceTrackerDraw()
	{
		curStance.StanceDraw();
	}

	public void CancelActionIfPossible()
	{
		if (curStance is Stance_Warmup)
		{
			SetStance(new Stance_Mobile());
		}
	}

	public void SetStance(Stance newStance)
	{
		newStance.stanceTracker = this;
		curStance = newStance;
	}

	public void Notify_DamageTaken(DamageInfo dinfo)
	{
	}
}
