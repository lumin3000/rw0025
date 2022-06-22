using UnityEngine;

public class ITab_Building_Storage : ITab
{
	private static readonly Vector2 WinSize = new Vector2(300f, 400f);

	public ITab_Building_Storage()
	{
		Size = WinSize;
		Label = "Storage";
	}

	protected override void FillTab()
	{
		SlotGroup slotGroup = ((Building_Storage)base.SelThing).slotGroup;
		GenUI.SetFontSmall();
		Vector2 winSize = WinSize;
		float x = winSize.x;
		Vector2 winSize2 = WinSize;
		Rect innerRect = new Rect(0f, 0f, x, winSize2.y).GetInnerRect(10f);
		UI_Listing uI_Listing = new UI_Listing(innerRect);
		foreach (StoreType storable in base.SelThing.def.storables)
		{
			if (storable != StoreType.Corpse)
			{
				bool flag = slotGroup.acceptSettings[storable];
				bool val = flag;
				uI_Listing.DoCheckbox(storable.ToString(), ref val);
				if (val != flag)
				{
					slotGroup.acceptSettings[storable] = val;
				}
			}
		}
		if (base.SelThing.def.storables.Contains(StoreType.Corpse))
		{
			uI_Listing.DoCheckbox("Corpses (colonists)", ref slotGroup.acceptColonistCorpses);
			uI_Listing.DoCheckbox("Corpses (strangers)", ref slotGroup.acceptStrangerCorpses);
			uI_Listing.DoCheckbox("Corpses (animals)", ref slotGroup.acceptAnimalCorpses);
		}
		uI_Listing.End();
	}
}
