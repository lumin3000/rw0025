using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Pawn_FilthTracker : Saveable
{
	private const float FilthPickupChance = 0.4f;

	private const float FilthDropChance = 0.15f;

	private const int MaxCarriedTerrainFilthThickness = 1;

	private Pawn pawn;

	private List<Filth> carriedFilth = new List<Filth>();

	public string FilthReport
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Carried filth:");
			if (carriedFilth.Count == 0)
			{
				stringBuilder.Append("(none)");
			}
			else
			{
				foreach (Filth item in carriedFilth)
				{
					stringBuilder.Append(item.Label);
					stringBuilder.AppendLine();
				}
			}
			return stringBuilder.ToString();
		}
	}

	public Pawn_FilthTracker(Pawn pawn)
	{
		this.pawn = pawn;
	}

	public void ExposeData()
	{
		Scribe.LookList(ref carriedFilth, "CarriedFilth");
	}

	public void Notify_EnteredNewSquare()
	{
		if (Random.value < 0.15f)
		{
			TryDropFilth();
		}
		if (Random.value < 0.4f)
		{
			TryPickupFilth();
		}
	}

	private void TryPickupFilth()
	{
		TerrainDefinition terrDef = Find.TerrainGrid.TerrainAt(pawn.Position);
		if (terrDef.TerrainFilthDef != null)
		{
			foreach (Filth item in carriedFilth.ListFullCopy())
			{
				if (item.def.isTerrainSourceFilth && item.def != terrDef.TerrainFilthDef)
				{
					ThinCarriedFilth(item);
				}
			}
			Filth filth = carriedFilth.Where((Filth f) => f.def == terrDef.TerrainFilthDef).FirstOrDefault();
			if (filth == null || filth.thickness < 1)
			{
				GainFilth(terrDef.TerrainFilthDef);
			}
		}
		foreach (Thing item2 in Find.Grids.ThingsAt(pawn.Position).ListFullCopy())
		{
			Filth filth2 = item2 as Filth;
			if (filth2 != null && filth2.CanPickUpNow)
			{
				GainFilth(filth2.def, filth2.sources);
				filth2.ThinFilth();
			}
		}
	}

	private void TryDropFilth()
	{
		if (carriedFilth.Count == 0)
		{
			return;
		}
		foreach (Filth item in carriedFilth.ListFullCopy())
		{
			if (item.CanDropAt(pawn.Position))
			{
				DropCarriedFilth(item);
			}
		}
	}

	private void DropCarriedFilth(Filth f)
	{
		ThinCarriedFilth(f);
		FilthUtility.AddFilthAt(pawn.Position, f.def, f.sources);
	}

	private void ThinCarriedFilth(Filth f)
	{
		f.ThinFilth();
		if (f.thickness <= 0)
		{
			carriedFilth.Remove(f);
		}
	}

	public void GainFilth(ThingDefinition filthDef)
	{
		GainFilth(filthDef, null);
	}

	public void GainFilth(ThingDefinition filthDef, List<string> sources)
	{
		Filth filth = carriedFilth.Where((Filth f) => f.def == filthDef).FirstOrDefault();
		if (filth != null)
		{
			if (filth.CanBeThickened)
			{
				filth.ThickenFilth();
				filth.AddSources(sources);
			}
		}
		else
		{
			Filth filth2 = (Filth)ThingMaker.MakeThing(filthDef);
			filth2.AddSources(sources);
			carriedFilth.Add(filth2);
		}
	}
}
