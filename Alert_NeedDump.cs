using System.Linq;

public class Alert_NeedDump : Alert
{
	public override AlertReport Report
	{
		get
		{
			Thing thing = Find.DesignationManager.DesignatedThingsToHaul.Where((Thing h) => h.def.isDebris).FirstOrDefault();
			if (thing == null)
			{
				return false;
			}
			StorageUtility.ClosestAvailableStorageSquareFor(thing, out var succeeded);
			if (!succeeded)
			{
				return thing;
			}
			return false;
		}
	}

	public Alert_NeedDump()
	{
		basePriority = AlertPriority.Medium;
		baseLabel = "Need dump";
		baseExplanation = "You have debris your colonists want to haul, but they have nowhere to take it.\n\nBuild a dumping area and set its storage settings.";
	}
}
