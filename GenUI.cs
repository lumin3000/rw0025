using System.Collections.Generic;
using UnityEngine;

public static class GenUI
{
	public const float SPad = 8f;

	public const float Pad = 10f;

	public const float Gap = 17f;

	public const float GapWide = 26f;

	public const float ListSpacing = 28f;

	public const float StatBarHeight = 4f;

	public const float MouseAttachIconSize = 32f;

	public const float MouseAttachIconOffset = 8f;

	public const float DesButSize = 75f;

	public const float ScrollBarWidth = 24f;

	private const float MouseIconSize = 32f;

	private const float MouseIconOffset = 12f;

	public const float PawnDirectClickRadius = 0.4f;

	public static readonly Vector2 TradeableDrawSize = new Vector2(150f, 45f);

	public static readonly Color MouseoverColor = new Color(0.3f, 0.7f, 0.9f);

	public static readonly Texture2D HighlightTex = GenRender.SolidColorTexture(new Color(1f, 1f, 1f, 0.1f));

	public static readonly Texture2D DarkTransparentTex = GenRender.SolidColorTexture(new Color(0f, 0f, 0f, 0.1f));

	public static readonly Texture2D TextBGBlack = Res.LoadTexture("UI/Widgets/TextBGBlack");

	public static readonly Texture2D ArrowTexRight = Res.LoadTexture("UI/Widgets/ArrowRight");

	public static readonly Texture2D ArrowTexLeft = Res.LoadTexture("UI/Widgets/ArrowLeft");

	public static readonly Texture2D MissingContentTex = Res.LoadTexture("UI/Widgets/MissingContent");

	public static readonly Texture2D GrayTextBG = Res.LoadTexture("UI/Overlays/GrayTextBG");

	public static readonly Material MouseoverBracketMaterial = MaterialPool.MatFrom("UI/Overlays/MouseoverBracketTex", MatBases.MetaOverlay);

	public static readonly Texture2D BlackTex = GenRender.SolidColorTexture(Color.black);

	public static readonly Texture2D RedTex = GenRender.SolidColorTexture(Color.red);

	public static readonly Texture2D GreenTex = GenRender.SolidColorTexture(Color.green);

	public static readonly Texture2D WhiteTex = GenRender.SolidColorTexture(Color.white);

	public static readonly Texture2D ClearTex = GenRender.SolidColorTexture(Color.clear);

	private static readonly Font FontTiny = (Font)Resources.Load("Fonts/Tiny");

	private static readonly Font FontSmall = (Font)Resources.Load("Fonts/Small");

	private static readonly Font FontHeader = (Font)Resources.Load("Fonts/Header");

	private static readonly Vector2 PieceBarSize = new Vector2(100f, 17f);

	public static void SetFontTiny()
	{
		GUI.skin.font = FontTiny;
	}

	public static void SetFontSmall()
	{
		GUI.skin.font = FontSmall;
	}

	public static void SetFontMedium()
	{
		GUI.skin.font = FontHeader;
	}

	public static void SetLabelAlign(TextAnchor a)
	{
		GUI.skin.label.alignment = a;
	}

	public static void ResetLabelAlign()
	{
		GUI.skin.label.alignment = TextAnchor.UpperLeft;
	}

	public static void DrawLineHorizontal(Vector2 start, float length)
	{
		Rect position = new Rect(start.x, start.y, length, 1f);
		GUI.DrawTexture(position, WhiteTex);
	}

	public static void DrawBox(Rect drawRect)
	{
		DrawBox(drawRect, 1);
	}

	public static void DrawBox(Rect drawRect, int Thickness)
	{
		Vector2 vector = new Vector2(drawRect.x, drawRect.y);
		Vector2 vector2 = new Vector2(drawRect.x + drawRect.width, drawRect.y + drawRect.height);
		if (vector.x > vector2.x)
		{
			float x = vector.x;
			vector.x = vector2.x;
			vector2.x = x;
		}
		if (vector.y > vector2.y)
		{
			float y = vector.y;
			vector.y = vector2.y;
			vector2.y = y;
		}
		Vector3 vector3 = vector2 - vector;
		GUI.DrawTexture(new Rect(vector.x, vector.y, Thickness, vector3.y), WhiteTex);
		GUI.DrawTexture(new Rect(vector2.x - (float)Thickness, vector.y, Thickness, vector3.y), WhiteTex);
		GUI.DrawTexture(new Rect(vector.x + (float)Thickness, vector.y, vector3.x - (float)(Thickness * 2), Thickness), WhiteTex);
		GUI.DrawTexture(new Rect(vector.x + (float)Thickness, vector2.y - (float)Thickness, vector3.x - (float)(Thickness * 2), Thickness), WhiteTex);
	}

	public static void DrawMouseIcon(Texture2D tex)
	{
		Vector2 mousePosition = Event.current.mousePosition;
		Rect position = new Rect(mousePosition.x + 12f, mousePosition.y + 12f, 32f, 32f);
		GUI.DrawTexture(position, tex);
	}

