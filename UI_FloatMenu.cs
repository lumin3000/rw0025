using System.Collections.Generic;
using UnityEngine;

public class UI_FloatMenu
{
	private const float ChoiceSpacing = 4f;

	private const float Volume_MenuCreate = 0.07f;

	private const float Volume_MenuCancel = 0.15f;

	public bool active;

	private Vector2 clickRoot;

	private List<FloatMenuChoice> activeChoices = new List<FloatMenuChoice>();

	private static readonly Vector2 PawnNameOffset = new Vector2(30f, -25f);

	private static readonly AudioClip Sound_MenuCreate = (AudioClip)Resources.Load("Sounds/Interface/FloatMenuOpen");

	private static readonly AudioClip Sound_MenuCancel = (AudioClip)Resources.Load("Sounds/Interface/FloatMenuCancel");

	public void FloatMenuOnGUI()
	{
		if (active)
		{
			DoFloatMenuOnGUI();
		}
	}

	public void MakeAtMouseFor(Pawn p)
	{
		MakeFloatMenu(FloatMenuMaker.ChoicesAtFor(Gen.MouseWorldSquare(), p));
	}

	public void Cancel()
	{
		if (active && activeChoices.Count > 1)
		{
			GenSound.PlaySoundOnCamera(Sound_MenuCancel, 0.15f);
		}
		Close();
	}

	public void Close()
	{
		active = false;
		activeChoices.Clear();
	}

	private void MakeFloatMenu(List<FloatMenuChoice> newChoices)
	{
		if (newChoices.Count != 0)
		{
			if (newChoices.Count == 1 && newChoices[0].autoTakeable)
			{
				newChoices[0].Chosen();
				return;
			}
			active = true;
			clickRoot = Event.current.mousePosition;
			activeChoices = newChoices;
			GenSound.PlaySoundOnCamera(Sound_MenuCreate, 0.07f);
			MouseoverSounds.SilenceForNextFrame();
		}
	}

	private void DoFloatMenuOnGUI()
	{
		if (!active)
		{
			return;
		}
		if (Find.Selector.SingleSelectedThing == null)
		{
			Close();
			return;
		}
		Vector2 root = clickRoot;
		GenUI.SetFontSmall();
		float x = root.x;
		Vector2 pawnNameOffset = PawnNameOffset;
		float left = x + pawnNameOffset.x;
		float y = root.y;
		Vector2 pawnNameOffset2 = PawnNameOffset;
		Rect position = new Rect(left, y + pawnNameOffset2.y, 150f, 23f);
		GUI.DrawTexture(position, GenUI.TextBGBlack);
		position.width = 999f;
		position.x += 15f;
		GUI.skin.label.alignment = TextAnchor.MiddleLeft;
		GUI.Label(position, Find.Selector.SingleSelectedThing.Label);
		GUI.skin.label.alignment = TextAnchor.UpperLeft;
		foreach (FloatMenuChoice activeChoice in activeChoices)
		{
			if (activeChoice.ChoiceButton(root))
			{
				return;
			}
			float y2 = root.y;
			Vector2 size = FloatMenuChoice.Size;
			root.y = y2 + (size.y + 4f);
		}
		if (Event.current.type == EventType.MouseDown && (Event.current.button == 0 || Event.current.button == 1))
		{
			Cancel();
			Event.current.Use();
		}
		GenUI.AbsorbAllInput();
	}
}
