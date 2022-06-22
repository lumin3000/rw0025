using System.Linq;
using UnityEngine;

public class ShootLine
{
	public bool found;

	public IntVec3 source;

	public IntVec3 dest;

	public static ShootLine NotFound
	{
		get
		{
			ShootLine shootLine = new ShootLine();
			shootLine.found = false;
			return shootLine;
		}
	}

	public ShootLine(IntVec3 source, IntVec3 dest)
	{
		found = true;
		this.source = source;
		this.dest = dest;
	}

	private ShootLine()
	{
	}

	public void ChangeDestToMissWild()
	{
		if ((double)(dest - source).LengthHorizontal < 2.5)
		{
			IntVec3 intVec = IntVec3.FromVector3((dest - source).ToVector3().normalized * 2f);
			dest += intVec;
		}
		dest = dest.AdjacentSquares8Way().ToList()[Random.Range(0, 8)];
	}

	public override string ToString()
	{
		if (!found)
		{
			return "ShootLine - Not Found";
		}
		return string.Concat("ShootLine - ", source, " to ", dest);
	}
}
