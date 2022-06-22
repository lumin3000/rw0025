using System;
using System.Collections.Generic;

public static class JobDefs
{
	private static List<JobDefinition> JobDefinitionList;

	static JobDefs()
	{
		BuildJobTypeList();
	}

	public static JobDefinition DefinitionOf(JobType t)
	{
		return JobDefinitionList[(int)t];
	}

	private static void BuildJobTypeList()
	{
		JobDefinitionList = new List<JobDefinition>();
		foreach (int value in Enum.GetValues(typeof(JobType)))
		{
			JobDefinition item = NewDefinitionForType((JobType)value);
			JobDefinitionList.Add(item);
		}
	}

	private static JobDefinition NewDefinitionForType(JobType newType)
	{
		JobDefinition jobDefinition = new JobDefinition();
		jobDefinition.jType = newType;
		if (newType == JobType.Goto)
		{
			jobDefinition.driverClass = typeof(JobDriver_Goto);
		}
		if (newType == JobType.Wait)
		{
			jobDefinition.driverClass = typeof(JobDriver_Wait);
		}
		if (newType == JobType.AttackStatic)
		{
			jobDefinition.driverClass = typeof(JobDriver_AttackStatic);
			jobDefinition.easyInterrupt = false;
		}
		if (newType == JobType.AttackMelee)
		{
			jobDefinition.driverClass = typeof(JobDriver_AttackMelee);
			jobDefinition.easyInterrupt = false;
		}
		if (newType == JobType.UseVerbOnThing)
		{
			jobDefinition.driverClass = typeof(JobDriver_UseVerb);
			jobDefinition.easyInterrupt = false;
		}
		if (newType == JobType.Equip)
		{
			jobDefinition.driverClass = typeof(JobDriver_Equip);
		}
		if (newType == JobType.Ignite)
		{
			jobDefinition.driverClass = typeof(JobDriver_Ignite);
		}
		if (newType == JobType.BeatFire)
		{
			jobDefinition.driverClass = typeof(JobDriver_BeatFire);
		}
		if (newType == JobType.ExtinguishSelf)
		{
			jobDefinition.easyInterrupt = false;
			jobDefinition.driverClass = typeof(JobDriver_ExtinguishSelf);
		}
		if (newType == JobType.EatFood)
		{
			jobDefinition.driverClass = typeof(JobDriver_FoodEat);
		}
		if (newType == JobType.DeliverFood)
		{
			jobDefinition.driverClass = typeof(JobDriver_FoodDeliver);
		}
		if (newType == JobType.FeedPatient)
		{
			jobDefinition.driverClass = typeof(JobDriver_FoodFeedPatient);
		}
		if (newType == JobType.HaulToCargo)
		{
			jobDefinition.driverClass = typeof(JobDriver_Haul);
		}
		if (newType == JobType.HaulToSlot)
		{
			jobDefinition.driverClass = typeof(JobDriver_Haul);
		}
		if (newType == JobType.Rescue)
		{
			jobDefinition.driverClass = typeof(JobDriver_TakeToBed);
		}
		if (newType == JobType.Capture)
		{
			jobDefinition.driverClass = typeof(JobDriver_TakeToBed);
		}
		if (newType == JobType.Arrest)
		{
			jobDefinition.driverClass = typeof(JobDriver_TakeToBed);
		}
		if (newType == JobType.EscortPrisonerToBed)
		{
			jobDefinition.driverClass = typeof(JobDriver_TakeToBed);
		}
		if (newType == JobType.TakeWoundedPrisonerToBed)
		{
			jobDefinition.driverClass = typeof(JobDriver_TakeToBed);
		}
		if (newType == JobType.Interact)
		{
			jobDefinition.driverClass = typeof(JobDriver_Interact);
		}
		if (newType == JobType.Sleep)
		{
			jobDefinition.driverClass = typeof(JobDriver_Interact);
			jobDefinition.interactLocation = InteractionLocationType.OnPosition;
			jobDefinition.reservationType = ReservationType.UseDevice;
			jobDefinition.interactTakeBreaks = false;
			jobDefinition.jobReportSpecial = new JobReport("Sleeping.", JobReportOverlays.sleeper);
		}
		if (newType == JobType.Research)
		{
			jobDefinition.driverClass = typeof(JobDriver_Interact);
			jobDefinition.interactLocation = InteractionLocationType.InteractionSquare;
			jobDefinition.reservationType = ReservationType.Research;
			jobDefinition.jobReportSpecial = new JobReport("Researching.", JobReportOverlays.research);
			jobDefinition.interactEffectMakerType = typeof(EffectMaker_Research);
		}
		if (newType == JobType.UseCommsConsole)
		{
			jobDefinition.driverClass = typeof(JobDriver_Interact);
			jobDefinition.interactLocation = InteractionLocationType.InteractionSquare;
			jobDefinition.reservationType = ReservationType.UseDevice;
			jobDefinition.jobReportSpecial = new JobReport("Using comms console.", null);
			jobDefinition.interactFailCondition = (Thing t) => !((Building_CommsConsole)t).CanUseCommsNow;
		}
		if (newType == JobType.Mine)
		{
			jobDefinition.driverClass = typeof(JobDriver_InteractMine);
			jobDefinition.reservationType = ReservationType.Total;
		}
		if (newType == JobType.Clean)
		{
			jobDefinition.driverClass = typeof(JobDriver_InteractClean);
			jobDefinition.reservationType = ReservationType.Total;
			jobDefinition.interactMoteName = "Clean";
		}
		if (newType == JobType.Repair)
		{
			jobDefinition.driverClass = typeof(JobDriver_InteractRepair);
			jobDefinition.reservationType = ReservationType.Construction;
			jobDefinition.interactEffectMakerType = typeof(EffectMaker_Repair);
		}
		if (newType == JobType.Construct)
		{
			jobDefinition.driverClass = typeof(JobDriver_InteractConstruct);
			jobDefinition.reservationType = ReservationType.Construction;
		}
		if (newType == JobType.Sow)
		{
			jobDefinition.driverClass = typeof(JobDriver_InteractSow);
			jobDefinition.reservationType = ReservationType.Sowing;
			jobDefinition.interactMoteName = "Sow";
		}
		if (newType == JobType.Harvest)
		{
			jobDefinition.driverClass = typeof(JobDriver_InteractHarvest);
			jobDefinition.reservationType = ReservationType.Total;
			jobDefinition.interactMoteName = "Harvest";
		}
		if (newType == JobType.CutPlant)
		{
			jobDefinition.driverClass = typeof(JobDriver_InteractCutPlant);
			jobDefinition.reservationType = ReservationType.Total;
			jobDefinition.interactMoteName = "Harvest";
		}
		if (newType == JobType.PrisonerFriendlyChat)
		{
			jobDefinition.driverClass = typeof(JobDriver_TalkToPrisoner);
			jobDefinition.speechToGive = new SpeechConfig();
			jobDefinition.speechToGive.thoughtToGive = ThoughtType.PrisonerFriendlyChat;
		}
		if (newType == JobType.PrisonerBeatingMild)
		{
			jobDefinition.driverClass = typeof(JobDriver_BeatPrisoner);
			jobDefinition.meleeMode = MeleeAttackMode.MildBeating;
		}
		if (newType == JobType.PrisonerBeatingVicious)
		{
			jobDefinition.driverClass = typeof(JobDriver_BeatPrisoner);
			jobDefinition.meleeMode = MeleeAttackMode.ViciousBeating;
		}
		if (newType == JobType.PrisonerExecution)
		{
			jobDefinition.driverClass = typeof(JobDriver_PrisonerExecute);
		}
		return jobDefinition;
	}
}
