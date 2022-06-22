using System;
using System.Collections.Generic;
using UnityEngine;

public class DragBox
{
	private const float DragBoxMinDiagonal = 0.5f;

	public bool active;

	public Vector3 start;

	public float LeftX => Math.Min(start.x, Gen.MouseWorldPosVector3().x);

	public float RightX => Math.Max(start.x, Gen.MouseWorldPosVector3().x);

	public float BotZ => Math.Min(start.z, Gen.MouseWorldPosVector3().z);

	public float TopZ => Math.Max(start.z, Gen.MouseWorldPosVector3().z);

	public Rect ScreenRect
	{
		get
		{
			Vector2 vector = Find.CameraMap.InvertedWorldToScreenPoint(start);
			Vector2 mousePosition = Event.current.mousePosition;
			if (mousePosition.x < vector.x)
			{
				float x = mousePosition.x;
				mousePosition.x = vector.x;
				vector.x = x;
			}
			if (mousePosition.y < vector.y)
			{
				float y = mousePosition.y;
				mousePosition.y = vector.y;
				vector.y = y;
			}
			Rect result = default(Rect);
			result.xMin = vector.x;
			result.xMax = mousePosition.x;
			result.yMin = vector.y;
			result.yMax = mousePosition.y;
			return result;
		}
	}

	public bool IsValid => (start - Gen.MouseWorldPosVector3()).magnitude > 0.5f;

	public void DragBoxOnGUI()
	{
		if (active && IsValid)
		{
			GenUI.DrawBox(ScreenRect, 2);
		}
	}

	public IEnumerable<Thing> ContainedThings()
	{
		return SelectionUtility.MultiSelectableThingsInRect(ScreenRect);
	}

	public bool Contains(Thing t)
	{
		if (t is Pawn)
		{
			return Contains((t as Pawn).drawer.DrawPos);
		}
		foreach (IntVec3 item in Gen.SquaresOccupiedBy(t))
		{
			if (Contains(item.ToVector3Shifted()))
			{
				return true;
			}
		}
		return false;
	}

	public bool Contains(Vector3 v)
	{
		if (v.x + 0.5f > LeftX && v.x - 0.5f < RightX && v.z + 0.5f > BotZ && v.z - 0.5f < TopZ)
		{
			return true;
		}
		return false;
	}
}
