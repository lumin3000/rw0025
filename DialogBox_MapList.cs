using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public abstract class DialogBox_MapList : DialogBox
{
	private const float BoxWidth = 600f;

	private const float BoxHeight = 700f;

	protected const float BoxMargin = 20f;

	protected const float MapEntrySpacing = 8f;

	protected const float MapEntryMargin = 6f;

	protected const float MapNameExtraLeftMargin = 15f;

	protected const float MapDateExtraLeftMargin = 220f;

	protected const float DeleteButtonSpace = 5f;

	protected string interactButLabel = "Error";

	protected float bottomAreaHeight;

	private Vector2 scrollPosition = Vector2.zero;

	protected readonly Vector2 MapEntrySize;

	protected readonly Vector2 InteractButSize;

	protected readonly Texture2D DeleteButtonIcon = Res.LoadTexture("UI/Widgets/DeleteButton");

	private static readonly Color ManualSaveTextColor = new Color(1f, 1f, 0.6f);

	private static readonly Color AutosaveTextColor = new Color(0.75f, 0.75f, 0.75f);

	private float DeleteButtonSize
	{
		get
		{
			Vector2 interactButSize = InteractButSize;
			return interactButSize.y;
		}
	}

	public DialogBox_MapList()
	{
		MapEntrySize = new Vector2(536f, 48f);
		InteractButSize = new Vector2(100f, MapEntrySize.y - 12f);
		SetWinCentered(600f, 700f);
	}

	public override void DoDialogBoxGUI()
	{
		UIWidgets.DrawWindow(winRect);
		Rect innerRect = winRect.GetInnerRect(20f);
		innerRect.height -= 45f;
		List<FileInfo> list = MapFiles.AllMapFiles.ToList();
		Vector2 mapEntrySize = MapEntrySize;
		float num = mapEntrySize.y + 8f;
		float height = (float)list.Count * num;
		Rect viewRect = new Rect(0f, 0f, innerRect.width - 24f, height);
		Rect position = new Rect(innerRect);
		position.height -= bottomAreaHeight;
		scrollPosition = GUI.BeginScrollView(position, scrollPosition, viewRect);
		float num2 = 0f;
		foreach (FileInfo item in list)
		{
			float top = num2;
			Vector2 mapEntrySize2 = MapEntrySize;
			float x = mapEntrySize2.x;
			Vector2 mapEntrySize3 = MapEntrySize;
			Rect rect = new Rect(0f, top, x, mapEntrySize3.y);
			UIWidgets.DrawMenuSection(rect);
			Rect innerRect2 = rect.GetInnerRect(6f);
			GUI.BeginGroup(innerRect2);
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(item.Name);
			if (MapFiles.IsAutoSave(fileNameWithoutExtension))
			{
				GUI.color = AutosaveTextColor;
			}
			else
			{
				GUI.color = ManualSaveTextColor;
			}
			Rect position2 = new Rect(15f, 0f, innerRect2.width, innerRect2.height);
			GUI.skin.label.alignment = TextAnchor.MiddleLeft;
			GenUI.SetFontSmall();
			GUI.Label(position2, fileNameWithoutExtension);
			GUI.color = Color.white;
			Rect position3 = new Rect(220f, 0f, innerRect2.width, innerRect2.height);
			GenUI.SetFontTiny();
			GUI.color = new Color(1f, 1f, 1f, 0.5f);
			GUI.Label(position3, item.LastWriteTime.ToString());
			GUI.color = Color.white;
			GUI.skin.label.alignment = TextAnchor.UpperLeft;
			GenUI.SetFontSmall();
			Vector2 mapEntrySize4 = MapEntrySize;
			float num3 = mapEntrySize4.x - 12f;
			Vector2 interactButSize = InteractButSize;
			float num4 = num3 - interactButSize.x - DeleteButtonSize;
			Vector2 interactButSize2 = InteractButSize;
			float x2 = interactButSize2.x;
			Vector2 interactButSize3 = InteractButSize;
			Rect butRect = new Rect(num4, 0f, x2, interactButSize3.y);
			if (UIWidgets.TextButton(butRect, interactButLabel))
			{
				DoMapEntryInteraction(Path.GetFileNameWithoutExtension(item.Name));
			}
			Vector2 interactButSize4 = InteractButSize;
			Rect rect2 = new Rect(num4 + interactButSize4.x + 5f, 0f, DeleteButtonSize, DeleteButtonSize);
			if (UIWidgets.ImageButton(rect2, DeleteButtonIcon))
			{
				Find.UIRoot.dialogs.AddDialogBox(new DialogBox_ConfirmDelete(item));
			}
			TooltipHandler.TipRegion(rect2, "Delete this savegame.");
			GUI.EndGroup();
			float num5 = num2;
			Vector2 mapEntrySize5 = MapEntrySize;
			num2 = num5 + (mapEntrySize5.y + 8f);
		}
		GUI.EndScrollView();
		DoSpecialSaveLoadGUI(innerRect);
		DetectShouldClose(doButton: true);
		GenUI.AbsorbAllInput();
	}

	protected virtual void DoSpecialSaveLoadGUI(Rect InRect)
	{
	}

	protected abstract void DoMapEntryInteraction(string MapName);
}
