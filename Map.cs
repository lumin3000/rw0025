using System.Collections.Generic;
using UnityEngine;

public class Map
{
	public bool initialized;

	public MapInfo info = new MapInfo();

	public ThingLister thingLister;

	public GameEnder gameEnder;

	public Grids grids;

	public ThingDrawManager drawManager;

	public MapDrawer mapDrawer;

	public BuildingManager buildingManager;

	public FogGrid fogGrid;

	public RoomManager roomManager;

	public ResearchManager researchManager;

	public PawnDestinationManager pawnDestinationManager;

	public TooltipGiverList tooltipGiverList;

	public BreakdownManager breakdownManager;

	public ReservationManager reservationManager;

	public PawnManager pawnManager;

	public UI_MapRoot mapUIRoot;

	public DesignationManager designationManager;

	public AIKingManager aiKingManager;

	public VisitorManager visitorManager;

	public Storyteller storyteller;

	public UI_Tutor tutor;

	public SlotGroupManager slotGroupManager;

	public Autosaver autosaver;

	public ReachabilityRegions reachabilityRegions;

	public DebugDrawer debugDrawer;

	public ColonyInfo colonyInfo;

	public LetterStack letterStack;

	public GlowGrid glowGrid;

	public TerrainGrid terrainGrid;

	public PathGrid pathGrid;

	public RoofGrid roofGrid;

	public MapConditionManager mapConditionManager;

	public HomeZoneGrid homeZoneGrid;

	public FertilityGrid fertilityGrid;

	public WeatherManager weatherManager;

	public PlaySettings playSettings;

	public HistoricalPawns historicalPawns;

	public MapSound mapSound;

	public IntVec3 Size => info.size;

	public IntVec3 Center => new IntVec3(Size.x / 2, 0, Size.z / 2);

	public int Area => Size.x * Size.z;

	public IEnumerable<IntVec3> AllSquares
	{
		get
		{
			for (int z = 0; z < Size.x; z++)
			{
				for (int y = 0; y < Size.y; y++)
				{
					for (int x = 0; x < Size.x; x++)
					{
						yield return new IntVec3(x, y, z);
					}
				}
			}
		}
	}

	public void InitComponents()
	{
		thingLister = new ThingLister();
		gameEnder = new GameEnder();
		grids = new Grids();
		drawManager = new ThingDrawManager();
		mapDrawer = new MapDrawer();
		tooltipGiverList = new TooltipGiverList();
		buildingManager = new BuildingManager();
		roomManager = new RoomManager();
		fogGrid = new FogGrid();
		glowGrid = new GlowGrid();
		pawnDestinationManager = new PawnDestinationManager();
		mapSound = new MapSound();
		researchManager = new ResearchManager();
		breakdownManager = new BreakdownManager();
		reservationManager = new ReservationManager();
		pawnManager = new PawnManager();
		mapUIRoot = new UI_MapRoot();
		designationManager = new DesignationManager();
		aiKingManager = new AIKingManager();
		debugDrawer = new DebugDrawer();
		colonyInfo = new ColonyInfo();
		visitorManager = new VisitorManager();
		tutor = new UI_Tutor();
		autosaver = new Autosaver();
		slotGroupManager = new SlotGroupManager();
		reachabilityRegions = new ReachabilityRegions();
		letterStack = new LetterStack();
		terrainGrid = new TerrainGrid();
		pathGrid = new PathGrid();
		roofGrid = new RoofGrid();
		mapConditionManager = new MapConditionManager();
		homeZoneGrid = new HomeZoneGrid();
		fertilityGrid = new FertilityGrid();
		weatherManager = new WeatherManager();
		historicalPawns = new HistoricalPawns();
		playSettings = new PlaySettings();
		LinkGrid.ResetStaticData();
		GlowFlooder.ResetStaticData();
		PowerNetManager.ResetStaticData();
		PowerNetGrid.ResetStaticData();
	}

	public void MapUpdate()
	{
		PowerNetManager.UpdatePowerNetsAndConnections_First();
		RoofCollapseChecker.RoofCollapseCheckerUpdate_First();
		roomManager.ResolveRoomChangesUpdate_First();
		glowGrid.GlowGridUpdate_First();
		mapDrawer.MapMeshDrawerUpdate_First();
		mapDrawer.DrawMapMesh();
		drawManager.DrawDynamicThings();
		roomManager.DrawRooms();
		designationManager.DrawDesignations();
		if (DebugSettings.drawPawnDebug)
		{
			pawnDestinationManager.DrawDestinations();
			reservationManager.DrawWorkReservations();
		}
		debugDrawer.DebugDrawerUpdate();
		roofGrid.DebugDrawRoots();
		reachabilityRegions.DebugDrawReachability();
		mapUIRoot.PlayUIUpdate();
		OverlayDrawer.DrawAllOverlays();
	}

	public void MapCleanup()
	{
		foreach (Thing item in thingLister.spawnedThings.ListFullCopy())
		{
			item.Destroy();
		}
		AudioSource[] components = Find.CameraMap.GetComponents<AudioSource>();
		foreach (AudioSource audioSource in components)
		{
			if (audioSource.name == "SoundLooperSource")
			{
				Object.Destroy(audioSource);
			}
		}
	}
}
