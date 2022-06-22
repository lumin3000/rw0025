using System.Collections.Generic;
using UnityEngine;

public static class GhostDrawer
{
	private static Dictionary<int, Material> ghostMatDict = new Dictionary<int, Material>();

	public static void DrawGhostThing(IntVec3 loc, IntRot rot, ThingDefinition thingDef, Color color, AltitudeLayer drawAltitude)
	{
		Material material = GhostMatFor(thingDef, color);
		IntVec2 intVec = new IntVec2(thingDef.size.x, thingDef.size.z);
		if (thingDef.overDraw)
		{
			intVec.x += 2;
			intVec.z += 2;
		}
		Vector3 s = new Vector3(intVec.x, 1f, intVec.z);
		Matrix4x4 matrix = default(Matrix4x4);
		matrix.SetTRS(Gen.TrueCenter(loc, rot, thingDef.size, Altitudes.AltitudeFor(drawAltitude)), rot.AsQuat, s);
		Graphics.DrawMesh(MeshPool.plane10, matrix, material, 0);
	}

	private static Material GhostMatFor(ThingDefinition thingDef, Color col)
	{
		int key = MatWithColorHashCode(thingDef, col);
		if (!ghostMatDict.ContainsKey(key))
		{
			Material material = new Material(MatBases.MetaOverlay);
			material.mainTexture = thingDef.uiIcon;
			material.color = col;
			ghostMatDict.Add(key, material);
		}
		return ghostMatDict[key];
	}

	private static int MatWithColorHashCode(ThingDefinition thingDef, Color color)
	{
		return thingDef.GetHashCode() * color.GetHashCode();
	}
}
