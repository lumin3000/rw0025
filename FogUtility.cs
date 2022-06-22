public static class FogUtility
{
	public static bool IsFogged(this IntVec3 Sq)
	{
		return Find.FogGrid.IsFogged(Sq);
	}
}
