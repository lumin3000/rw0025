using System.Collections.Generic;
using UnityEngine;

public class Pawn_TraitsTracker : Saveable
{
	protected Pawn pawn;

	public List<Trait> traitList = new List<Trait>();

	public Pawn_TraitsTracker(Pawn pawn)
	{
		this.pawn = pawn;
	}

	public void ExposeData()
	{
		Scribe.LookList(ref traitList, "TraitList");
	}

	public void GainTrait(TraitDefinition tDef)
	{
		if (HasTrait(tDef))
		{
			Debug.LogWarning(string.Concat(pawn, " already has trait ", tDef));
			return;
		}
		Trait item = new Trait(tDef);
		traitList.Add(item);
	}

	public bool HasTrait(TraitDefinition tDef)
	{
		foreach (Trait trait in traitList)
		{
			if (trait.def == tDef)
			{
				return true;
			}
		}
		return false;
	}

	public bool HasTraitEffect(TraitEffect effect)
	{
		foreach (Trait trait in traitList)
		{
			if (trait.def.effect == effect)
			{
				return true;
			}
		}
		return false;
	}

	public void MakeRandomTraits()
	{
		while (traitList.Count < 2)
		{
			TraitDefinition tDef = TraitDefDatabase.allTraitDefs.RandomElement();
			if (!HasTrait(tDef))
			{
				GainTrait(tDef);
			}
		}
	}
}
