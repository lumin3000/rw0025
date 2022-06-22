using UnityEngine;

public class DamageInfo
{
	public readonly DamageType type;

	public float direction;

	private int amountInternal;

	public int Amount
	{
		get
		{
			if (!DebugSettings.damageEnabled)
			{
				return 0;
			}
			return amountInternal;
		}
	}

	public DamageInfo(DamageType newType, int newAmount, float newDirection)
	{
		type = newType;
		amountInternal = newAmount;
		direction = newDirection;
	}

	public DamageInfo(DamageType newType, int newAmount, Vector3 newDirection)
	{
		type = newType;
		amountInternal = newAmount;
		if (newDirection.x != 0f || newDirection.z != 0f)
		{
			direction = Quaternion.LookRotation(newDirection).eulerAngles.y;
		}
		else
		{
			direction = Random.Range(0, 360);
		}
	}

	public DamageInfo(DamageType newType, int newAmount)
	{
		type = newType;
		amountInternal = newAmount;
		direction = Random.Range(0, 360);
	}

	public DamageInfo(DamageInfo CloneSource)
	{
		type = CloneSource.type;
		amountInternal = CloneSource.Amount;
	}

	public override string ToString()
	{
		return string.Concat("Type=", type, ", Amount= ", Amount);
	}
}
