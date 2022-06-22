using UnityEngine;

public class DebugSquare
{
	public IntVec3 sq;

	public string displayString;

	public int spectrumIndex;

	public float ticksLeft;

	public void DebugSquareDraw()
	{
		DebugRender.RenderSquareSpectrum(sq, spectrumIndex);
	}

	public void DebugSquareOnGUI()
	{
		Vector2 vector = sq.ToScreenPosition();
		Rect position = new Rect(vector.x - 20f, vector.y - 20f, 40f, 40f);
		if (displayString != null && displayString.Length > 0)
		{
			GUI.Label(position, displayString);
		}
	}
}
