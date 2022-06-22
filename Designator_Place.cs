using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Designator_Place : Designator
{
	private const float RotButSize = 64f;

	private const float RotButSpacing = 10f;

	private const float ResLineSpacing = 29f;

	private const float DragPriceDrawNumberX = 29f;

	public EntityDefinition entDefToPlace;

	public IntRot placingRot = IntRot.north;

	private static float middleMouseDownTime;

	private static readonly Texture2D RotLeftTex = Res.LoadTexture("UI/Widgets/RotLeft");

	private static readonly Texture2D RotRightTex = Res.LoadTexture("UI/Widgets/RotRight");

	private static readonly Vector2 DragPriceDrawOffset = new Vector2(19f, 17f);

	private static readonly Material InteractionSquareMaterial = MaterialPool.MatFrom("UI/Overlays/InteractionSquare");

	public override int DraggableDimensions => entDefToPlace.placingDraggableDimensions;

	public override string DescText
	{
		get
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(entDefToPlace.desc);
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			if (entDefToPlace.buildingWantsAir && !DebugSettings.worldBreathable)
			{
				stringBuilder.AppendLine("Needs air");
			}
			if (entDefToPlace is ThingDefinition && (entDefToPlace as ThingDefinition).transmitsPower)
			{
				stringBuilder.AppendLine("Needs direct power link");
			}
			return stringBuilder.ToString();
		}
	}

	public override bool Visible
	{
		get
		{
			if (Game.InEditMode)
			{
				return true;
			}
			if (entDefToPlace.researchPrerequisite != 0 && !Find.ResearchManager.HasResearched(entDefToPlace.researchPrerequisite))
			{
				return false;
			}
			foreach (EntityType buildingPrerequisite in entDefToPlace.buildingPrerequisites)
			{
				if (!Find.BuildingManager.PlayerHasBuildingOfType(buildingPrerequisite))
				{
					return false;
				}
			}
			return true;
		}
	}

	public Designator_Place(EntityType baseThingToPlace)
	{
		SetupWith(baseThingToPlace.DefinitionOfType());
	}

	public Designator_Place(EntityDefinition entDefToPlace)
	{
		SetupWith(entDefToPlace);
	}

	protected virtual void SetupWith(EntityDefinition entDefToPlace)
	{
		this.entDefToPlace = entDefToPlace;
		buttonLabel = entDefToPlace.label;
		buttonTexture = entDefToPlace.uiIcon;
		buttonTextureProps = entDefToPlace.size.ToVector2();
		buttonTextureOverdraw = entDefToPlace.overDraw;
		dragStartClip = Res.LoadSound("Interface/DesignateDragStart_Mech");
		dragProgressClip = Res.LoadSound("Interface/DesignateDragLoop_Mech");
	}

	public override AcceptanceReport CanDesignateAt(IntVec3 loc)
	{
		List<IntVec3> list = Gen.SquaresOccupiedBy(loc, placingRot, entDefToPlace.size).ToList();
		foreach (IntVec3 item in list)
		{
			if (!item.InBounds())
			{
				return new AcceptanceReport(entDefToPlace.label + " out of bounds.");
			}
		}
		if (loc.IsFogged())
		{
			return false;
		}
		foreach (PlacementRestriction placementRestriction in entDefToPlace.placementRestrictions)
		{
			AcceptanceReport acceptanceReport = PlacementRestrictions.CanPlaceWithRestriction(entDefToPlace, placementRestriction, loc, placingRot);
			if (!acceptanceReport.accepted)
			{
				return acceptanceReport;
			}
		}
		if (entDefToPlace.hasInteractionSquare)
		{
			IntVec3 intVec = Building.InteractionSquareWhenAt(entDefToPlace, loc, placingRot);
			if (!intVec.Standable() || !intVec.InBounds())
			{
				return new AcceptanceReport("Interaction location is blocked.");
			}
			foreach (Thing item2 in Find.Grids.ThingsAt(intVec))
			{
				Blueprint blueprint = item2 as Blueprint;
				if (blueprint != null && blueprint.def.entityDefToBuild.passability != 0)
				{
					return new AcceptanceReport("Interaction location will be blocked.");
				}
			}
		}
		if (entDefToPlace.passability != 0)
		{
			foreach (IntVec3 item3 in Gen.AdjacentSquaresCardinal(loc, placingRot, entDefToPlace.size))
			{
				if (!item3.InBounds())
				{
					continue;
				}
				foreach (Thing item4 in Find.Grids.ThingsAt(item3))
				{
					ThingDefinition thingDefinition = null;
					Blueprint blueprint2 = item4 as Blueprint;
					if (blueprint2 != null)
					{
						ThingDefinition thingDefinition2 = blueprint2.def.entityDefToBuild as ThingDefinition;
						if (thingDefinition2 == null)
						{
							continue;
						}
						thingDefinition = thingDefinition2;
					}
					else
					{
						thingDefinition = item4.def;
					}
					if (thingDefinition.hasInteractionSquare && list.Contains(Building.InteractionSquareWhenAt(thingDefinition, item4.Position, item4.rotation)))
					{
						return new AcceptanceReport(entDefToPlace.label + " would block " + thingDefinition.label + "'s interaction square.");
					}
				}
			}
		}
		TerrainDefinition terrainDefinition = entDefToPlace as TerrainDefinition;
		if (terrainDefinition != null && Find.TerrainGrid.TerrainAt(loc) == terrainDefinition)
		{
			return new AcceptanceReport(string.Concat("The terrain here is already ", terrainDefinition, "."));
		}
		foreach (Thing item5 in Find.Grids.ThingsAt(loc))
		{
			if (item5.Position == loc && item5.rotation == placingRot)
			{
				if (item5.def == entDefToPlace)
				{
					return new AcceptanceReport("Identical thing already exists here.");
				}
				Blueprint blueprint3 = item5 as Blueprint;
				if (blueprint3 != null && blueprint3.def.entityDefToBuild == entDefToPlace)
				{
					return new AcceptanceReport("Identical blueprint already exists here.");
				}
			}
		}
		if (!ConstructionUtility.BuildingCanGoOnTerrain(entDefToPlace, loc, placingRot))
		{
			return new AcceptanceReport("The terrain here cannot support this building.");
		}
		if (!Game.InEditMode)
		{
			foreach (ResourceCost cost in entDefToPlace.costList)
			{
				if (Find.ResourceManager.TotalAmountOf(cost.rType) < cost.Amount)
				{
					return new AcceptanceReport("Not enough " + cost.rType.DefinitionOfType().label + " for " + entDefToPlace.label + ".");
				}
			}
			foreach (IntVec3 item6 in Gen.SquaresOccupiedBy(loc, placingRot, entDefToPlace.size))
			{
				foreach (Thing item7 in Find.Grids.ThingsAt(item6))
				{
					if (!CanPlaceOver(item7))
					{
						return new AcceptanceReport("Space already occupied.");
					}
				}
			}
		}
		return AcceptanceReport.WasAccepted;
	}

	protected bool CanPlaceOver(Thing oldThing)
	{
		if (oldThing.def.isDebris)
		{
			return true;
		}
		if (entDefToPlace.eType == EntityType.Building_PowerPlantGeothermal && oldThing.def.eType == EntityType.SteamGeyser)
		{
			return true;
		}
		EntityDefinition entityDefinition = oldThing.def;
		Blueprint blueprint = oldThing as Blueprint;
		BuildingFrame buildingFrame = oldThing as BuildingFrame;
		EntityDefinition entityDefinition2 = null;
		if (blueprint != null)
		{
			entityDefinition = blueprint.def.entityDefToBuild;
		}
		if (buildingFrame != null)
		{
			entityDefinition = buildingFrame.def.entityDefToBuild;
		}
		if (blueprint != null && blueprint.def.entityDefToBuild.category == EntityCategory.Terrain)
		{
			return true;
		}
		if (oldThing.def.category == EntityCategory.Building || oldThing.def.eType == EntityType.Blueprint || oldThing.def.eType == EntityType.BuildingFrame)
		{
			if (entDefToPlace.eType == EntityType.Door && entityDefinition.eType == EntityType.Wall)
			{
				return true;
			}
			ThingDefinition thingDefinition = entDefToPlace as ThingDefinition;
			if (thingDefinition != null && thingDefinition.transmitsPower && entDefToPlace.eType != EntityType.Building_PowerConduit && entityDefinition.eType == EntityType.Building_PowerConduit)
			{
				return true;
			}
			if (entDefToPlace.category == EntityCategory.Terrain && entityDefinition is ThingDefinition && !((ThingDefinition)entityDefinition).fillsSquare && !((ThingDefinition)entityDefinition).neverBuildFloorsOver)
			{
				return true;
			}
			return false;
		}
		return true;
	}

	public override void DesignateAt(IntVec3 loc)
	{
		if (!Game.InEditMode)
		{
			foreach (ResourceCost cost in entDefToPlace.costList)
			{
				Find.ResourceManager.Gain(cost.rType, -cost.Amount);
			}
		}
		if (Game.InEditMode || entDefToPlace.workToBuild == 0f)
		{
			if (entDefToPlace.category == EntityCategory.Terrain)
			{
				Find.TerrainGrid.SetTerrain(loc, (TerrainDefinition)entDefToPlace);
			}
			else
			{
				Thing thing = ThingMaker.MakeThing((ThingDefinition)entDefToPlace);
				thing.Team = TeamType.Colonist;
				ThingMaker.Spawn(thing, loc, placingRot);
			}
		}
		else
		{
			ThingDefinition newDef = ConstructionUtility.BlueprintDefinitionOf(entDefToPlace);
			ThingMaker.OverwriteExistingThings(loc, placingRot, newDef, reclaimResources: true);
			ConstructionUtility.PlaceBlueprintOf(entDefToPlace, loc, placingRot);
		}
		if (Find.TickManager.Paused)
		{
			return;
		}
		foreach (IntVec3 item in Gen.SquaresOccupiedBy(loc, placingRot, entDefToPlace.size))
		{
			int num = Random.Range(4, 6);
			for (int i = 0; i < num; i++)
			{
				MoteMaker.ThrowDustPuff(item, 1f);
			}
		}
	}

	public override void FinalizeDesignationSucceeded()
	{
		GenSound.PlaySoundOnCamera("Interface/PlaceBuilding", 0.15f);
	}

	public override void DesOptionOnGUI()
	{
		base.DesOptionOnGUI();
		DoRotateControls();
		DrawCurrentPrice();
	}

	private void DoRotateControls()
	{
		if (!entDefToPlace.rotatable)
		{
			return;
		}
		Rect rect = new Rect(0f, (float)(Screen.height - 35) - 152f - 230f - 90f, 200f, 90f);
		UIWidgets.DrawWindow(rect);
		bool flag = false;
		if (Event.current.button == 2)
		{
			if (Event.current.type == EventType.MouseDown)
			{
				middleMouseDownTime = Time.realtimeSinceStartup;
			}
			if (Event.current.type == EventType.MouseUp && Time.realtimeSinceStartup - middleMouseDownTime < 0.15f)
			{
				flag = true;
			}
		}
		GUI.BeginGroup(rect);
		RotationDirection rotationDirection = RotationDirection.None;
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		GenUI.SetFontMedium();
		Rect rect2 = new Rect(rect.width / 2f - 64f - 5f, 15f, 64f, 64f);
		if (UIWidgets.ImageButton(rect2, RotLeftTex) || (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Q))
		{
			GenSound.PlaySoundOnCamera(UISounds.TickLow, 0.1f);
			rotationDirection = RotationDirection.Counterclockwise;
			Event.current.Use();
		}
		GUI.Label(rect2, "Q");
		Rect rect3 = new Rect(rect.width / 2f + 5f, 15f, 64f, 64f);
		if (UIWidgets.ImageButton(rect3, RotRightTex) || (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.E) || flag)
		{
			GenSound.PlaySoundOnCamera(UISounds.TickHigh, 0.1f);
			rotationDirection = RotationDirection.Clockwise;
			Event.current.Use();
		}
		GUI.Label(rect3, "E");
		if (rotationDirection != 0)
		{
			placingRot.Rotate(rotationDirection);
		}
		GUI.EndGroup();
		GenUI.AbsorbClicksInRect(rect.GetInnerRect(-8f));
		GUI.skin.label.alignment = TextAnchor.UpperLeft;
	}

	private void DrawCurrentPrice()
	{
		DesignationDragger dragger = Find.UIMapRoot.modeControls.tabArchitect.selectedPanel.dragger;
		if (!dragger.dragging)
		{
			return;
		}
		int num = dragger.DragSquares.Count();
		float num2 = 0f;
		Vector2 vector = Event.current.mousePosition + DragPriceDrawOffset;
		foreach (ResourceCost cost in entDefToPlace.costList)
		{
			float top = vector.y + num2;
			Rect position = new Rect(vector.x, top, 27f, 27f);
			GUI.DrawTexture(position, cost.rType.DefinitionOfType().uiIcon);
			Rect position2 = new Rect(vector.x + 29f, top, 999f, 29f);
			GUI.skin.label.alignment = TextAnchor.MiddleLeft;
			GUI.Label(position2, (num * cost.Amount).ToString());
			GUI.skin.label.alignment = TextAnchor.UpperLeft;
			num2 += 29f;
		}
	}

	public override void DesignatorUpdate()
	{
		IntVec3 intVec = Gen.MouseWorldSquare();
		if (entDefToPlace.category == EntityCategory.Terrain)
		{
			GenUI.RenderMouseoverBracket();
			return;
		}
		GhostDrawer.DrawGhostThing(color: (!CanDesignateAt(intVec).accepted) ? new Color(1f, 0f, 0f, 0.4f) : new Color(0.5f, 1f, 0.6f, 0.4f), loc: intVec, rot: placingRot, thingDef: (ThingDefinition)entDefToPlace, drawAltitude: AltitudeLayer.Blueprint);
		if (CanDesignateAt(intVec).accepted && entDefToPlace.placingDisplayMethod != null)
		{
			entDefToPlace.placingDisplayMethod();
		}
		if (entDefToPlace.hasInteractionSquare)
		{
			Vector3 position = Building.InteractionSquareWhenAt(entDefToPlace, intVec, placingRot).ToVector3ShiftedWithAltitude(AltitudeLayer.MetaOverlays);
			Graphics.DrawMesh(MeshPool.plane10, position, Quaternion.identity, InteractionSquareMaterial, 0);
		}
		ThingDefinition thingDefinition = entDefToPlace as ThingDefinition;
		if (thingDefinition == null || (!thingDefinition.transmitsPower && !thingDefinition.ConnectToPower))
		{
			return;
		}
		OverlayDrawHandler.DrawPowerGridOverlay();
		if (thingDefinition.ConnectToPower)
		{
			Building building = PowerConnectionMaker.BestTransmitterForConnector(intVec);
			if (building != null)
			{
				PowerNetGraphics.RenderAnticipatedWirePieceConnecting(intVec, building.Position);
			}
		}
	}
}
