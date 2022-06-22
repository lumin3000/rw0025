using UnityEngine;

public class Trait : Saveable
{
	public TraitDefinition def;

	public Trait()
	{
	}

	public Trait(TraitDefinition def)
	{
		this.def = def;
	}

	public void ExposeData()
	{
		string value = string.Empty;
		if (def != null)
		{
			value = def.label;
		}
		Scribe.LookField(ref value, "DefName");
		def = TraitDefDatabase.DefinitionWithLabel(value);
		if (def == null)
		{
			Debug.LogWarning("Failed to load trait with label " + value + ". Replacing with ShootQuick.");
			def = TraitDefDatabase.DefinitionOf(TraitEffect.ShootQuick);
		}
	}

	public void DrawTrait(Vector2 Loc)
	{
		Rect rect = new Rect(Loc.x, Loc.y, 150f, 35f);
		GUI.Label(rect, def.label);
		TooltipHandler.TipRegion(rect, def.label + "\n\n" + def.description);
	}

	public override string ToString()
	{
		return "Trait(" + def.ToString() + ")";
	}
}
