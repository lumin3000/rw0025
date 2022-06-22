public class StunHandler
{
	public Thing baseThing;

	private int stunTicksLeft;

	private Mote moteStun;

	public bool Stunned => stunTicksLeft > 0;

	public StunHandler(Thing Base)
	{
		baseThing = Base;
	}

	public void StunHandlerTick()
	{
		if (stunTicksLeft > 0)
		{
			stunTicksLeft--;
			Pawn pawn = baseThing as Pawn;
			if (pawn != null && pawn.Incapacitated)
			{
				stunTicksLeft = 0;
			}
			if (moteStun != null)
			{
				moteStun.Maintain();
			}
		}
	}

	public void Notify_DamageApplied(DamageInfo dinfo)
	{
		if (dinfo.type == DamageType.Stun)
		{
			StunFor(dinfo.Amount * 11);
		}
	}

	protected void StunFor(int NumTicks)
	{
		stunTicksLeft = NumTicks;
		if (moteStun == null)
		{
			moteStun = MoteMaker.MakeStunOverlay(baseThing);
		}
	}
}
