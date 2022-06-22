using System;
using System.Collections.Generic;
using System.Linq;

public class Pawn_StoryTracker : Saveable
{
	private const int BaseSkillLevel = 3;

	private Pawn pawn;

	public List<int> storyItemIndices = new List<int>();

	public IEnumerable<PawnStoryItem> AllStoryItems
	{
		get
		{
			foreach (int index in storyItemIndices)
			{
				yield return PawnStoryDatabase.ItemAtIndex(index);
			}
		}
	}

	public PawnStoryItem Childhood => GetItemInSlot(CharHistorySlot.Childhood);

	public PawnStoryItem Adulthood => GetItemInSlot(CharHistorySlot.Adulthood);

	public IEnumerable<WorkType> DisabledWorks
	{
		get
		{
			foreach (PawnStoryItem item in AllStoryItems)
			{
				if (item == null)
				{
					continue;
				}
				foreach (WorkType disabledWorkType in item.DisabledWorkTypes)
				{
					yield return disabledWorkType;
				}
			}
		}
	}

	public IEnumerable<WorkTags> DisabledWorkTags
	{
		get
		{
			WorkTags combinedTags = WorkTags.None;
			foreach (PawnStoryItem item in AllStoryItems)
			{
				if (item != null)
				{
					combinedTags |= item.workDisables;
				}
			}
			foreach (int tag in Enum.GetValues(typeof(WorkTags)))
			{
				if (((uint)combinedTags & (uint)tag) != 0)
				{
					yield return (WorkTags)tag;
				}
			}
		}
	}

	public int NumStorySlots => Enum.GetValues(typeof(CharHistorySlot)).Length;

	public Pawn_StoryTracker(Pawn pawn)
	{
		this.pawn = pawn;
		for (int i = 0; i < NumStorySlots; i++)
		{
			storyItemIndices.Add(-1);
		}
	}

	public void ExposeData()
	{
		Scribe.LookList(ref storyItemIndices, "StoryItems");
	}

	public bool WorkIsDisabled(WorkType w)
	{
		return DisabledWorks.Contains(w);
	}

	public PawnStoryItem GetItemInSlot(CharHistorySlot slot)
	{
		return PawnStoryDatabase.ItemAtIndex(storyItemIndices[(int)slot]);
	}

	public void SetItemInSlot(CharHistorySlot slot, PawnStoryItem item)
	{
		storyItemIndices[(int)slot] = PawnStoryDatabase.IndexOfItem(item);
	}

	public void RandomizeHistory()
	{
		foreach (int value in Enum.GetValues(typeof(CharHistorySlot)))
		{
			SetItemInSlot((CharHistorySlot)value, PawnStoryDatabase.AllItemsFor(pawn.kindDef.historyCategory, (CharHistorySlot)value).RandomElement());
		}
	}

	public void MakeSkillsFromHistory()
	{
		foreach (SkillType item in SkillDefinitions.allSkills.Select((SkillDefinition s) => s.sType))
		{
			pawn.skills.SetLevel(item, FinalLevelOfSkill(item));
		}
	}

	private int FinalLevelOfSkill(SkillType skill)
	{
		int num = 3;
		foreach (PawnStoryItem item in AllStoryItems.Where((PawnStoryItem item) => item != null))
		{
			foreach (KeyValuePair<SkillType, int> skillGain in item.skillGains)
			{
				if (skillGain.Key == skill)
				{
					num += skillGain.Value;
				}
			}
		}
		if (num < 0)
		{
			num = 0;
		}
		if (num > 20)
		{
			num = 20;
		}
		return num;
	}
}
