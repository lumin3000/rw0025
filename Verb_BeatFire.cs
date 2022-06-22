using UnityEngine;

public class Verb_BeatFire : Verb
{
	private const int TargetCooldown = 50;

	public Verb_BeatFire()
	{
		vDefNative = NativeVerbDefDatabase.VerbWithID(VerbID.BeatFire);
	}

	protected override bool TryShotSpecialEffect()
	{
		Fire fire = (Fire)currentTarget.thing;
		Pawn ownerPawn = base.OwnerPawn;
		if (ownerPawn.stances.FullBodyBusy)
		{
			return false;
		}
		if (!CanHitTarget(fire))
		{
			Debug.LogWarning(string.Concat(ownerPawn, " beat flames ", fire, " from out of position."));
		}
		fire.TakeDamage(new DamageInfo(DamageType.Extinguish, 20));
		(fire.parent as Pawn)?.TakeDamage(new DamageInfo(DamageType.Stun, 10));
		GenSound.PlaySoundAt(ownerPawn.Position, GenSound.RandomClipInFolder("Impact/BeatFire"), 0.08f);
		ownerPawn.drawer.Notify_MeleeAttackOn(fire);
		return true;
	}
}
