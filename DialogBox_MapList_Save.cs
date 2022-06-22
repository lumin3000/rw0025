using System.Text.RegularExpressions;
using UnityEngine;

public class DialogBox_MapList_Save : DialogBox_MapList
{
	protected const float NewSaveNameWidth = 400f;

	protected const float NewSaveHeight = 35f;

	protected const float NewSaveNameButtonSpace = 20f;

	protected const int NameMaxLength = 28;

	private bool focusedMapNameArea;

	public DialogBox_MapList_Save()
	{
		interactButLabel = "Overwrite";
		bottomAreaHeight = 85f;
	}

	protected override void DoMapEntryInteraction(string MapName)
	{
		Find.Map.info.fileName = MapName;
		MapSaveLoad.SaveToFile(Find.Map, Find.Map.info.fileName);
		UI_Messages.Message("Saved as " + Find.Map.info.fileName + ".");
		Find.Dialogs.PopBox();
	}

	protected override void DoSpecialSaveLoadGUI(Rect InRect)
	{
		GUI.BeginGroup(InRect);
		bool flag = Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return;
		float top = InRect.height - 52f;
		GenUI.SetFontSmall();
		GUI.skin.textField.alignment = TextAnchor.MiddleLeft;
		GUI.skin.textField.contentOffset = new Vector2(12f, 0f);
		GUI.skin.settings.doubleClickSelectsWord = true;
		GUI.SetNextControlName("MapNameField");
		string text = GUI.TextField(new Rect(5f, top, 400f, 35f), Find.Map.info.fileName);
		if (IsValidSaveGameName(text))
		{
			Find.Map.info.fileName = text;
		}
		if (!focusedMapNameArea)
		{
			GUI.FocusControl("MapNameField");
			focusedMapNameArea = true;
		}
		Rect butRect = new Rect(420f, top, InRect.width - 400f - 20f, 35f);
		if (UIWidgets.TextButton(butRect, "Save") || flag)
		{
			if (Find.Map.info.fileName.Length == 0)
			{
				UI_Messages.Message("Please enter a name.", UIMessageSound.Reject);
			}
			else
			{
				MapSaveLoad.SaveToFile(Find.Map, Find.Map.info.fileName);
				UI_Messages.Message("Saved as " + Find.Map.info.fileName + ".");
				Find.Dialogs.PopBox();
			}
		}
		GUI.skin.label.alignment = TextAnchor.UpperLeft;
		GUI.EndGroup();
	}

	public static bool IsValidSaveGameName(string Name)
	{
		if (Name.Length > 28)
		{
			return false;
		}
		Regex regex = new Regex("^[a-zA-Z0-9 ]*$");
		return regex.IsMatch(Name);
	}
}
