using UnityEngine;

public static class DebugRender
{
	private static int lastCameraUpdateFrame = -1;

	private static IntRect viewRect;

	public static void RenderSquareSpectrum(IntVec3 Sq, int colorIndex)
	{
		RenderSquare(Sq, DebugMatsSpectrum.Mat(colorIndex));
	}

	public static void RenderSquare(IntVec3 Sq)
	{
		RenderSquare(Sq, 0);
	}

	public static void RenderSquare(IntVec3 Sq, int colorIndex)
	{
		colorIndex %= 100;
		RenderSquare(Sq, DebugMatsRandom.Mat(colorIndex));
	}

	public static void RenderSquare(IntVec3 sq, Material mat)
	{
		if (Time.frameCount != lastCameraUpdateFrame)
		{
			viewRect = Find.CameraMap.CurrentViewRect;
			lastCameraUpdateFrame = Time.frameCount;
		}
		if (viewRect.Contains(sq))
		{
			Graphics.DrawMesh(MeshPool.plane10, sq.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays), Quaternion.identity, mat, 0);
		}
	}
}
