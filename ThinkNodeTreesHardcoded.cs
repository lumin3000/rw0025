public static class ThinkNodeTreesHardcoded
{
	private static ThinkNode BurningResponse
	{
		get
		{
			ThinkNode_ConditionalBurning thinkNode_ConditionalBurning = new ThinkNode_ConditionalBurning();
			ThinkNode_Priority thinkNode_Priority = new ThinkNode_Priority();
			thinkNode_ConditionalBurning.subNodes.Add(thinkNode_Priority);
			thinkNode_Priority.subNodes.Add(new JobGiver_ExtinguishSelf());
			thinkNode_Priority.subNodes.Add(new JobGiver_RunRandom());
			return thinkNode_ConditionalBurning;
		}
	}

	private static ThinkNode Psychotic
	{
		get
		{
			ThinkNode_ConditionalBroken thinkNode_ConditionalBroken = new ThinkNode_ConditionalBroken(MindBrokenState.Psychotic);
			ThinkNode_Priority thinkNode_Priority = new ThinkNode_Priority();
			thinkNode_ConditionalBroken.subNodes.Add(thinkNode_Priority);
			thinkNode_Priority.subNodes.Add(new JobGiver_Psychotic());
			thinkNode_Priority.subNodes.Add(new JobGiver_WanderAnywhere());
			return thinkNode_ConditionalBroken;
		}
	}

	private static ThinkNode SelfDefense => new JobGiver_SelfDefense();

	public static ThinkNode NewNodes_Herbivore(Pawn pawn)
	{
		ThinkNode thinkNode = new ThinkNode_Priority();
		thinkNode.subNodes.Add(BurningResponse);
		thinkNode.subNodes.Add(Psychotic);
		thinkNode.subNodes.Add(SelfDefense);
		thinkNode.subNodes.Add(new ThinkNode_SatisfyNeeds());
		thinkNode.subNodes.Add(new JobGiver_WanderCurrentRoom());
		thinkNode.subNodes.Add(new JobGiver_IdleError());
		thinkNode.SetPawn(pawn);
		return thinkNode;
	}

	public static ThinkNode NewNodes_HerbivoreHerd(Pawn pawn)
	{
		ThinkNode thinkNode = new ThinkNode_Priority();
		thinkNode.subNodes.Add(BurningResponse);
		thinkNode.subNodes.Add(Psychotic);
		thinkNode.subNodes.Add(SelfDefense);
		thinkNode.subNodes.Add(new ThinkNode_SatisfyNeeds());
		thinkNode.subNodes.Add(new JobGiver_WanderHerd());
		thinkNode.subNodes.Add(new JobGiver_IdleError());
		thinkNode.SetPawn(pawn);
		return thinkNode;
	}

	public static ThinkNode NewNodes_Human(Pawn pawn, Pawn_MindHuman mindHuman)
	{
		ThinkNode thinkNode = new ThinkNode_Priority();
		thinkNode.subNodes.Add(BurningResponse);
		thinkNode.subNodes.Add(Psychotic);
		ThinkNode_ConditionalFunc thinkNode_ConditionalFunc = new ThinkNode_ConditionalFunc();
		thinkNode_ConditionalFunc.condition = () => !pawn.MindHuman.drafted;
		thinkNode.subNodes.Add(thinkNode_ConditionalFunc);
		thinkNode_ConditionalFunc.subNodes.Add(SelfDefense);
		ThinkNode_ConditionalBroken thinkNode_ConditionalBroken = new ThinkNode_ConditionalBroken(MindBrokenState.GiveUpExit);
		thinkNode.subNodes.Add(thinkNode_ConditionalBroken);
		thinkNode_ConditionalBroken.subNodes.Add(new JobGiver_ExitMapWalkRandom());
		ThinkNode_ConditionalBroken thinkNode_ConditionalBroken2 = new ThinkNode_ConditionalBroken(MindBrokenState.PanicFlee);
		thinkNode.subNodes.Add(thinkNode_ConditionalBroken2);
		thinkNode_ConditionalBroken2.subNodes.Add(new JobGiver_PanicFlee());
		ThinkNode_ConditionalBroken thinkNode_ConditionalBroken3 = new ThinkNode_ConditionalBroken(MindBrokenState.DazedWander);
		thinkNode.subNodes.Add(thinkNode_ConditionalBroken3);
		thinkNode_ConditionalBroken3.subNodes.Add(new JobGiver_WanderAnywhere());
		ThinkNode_ConditionalTeam thinkNode_ConditionalTeam = new ThinkNode_ConditionalTeam(TeamType.Prisoner);
		thinkNode.subNodes.Add(thinkNode_ConditionalTeam);
		ThinkNode_Priority thinkNode_Priority = new ThinkNode_Priority();
		thinkNode_ConditionalTeam.subNodes.Add(thinkNode_Priority);
		thinkNode_Priority.subNodes.Add(new JobGiver_PrisonerEscape());
		thinkNode_Priority.subNodes.Add(new ThinkNode_SatisfyNeeds());
		thinkNode_Priority.subNodes.Add(new JobGiver_WanderCurrentRoom());
		thinkNode_Priority.subNodes.Add(new JobGiver_IdleError());
		ThinkNode_ConditionalTeam thinkNode_ConditionalTeam2 = new ThinkNode_ConditionalTeam(TeamType.Raider);
		thinkNode.subNodes.Add(thinkNode_ConditionalTeam2);
		ThinkNode_Priority thinkNode_Priority2 = new ThinkNode_Priority();
		thinkNode_ConditionalTeam2.subNodes.Add(thinkNode_Priority2);
		thinkNode_Priority2.subNodes.Add(new Decorator_UpdateEnemyTarget());
		ThinkNode_ConditionalNoTarget thinkNode_ConditionalNoTarget = new ThinkNode_ConditionalNoTarget();
		thinkNode_Priority2.subNodes.Add(thinkNode_ConditionalNoTarget);
		JobGiver_AITrashCloseBuildings item = new JobGiver_AITrashCloseBuildings();
		thinkNode_ConditionalNoTarget.subNodes.Add(item);
		ThinkNode_AIDuty thinkNode_AIDuty = new ThinkNode_AIDuty();
		thinkNode_AIDuty.nodeAssault = new ThinkNode_Priority();
		thinkNode_AIDuty.nodeAssault.subNodes.Add(new JobGiver_AIAttackTarget());
		thinkNode_AIDuty.nodeAssault.subNodes.Add(new JobGiver_AITrashBuildings());
		thinkNode_AIDuty.nodeDefend = new JobGiver_AIDefendPoint();
		thinkNode_AIDuty.nodeStage = new JobGiver_WanderNearPoint();
		thinkNode_AIDuty.nodeExit = new JobGiver_ExitMapWalkRandom();
		thinkNode_AIDuty.subNodes.Add(thinkNode_AIDuty.nodeAssault);
		thinkNode_AIDuty.subNodes.Add(thinkNode_AIDuty.nodeDefend);
		thinkNode_AIDuty.subNodes.Add(thinkNode_AIDuty.nodeStage);
		thinkNode_AIDuty.subNodes.Add(thinkNode_AIDuty.nodeExit);
		thinkNode_Priority2.subNodes.Add(thinkNode_AIDuty);
		thinkNode_Priority2.subNodes.Add(new JobGiver_WanderAnywhere());
		thinkNode_Priority2.subNodes.Add(new JobGiver_IdleError());
		ThinkNode_ConditionalTeam thinkNode_ConditionalTeam3 = new ThinkNode_ConditionalTeam(TeamType.Traveler);
		thinkNode.subNodes.Add(thinkNode_ConditionalTeam3);
		thinkNode_ConditionalTeam3.subNodes.Add(new JobGiver_ExitMapTravelDestination());
		ThinkNode_ConditionalTeam thinkNode_ConditionalTeam4 = new ThinkNode_ConditionalTeam(TeamType.Colonist);
		thinkNode.subNodes.Add(thinkNode_ConditionalTeam4);
		thinkNode_ConditionalTeam4.subNodes.Add(new JobGiver_Orders());
		JobGiver_JobQueue item2 = (mindHuman.workJobQueue = new JobGiver_JobQueue());
		thinkNode_ConditionalTeam4.subNodes.Add(item2);
		JobGiver_WorkRoot jobGiver_WorkRoot = new JobGiver_WorkRoot();
		jobGiver_WorkRoot.emergencyWork = true;
		thinkNode_ConditionalTeam4.subNodes.Add(jobGiver_WorkRoot);
		thinkNode.subNodes.Add(new ThinkNode_SatisfyNeeds());
		ThinkNode_ConditionalTeam thinkNode_ConditionalTeam5 = new ThinkNode_ConditionalTeam(TeamType.Colonist);
		thinkNode.subNodes.Add(thinkNode_ConditionalTeam5);
		JobGiver_WorkRoot jobGiver_WorkRoot2 = new JobGiver_WorkRoot();
		jobGiver_WorkRoot2.emergencyWork = false;
		thinkNode_ConditionalTeam5.subNodes.Add(jobGiver_WorkRoot2);
		ThinkNode_Tagger thinkNode_Tagger = new ThinkNode_Tagger(JobTag.Idle);
		thinkNode.subNodes.Add(thinkNode_Tagger);
		thinkNode_Tagger.subNodes.Add(new JobGiver_WanderColony());
		thinkNode.subNodes.Add(new JobGiver_IdleError());
		thinkNode.SetPawn(pawn);
		return thinkNode;
	}
}
