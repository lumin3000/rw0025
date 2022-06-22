using System;
using UnityEngine;

public class DialogBox_DebugIncidentChooser : DialogBox_Debug
{
	public override void DoDialogBoxGUI()
	{
		GenUI.SetFontSmall();
		GUI.skin.button.alignment = TextAnchor.MiddleLeft;
		curOffset = new Vector2(0f, 0f);
		UIWidgets.DrawWindow(winRect);
		GUI.BeginGroup(winRect.GetInnerRect(10f));
		foreach (IncidentDefinition allIncidentDef in IncidentDatabase.allIncidentDefs)
		{
			IncidentDefinition localDef = allIncidentDef;
			Action action = delegate
			{
				IncidentParms incidentParms = new IncidentParms();
				if (localDef.pointsScaleable)
				{
					incidentParms.points = Find.Storyteller.incidentMaker.PointsForIncidentNow(localDef);
				}
				Debug.Log(string.Concat("Test executing ", localDef, " with ", incidentParms));
				localDef.TryExecute(incidentParms);
			};
			AddOption(localDef.GetType().Name, action);
		}
		GUI.EndGroup();
		DetectShouldClose(doButton: true);
		GenUI.AbsorbAllInput();
		GUI.skin.button.alignment = TextAnchor.UpperLeft;
	}
}
