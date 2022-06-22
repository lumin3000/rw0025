using UnityEngine;

public class DialogBox_StorytellerChooser : DialogBox
{
	private const float TitleAreaHeight = 50f;

	private const float DescAreaHeight = 90f;

	private const float SpaceBelowStorytellers = 60f;

	private const float AdvancedAreaHeight = 100f;

	private Storyteller mouseoverTeller;

	private Vector2 tellersScrollSpot = default(Vector2);

	private static readonly Vector2 WinSize = new Vector2(900f, 750f);

	private static readonly Vector2 StorytellerPortSizeSmall = new Vector2(117f, 89f);

	private static readonly Vector2 StorytellerPortSizeLarge = new Vector2(234f, 178f);

	private static readonly Texture2D StorytellerHighlightTex = Res.LoadTexture("UI/HeroArt/Storytellers/Highlight");

	public override void DoDialogBoxGUI()
	{
		float num = Screen.width / 2;
		Vector2 winSize = WinSize;
		float left = num - winSize.x / 2f;
		float num2 = Screen.height / 2;
		Vector2 winSize2 = WinSize;
		float top = num2 - winSize2.y / 2f;
		Vector2 winSize3 = WinSize;
		float x = winSize3.x;
		Vector2 winSize4 = WinSize;
		Rect rect = new Rect(left, top, x, winSize4.y);
		UIWidgets.DrawWindow(rect);
		Rect innerRect = rect.GetInnerRect(17f);
		GUI.BeginGroup(innerRect);
		GenUI.SetFontMedium();
		GUI.Label(new Rect(0f, 0f, 300f, 300f), "Choose AI Storyteller");
		Rect position = new Rect(100f, 50f, innerRect.width - 200f, 90f);
		GenUI.SetFontSmall();
		GUI.Label(position, "The AI Storyteller controls incidents like raids, weather, and trader arrivals to create a certain kind of story in your colony.");
		float num3 = 140f;
		Rect position2 = new Rect(100f, num3, innerRect.width - 200f, innerRect.height - num3 - 50f);
		GUI.BeginGroup(position2);
		Vector2 storytellerPortSizeSmall = StorytellerPortSizeSmall;
		Rect position3 = new Rect(0f, 0f, storytellerPortSizeSmall.x + 24f, position2.height);
		Vector2 storytellerPortSizeSmall2 = StorytellerPortSizeSmall;
		Rect viewRect = new Rect(0f, 0f, storytellerPortSizeSmall2.x, 1000f);
		tellersScrollSpot = GUI.BeginScrollView(position3, tellersScrollSpot, viewRect);
		Vector2 storytellerPortSizeSmall3 = StorytellerPortSizeSmall;
		float x2 = storytellerPortSizeSmall3.x;
		Vector2 storytellerPortSizeSmall4 = StorytellerPortSizeSmall;
		Rect rect2 = new Rect(0f, 0f, x2, storytellerPortSizeSmall4.y);
		mouseoverTeller = null;
		foreach (Storyteller allStoryteller in MapInitParams.allStorytellers)
		{
			DrawStoryteller(rect2, allStoryteller);
			if (rect2.Contains(Event.current.mousePosition))
			{
				mouseoverTeller = allStoryteller;
			}
			rect2.y += rect2.height + 8f;
		}
		GUI.EndGroup();
		Rect position4 = new Rect(167f, 0f, position2.width - 150f - 34f, position2.height - 17f);
		GUI.BeginGroup(position4);
		Storyteller chosenStoryteller = mouseoverTeller;
		if (chosenStoryteller == null)
		{
			chosenStoryteller = MapInitParams.chosenStoryteller;
		}
		float num4 = position4.width / 2f;
		Vector2 storytellerPortSizeLarge = StorytellerPortSizeLarge;
		float left2 = num4 - storytellerPortSizeLarge.x / 2f;
		Vector2 storytellerPortSizeLarge2 = StorytellerPortSizeLarge;
		float x3 = storytellerPortSizeLarge2.x;
		Vector2 storytellerPortSizeLarge3 = StorytellerPortSizeLarge;
		Rect position5 = new Rect(left2, 0f, x3, storytellerPortSizeLarge3.y);
		GUI.DrawTexture(position5, chosenStoryteller.portrait);
		Vector2 storytellerPortSizeLarge4 = StorytellerPortSizeLarge;
		float top2 = storytellerPortSizeLarge4.y + 17f;
		float width = position4.width;
		float height = position4.height;
		Vector2 storytellerPortSizeLarge5 = StorytellerPortSizeLarge;
		Rect rect3 = new Rect(0f, top2, width, height - storytellerPortSizeLarge5.y - 17f - 100f);
		UIWidgets.DrawMenuSection(rect3);
		Rect innerRect2 = rect3.GetInnerRect(17f);
		GUI.BeginGroup(innerRect2);
		GUI.skin.label.alignment = TextAnchor.UpperLeft;
		GenUI.SetFontMedium();
		Rect position6 = new Rect(0f, 0f, innerRect2.width, 35f);
		GUI.Label(position6, chosenStoryteller.name);
		GenUI.SetFontSmall();
		Rect position7 = new Rect(0f, 35f, innerRect2.width, innerRect2.height - 35f);
		GUI.Label(position7, chosenStoryteller.description + "\n\n\"" + chosenStoryteller.quotation + "\"");
		GUI.EndGroup();
		Rect baseRect = new Rect(rect3);
		baseRect.y += baseRect.height;
		baseRect.height = 100f;
		GUI.BeginGroup(baseRect.GetInnerRect(17f));
		if (UIWidgets.TextButton(new Rect(10f, 10f, 90f, 32f), "Advanced"))
		{
			Find.UIRoot.dialogs.AddDialogBox(new DialogBox_AdvancedGameConfig());
		}
		GUI.EndGroup();
		GUI.EndGroup();
		GUI.EndGroup();
		EntryDialogUtility.DoNextBackButtons(rect, "Next", delegate
		{
			Find.Dialogs.AddDialogBox(new DialogBox_CharMaker());
		}, delegate
		{
			Find.Dialogs.AddDialogBox(new DialogBox_MainMenu());
		});
		GUI.EndGroup();
	}

	private void DrawStoryteller(Rect rect, Storyteller teller)
	{
		if (UIWidgets.ImageButton(rect, teller.portrait))
		{
			MapInitParams.chosenStoryteller = teller;
		}
		if (MapInitParams.chosenStoryteller == teller)
		{
			GUI.DrawTexture(rect, StorytellerHighlightTex);
		}
	}
}
