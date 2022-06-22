using UnityEngine;

public class MapPointer : TutorItem, Saveable
{
	protected virtual Vector3 Location => Find.Map.Center.ToVector3Shifted();

	protected virtual string LabelText => "LabelText";

	public virtual void ExposeData()
	{
	}

	public virtual void InitializePointer()
	{
	}

	public override void TutorItemOnGUI()
	{
		Vector3 vector = Gen.InvertedWorldToScreen(Location);
		Rect position = new Rect(vector.x - 100f, vector.y + 5f, 200f, 200f);
		GUI.skin.label.alignment = TextAnchor.UpperCenter;
		GenUI.SetFontTiny();
		GUI.Label(position, LabelText);
		GUI.skin.label.alignment = TextAnchor.UpperLeft;
	}
}
