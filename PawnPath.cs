using System.Collections.Generic;
using UnityEngine;

public class PawnPath
{
	public Pawn pathingPawn;

	public bool found;

	public float cost;

	public List<PathFinderNode> nodeList = new List<PathFinderNode>();

	private readonly float PathAltitude;

	public IntVec3 LastNode => nodeList[nodeList.Count - 1].Position;

	public IntVec3 SecondLastNode => nodeList[nodeList.Count - 2].Position;

	public static PawnPath NotFound
	{
		get
		{
			PawnPath pawnPath = new PawnPath();
			pawnPath.found = false;
			return pawnPath;
		}
	}

	public PawnPath()
	{
		PathAltitude = Altitudes.AltitudeFor(AltitudeLayer.OverWaist);
	}

	public override string ToString()
	{
		return string.Concat("PawnPath[", pathingPawn, ", found=", found, ", cost=", cost, " node count= ", nodeList.Count, "]");
	}

	public void DrawPath()
	{
		if (!found || nodeList.Count <= 0)
		{
			return;
		}
		for (int i = 0; i < nodeList.Count - 1; i++)
		{
			Vector3 aPos = nodeList[i].Position.ToVector3Shifted();
			aPos.y = PathAltitude;
			Vector3 bPos = nodeList[i + 1].Position.ToVector3Shifted();
			bPos.y = PathAltitude;
			GenRender.DrawLineBetween(aPos, bPos);
		}
		if (pathingPawn != null)
		{
			Vector3 drawPos = pathingPawn.DrawPos;
			drawPos.y = PathAltitude;
			Vector3 vector = nodeList[0].Position.ToVector3Shifted();
			vector.y = PathAltitude;
			if ((drawPos - vector).sqrMagnitude > 0.01f)
			{
				GenRender.DrawLineBetween(drawPos, vector);
			}
		}
	}
}
