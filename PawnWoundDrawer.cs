using System.Collections.Generic;
using UnityEngine;

public class PawnWoundDrawer
{
	private class Wound
	{
		private Dictionary<IntRot, Vector2> locsPerSide = new Dictionary<IntRot, Vector2>();

		private Vector2 location;

		private Material mat;

		private Quaternion quat;

		private static readonly Vector2 WoundSpan = new Vector2(0.18f, 0.3f);

		public Wound()
		{
			mat = WoundOverlays.RandomElement();
			quat = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up);
			for (int i = 0; i < 4; i++)
			{
				locsPerSide.Add(new IntRot(i), new Vector2(Random.value, Random.value));
			}
		}

		public void DrawWound(Vector3 bodyLoc, Quaternion bodyQuat, IntRot bodyRot)
		{
			Vector3 vector = bodyLoc + Altitudes.AltIncVect;
			Vector2 vector2 = locsPerSide[bodyRot];
			Vector3 vector3 = vector;
			float num = vector2.x - 0.5f;
			Vector2 woundSpan = WoundSpan;
			float x = num * woundSpan.x;
			float num2 = vector2.y - 0.5f;
			Vector2 woundSpan2 = WoundSpan;
			vector = vector3 + new Vector3(x, 0f, num2 * woundSpan2.y);
			vector.z -= 0.3f;
			Graphics.DrawMesh(MeshPool.plane03, vector, quat, mat, 0);
		}
	}

	protected Pawn pawn;

	private List<Wound> wounds = new List<Wound>();

	private static List<Material> WoundOverlays;

	private static float DamagePerWound = 15f;

	public PawnWoundDrawer(Pawn pawn)
	{
		this.pawn = pawn;
		if (WoundOverlays == null)
		{
			WoundOverlays = new List<Material>();
			WoundOverlays.Add(MaterialPool.MatFrom("Icons/Pawn/Wounds/WoundA"));
			WoundOverlays.Add(MaterialPool.MatFrom("Icons/Pawn/Wounds/WoundB"));
			WoundOverlays.Add(MaterialPool.MatFrom("Icons/Pawn/Wounds/WoundC"));
		}
	}

	public void RenderOverBody(Vector3 bodyLoc, Mesh bodyMesh, Quaternion quat)
	{
		int num = Mathf.RoundToInt((float)(pawn.healthTracker.MaxHealth - pawn.health) / DamagePerWound);
		if (num < 0)
		{
			num = 0;
		}
		while (wounds.Count < num)
		{
			wounds.Add(new Wound());
		}
		while (wounds.Count > num)
		{
			wounds.Remove(wounds.RandomElement());
		}
		foreach (Wound wound in wounds)
		{
			wound.DrawWound(bodyLoc, quat, pawn.rotation);
		}
	}
}
