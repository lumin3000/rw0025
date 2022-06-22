using UnityEngine;

public static class Find
{
	private static GameRoot gameRoot;

	private static CameraMap cameraMap;

	private static CameraFade cameraFade;

	private static GameObject audioListenerDummy;

	public static EntryRoot EntryRoot => GameObject.FindGameObjectWithTag("GameCoreDummy").GetComponent<EntryRoot>();

	public static GameRoot GameRoot => gameRoot;

	public static UI_Root UIRoot
	{
		get
		{
			if (Game.GMode == GameMode.Gameplay)
			{
				return UIMapRoot;
			}
			return UIEntryRoot;
		}
	}

	public static UI_EntryRoot UIEntryRoot => EntryRoot.UIEntryRoot;

	public static UI_MapRoot UIMapRoot => Map.mapUIRoot;

	public static UI_DialogBoxHandler Dialogs => UIRoot.dialogs;

	public static DialogBox_CharMaker CharMaker => (DialogBox_CharMaker)Dialogs.TopDialog;

	public static UI_WeaponsControl WeaponsControl => UIMapRoot.weaponsControl;

	public static GameControls_TabInspect TabDirect => UIMapRoot.modeControls.tabInspect;

	public static UI_ThingSelector Selector => UIMapRoot.modeControls.tabInspect.selector;

	public static UI_ThingOverlays MapUI => UIMapRoot.thingOverlays;

	public static UI_FloatMenu FloatMenu => UIMapRoot.floatMenu;

	public static UI_Alerts Alerts => UIMapRoot.alerts;

	public static CameraMap CameraMap => cameraMap;

	public static CameraFade CameraFade => cameraFade;

	public static Camera CameraCurrent => CameraMap.camera;

	public static ColorCorrectionCurves CameraColor => CameraMap.GetComponent<ColorCorrectionCurves>();

	public static GameObject AudioListenerDummy => audioListenerDummy;

	public static AudioSource CameraAudioSource => GameObject.Find("CameraSoundMaker").GetComponent<AudioSource>();

	public static Map Map => gameRoot.curMap;

	public static TickManager TickManager => gameRoot.tickManager;

	public static DestroyManager DestroyManager => gameRoot.destroyManager;

	public static RealTime RealTime => gameRoot.realTime;

	public static ResourceManager ResourceManager => gameRoot.resourceManager;

	public static Grids Grids => Map.grids;

	public static FogGrid FogGrid => Map.fogGrid;

	public static GlowGrid GlowGrid => Map.glowGrid;

	public static ThingDrawManager DrawManager => Map.drawManager;

	public static PawnDestinationManager PawnDestinationManager => Map.pawnDestinationManager;

	public static TooltipGiverList TooltipGiverList => Map.tooltipGiverList;

	public static BuildingManager BuildingManager => Map.buildingManager;

	public static RoomManager RoomManager => Map.roomManager;

	public static ResearchManager ResearchManager => Map.researchManager;

	public static ReservationManager ReservationManager => Map.reservationManager;

	public static PawnManager PawnManager => Map.pawnManager;

	public static DesignationManager DesignationManager => Map.designationManager;

	public static AIKingManager AIKingManager => Map.aiKingManager;

	public static DebugDrawer DebugDrawer => Map.debugDrawer;

	public static ColonyInfo BaseStats => Map.colonyInfo;

	public static VisitorManager VisitorManager => Map.visitorManager;

	public static Storyteller Storyteller => Map.storyteller;

	public static UI_Tutor Tutor => Map.tutor;

	public static SlotGroupManager SlotGroupManager => Map.slotGroupManager;

	public static ReachabilityRegions ReachabilityRegions => Map.reachabilityRegions;

	public static ThingLister ThingLister => Map.thingLister;

	public static GameEnder GameEnder => Map.gameEnder;

	public static ColonyInfo ColonyInfo => Map.colonyInfo;

	public static LetterStack LetterStack => Map.letterStack;

	public static MapDrawer MapDrawer => Map.mapDrawer;

	public static TerrainGrid TerrainGrid => Map.terrainGrid;

	public static PathGrid PathGrid => Map.pathGrid;

	public static RoofGrid RoofGrid => Map.roofGrid;

	public static MapConditionManager MapConditionManager => Map.mapConditionManager;

	public static HomeZoneGrid HomeZoneGrid => Map.homeZoneGrid;

	public static FertilityGrid FertilityGrid => Map.fertilityGrid;

	public static WeatherManager WeatherManager => Map.weatherManager;

	public static HistoricalPawns HistoricalPawns => Map.historicalPawns;

	public static PlaySettings PlaySettings => Map.playSettings;

	public static Trader ActiveTrader => VisitorManager.ActiveTrader;

	static Find()
	{
		ResetFind();
	}

	public static void ResetFind()
	{
		gameRoot = null;
		cameraMap = null;
		cameraFade = null;
		audioListenerDummy = null;
		audioListenerDummy = GameObject.FindGameObjectWithTag("AudioListenerDummy");
		if (Application.loadedLevelName != "Entry")
		{
			cameraMap = GameObject.FindGameObjectWithTag("CameraMap").GetComponent<CameraMap>();
			gameRoot = GameObject.FindGameObjectWithTag("GameCoreDummy").GetComponent<GameRoot>();
			GameObject gameObject = GameObject.FindGameObjectWithTag("CameraFadeDummy");
			if (gameObject != null)
			{
				cameraFade = gameObject.GetComponent<CameraFade>();
			}
		}
	}
}
