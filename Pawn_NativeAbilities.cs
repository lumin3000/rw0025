using System;
using System.Collections.Generic;

public class Pawn_NativeAbilities
{
	private Pawn pawn;

	private Dictionary<MeleeAttackMode, Verb_MeleeAttack> meleeAbilities;

	private Verb_BeatFire beatFireVerb;

	private Verb_Ignite igniteVerb;

	public Pawn_NativeAbilities(Pawn pawn)
	{
		this.pawn = pawn;
		meleeAbilities = new Dictionary<MeleeAttackMode, Verb_MeleeAttack>();
		foreach (object value2 in Enum.GetValues(typeof(MeleeAttackMode)))
		{
			Verb_MeleeAttack value = new Verb_MeleeAttack((MeleeAttackMode)(int)value2)
			{
				owner = pawn
			};
			meleeAbilities.Add((MeleeAttackMode)(int)value2, value);
		}
		beatFireVerb = new Verb_BeatFire();
		beatFireVerb.owner = pawn;
		igniteVerb = new Verb_Ignite();
		igniteVerb.owner = pawn;
	}

	public bool CanTouch(Thing target)
	{
		return pawn.Position.AdjacentTo8WayOrInside(target);
	}

	public bool TryMeleeAttack(Thing target)
	{
		return TryMeleeAttack(target, MeleeAttackMode.LethalToIncap);
	}

	public bool TryMeleeAttack(Thing target, MeleeAttackMode mode)
	{
		if (pawn.stances.FullBodyBusy)
		{
			return false;
		}
		return meleeAbilities[mode].TryStartCastOn(target);
	}

	public bool TryIgnite(Thing target)
	{
		if (pawn.stances.FullBodyBusy)
		{
			return false;
		}
		return igniteVerb.TryStartCastOn(target);
	}

	public bool TryBeatFire(Fire targetFire)
	{
		if (pawn.stances.FullBodyBusy)
		{
			return false;
		}
		return beatFireVerb.TryStartCastOn(targetFire);
	}
}
