using System;
using UnityEngine;

public class DiaOption
{
	protected string Text;

	public DiaNode Link;

	public bool ResolveTree;

	public bool PassTurnIfWorldMode = true;

	public Action ChosenCallback;

	public bool Disabled;

	public string DisabledReason = string.Empty;

	protected readonly Color DisabledOptionColor = new Color(1f, 0.5f, 0.5f);

	protected DialogBox_DialogTree OwnerBox => Find.UIMapRoot.dialogs.TopDialog as DialogBox_DialogTree;

	public static DiaOption DefaultOK
	{
		get
		{
			DiaOption diaOption = new DiaOption("OK");
			diaOption.ResolveTree = true;
			return diaOption;
		}
	}

	public DiaOption()
	{
		Text = "OK";
	}

	public DiaOption(string Text)
	{
		this.Text = Text;
	}

	public DiaOption(DiaOptionDef Def)
	{
		Text = Def.Text;
		if (Def.ReqSet != null && !Def.ReqSet.RequirementsAreSatisfied())
		{
			Disable(Def.ReqSet.DissatisfactionReason());
		}
		DiaNodeDef diaNodeDef = Def.RandomLinkNode();
		if (diaNodeDef != null)
		{
			Link = new DiaNode(diaNodeDef);
		}
	}

	public void Disable(string newDisabledReason)
	{
		Disabled = true;
		DisabledReason = newDisabledReason;
	}

	public void OptOnGUI(Rect drawRect)
	{
		string text = Text;
		if (Disabled)
		{
			GUI.color = DisabledOptionColor;
			text = text + " (" + DisabledReason + ")";
		}
		if (UIWidgets.TextButtonNaked(drawRect, text))
		{
			OptionChosen();
		}
	}

	protected void OptionChosen()
	{
		GenSound.PlaySoundOnCamera(UISounds.PageChange, 0.1f);
		if (ResolveTree)
		{
			Find.Dialogs.PopBox();
		}
		if (ChosenCallback != null)
		{
			ChosenCallback();
		}
		if (Link != null)
		{
			OwnerBox.GotoNode(Link);
		}
	}
}
