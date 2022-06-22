using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CompressionUtility
{
	private const int NewlineInterval = 100;

	private static bool loggedByteCompressFailure;

	private static HashSet<Thing> referencedThings;

	public static void InitForSave()
	{
		referencedThings = new HashSet<Thing>();
		foreach (TargetPack item3 in Find.DesignationManager.designationList.Select((Designation des) => des.target))
		{
			Thing item = item3;
			referencedThings.Add(item);
		}
		foreach (TargetPack item4 in Find.ReservationManager.reservations.Select((ThingReservation res) => res.target))
		{
			Thing item2 = item4;
			referencedThings.Add(item2);
		}
	}

	public static bool IsSaveCompressible(this Thing t)
	{
		if (Scribe.writingForDebug)
		{
			return false;
		}
		if (!t.def.saveCompressible)
		{
			return false;
		}
		if (!t.def.useStandardHealth && t.health != t.def.maxHealth)
		{
			return false;
		}
		if (t.def.size.x > 1 || t.def.size.z > 1)
		{
			return false;
		}
		if (t.carrier != null)
		{
			return false;
		}
		if (referencedThings.Contains(t))
		{
			return false;
		}
		if (t.def.eType > (EntityType)255)
		{
			if (!loggedByteCompressFailure)
			{
				Debug.LogWarning(string.Concat("Cannot compress ThingType of ", t, " into a byte. Ignoring further warnings."));
				loggedByteCompressFailure = true;
			}
			return false;
		}
		return true;
	}
}