	public static void RenderMouseoverBracket()
	{
		Vector3 position = Gen.MouseWorldSquare().ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays);
		Graphics.DrawMesh(MeshPool.plane10, position, Quaternion.identity, MouseoverBracketMaterial, 0);
	}

	public static void DrawThoughtLevel(StatusLevel p, Rect PieceArea)
	{
		GUI.BeginGroup(PieceArea);
		Rect position = new Rect(0f, 4f, PieceArea.width, 99f);
		GUI.Label(position, p.Label);
		float num = PieceArea.width / 2f;
		Vector2 pieceBarSize = PieceBarSize;
		float left = num - pieceBarSize.x / 2f;
		float num2 = PieceArea.height - 6f;
		Vector2 pieceBarSize2 = PieceBarSize;
		float top = num2 - pieceBarSize2.y;
		Vector2 pieceBarSize3 = PieceBarSize;
		float x = pieceBarSize3.x;
		Vector2 pieceBarSize4 = PieceBarSize;
		Rect rect = new Rect(left, top, x, pieceBarSize4.y);
		UIWidgets.FillableBar(rect, p.PercentFull);
		UIWidgets.FillableBarChangeArrows(rect, p.RateOfChange);
		GUI.EndGroup();
		TooltipHandler.TipRegion(PieceArea, p.GetTooltipDef());
		if (PieceArea.Contains(Event.current.mousePosition))
		{
			GUI.DrawTexture(PieceArea, HighlightTex);
		}
	}

	public static void AbsorbAllInput()
	{
		if (Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseUp || Event.current.type == EventType.KeyDown || Event.current.type == EventType.KeyUp || Event.current.type == EventType.ScrollWheel)
		{
			Event.current.Use();
		}
	}

	public static TargetPack ClickTargetUnderMouse(TargetingParameters ClickParams)
	{
		List<Thing> list = ThingsUnderMouse(0.8f, ClickParams);
		if (list.Count > 0)
		{
			return new TargetPack(list[0]);
		}
		TargetPack targetPack = new TargetPack(Gen.MouseWorldSquare());
		if (ClickParams.CanTarget(targetPack))
		{
			return targetPack;
		}
		return null;
	}

	public static List<Thing> ThingsUnderMouse(float PawnWideClickRadius, TargetingParameters ClickParams)
	{
		IntVec3 sq = Gen.MouseWorldSquare();
		Vector3 vector = Gen.MouseWorldPosVector3();
		List<Thing> list = new List<Thing>();
		List<Pawn> list2 = new List<Pawn>();
		foreach (Pawn allPawn in Find.PawnManager.AllPawns)
		{
			if ((allPawn.DrawPos - vector).MagnitudeHorizontal() < 0.4f && ClickParams.CanTarget(new TargetPack(allPawn)))
			{
				list2.Add(allPawn);
			}
		}
		list2.Sort(CompareThingsByDistanceToMousePointer);
		foreach (Pawn item in list2)
		{
			list.Add(item);
		}
		List<Thing> list3 = new List<Thing>();
		foreach (Thing item2 in Find.Grids.ThingsAt(sq))
		{
			if (!list.Contains(item2) && ClickParams.CanTarget(new TargetPack(item2)))
			{
				list3.Add(item2);
			}
		}
		list3.Sort(CompareThingsByDrawAltitude);
		foreach (Thing item3 in list3)
		{
			list.Add(item3);
		}
		List<Thing> list4 = new List<Thing>();
		foreach (Pawn allPawn2 in Find.PawnManager.AllPawns)
		{
			if ((allPawn2.DrawPos - vector).MagnitudeHorizontal() < PawnWideClickRadius && ClickParams.CanTarget(new TargetPack(allPawn2)))
			{
				list4.Add(allPawn2);
			}
		}
		list4.Sort(CompareThingsByDistanceToMousePointer);
		foreach (Thing item4 in list4)
		{
			if (!list.Contains(item4))
			{
				list.Add(item4);
			}
		}
		list.RemoveAll((Thing t) => !t.spawnedInWorld);
		return list;
	}

	private static int CompareThingsByDistanceToMousePointer(Thing A, Thing B)
	{
		Vector3 vector = Gen.MouseWorldPosVector3();
		float num = (A.DrawPos - vector).MagnitudeHorizontalSquared();
		float num2 = (B.DrawPos - vector).MagnitudeHorizontalSquared();
		if (num < num2)
		{
			return -1;
		}
		if (num == num2)
		{
			return 0;
		}
		return 1;
	}

	private static int CompareThingsByDrawAltitude(Thing A, Thing B)
	{
		if (A.def.altitude < B.def.altitude)
		{
			return 1;
		}
		if (A.def.altitude == B.def.altitude)
		{
			return 0;
		}
		return -1;
	}

	public static Rect GetInnerRect(this Rect BaseRect, float Margin)
	{
		return new Rect(BaseRect.x + Margin, BaseRect.y + Margin, BaseRect.width - Margin * 2f, BaseRect.height - Margin * 2f);
	}

	public static void AbsorbClicksInRect(Rect r)
	{
		if (Event.current.type == EventType.MouseDown && r.Contains(Event.current.mousePosition))
		{
			Event.current.Use();
		}
	}

	public static void UIToggleDrafted(this Pawn p)
	{
		p.MindHuman.drafted = !p.MindHuman.drafted;
		if (p.MindHuman.drafted)
		{
			GenSound.PlaySoundOnCamera(Res.LoadSound("Interface/DraftOn"), 0.4f);
		}
		else
		{
			GenSound.PlaySoundOnCamera(Res.LoadSound("Interface/DraftOff"), 0.4f);
		}
		if (!p.HasAttachment(EntityType.Fire))
		{
			p.jobs.EndCurrentJob(JobCondition.ForcedInterrupt);
		}
	}
}
