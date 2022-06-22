using System.Collections.Generic;
using UnityEngine;

public class DebugDrawer
{
	private List<DebugSquare> debugSquares = new List<DebugSquare>();

	public void MakeDebugSquare(IntVec3 Sq)
	{
		MakeDebugSquare(Sq, string.Empty, 50, 100, 50);
	}

	public void MakeDebugSquare(IntVec3 Sq, string DisplayString, int ColorValue, int ColorMax)
	{
		MakeDebugSquare(Sq, DisplayString, ColorValue, ColorMax, 50);
	}

	public void MakeDebugSquare(IntVec3 Sq, string DisplayString, int ColorValue, int ColorMax, int LifespanTicks)
	{
		DebugSquare debugSquare = new DebugSquare();
		debugSquare.sq = Sq;
		debugSquare.displayString = DisplayString;
		debugSquare.spectrumIndex = (int)((float)ColorValue / (float)ColorMax * 100f);
		debugSquare.ticksLeft = LifespanTicks;
		debugSquares.Add(debugSquare);
	}

	public void DebugDrawerUpdate()
	{
		foreach (DebugSquare debugSquare in debugSquares)
		{
			debugSquare.DebugSquareDraw();
		}
	}

	public void DebugDrawerTick()
	{
		List<DebugSquare> list = new List<DebugSquare>();
		foreach (DebugSquare debugSquare in debugSquares)
		{
			debugSquare.ticksLeft -= 1f;
			if (debugSquare.ticksLeft <= 0f)
			{
				list.Add(debugSquare);
			}
		}
		foreach (DebugSquare item in list)
		{
			debugSquares.Remove(item);
		}
	}

	public void DebugDrawerOnGUI()
	{
		if (Find.CameraMap.CurrentZoom != 0)
		{
			return;
		}
		GenUI.SetFontTiny();
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		GUI.color = new Color(1f, 1f, 1f, 0.5f);
		foreach (DebugSquare debugSquare in debugSquares)
		{
			debugSquare.DebugSquareOnGUI();
		}
		GUI.color = Color.white;
		GUI.skin.label.alignment = TextAnchor.UpperLeft;
	}
}
