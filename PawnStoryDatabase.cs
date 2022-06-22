using System.Collections.Generic;
using System.Linq;

public static class PawnStoryDatabase
{
	private static List<PawnStoryItem> allItems;

	static PawnStoryDatabase()
	{
		allItems = DataLoader.LoadDataInFolder<PawnStoryItem>("CharHistories");
	}

	public static IEnumerable<PawnStoryItem> AllItemsFor(CharHistoryCategory cat, CharHistorySlot slot)
	{
		return from item in allItems
			where item.categories.Contains(cat)
			where item.slot == slot
			select item;
	}

	public static PawnStoryItem ItemAtIndex(int index)
	{
		if (index == -1)
		{
			return null;
		}
		return allItems[index];
	}

	public static int IndexOfItem(PawnStoryItem item)
	{
		if (item == null)
		{
			return -1;
		}
		return allItems.IndexOf(item);
	}
}
