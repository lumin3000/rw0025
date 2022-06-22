using UnityEngine;

public class Tower_Laser : Building_Turret
{
	protected const int WarmupTicksTotal = 160;

	protected const int WarmupTicksRefire = 40;

	protected const float LaserDrawAltitude = 15f;

	protected const float MaxRange = 12f;

	protected const float MinRange = 0f;

	protected const float MaxRangeSquared = 144f;

	protected const float MinRangeSquared = 0f;

	protected const float CenterLaserWidth = 0.05f;

	protected const float SideLasersWidth = 0.08f;

	protected const float DegreesOffPerWarmupTick = 0.1f;

	protected Thing curTarget;

	protected int WarmupTicksLeft;

	protected SoundLooper TargetingLooper;

	protected readonly Material LaserMat = MaterialPool.MatFrom("Special/LaserBeamEnemy");

	public override Thing CurrentTarget => curTarget;

	public override void Tick()
	{
		base.Tick();
		if (stunner.Stunned)
		{
			LoseTarget();
			return;
		}
		if (curTarget != null && curTarget.destroyed)
		{
			LoseTarget();
		}
		if (curTarget == null)
		{
			curTarget = this.ClosestReachableEnemyTarget(null, 12f, needsLOStoDynamic: true, needsLOStoStatic: true);
			WarmupTicksLeft = 160;
			if (curTarget != null)
			{
				GenSound.PlaySoundAt(base.Position, "Tower/LaserDetect", 1f);
				TargetingLooper = new SoundLooperThing(this, (AudioClip)Resources.Load("Sounds/Tower/LaserTargetLoop"), 0.5f);
			}
		}
		else if (CanShoot(curTarget))
		{
			if (WarmupTicksLeft > 0)
			{
				WarmupTicksLeft--;
			}
			if (WarmupTicksLeft <= 0)
			{
				Shoot(curTarget);
			}
		}
		else
		{
			LoseTarget();
		}
	}

	protected void LoseTarget()
	{
		if (TargetingLooper != null)
		{
			TargetingLooper.Cleanup();
			TargetingLooper = null;
		}
		if (curTarget != null)
		{
			GenSound.PlaySoundAt(base.Position, "Tower/LaserLoseTarget", 1f);
			curTarget = null;
		}
	}

	public override void Draw()
	{
		base.Draw();
		if (curTarget != null && CanShoot(curTarget))
		{
			DrawAimingLaserToward(curTarget);
		}
	}

	protected bool CanShoot(Thing t)
	{
		float num = SquaredDistTo(t);
		if (num < 0f || num > 144f)
		{
			return false;
		}
		if (!GenGrid.LineOfSight(base.Position, t.Position))
		{
			return false;
		}
		return true;
	}

	protected float SquaredDistTo(Thing t)
	{
		return (t.Position.ToVector3() - this.TrueCenter()).MagnitudeHorizontalSquared();
	}

	protected void Shoot(Thing t)
	{
		GenSound.PlaySoundAt(base.Position, "Tower/LaserShot", 1f);
		t.TakeDamage(new DamageInfo(DamageType.Bullet, 25));
		WarmupTicksLeft = 40;
	}

	protected void DrawAimingLaserToward(Thing t)
	{
		Vector3 vector = this.TrueCenter();
		vector.y = 0f;
		Vector3 drawPos = t.DrawPos;
		drawPos.y = 0f;
		DrawLaserBetween(vector, drawPos, 0.05f);
		float num = (float)WarmupTicksLeft * 0.1f;
		float y = Quaternion.LookRotation(drawPos - vector).eulerAngles.y;
		float num2 = y + num;
		if (num2 > 360f)
		{
			num2 -= 360f;
		}
		float num3 = y - num;
		if (num3 < 0f)
		{
			num3 += 360f;
		}
		float num4 = (vector - drawPos).MagnitudeHorizontal();
		Vector3 lineEnd = vector + Quaternion.AngleAxis(num2, Vector3.up) * Vector3.forward * num4;
		Vector3 lineEnd2 = vector + Quaternion.AngleAxis(num3, Vector3.up) * Vector3.forward * num4;
		DrawLaserBetween(vector, lineEnd, 0.08f);
		DrawLaserBetween(vector, lineEnd2, 0.08f);
	}

	protected void DrawLaserBetween(Vector3 LineStart, Vector3 LineEnd, float LaserWidth)
	{
		Vector3 pos = (LineStart + LineEnd) / 2f;
		pos.y = 15f;
		float z = (LineStart - LineEnd).MagnitudeHorizontal();
		Vector3 s = new Vector3(LaserWidth, 1f, z);
		Quaternion q = Quaternion.LookRotation(LineEnd - LineStart);
		Matrix4x4 matrix = default(Matrix4x4);
		matrix.SetTRS(pos, q, s);
		Graphics.DrawMesh(MeshPool.plane10, matrix, LaserMat, 0);
	}

	public override void Destroy()
	{
		LoseTarget();
		base.Destroy();
	}
}
