using UnityEngine;

public static class UISounds
{
	public const float VolumeQuiet = 0.04f;

	public const float Volume = 0.1f;

	public const float VolumeHigh = 0.2f;

	public const float VolumeExtraHigh = 0.3f;

	public static readonly AudioClip TickLow = Res.LoadSound("Interface/TickLow");

	public static readonly AudioClip TickHigh = Res.LoadSound("Interface/TickHigh");

	public static readonly AudioClip TickTiny = Res.LoadSound("Interface/TickTiny");

	public static readonly AudioClip WhiteNoiseLoop = Res.LoadSound("Interface/WhiteNoiseLoop");

	public static readonly AudioClip DragLoopMeta = Res.LoadSound("Interface/DragLoopMeta");

	public static readonly AudioClip RadioButtonClicked = TickTiny;

	public static readonly AudioClip CheckboxTurnedOff = TickLow;

	public static readonly AudioClip CheckboxTurnedOn = TickHigh;

	public static readonly AudioClip TutorMessageAppear = Res.LoadSound("Interface/GentleBeep");

	public static readonly AudioClip LevelUp = Res.LoadSound("Interface/LevelUp");

	public static readonly AudioClip Click = Res.LoadSound("Interface/Click");

	public static readonly AudioClip ClickReject = Res.LoadSound("Interface/ClickReject");

	public static readonly AudioClip PageChange = Res.LoadSound("Interface/PageChange");

	public static readonly AudioClip TabOpen = Res.LoadSound("Interface/TabOpen");

	public static readonly AudioClip TabClose = Res.LoadSound("Interface/TabClose");

	public static readonly AudioClip SubmenuSelect = Res.LoadSound("Interface/SubmenuSelect");

	public static readonly AudioClip MessageAlert = Res.LoadSound("Interface/AlertBasic");

	public static readonly AudioClip MessageAlertNegative = Res.LoadSound("Interface/AlertNegative");

	public static readonly AudioClip MessageSeriousAlert = Res.LoadSound("Interface/AlertRed");

	public static readonly AudioClip BuyThing = Res.LoadSound("Interface/BuyThing");

	public static readonly AudioClip BuyLevel = Res.LoadSound("Interface/TickHigh");

	public static readonly AudioClip SellLevel = Res.LoadSound("Interface/TickLow");
}
