using UnityEngine;

public class PawnUIOverlay
{
	private const float PawnLabelOffsetY = -0.6f;

	private const int PawnStatBarWidth = 32;

	private const float ActivityIconSize = 13f;

	private const float ActivityIconOffsetY = 12f;

	private const float NameUnderlineDist = 11f;

	private const float MinNameWidth = 20f;

	private Pawn pawn;

	public static readonly Texture2D HealthTex = GenRender.SolidColorTexture(new Color(1f, 0f, 0f, 0.25f));

	private Texture2D CurActivityIcon
	{
		get
		{
			Texture2D result = null;
			if (pawn.jobs.CurJob != null)
			{
				result = pawn.jobs.CurJobDriver.GetReport().overlayTex;
			}
			if (pawn.Team == TeamType.Psychotic)
			{
				result = null;
			}
			if (pawn.mind is Pawn_MindHuman && pawn.MindHuman.drafted)
			{
				result = JobReportOverlays.drafted;
			}
			if (pawn.Incapacitated)
			{
				result = JobReportOverlays.incapacitated;
			}
			return result;
		}
	}

	public PawnUIOverlay(Pawn pawn)
	{
		this.pawn = pawn;
	}

	public void DrawPawnGUIOverlay()
	{
		if (!pawn.spawnedInWorld || Find.FogGrid.IsFogged(pawn.Position) || !pawn.raceDef.hasIdentity)
		{
			return;
		}
		Vector3 vector = GenWorldUI.LabelDrawPosFor(pawn, -0.6f);
		float y = vector.y;
		if (ShouldDrawOverlayOnMap(pawn))
		{
			GenUI.SetFontTiny();
			float num = GUI.skin.label.CalcSize(new GUIContent(pawn.characterName)).x;
			if (num < 20f)
			{
				num = 20f;
			}
			Rect rect = new Rect(vector.x - num / 2f - 4f, vector.y, num + 8f, 12f);
			GUI.DrawTexture(rect, GenUI.GrayTextBG);
			if (pawn.healthTracker.Health < pawn.healthTracker.MaxHealth)
			{
				Rect innerRect = rect.GetInnerRect(1f);
				UIWidgets.FillableBar(innerRect, (float)pawn.healthTracker.Health / (float)pawn.healthTracker.MaxHealth, HealthTex, doBlackBorder: false, GenUI.ClearTex);
			}
			GUI.color = TeamColorUtility.TeamNameColorOf(pawn.Team);
			GUI.skin.label.alignment = TextAnchor.UpperCenter;
			GUI.Label(new Rect(vector.x - num / 2f, vector.y - 2f, num, 999f), pawn.characterName);
			if (pawn.MindHuman != null && pawn.MindHuman.drafted)
			{
				GenUI.DrawLineHorizontal(new Vector2(vector.x - num / 2f, vector.y + 11f), num);
			}
			GUI.color = Color.white;
			GUI.skin.label.alignment = TextAnchor.UpperLeft;
			y += 12f;
		}
	}

	private static bool ShouldDrawOverlayOnMap(Pawn p)
	{
		return true;
	}
}
