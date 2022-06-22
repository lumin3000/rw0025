using UnityEngine;

public class PawnHeadOverlays
{
	private const float AngerBlinkPeriod = 1.2f;

	private const float AngerBlinkLength = 0.4f;

	private Pawn pawn;

	private static readonly Material UnhappyMat = MaterialPool.MatFrom("Icons/Pawn/Effects/Unhappy");

	private static readonly Material MentalBreakImminentMat = MaterialPool.MatFrom("Icons/Pawn/Effects/MentalBreakImminent");

	private static readonly ObjectGraphic GraphicMask = new ObjectGraphic("Icons/Pawn/Overlays/Mask");

	public PawnHeadOverlays(Pawn pawn)
	{
		this.pawn = pawn;
	}

	public void RenderHeadOverlays(Vector3 headLoc, Quaternion quat, Mesh headMesh)
	{
		if (pawn.Team != TeamType.Colonist)
		{
			return;
		}
		if (!pawn.HasAir())
		{
			Material material = GraphicMask.MatAt(pawn.rotation);
			Graphics.DrawMesh(headMesh, headLoc + Altitudes.AltIncVect, quat, material, 0);
		}
		if (pawn.psychology == null || pawn.Incapacitated || pawn.health <= 0)
		{
			return;
		}
		if (pawn.psychology.MentalBreakImminent)
		{
			if (Time.time % 1.2f < 0.4f)
			{
				DrawHeadGlow(headLoc, MentalBreakImminentMat);
			}
		}
		else if (pawn.psychology.MentalBreakApproaching && Time.time % 1.2f < 0.4f)
		{
			DrawHeadGlow(headLoc, UnhappyMat);
		}
	}

	private void DrawHeadGlow(Vector3 headLoc, Material mat)
	{
		Graphics.DrawMesh(MeshPool.plane20, headLoc, Quaternion.identity, mat, 0);
	}
}
