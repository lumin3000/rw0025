using System.Collections.Generic;
using UnityEngine;

public class DiaNode
{
	public string text;

	public List<DiaOption> optionList = new List<DiaOption>();

	public List<Tradeable> rewardsGiven = new List<Tradeable>();

	protected DiaNodeDef def;

	protected DialogBox_DialogTree OwnerBox => Find.UIMapRoot.dialogs.TopDialog as DialogBox_DialogTree;

	public DiaNode(string Text)
	{
		text = Text;
	}

	public DiaNode(DiaNodeDef newDef)
	{
		def = newDef;
		def.Used = true;
		text = def.Texts.RandomElement();
		rewardsGiven = GenRewards.GenerateRewards(def.Reward);
		if (def.OptionList.Count > 0)
		{
			foreach (DiaOptionDef option in def.OptionList)
			{
				optionList.Add(new DiaOption(option));
			}
		}
		else
		{
			optionList.Add(new DiaOption("OK"));
		}
	}

	public void Opened()
	{
		if (def != null && def.ResourceModifications != null)
		{
			def.ResourceModifications.DoResourceModifications();
		}
	}

	public void NodeOnGUI(Rect drawRect)
	{
		GUI.BeginGroup(drawRect);
		GenUI.SetFontSmall();
		GUI.Label(new Rect(5f, 5f, drawRect.width - 10f, drawRect.height - 10f), text);
		int num = optionList.Count;
		if (num < 3)
		{
			num = 3;
		}
		float num2 = drawRect.height - (float)(40 * num);
		foreach (DiaOption option in optionList)
		{
			option.OptOnGUI(new Rect(20f, num2, drawRect.width - 40f, 40f));
			num2 += 40f;
		}
		GUI.EndGroup();
	}

	public void PreClose()
	{
		if (rewardsGiven == null)
		{
			return;
		}
		foreach (Tradeable item in rewardsGiven)
		{
			item.GiveToPlayer();
		}
	}
}
