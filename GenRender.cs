using System;
using System.Collections.Generic;
using UnityEngine;

public static class GenRender
{
	public class FillableBarRequest
	{
		public Vector3 Center;

		public Vector2 BarSize;

		public float FillPercent;

		public Material FilledMat;

		public Material UnfilledMat;

		public float Margin;

		public IntRot Rotation = IntRot.north;

		public Vector2 PreRotationOffset = new Vector2(0f, 0f);
	}

	private const float TargetPulseFrequency = 8f;

	private const float LineWidth = 0.2f;

	private static readonly Material TargetSquareMatSingle;

	private static readonly Material TargetSquareMatSide;

	private static readonly Material LineMat;

	private static Material CurTargetingMat
	{
		get
		{
			float num = (float)Math.Sin(Time.time * 8f);
			num *= 0.2f;
			num += 0.8f;
			Color color = new Color(1f, num, num);
			TargetSquareMatSingle.color = color;
			return TargetSquareMatSingle;
		}
	}

	static GenRender()
	{
		TargetSquareMatSingle = MaterialPool.MatFrom("UI/Overlays/TargetHighlight_Square", MatBases.Transparent);
		TargetSquareMatSide = MaterialPool.MatFrom("UI/Overlays/TargetHighlight_Side", MatBases.Transparent);
		LineMat = MaterialPool.MatFrom("UI/Overlays/ThingLine", MatBases.Transparent);
		TargetSquareMatSide.GetTexture("_MainTex").wrapMode = TextureWrapMode.Clamp;
	}

	public static void DrawLineBetween(Vector3 aPos, Vector3 bPos)
	{
		Vector3 pos = (aPos + bPos) / 2f;
		if (!(aPos == bPos))
		{
			float z = (aPos - bPos).MagnitudeHorizontal();
			Quaternion q = Quaternion.LookRotation(aPos - bPos);
			Vector3 s = new Vector3(0.2f, 1f, z);
			Matrix4x4 matrix = default(Matrix4x4);
			matrix.SetTRS(pos, q, s);
			Graphics.DrawMesh(MeshPool.plane10, matrix, LineMat, 0);
		}
	}

	public static void RenderTargetHighlight(TargetPack targ)
	{
		if (targ.thing != null)
		{
			RenderTargetingHighlight_Thing(targ.thing);
		}
		else
		{
			RenderTargetingHighlight_Square(targ.Loc);
		}
	}

	private static void RenderTargetingHighlight_Square(IntVec3 Sq)
	{
		Vector3 position = Sq.ToVector3ShiftedWithAltitude(AltitudeLayer.Waist);
		Graphics.DrawMesh(MeshPool.plane10, position, Quaternion.identity, CurTargetingMat, 0);
	}

	private static void RenderTargetingHighlight_Thing(Thing t)
	{
		Graphics.DrawMesh(MeshPool.plane10, t.TrueCenter() + Altitudes.AltIncVect, t.rotation.AsQuat, CurTargetingMat, 0);
	}

	public static void RenderTargetingHightlight_Explosion(IntVec3 Sq, float Radius)
	{
		RenderRadiusRing(Sq, Radius);
	}

	public static void RenderLOSTrace(IntVec3 Start, IntVec3 End)
	{
	}

	public static void RenderRadiusRing(IntVec3 Center, float Radius)
	{
		List<IntVec3> list = new List<IntVec3>();
		int num = Gen.NumSquaresInRadius(Radius);
		for (int i = 0; i < num; i++)
		{
			list.Add(Center + Gen.RadialPattern[i]);
		}
		RenderFieldEdges(list);
	}

