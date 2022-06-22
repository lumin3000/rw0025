using UnityEngine;

internal class Verb_Ignite : Verb
{
	public Verb_Ignite()
	{
		vDefNative = NativeVerbDefDatabase.VerbWithID(VerbID.Ignite);
	}

	protected override bool TryShotSpecialEffect()
	{
		Thing thing = currentTarget.thing;
		Pawn ownerPawn = base.OwnerPawn;
		if (ownerPawn.stances.FullBodyBusy)
		{
			return false;
		}
		if (!CanHitTarget(thing))
		{
			Debug.LogWarning(string.Concat(ownerPawn, " ignited fire on ", thing, " from out of position."));
		}
		FireUtility.TryStartFireIn(thing.Position, 0.3f);
		GenSound.PlaySoundAt(ownerPawn.Position, GenSound.RandomClipInFolder("Impact/Ignite"), 0.08f);
		ownerPawn.drawer.Notify_MeleeAttackOn(thing);
		return true;
	}
}
