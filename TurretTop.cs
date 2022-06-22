using UnityEngine;

public class TurretTop
{
	private const float IdleTurnDegreesPerTick = 0.26f;

	private const int IdleTurnDuration = 140;

	private const int IdleTurnIntervalMin = 150;

	private const int IdleTurnIntervalMax = 350;

	private Building_Turret parentTurret;

	private float CurRotationInt;

	private int ticksUntilIdleTurn;

	private int idleTurnTicksLeft;

	private bool idleTurnClockwise;

	private static readonly Material TurretTopIcon = MaterialPool.MatFrom("Icons/Building/TurretGun_Top");

	private float CurRotation
	{
		get
		{
			return CurRotationInt;
		}
		set
		{
			CurRotationInt = value;
			if (CurRotationInt > 360f)
			{
				CurRotationInt -= 360f;
			}
			if (CurRotationInt < 0f)
			{
				CurRotationInt += 360f;
			}
		}
	}

	public TurretTop(Building_Turret ParentTurret)
	{
		parentTurret = ParentTurret;
	}

	public void TurretTopTick()
	{
		Thing currentTarget = parentTurret.CurrentTarget;
		if (currentTarget != null)
		{
			float num2 = (CurRotation = (currentTarget.DrawPos - parentTurret.DrawPos).AngleFlat());
			ticksUntilIdleTurn = Random.Range(150, 351);
		}
		else if (ticksUntilIdleTurn > 0)
		{
			ticksUntilIdleTurn--;
			if (ticksUntilIdleTurn == 0)
			{
				if (Random.value < 0.5f)
				{
					idleTurnClockwise = true;
				}
				else
				{
					idleTurnClockwise = false;
				}
				idleTurnTicksLeft = 140;
			}
		}
		else
		{
			if (idleTurnClockwise)
			{
				CurRotation += 0.26f;
			}
			else
			{
				CurRotation -= 0.26f;
			}
			idleTurnTicksLeft--;
			if (idleTurnTicksLeft <= 0)
			{
				ticksUntilIdleTurn = Random.Range(150, 351);
			}
		}
	}

	public void DrawTurret()
	{
		Matrix4x4 matrix = default(Matrix4x4);
		matrix.SetTRS(parentTurret.DrawPos + Altitudes.AltIncVect, CurRotation.ToQuat(), Vector3.one);
		Graphics.DrawMesh(MeshPool.plane20, matrix, TurretTopIcon, 0);
	}
}
