using UnityEngine;

public class Letter : Saveable
{
	public const float DrawWidth = 38f;

	public const float DrawHeight = 30f;

	private const float FallTime = 1f;

	private const float FallDistance = 200f;

	public string text = "No text.";

	public TargetPack lookTarget;

	private bool opened;

	public float arrivalTime;

	private static readonly Texture2D IconUnopened = Res.LoadTexture("UI/Letters/LetterUnopened");

	private static readonly Texture2D IconOpened = Res.LoadTexture("UI/Letters/LetterUnopened");

	private Texture2D CurIcon
	{
		get
		{
			if (opened)
			{
				return IconOpened;
			}
			return IconUnopened;
		}
	}

	public Letter()
	{
	}

	public Letter(string text)
	{
		this.text = text;
	}

	public Letter(string text, TargetPack lookTarget)
		: this(text)
	{
		this.lookTarget = lookTarget;
	}

	public void ExposeData()
	{
		Scribe.LookField(ref text, "Text");
		Scribe.LookField(ref opened, "Opened", defaultValue: false, forceSave: false);
		Scribe.LookSaveable(ref lookTarget, "LookTarget");
	}

	public void DrawButtonAt(Vector2 topLeft)
	{
		float num = topLeft.y;
		float num2 = Time.time - arrivalTime;
		if (num2 < 1f)
		{
			num -= (1f - num2) * 200f;
			GUI.color = new Color(1f, 1f, 1f, num2 / 1f);
		}
		Rect rect = new Rect(topLeft.x, num, 38f, 30f);
		UIWidgets.DrawShadowAround(rect);
		if (rect.Contains(Event.current.mousePosition))
		{
			GenUI.SetFontSmall();
			GUI.skin.label.alignment = TextAnchor.UpperLeft;
			float num3 = GUI.skin.label.CalcHeight(new GUIContent(text), 310f);
			num3 += 20f;
			Rect rect2 = new Rect(topLeft.x - 330f - 10f, topLeft.y - num3 / 2f, 330f, num3);
			UIWidgets.DrawWindow(rect2);
			Rect innerRect = rect2.GetInnerRect(10f);
			GUI.BeginGroup(innerRect);
			GUI.Label(new Rect(0f, 0f, innerRect.width, innerRect.height), text);
			GUI.EndGroup();
		}
		if (Event.current.type == EventType.MouseDown && Event.current.button == 1 && rect.Contains(Event.current.mousePosition))
		{
			GenSound.PlaySoundOnCamera(UISounds.Click, 0.1f);
			Find.LetterStack.RemoveLetter(this);
			Event.current.Use();
		}
		if (UIWidgets.ImageButton(rect, CurIcon))
		{
			OpenLetter();
			Event.current.Use();
		}
		GUI.color = Color.white;
	}

	private void OpenLetter()
	{
		opened = true;
		DiaNode diaNode = new DiaNode(text);
		DiaOption diaOption = new DiaOption("OK");
		diaOption.ChosenCallback = delegate
		{
			Find.LetterStack.RemoveLetter(this);
		};
		diaOption.ResolveTree = true;
		diaNode.optionList.Add(diaOption);
		if (lookTarget != null)
		{
			DiaOption diaOption2 = new DiaOption("Jump to location");
			diaOption2.ChosenCallback = delegate
			{
				Find.CameraMap.JumpTo(lookTarget.Loc);
				Find.LetterStack.RemoveLetter(this);
			};
			diaOption2.ResolveTree = true;
			diaNode.optionList.Add(diaOption2);
		}
		DialogBoxHelper.InitDialogTree(diaNode);
	}

	public static implicit operator Letter(string str)
	{
		return new Letter(str);
	}
}
