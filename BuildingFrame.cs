using System;
using UnityEngine;

public class BuildingFrame : Building, Interactive
{
	protected const float BaseWorkAmountPerTick = 0.3f;

	protected const float UnderfieldOverdrawFactor = 1.15f;

	protected const float CenterOverdrawFactor = 0.5f;

	private float workDone;

	protected static readonly Material UnderfieldMat = MaterialPool.MatFrom("Icons/Building/BuildingFrame/Underfield", MatBases.Transparent);

	protected static readonly Material CenterMat = MaterialPool.MatFrom("Icons/Building/BuildingFrame/Center");

	protected static readonly Material CornerMat = MaterialPool.MatFrom("Icons/Building/BuildingFrame/Corner");

	protected static readonly Material PlateMat = MaterialPool.MatFrom("Icons/Building/BuildingFrame/Plate");

	private static readonly AudioClip BuildingCompleteSound = Res.LoadSound("Interaction/BuildingComplete");

	public float PercentComplete => workDone / def.entityDefToBuild.workToBuild;

	public override string Label => def.entityDefToBuild.label + " (building)";

	private float WorkLeft => def.entityDefToBuild.workToBuild - workDone;

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe.LookField(ref workDone, "WorkDone");
	}

	public JobCondition InteractedWith(ReservationType WType, Pawn pawn)
	{
		float num = 0.5f + 0.15f * (float)pawn.skills.LevelOf(SkillType.Construction);
		num *= pawn.healthTracker.CurEffectivenessPercent;
		float num2 = 0.3f * num;
		workDone += num2;
		pawn.skills.Learn(SkillType.Construction, 0.7f);
		if (WorkLeft <= 0f)
		{
			Find.ReservationManager.UnReserve(this, ReservationType.Construction);
			Destroy();
			if (def.entityDefToBuild.workToBuild > 150f && def.entityDefToBuild.category == EntityCategory.Building)
			{
				GenSound.PlaySoundAt(base.Position, BuildingCompleteSound, 0.25f);
			}
			ThingDefinition thingDefinition = def.entityDefToBuild as ThingDefinition;
			if (thingDefinition != null)
			{
				Thing thing = ThingMaker.MakeThing(thingDefinition);
				thing.Team = TeamType.Colonist;
				thing.health = health;
				ThingMaker.Spawn(thing, base.Position, rotation);
			}
			else
			{
				Find.TerrainGrid.SetTerrain(base.Position, (TerrainDefinition)def.entityDefToBuild);
			}
			return JobCondition.Succeeded;
		}
		return JobCondition.Ongoing;
	}

	public override void Draw()
	{
		Vector3 drawPos = DrawPos;
		Vector2 vector = new Vector2(def.size.x, def.size.z);
		vector.x *= 1.15f;
		vector.y *= 1.15f;
		Vector3 s = new Vector3(vector.x, 1f, vector.y);
		Matrix4x4 matrix = default(Matrix4x4);
		matrix.SetTRS(drawPos, rotation.AsQuat, s);
		Graphics.DrawMesh(MeshPool.plane10, matrix, UnderfieldMat, 0);
		int num = (int)Math.Floor(PercentComplete / 0.2f / 0.333f);
		if (num < 1)
		{
			num = 1;
		}
		if (num > 4)
		{
			num = 4;
		}
		for (int i = 0; i < num; i++)
		{
			float num2 = Math.Min(base.RotatedSize.x, base.RotatedSize.z);
			float num3 = num2 * 0.38f;
			IntVec3 intVec = default(IntVec3);
			switch (i)
			{
			case 0:
				intVec = new IntVec3(-1, 0, -1);
				break;
			case 1:
				intVec = new IntVec3(-1, 0, 1);
				break;
			case 2:
				intVec = new IntVec3(1, 0, 1);
				break;
			case 3:
				intVec = new IntVec3(1, 0, -1);
				break;
			}
			Vector3 vector2 = default(Vector3);
			vector2.x = (float)intVec.x * ((float)base.RotatedSize.x / 2f - num3 / 2f);
			vector2.z = (float)intVec.z * ((float)base.RotatedSize.z / 2f - num3 / 2f);
			Vector3 s2 = new Vector3(num3, 1f, num3);
			Matrix4x4 matrix2 = default(Matrix4x4);
			matrix2.SetTRS(drawPos + Vector3.up * 0.03f + vector2, new IntRot(i).AsQuat, s2);
			Graphics.DrawMesh(MeshPool.plane10, matrix2, CornerMat, 0);
		}
		float num4 = (PercentComplete - 0.2f) / 0.8f;
		int num5 = (int)Math.Ceiling(num4 * (float)base.RotatedSize.x * (float)base.RotatedSize.z * 4f);
		IntVec2 intVec2 = base.RotatedSize * 2;
		for (int j = 0; j < num5; j++)
		{
			IntVec2 intVec3 = default(IntVec2);
			intVec3.z = j / intVec2.x;
			intVec3.x = j - intVec3.z * intVec2.x;
			Vector3 vector3 = new Vector3((float)intVec3.x * 0.5f, 0f, (float)intVec3.z * 0.5f) + drawPos;
			vector3.x -= (float)base.RotatedSize.x * 0.5f - 0.25f;
			vector3.z -= (float)base.RotatedSize.z * 0.5f - 0.25f;
			Vector3 s3 = new Vector3(0.5f, 1f, 0.5f);
			Matrix4x4 matrix3 = default(Matrix4x4);
			matrix3.SetTRS(vector3 + Vector3.up * 0.02f, Quaternion.identity, s3);
			Graphics.DrawMesh(MeshPool.plane10, matrix3, PlateMat, 0);
		}
	}

	public override string GetInspectString()
	{
		return base.GetInspectString() + "\nWork left: " + WorkLeft.ToString("######0");
	}
}
