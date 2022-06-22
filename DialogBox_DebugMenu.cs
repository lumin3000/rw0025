using System;
using System.Linq;
using System.Text;
using UnityEngine;

public class DialogBox_DebugMenu : DialogBox_Debug
{
	public override void DoDialogBoxGUI()
	{
		GenUI.SetFontSmall();
		GUI.skin.button.alignment = TextAnchor.MiddleLeft;
		curOffset = new Vector2(0f, 0f);
		UIWidgets.DrawWindow(winRect);
		GUI.BeginGroup(winRect.GetInnerRect(10f));
		AddOption("+10000 Money", delegate
		{
			Find.ResourceManager.Gain(EntityType.Money, 10000);
		});
		AddOption("+200 Food", delegate
		{
			Find.ResourceManager.Gain(EntityType.Food, 200);
		});
		AddOption("+500 Metal", delegate
		{
			Find.ResourceManager.Gain(EntityType.Metal, 500);
		});
		AddOption("No Resources", delegate
		{
			foreach (ThingDefinition allThingDefinition in ThingDefDatabase.AllThingDefinitions)
			{
				Find.ResourceManager.SetAmount(allThingDefinition.eType, 0);
			}
		});
		AddSectionSpace();
		AddOption("Execute incident", delegate
		{
			Find.Dialogs.AddDialogBox(new DialogBox_DebugIncidentChooser());
		});
		AddOption("Change weather", delegate
		{
			Find.Dialogs.AddDialogBox(new DialogBox_DebugWeatherChooser());
		});
		AddOption("Change camera config", delegate
		{
			Find.Dialogs.AddDialogBox(new DialogBox_DebugCameraConfigChooser());
		});
		AddOption("Name Colony", delegate
		{
			Find.Dialogs.AddDialogBox(new DialogBox_NameColony());
		});
		AddOption("Next Tutor Item", delegate
		{
			BasicTrainingSignaller.ShowNextBasicTrainingItem();
		});
		AddOption("Regen all map mesh sections", delegate
		{
			Find.Map.mapDrawer.RegenerateEverythingNow();
		});
		AddOption("Write test story events", delegate
		{
			Find.Storyteller.incidentMaker.DebugLogTestFutureIncidents();
		});
		AddSectionSpace();
		AddOption("Prisoner", delegate
		{
			foreach (Building_Bed item in Find.BuildingManager.AllBuildingsColonistOfClass<Building_Bed>())
			{
				if (item.forPrisoners && item.owner == null)
				{
					Pawn pawn = PawnMaker.GeneratePawn("Colonist", TeamType.Prisoner);
					ThingMaker.Spawn(pawn, item.Position);
					pawn.ownership.ClaimBed(item);
					break;
				}
			}
		});
		Action<int> ForceRaidWith = delegate(int pts)
		{
			Find.Dialogs.PopBox();
			AIKing aIKing = new AIKing(new AIKing_Config
			{
				team = TeamType.Raider
			}, PawnPoolMaker.GenerateRaidPawns(new PawnPoolRequest
			{
				points = pts,
				team = TeamType.Raider
			}));
			aIKing.DropInitialPawns();
		};
		AddOption("Raid (35 pts)", delegate
		{
			ForceRaidWith(35);
		});
		AddOption("Raid (75 pts)", delegate
		{
			ForceRaidWith(75);
		});
		AddOption("Raid (300 pts)", delegate
		{
			ForceRaidWith(300);
		});
		AddOption("Raid (1000 pts)", delegate
		{
			ForceRaidWith(1000);
		});
		AddOption("Force enemy assault", delegate
		{
			foreach (AIKing allKing in Find.AIKingManager.allKings)
			{
				allKing.cortex.StartAssault();
			}
		});
		AddOption("Kill all enemies ", delegate
		{
			foreach (Pawn item2 in Find.PawnManager.Hostiles.ListFullCopy())
			{
				DamageInfo dam = new DamageInfo(DamageType.Bludgeon, 100);
				item2.Killed(dam);
			}
		});
		AddOption("Force enemy flee", delegate
		{
			foreach (AIKing allKing2 in Find.AIKingManager.allKings)
			{
				allKing2.fleeChecker.Flee();
			}
		});
		AddSectionSpace();
		AddOption("List all assets", delegate
		{
			object[] array = Resources.FindObjectsOfTypeAll(typeof(Mesh));
			object[] array2 = Resources.FindObjectsOfTypeAll(typeof(Material));
			object[] array3 = Resources.FindObjectsOfTypeAll(typeof(Texture));
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Meshes: " + array.Length);
			stringBuilder.AppendLine("Materials: " + array2.Length);
			stringBuilder.AppendLine("Textures: " + array3.Length);
			Debug.Log(stringBuilder.ToString());
		});
		AddOption("Unload unused assets", delegate
		{
			LongEventHandler.QueueLongEvent(delegate
			{
				Resources.UnloadUnusedAssets();
			}, "Unloading unused assets");
		});
		StartNextColumn();
		AddTool("Tool: Light damage", delegate
		{
			foreach (Thing item3 in Find.Grids.ThingsAt(Gen.MouseWorldSquare()).ToList())
			{
				item3.TakeDamage(new DamageInfo(DamageType.Bomb, 10));
			}
		});
		AddTool("Tool: Huge damage", delegate
		{
			foreach (Thing item4 in Find.Grids.ThingsAt(Gen.MouseWorldSquare()).ToList())
			{
				item4.TakeDamage(new DamageInfo(DamageType.Bomb, 5000));
			}
		});
		AddTool("Tool: Explosion", delegate
		{
			Explosion.DoExplosion(Gen.MouseWorldSquare(), 3.9f, DamageType.Bomb);
		});
		AddTool("Tool: Lightning strike", delegate
		{
			Find.WeatherManager.eventHandler.AddEvent(new WeatherEvent_LightningStrike(Gen.MouseWorldSquare()));
		});
		AddTool("Tool: Regen section", delegate
		{
			Find.MapDrawer.SectionAt(Gen.MouseWorldSquare()).RegenerateAllLayers();
		});
		AddSectionSpace();
		AddTool("Tool: Make hungry", delegate
		{
			foreach (Pawn item5 in (from t in Find.Grids.ThingsAt(Gen.MouseWorldSquare())
				where t is Pawn
				select t).Cast<Pawn>().ToList())
			{
				if (item5.food != null)
				{
					item5.food.Food.curLevel = 5f;
					AffectedThingEffect(item5);
				}
			}
		});
		AddTool("Tool: Make tired", delegate
		{
			foreach (Pawn item6 in (from t in Find.Grids.ThingsAt(Gen.MouseWorldSquare())
				where t is Pawn
				select t).Cast<Pawn>().ToList())
			{
				if (item6.rest != null)
				{
					item6.rest.Rest.curLevel = 5f;
					AffectedThingEffect(item6);
				}
			}
		});
		AddTool("Tool: Force mental break", delegate
		{
			foreach (Pawn item7 in (from t in Find.Grids.ThingsAt(Gen.MouseWorldSquare())
				where t is Pawn
				select t).Cast<Pawn>().ToList())
			{
				PsychologyUtility.DoMentalBreak(item7);
				AffectedThingEffect(item7);
			}
		});
		AddSectionSpace();
		AddOption("Tool: Spawn pawn", delegate
		{
			Find.Dialogs.AddDialogBox(new DialogBox_DebugLister_SpawnPawn());
		});
		AddOption("Tool: Spawn thing", delegate
		{
			Find.Dialogs.AddDialogBox(new DialogBox_DebugLister_SpawnThing());
		});
		AddOption("Tool: Spawn gun", delegate
		{
			Find.Dialogs.AddDialogBox(new DialogBox_DebugLister_SpawnGun());
		});
		AddSectionSpace();
		AddOption("Draw terrain", ref DebugSettings.drawTerrain);
		AddOption("Draw things (dynamic)", ref DebugSettings.drawThingsDynamic);
		AddOption("Draw things (printed)", ref DebugSettings.drawThingsPrinted);
		AddOption("Draw shadows", ref DebugSettings.drawShadows);
		AddOption("Draw glow overlay", ref DebugSettings.drawLightingOverlay);
		AddOption("Draw fog", ref DebugSettings.drawFog);
		AddOption("Draw world overlays", ref DebugSettings.drawWorldOverlays);
		StartNextColumn();
		AddOption("Draw section edges", ref DebugSettings.drawSectionEdges);
		AddOption("Draw pawn reservations", ref DebugSettings.drawPawnDebug);
		AddOption("Draw pawn rotator targets", ref DebugSettings.drawPawnRotatorTarget);
		AddOption("Draw paths", ref DebugSettings.drawPaths);
		AddOption("Draw cast position search", ref DebugSettings.drawCastPositionSearch);
		AddOption("Draw dest searches", ref DebugSettings.drawDestSearch);
		AddOption("Draw reachability checks", ref DebugSettings.drawReachabilityChecks);
		AddOption("Draw reachability", ref DebugSettings.drawReachability);
		AddOption("Draw rooms", ref DebugSettings.drawRooms);
		AddOption("Draw roofs", ref DebugSettings.drawRoofs);
		AddOption("Draw/report power", ref DebugSettings.drawReportPower);
		AddOption("Report glow", ref DebugSettings.reportGlow);
		AddOption("Report path costs", ref DebugSettings.reportPathCosts);
		AddOption("Report fertility", ref DebugSettings.reportFertility);
		AddOption("Report link flags", ref DebugSettings.reportLinkFlags);
		AddOption("Write beauty perception", ref DebugSettings.writeBeauty);
		AddOption("Write openness perception", ref DebugSettings.writeOpenness);
		AddOption("Write storyteller", ref DebugSettings.writeStoryteller);
		AddOption("Log incap chance", ref DebugSettings.logIncapChance);
		StartNextColumn();
		AddOption("All damage", ref DebugSettings.damageEnabled);
		AddOption("Colonist damage", ref DebugSettings.playerDamageEnabled);
		AddOption("Breathable outdoors", ref DebugSettings.worldBreathable);
		AddOption("Fast research", ref DebugSettings.fastResearch);
		AddOption("Fast learning", ref DebugSettings.fastLearning);
		AddOption("Fast ecology", ref DebugSettings.fastEcology);
		AddOption("Tooltip region edges", ref DebugSettings.showTooltipEdges);
		AddOption("Unlimited power", ref DebugSettings.unlimitedPower);
		GUI.EndGroup();
		DetectShouldClose(doButton: true);
		GenUI.AbsorbAllInput();
		GUI.skin.button.alignment = TextAnchor.UpperLeft;
	}
}
