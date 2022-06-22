using UnityEngine;

public static class PowerNetGraphics
{
	private const AltitudeLayer WireAltitude = AltitudeLayer.FloorDeco;

	private static readonly Material WireMat = MaterialPool.MatFrom("Icons/Special/Power/Wire");

	public static MapMeshPiece WirePieceConnecting(Building A, Building B, bool forPowerOverlay)
	{
		Material mat = WireMat;
		float y = Altitudes.AltitudeFor(AltitudeLayer.FloorDeco);
		if (forPowerOverlay)
		{
			mat = PowerOverlayMats.MatConnectorLine;
			y = Altitudes.AltitudeFor(AltitudeLayer.WorldDataOverlay);
		}
		Vector3 center = (A.TrueCenter() + B.TrueCenter()) / 2f;
		center.y = y;
		Vector3 v = B.TrueCenter() - A.TrueCenter();
		Vector2 size = new Vector2(1f, v.MagnitudeHorizontal());
		float rot = v.AngleFlat();
		return new MapMeshPiece_Plane(center, size, mat, rot);
	}

	public static void RenderAnticipatedWirePieceConnecting(IntVec3 userPos, IntVec3 transmitterPos)
	{
		Vector3 vector = userPos.ToVector3ShiftedWithAltitude(AltitudeLayer.WorldDataOverlay);
		if (userPos != transmitterPos)
		{
			Vector3 vector2 = transmitterPos.ToVector3ShiftedWithAltitude(AltitudeLayer.WorldDataOverlay);
			Vector3 pos = (vector + vector2) / 2f;
			Vector3 v = vector2 - vector;
			float num = v.AngleFlat();
			Vector3 s = new Vector3(1f, 1f, v.MagnitudeHorizontal());
			Quaternion q = Quaternion.LookRotation(vector - vector2);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(pos, q, s);
			Graphics.DrawMesh(MeshPool.plane10, matrix, PowerOverlayMats.MatConnectorAnticipated, 0);
		}
	}

	public static MapMeshPiece OverlayConnectorBaseFor(Thing t)
	{
		Vector3 center = t.TrueCenter();
		center.y = Altitudes.AltitudeFor(AltitudeLayer.WorldDataOverlay);
		return new MapMeshPiece_Plane(center, new Vector2(1f, 1f), PowerOverlayMats.MatConnectorBase, 0f);
	}
}
