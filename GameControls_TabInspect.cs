using UnityEngine;

public class GameControls_TabInspect : UIPanel_Tab
{
	public UI_InspectPane inspector = new UI_InspectPane();

	public UI_ThingSelector selector = new UI_ThingSelector();

	public UI_VerbTargeter targeter = new UI_VerbTargeter();

	public override void PanelOpened()
	{
		inspector.Reset();
	}

	public override void PanelOnGUI(Rect fillRect)
	{
		inspector.InspectPaneOnGUI();
		targeter.TargeterOnGUI();
		selector.SelectorOnGUI();
	}

	public override void PanelUpdate()
	{
		selector.SelectorUpdate();
		targeter.TargeterUpdate();
	}
}
