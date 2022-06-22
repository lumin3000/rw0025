using System;
using UnityEngine;

public static class ToilTools
{
	public static Func<bool> StandardTickFail(Pawn pawn, Thing targetThing)
	{
		return () => !CanInteractStandard(pawn, targetThing);
	}

	public static bool CanInteractStandard(Pawn pawn, Thing targetThing)
	{
		if (targetThing == null)
		{
			Debug.LogError(string.Concat("CanInteractStandard checked a null targetThing with pawn ", pawn, "."));
			return false;
		}
		if (targetThing.destroyed)
		{
			return false;
		}
		if (pawn.Team == TeamType.Colonist && targetThing.IsForbidden())
		{
			return false;
		}
		return true;
	}
}