	public static void RenderFieldEdges(List<IntVec3> FieldSquares)
	{
		int MinX = int.MaxValue;
		int num = int.MinValue;
		int MinZ = int.MaxValue;
		int num2 = int.MinValue;
		foreach (IntVec3 FieldSquare in FieldSquares)
		{
			if (FieldSquare.x < MinX)
			{
				MinX = FieldSquare.x;
			}
			if (FieldSquare.x > num)
			{
				num = FieldSquare.x;
			}
			if (FieldSquare.z < MinZ)
			{
				MinZ = FieldSquare.z;
			}
			if (FieldSquare.z > num2)
			{
				num2 = FieldSquare.z;
			}
		}
		IntVec2 intVec = new IntVec2(num - MinX + 3, num2 - MinZ + 3);
		bool[,] array = new bool[intVec.x, intVec.z];
		Func<int, int> func = (int x) => x - MinX + 1;
		Func<int, int> func2 = (int z) => z - MinZ + 1;
		foreach (IntVec3 FieldSquare2 in FieldSquares)
		{
			array[func(FieldSquare2.x), func2(FieldSquare2.z)] = true;
		}
		foreach (IntVec3 FieldSquare3 in FieldSquares)
		{
			if (!FieldSquare3.InBounds())
			{
				continue;
			}
			bool[] array2 = new bool[4]
			{
				!array[func(FieldSquare3.x), func2(FieldSquare3.z + 1)],
				!array[func(FieldSquare3.x + 1), func2(FieldSquare3.z)],
				!array[func(FieldSquare3.x), func2(FieldSquare3.z - 1)],
				!array[func(FieldSquare3.x - 1), func2(FieldSquare3.z)]
			};
			for (int i = 0; i < 4; i++)
			{
				if (array2[i])
				{
					Graphics.DrawMesh(MeshPool.plane10, FieldSquare3.ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays), new IntRot(i).AsQuat, TargetSquareMatSide, 0);
				}
			}
		}
	}

	public static void RenderFieldSolid(List<IntVec3> FieldSquares, Material RenderMat, float Altitude)
	{
		using List<IntVec3>.Enumerator enumerator = FieldSquares.GetEnumerator();
		while (enumerator.MoveNext())
		{
			Graphics.DrawMesh(position: enumerator.Current.ToVector3ShiftedWithAltitude(Altitude), mesh: MeshPool.plane10, rotation: Quaternion.identity, material: RenderMat, layer: 0);
		}
	}

	public static Material SolidColorMaterial(Color col)
	{
		return SolidColorMaterial(col, MatBases.SolidColor);
	}

	public static Material SolidColorMaterial(Color col, Material matBase)
	{
		Material material = new Material(matBase);
		material.color = col;
		return material;
	}

	public static Texture2D SolidColorTexture(Color Col)
	{
		Texture2D texture2D = new Texture2D(1, 1);
		texture2D.SetPixel(0, 0, Col);
		texture2D.Apply();
		return texture2D;
	}

	public static void RenderPie(Vector3 center, float facing, int degreesWide)
	{
		if (degreesWide > 0)
		{
			if (degreesWide > 360)
			{
				degreesWide = 360;
			}
			center += Quaternion.AngleAxis(facing, Vector3.up) * Vector3.forward * 0.1f;
			Graphics.DrawMesh(MeshPool.pies[degreesWide], center, Quaternion.AngleAxis(facing + (float)(degreesWide / 2) - 90f, Vector3.up), MatsSimple.TranslucentWhiteMaterial, 0);
		}
	}

	public static void RenderCircle(Vector3 center, float radius)
	{
		Vector3 s = new Vector3(radius, 1f, radius);
		Matrix4x4 matrix = default(Matrix4x4);
		matrix.SetTRS(center, Quaternion.identity, s);
		Graphics.DrawMesh(MeshPool.circle, matrix, MatsSimple.TranslucentWhiteMaterial, 0);
	}

	public static void RenderFillableBar(FillableBarRequest r)
	{
		Vector2 vector = r.PreRotationOffset.RotatedBy(r.Rotation.AsAngle);
		r.Center += new Vector3(vector.x, 0f, vector.y);
		if (r.Rotation == IntRot.south)
		{
			r.Rotation = IntRot.north;
		}
		if (r.Rotation == IntRot.west)
		{
			r.Rotation = IntRot.east;
		}
		Vector3 s = new Vector3(r.BarSize.x + r.Margin, 1f, r.BarSize.y + r.Margin);
		Matrix4x4 matrix = default(Matrix4x4);
		matrix.SetTRS(r.Center, r.Rotation.AsQuat, s);
		Graphics.DrawMesh(MeshPool.plane10, matrix, r.UnfilledMat, 0);
		s = new Vector3(r.BarSize.x * r.FillPercent, 1f, r.BarSize.y);
		matrix = default(Matrix4x4);
		Vector3 pos = r.Center + Vector3.up * 0.01f;
		if (!r.Rotation.IsHorizontal)
		{
			pos.x -= r.BarSize.x * 0.5f;
			pos.x += 0.5f * r.BarSize.x * r.FillPercent;
		}
		else
		{
			pos.z -= r.BarSize.x * 0.5f;
			pos.z += 0.5f * r.BarSize.x * r.FillPercent;
		}
		matrix.SetTRS(pos, r.Rotation.AsQuat, s);
		Graphics.DrawMesh(MeshPool.plane10, matrix, r.FilledMat, 0);
	}
}
