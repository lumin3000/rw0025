using UnityEngine;

public static class MapEdgeClipDrawer
{
	private const float ClipWidth = 500f;

	public static readonly Material ClipMat = GenRender.SolidColorMaterial(new Color(0.1f, 0.1f, 0.1f), MatBases.MetaOverlay);

	private static readonly float ClipAltitude = Altitudes.AltitudeFor(AltitudeLayer.WorldClipper);

	public static void DrawClippers()
	{
		IntVec3 size = Find.Map.Size;
		Vector3 s = new Vector3(500f, 1f, size.z);
		Matrix4x4 matrix = default(Matrix4x4);
		matrix.SetTRS(new Vector3(-250f, ClipAltitude, (float)size.z / 2f), Quaternion.identity, s);
		Graphics.DrawMesh(MeshPool.plane10, matrix, ClipMat, 0);
		matrix = default(Matrix4x4);
		matrix.SetTRS(new Vector3((float)size.x + 250f, ClipAltitude, (float)size.z / 2f), Quaternion.identity, s);
		Graphics.DrawMesh(MeshPool.plane10, matrix, ClipMat, 0);
		s = new Vector3(1000f, 1f, 500f);
		matrix = default(Matrix4x4);
		matrix.SetTRS(new Vector3(size.x / 2, ClipAltitude, (float)size.z + 250f), Quaternion.identity, s);
		Graphics.DrawMesh(MeshPool.plane10, matrix, ClipMat, 0);
		matrix = default(Matrix4x4);
		matrix.SetTRS(new Vector3(size.x / 2, ClipAltitude, -250f), Quaternion.identity, s);
		Graphics.DrawMesh(MeshPool.plane10, matrix, ClipMat, 0);
	}
}
