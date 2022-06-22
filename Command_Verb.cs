using UnityEngine;

internal class Command_Verb : Command
{
	public Verb commandVerb;

	public override bool DoButtonGUI(IntVec2 coords)
	{
		bool result = base.DoButtonGUI(coords);
		Rect rect = ButtonRect(coords);
		return result;
	}
}
