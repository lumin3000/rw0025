using UnityEngine;

public class UI_MapRoot : UI_Root
{
	public UI_ThingOverlays thingOverlays = new UI_ThingOverlays();

	public UI_FloatMenu floatMenu = new UI_FloatMenu();

	protected UI_DebugModeReadout debugReadout = new UI_DebugModeReadout();

	public UI_GameControlsRoot modeControls = new UI_GameControlsRoot();

	private UI_MouseoverReadout mouseoverReadout = new UI_MouseoverReadout();

	public UI_GlobalControls globalControls = new UI_GlobalControls();

	protected UI_ResourceList resourceList = new UI_ResourceList();

	public UI_Alerts alerts = new UI_Alerts();

	public UI_WeaponsControl weaponsControl = new UI_WeaponsControl();

	public override void UIRootOnGUI()
	{
		Find.TooltipGiverList.DispenseAllTooltips();
		if (Event.current.type != EventType.Repaint && Event.current.type != EventType.Layout)
		{
			base.UIRootOnGUI();
			DebugTools.DebugToolsOnGUI();
			floatMenu.FloatMenuOnGUI();
			if (!screenshotMode.ShouldFilterCurrentEvent)
			{
				Find.LetterStack.LettersOnGUI();
				alerts.AlertListOnGUI();
				Find.Tutor.TutorOnGUI();
				globalControls.GlobalControlsOnGUI();
				modeControls.GameControlsOnGUI();
				resourceList.ResourceListOnGUI();
				mouseoverReadout.MouseoverReadoutOnGUI();
			}
			Find.RoomManager.RoomManagerOnGUI();
			thingOverlays.ThingOverlaysOnGUI();
		}
		else
		{
			thingOverlays.ThingOverlaysOnGUI();
			Find.RoomManager.RoomManagerOnGUI();
			if (!screenshotMode.ShouldFilterCurrentEvent)
			{
				mouseoverReadout.MouseoverReadoutOnGUI();
				Find.Tutor.TutorOnGUI();
				resourceList.ResourceListOnGUI();
				modeControls.GameControlsOnGUI();
				globalControls.GlobalControlsOnGUI();
				alerts.AlertListOnGUI();
				Find.LetterStack.LettersOnGUI();
			}
			floatMenu.FloatMenuOnGUI();
			DebugTools.DebugToolsOnGUI();
			base.UIRootOnGUI();
		}
		RootInput.RootInputOnGUI();
		debugReadout.SquareContentsOnGUI();
		Find.DebugDrawer.DebugDrawerOnGUI();
		if (!Debug.isDebugBuild || Event.current.type != EventType.KeyDown)
		{
			return;
		}
		if (Event.current.keyCode == KeyCode.Slash)
		{
			Find.Dialogs.AddDialogBox(new DialogBox_DebugMenu());
		}
		if (Event.current.keyCode == KeyCode.Quote)
		{
			if (Game.editMode != EditMode.Simple)
			{
				Game.editMode = EditMode.Simple;
			}
			else
			{
				Game.editMode = EditMode.Off;
			}
		}
		if (Event.current.keyCode == KeyCode.Semicolon)
		{
			if (Game.editMode != EditMode.Full)
			{
				Game.editMode = EditMode.Full;
			}
			else
			{
				Game.editMode = EditMode.Off;
			}
		}
	}

	public void PlayUIUpdate()
	{
		UI_Messages.Update();
		modeControls.TabsUpdate();
		feedbackFloaters.FeedbackUpdate();
		debugReadout.DebugReadoutUpdate();
		alerts.AlertListUpdate();
	}
}
