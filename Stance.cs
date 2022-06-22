public abstract class Stance
{
	public Pawn_StanceTracker stanceTracker;

	public virtual bool StanceBusy()
	{
		return false;
	}

	public virtual void StanceTick()
	{
	}

	public virtual void StanceDraw()
	{
	}
}
