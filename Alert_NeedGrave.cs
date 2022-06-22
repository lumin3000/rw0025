using System.Linq;

public class Alert_NeedGrave : Alert
{
	public override AlertReport Report
	{
		get
		{
			Thing thing = Find.ThingLister.spawnedHaulables.Where((Thing h) => h.TType == EntityType.Corpse && !h.IsForbidden()).FirstOrDefault();
			if (thing == null)
			{
				return false;
			}
			if (thing.IsInStorage())
			{
				return false;
			}
			StorageUtility.ClosestAvailableStorageSquareFor(thing, out var succeeded);
			if (succeeded)
			{
				return false;
			}
			return thing;
		}
	}

	public Alert_NeedGrave()
	{
		basePriority = AlertPriority.High;
		baseLabel = "Need grave";
		baseExplanation = "There is a dead bodies laying around and nowhere for your colonists to take it.\n\nBuild a grave or a dumping area and set their storage settings.";
	}
}
