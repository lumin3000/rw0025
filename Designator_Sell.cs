using System.Linq;

public class Designator_Sell : Designator
{
	public Designator_Sell()
	{
		buttonLabel = "Sell";
		defaultDesc = "Deconstruct buildings and reclaim part of their resources.";
		buttonTexture = Res.LoadTexture("UI/DesignationIcons/Sell");
		dragStartClip = UISounds.Click;
		dragProgressClip = UISounds.DragLoopMeta;
		useMouseIcon = true;
	}

	public override AcceptanceReport CanDesignateAt(IntVec3 loc)
	{
		AcceptanceReport acceptanceReport = base.CanDesignateAt(loc);
		if (!acceptanceReport.accepted)
		{
			return acceptanceReport;
		}
		Thing thing = TopSellableInSquare(loc);
		if (thing != null)
		{
			if (thing.def.eType == EntityType.Area_Stockpile && Find.BuildingManager.AllBuildingsColonistOfType(EntityType.Area_Stockpile).Count() <= 1)
			{
				return "Cannot sell all stockpiles. Place another first.";
			}
			if (thing.def.useStandardHealth && thing.health < thing.def.maxHealth)
			{
				return "Cannot sell damaged buildings.";
			}
			return AcceptanceReport.WasAccepted;
		}
		return AcceptanceReport.WasRejected;
	}

	public override void DesignateAt(IntVec3 Loc)
	{
		DoDeconstruct(TopSellableInSquare(Loc));
	}

	private static Thing TopSellableInSquare(IntVec3 loc)
	{
		foreach (Thing item in from t in Find.Grids.ThingsAt(loc)
			orderby t.def.altitudeLayer descending
			select t)
		{
			if ((Game.InEditMode || item.Team == TeamType.Colonist) && item.def.category == EntityCategory.Building)
			{
				return item;
			}
		}
		return null;
	}

	public override void FinalizeDesignationSucceeded()
	{
		GenSound.PlaySoundOnCamera("Interface/Deconstruct", 0.15f);
	}

	protected void DoDeconstruct(Thing t)
	{
		if (Game.InEditMode)
		{
			t.Destroy();
		}
		else if (t.def.eType == EntityType.Blueprint)
		{
			foreach (ResourceCost cost in (t as Blueprint).def.ThingDefToBuild.costList)
			{
				Find.ResourceManager.Gain(cost.rType, cost.Amount);
			}
			t.Destroy();
		}
		else
		{
			Building building = t as Building;
			if (building != null)
			{
				GenMap.ReclaimResourcesFor(t);
				building.Destroy();
			}
		}
	}

	public override void DesignatorUpdate()
	{
		GenUI.RenderMouseoverBracket();
	}
}
