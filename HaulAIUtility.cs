public static class HaulAIUtility
{
	public static Job HaulJobFor(Pawn p, Thing t, bool ignoreDesignation)
	{
		if (!t.def.EverHaulable)
		{
			return null;
		}
		if (!ignoreDesignation && !t.def.alwaysHaulable && Find.DesignationManager.DesignationOn(t, DesignationType.Haul) == null)
		{
			return null;
		}
		if (!p.CanReserve(t, ReservationType.Total))
		{
			return null;
		}
		if (t.def.storeType == StoreType.Undefined)
		{
			return new Job(JobType.HaulToCargo, t);
		}
		if (t.IsInStorage() && t.StorageIsValid())
		{
			return null;
		}
		bool succeeded;
		IntVec3 intVec = StorageUtility.ClosestAvailableStorageSquareFor(t, out succeeded);
		if (!succeeded)
		{
			return null;
		}
		return new Job(JobType.HaulToSlot, t, intVec);
	}
}
