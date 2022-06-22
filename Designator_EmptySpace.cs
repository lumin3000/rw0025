using System;
using UnityEngine;

public class Designator_EmptySpace : Designator
{
	public override ButtonState DrawOptButton(Vector2 Loc)
	{
		return ButtonState.Clear;
	}

	public override void DesignateAt(IntVec3 sq)
	{
		throw new NotImplementedException();
	}
}
