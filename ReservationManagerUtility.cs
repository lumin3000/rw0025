public static class ReservationManagerUtility
{
	public static bool CanReserve(this Pawn p, TargetPack target, ReservationType iType)
	{
		return Find.ReservationManager.CanReserveFor(p, target, iType);
	}

	public static bool TryReserve(this Pawn p, TargetPack target, ReservationType iType)
	{
		return Find.ReservationManager.TryReserveFor(p, target, iType);
	}
}
