using System.Collections.Generic;
using UnityEngine;

public struct IntRect
{
	public int minX;

	public int maxX;

	public int minZ;

	public int maxZ;

	public int Area => Width * Height;

	public int Width
	{
		get
		{
			return maxX - minX + 1;
		}
		set
		{
			maxX = minX + value - 1;
		}
	}

	public int Height
	{
		get
		{
			return maxZ - minZ + 1;
		}
		set
		{
			maxZ = minZ + value - 1;
		}
	}

	public IntVec3 BottomLeft => new IntVec3(minX, 0, minZ);

	public IntVec3 TopRight => new IntVec3(maxX, 0, maxZ);

	public IntVec3 RandomSquare => new IntVec3(Random.Range(minX, maxX + 1), 0, Random.Range(minZ, maxZ + 1));

	public Vector3 RandomVector3 => new Vector3(Random.Range((float)minX, (float)maxX), 0f, Random.Range((float)minZ, (float)maxZ));

	public static IntRect WholeMap => new IntRect(0, 0, Find.Map.Size.x, Find.Map.Size.z);

	public IntRect(int minX, int minZ, int width, int height)
	{
		this.minX = minX;
		this.minZ = minZ;
		maxX = minX + width - 1;
		maxZ = minZ + height - 1;
	}

	public IEnumerator<IntVec3> GetEnumerator()
	{
		for (int z = minZ; z <= maxZ; z++)
		{
			for (int x = minX; x <= maxX; x++)
			{
				yield return new IntVec3(x, 0, z);
			}
		}
	}

	public static IntRect FromLimits(int minX, int minZ, int maxX, int maxZ)
	{
		IntRect result = default(IntRect);
		result.minX = minX;
		result.minZ = minZ;
		result.maxX = maxX;
		result.maxZ = maxZ;
		return result;
	}

	public static IntRect CenteredOn(IntVec3 center, int radius)
	{
		IntRect result = default(IntRect);
		result.minX = center.x - radius;
		result.maxX = center.x + radius;
		result.minZ = center.z - radius;
		result.maxZ = center.z + radius;
		return result;
	}

	public void ClipInsideMap()
	{
		if (minX < 0)
		{
			minX = 0;
		}
		if (minZ < 0)
		{
			minZ = 0;
		}
		if (maxX > Find.Map.Size.x - 1)
		{
			maxX = Find.Map.Size.x - 1;
		}
		if (maxZ > Find.Map.Size.z - 1)
		{
			maxZ = Find.Map.Size.z - 1;
		}
	}

	public void ClipInsideRect(IntRect otherRect)
	{
		if (minX < otherRect.minX)
		{
			minX = otherRect.minX;
		}
		if (maxX > otherRect.maxX)
		{
			maxX = otherRect.maxX;
		}
		if (minZ < otherRect.minZ)
		{
			minZ = otherRect.minZ;
		}
		if (maxZ > otherRect.maxZ)
		{
			maxZ = otherRect.maxZ;
		}
	}

	public bool Contains(IntVec3 sq)
	{
		return sq.x >= minX && sq.x <= maxX && sq.z >= minZ && sq.z <= maxZ;
	}

	public void Expand(int amount)
	{
		minX -= amount;
		minZ -= amount;
		maxX += amount;
		maxZ += amount;
	}

	public override string ToString()
	{
		return "IntRect(minX=" + minX + " minZ= " + minZ + " maxX=" + maxX + " maxZ=" + maxZ + " Width=" + Width + " Height=" + Height + ")";
	}
}
