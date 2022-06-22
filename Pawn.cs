using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Pawn : ThingWithComponents
{
	public RaceDefinition raceDef;

	public PawnKindDefinition kindDef;

	public string characterName = "Unnamed";

	public Gender gender = Gender.Male;

	public int age = 10;

	public Pawn_PathFollower pather;

	public Pawn_DrawTracker drawer;

	public Pawn_Mind mind;

	public Pawn_JobTracker jobs;

	public Pawn_EquipmentTracker equipment;

	public Pawn_StanceTracker stances;

	public Pawn_HealthTracker healthTracker;

	public Pawn_CarryHands carryHands;

	public Pawn_NativeAbilities natives;

	public Pawn_InventoryTracker inventory;

	public Pawn_FilthTracker filth;

	public Pawn_TraitsTracker traits;

	public Pawn_TalkTracker talker;

	public Pawn_SkillsTracker skills;

	public Pawn_PrisonerTracker prisoner;

	public Pawn_Ownership ownership;

	public Pawn_StoryTracker story;

	public Pawn_PsychologyTracker psychology;

	public Pawn_FoodTracker food;

	public Pawn_RestTracker rest;

	private static float SightRadiusPawn = 14.5f;

	private static readonly Texture2D OrdersStartCommandIcon = Res.LoadTexture("UI/Commands/OrdersStart");

	private static readonly Texture2D OrdersReleaseCommandIcon = Res.LoadTexture("UI/Commands/OrdersRelease");

	public Pawn_MindHuman MindHuman => mind as Pawn_MindHuman;

	public MindState MindState => mind.mindState;

	public ThinkNode ThinkNodeRoot => mind.thinkNodeRoot;

	public Pawn_WorkSettings WorkSettings => MindHuman.workSettings;

	public bool Incapacitated => healthTracker.Incapacitated;

	public override string Label
	{
		get
		{
			if (!raceDef.hasIdentity)
			{
				return raceDef.raceName;
			}
			if (story.Adulthood == null)
			{
				return characterName;
			}
			return characterName + ", " + story.Adulthood.titleShort;
		}
	}

	public override string LabelShort => characterName;

	public string StoryArchetypeLabel => story.GetItemInSlot(CharHistorySlot.Adulthood).title;

	public override Vector3 DrawPos => drawer.DrawPos;

	public override Material DrawMat => drawer.renderer.CurMatBody;

	public override Mesh DrawMesh => drawer.renderer.CurMeshBody;

	public string SexLabel
	{
		get
		{
			if (gender == Gender.Male)
			{
				return "Male";
			}
			if (gender == Gender.Female)
			{
				return "Female";
			}
			if (gender == Gender.Sexless)
			{
				return string.Empty;
			}
			return "NOSEXLABEL";
		}
	}

	public string KindLabel => kindDef.kindLabel;

	public PathingParameters PathParams => kindDef.pathParams;

	public int TicksPerMoveCardinal => MoveTicksProcessed(raceDef.baseMoveTicks_Cardinal);

	public int TicksPerMoveDiagonal => MoveTicksProcessed(raceDef.baseMoveTicks_Diagonal);

	public Pawn()
	{
		pather = new Pawn_PathFollower(this);
		drawer = new Pawn_DrawTracker(this);
		healthTracker = new Pawn_HealthTracker(this);
		carryHands = new Pawn_CarryHands(this);
		stances = new Pawn_StanceTracker(this);
		equipment = new Pawn_EquipmentTracker(this);
		jobs = new Pawn_JobTracker(this);
		natives = new Pawn_NativeAbilities(this);
		inventory = new Pawn_InventoryTracker(this);
		filth = new Pawn_FilthTracker(this);
		traits = new Pawn_TraitsTracker(this);
	}

	public override void ExposeData()
	{
		base.ExposeData();
		string value = "error";
		if (kindDef != null)
		{
			value = kindDef.kindLabel;
		}
		Scribe.LookField(ref value, "Kind");
		if (Scribe.mode == LoadSaveMode.LoadingVars)
		{
			kindDef = PawnKindDefDatabase.KindDefNamed(value);
			raceDef = RaceDefDatabase.DefinitionNamed(kindDef.raceName);
		}
		Scribe.LookField(ref characterName, "CharacterName");
		Scribe.LookField(ref gender, "Sex", Gender.Male);
		Scribe.LookField(ref age, "Age");
		Thing target = carryHands.carriedThing;
		Scribe.LookSaveable(ref target, "CarriedThing");
		if (Scribe.mode == LoadSaveMode.LoadingVars && target != null)
		{
			carryHands.StartCarry(target);
		}
		Scribe.LookSaveable(ref drawer, "Drawer", this);
		Scribe.LookSaveable(ref healthTracker, "HealthTracker", this);
		Scribe.LookSaveable(ref mind, "Mind", this);
		Scribe.LookSaveable(ref jobs, "Jobs", this);
		Scribe.LookSaveable(ref pather, "Pather", this);
		Scribe.LookSaveable(ref equipment, "Equipment", this);
		Scribe.LookSaveable(ref inventory, "Inventory", this);
		Scribe.LookSaveable(ref filth, "Filth", this);
		Scribe.LookSaveable(ref food, "Food", this);
		Scribe.LookSaveable(ref rest, "Rest", this);
		Scribe.LookSaveable(ref psychology, "Psychology", this);
		Scribe.LookSaveable(ref prisoner, "Prisoner", this);
		Scribe.LookSaveable(ref ownership, "Ownership", this);
		Scribe.LookSaveable(ref talker, "Talker", this);
		Scribe.LookSaveable(ref skills, "Skills", this);
		Scribe.LookSaveable(ref story, "Story", this);
		Scribe.LookSaveable(ref traits, "Traits", this);
	}

	public override string ToString()
	{
		return characterName;
	}

	public override void SpawnSetup()
	{
		base.SpawnSetup();
		Notify_Teleported();
		Find.PawnManager.RegisterPawn(this);
		drawer.Notify_Spawned();
		this.GetKing()?.Notify_PawnSpawned(this);
		if (MindHuman != null)
		{
			MindHuman.workSettings.ApplyWorkDisables();
		}
	}

	public override void DrawAt(Vector3 drawLoc)
	{
		drawer.renderer.RenderPawnAt(drawLoc);
		stances.StanceTrackerDraw();
		pather.PatherDraw();
	}

	public override void DrawGUIOverlay()
	{
		drawer.ui.DrawPawnGUIOverlay();
	}

	public override void Tick()
	{
		if (!stances.FullBodyBusy)
		{
			pather.PatherTick();
		}
		drawer.DrawTrackerTick();
		healthTracker.HealthTick();
		stances.StanceTrackerTick();
		equipment.EquipmentTrackerTick();
		jobs.JobTrackerTick();
		mind.MindTick();
		carryHands.CarryHandsTick();
		if (talker != null)
		{
			talker.TalkTrackerTick();
		}
		if (psychology != null)
		{
			psychology.StatusTick();
		}
		if (food != null)
		{
			food.FoodTick();
		}
		if (rest != null)
		{
			rest.RestTick();
		}
		if (prisoner != null)
		{
			prisoner.PrisonerTrackerTick();
		}
		if (base.Team == TeamType.Colonist && !healthTracker.Incapacitated)
		{
			Find.FogGrid.ClearFogCircle(base.Position, SightRadiusPawn);
		}
	}

	public void ChangePawnTeamTo(TeamType newTeam)
	{
		if (newTeam == base.Team)
		{
			Debug.LogWarning(string.Concat("Used ChangePawnTeamTo to change ", this, " to same team, ", newTeam));
			return;
		}
		Find.PawnManager.DeRegisterPawn(this);
		Find.PawnDestinationManager.RemovePawnFromSystem(this);
		this.GetKing()?.Notify_PawnChangingTeam(this);
		if (newTeam == TeamType.Colonist)
		{
			kindDef = PawnKindDefDatabase.KindDefNamed("Colonist");
		}
		if (ownership != null)
		{
			ownership.Notify_TeamChangingTo(newTeam);
		}
		canDoUnsafeTeamChange = true;
		base.Team = newTeam;
		Find.PawnManager.RegisterPawn(this);
		PawnMakerUtility.AddOrRemovePawnTrackersFor(this);
		PawnMakerUtility.GiveAppropriateKeysTo(this);
		if (MindHuman != null && base.Team != TeamType.Colonist)
		{
			MindHuman.drafted = false;
		}
		if (base.Team == TeamType.Prisoner)
		{
			MindState.brokenState = MindBrokenState.Unbroken;
			equipment.DropAllEquipment();
		}
		Find.GameEnder.CheckGameOver();
	}

	public void Notify_Teleported()
	{
		drawer.tweener.Notify_Teleported_Int();
		pather.Notify_Teleported_Int();
	}

	private int MoveTicksProcessed(float originalTicks)
	{
		float num = 1f;
		if (traits.HasTraitEffect(TraitEffect.MoveFast))
		{
			num -= 0.3f;
		}
		if (carryHands.carriedThing != null && carryHands.carriedThing.def.eType == EntityType.Pawn)
		{
			float num2 = 0.75f;
			num += num2;
		}
		if (!Find.RoofGrid.Roofed(base.Position))
		{
			num += Find.WeatherManager.CurMoveTicksAddon;
		}
		num /= healthTracker.CurEffectivenessPercent;
		if (base.Team == TeamType.Prisoner)
		{
			num *= 3f;
		}
		return (int)Math.Round(originalTicks * num);
	}

	public override void Destroy()
	{
		base.Destroy();
		if (ownership != null)
		{
			ownership.Notify_PawnDestroyed();
		}
		this.GetKing()?.Notify_PawnDestroyed(this);
		UniversalDereg();
		Find.GameEnder.CheckGameOver();
	}

	public override void DestroyFinalize()
	{
		base.DestroyFinalize();
		Find.PawnManager.DeRegisterPawn(this);
	}

	public void IncappedOrKilled()
	{
		MindState.Notify_IncappedOrKilled();
		equipment.DropAllEquipment();
		carryHands.DropCarriedThing();
		jobs.Notify_IncapacitatedOrKilled();
		UniversalDereg();
	}

	private void UniversalDereg()
	{
		Find.PawnDestinationManager.RemovePawnFromSystem(this);
		Find.ReservationManager.UnReserveAllForPawn(this);
	}

	protected override void ApplyDamage(DamageInfo dinfo)
	{
		healthTracker.ApplyDamage(dinfo);
		drawer.Notify_DamageApplied(dinfo);
		stances.Notify_DamageTaken(dinfo);
		jobs.Notify_DamageTaken(dinfo);
	}

	public override TooltipDef GetTooltip()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(Label + " (" + SexLabel);
		if (Label != KindLabel)
		{
			stringBuilder.Append(" " + KindLabel);
		}
		stringBuilder.AppendLine(")");
		if (equipment.Primary != null)
		{
			stringBuilder.AppendLine(equipment.Primary.Label);
		}
		stringBuilder.AppendLine("Health:  " + healthTracker.Health + " / " + healthTracker.MaxHealth);
		if (psychology != null)
		{
			stringBuilder.AppendLine("Loyalty:  " + psychology.Loyalty.curLevel.ToString("##0") + "%");
		}
		if (carryHands.carriedThing != null)
		{
			stringBuilder.AppendLine("Carrying: " + carryHands.carriedThing.Label);
		}
		foreach (AttachableThing attach in attachList)
		{
			stringBuilder.AppendLine(attach.InfoStringAddon);
		}
		if (Find.Selector.SingleSelectedThing != null)
		{
			Pawn pawn = Find.Selector.SingleSelectedThing as Pawn;
			if (pawn != null && pawn.equipment.Primary != null && pawn.Team != base.Team)
			{
				Verb_LaunchProjectile verb_LaunchProjectile = pawn.equipment.Primary.verb as Verb_LaunchProjectile;
				if (verb_LaunchProjectile != null)
				{
					stringBuilder.AppendLine();
					stringBuilder.Append("Shot by " + pawn.Label + ":");
					if (verb_LaunchProjectile.CanHitTarget(new TargetPack(this)))
					{
						HitReport hitReport = verb_LaunchProjectile.HitReportFor(this);
						stringBuilder.Append(hitReport.GetTextReadout());
					}
					else
					{
						stringBuilder.Append("Cannot hit target.");
					}
				}
			}
		}
		return new TooltipDef(stringBuilder.ToString().TrimEnd('\n'), Label);
	}

	public override string GetInspectString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (DebugSettings.writeBeauty)
		{
			stringBuilder.AppendLine("Instant beauty: " + psychology.Environment.CurrentInstantBeauty().ToString("##0.000"));
			stringBuilder.AppendLine("Averaged beauty: " + psychology.Environment.RollingAveragedLocalBeauty().ToString("##0.000"));
		}
		if (DebugSettings.writeOpenness)
		{
			stringBuilder.AppendLine("Instant openness: " + psychology.Openness.CurrentInstantOpenness().ToString("##0.000"));
		}
		stringBuilder.AppendLine(SexLabel + " " + KindLabel.ToLower());
		string value = (healthTracker.Incapacitated ? "Incapacitated" : ((jobs.CurJob == null) ? "driverReport error." : jobs.CurJobDriver.GetReport().text));
		stringBuilder.AppendLine(value);
		if (raceDef.UsesEquipment)
		{
			if (equipment.Primary != null)
			{
				stringBuilder.AppendLine(equipment.Primary.Label);
			}
			else
			{
				stringBuilder.AppendLine("No equipment");
			}
		}
		if (carryHands.carriedThing != null)
		{
			stringBuilder.Append("Carrying: ");
			stringBuilder.AppendLine(carryHands.carriedThing.Label);
		}
		foreach (AttachableThing attach in attachList)
		{
			stringBuilder.AppendLine(attach.InfoStringAddon);
		}
		if (base.Team == TeamType.Prisoner)
		{
			stringBuilder.AppendLine("In restraints (slowed)");
		}
		return stringBuilder.ToString();
	}

	public override IEnumerable<Command> GetCommandOptions()
	{
		if (base.Team != TeamType.Colonist)
		{
			yield break;
		}
		if (base.Team == TeamType.Colonist)
		{
			Command opt = new Command_Action
			{
				hotKey = KeyCode.R,
				action = delegate
				{
					this.UIToggleDrafted();
				}
			};
			if (!MindHuman.drafted)
			{
				opt.icon = OrdersStartCommandIcon;
				opt.tipDef = new TooltipDef("D[R]aft this person for military orders.\n\nDrafted people take orders and fight. They will not work, eat, or rest.");
			}
			else
			{
				opt.icon = OrdersReleaseCommandIcon;
				opt.tipDef = new TooltipDef("[R]elease from draft.\n\nDrafted people take orders and fight. They will not work, eat, or rest.");
			}
			if (healthTracker.Incapacitated)
			{
				opt.Disable(characterName + " is incapacitated.");
			}
			yield return opt;
		}
		foreach (Command equipmentCommandOption in equipment.GetEquipmentCommandOptions())
		{
			yield return equipmentCommandOption;
		}
	}
}
