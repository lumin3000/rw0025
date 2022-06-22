using System.Linq;
using UnityEngine;

public class Tradeable_Pawn : Tradeable
{
	private const float BasePawnPrice = 250f;

	private const float XpPerDollar = 80f;

	private const float HealthPriceImportance = 0.2f;

	public Pawn tradePawn;

	public override string Label => tradePawn.characterName;

	public override int BasePrice
	{
		get
		{
			float num = 250f;
			foreach (Skill allSkill in tradePawn.skills.AllSkills)
			{
				num += allSkill.XpTotalEarned / 80f;
			}
			num *= 0.8f + 0.2f * (float)(tradePawn.healthTracker.Health / tradePawn.healthTracker.MaxHealth);
			return (int)num;
		}
	}

	public override string InfoStringShort => tradePawn.story.GetItemInSlot(CharHistorySlot.Adulthood).title;

	public override DialogBox NewInfoDialog => new DialogBox_PawnCard(tradePawn);

	private Skill TopSkill
	{
		get
		{
			Skill skill = tradePawn.skills.AllSkills.OrderByDescending((Skill sk) => sk.level).First();
			if (skill.level >= 8)
			{
				return skill;
			}
			return null;
		}
	}

	public override AudioClip TakeSound => Tradeable.DefaultTakeSound;

	public Tradeable_Pawn()
	{
	}

	public Tradeable_Pawn(Pawn newTradePawn)
		: this()
	{
		tradePawn = newTradePawn;
	}

	public override void ExposeData()
	{
		Scribe.LookSaveable(ref tradePawn, "TradePawn");
	}

	public override void GiveToPlayer()
	{
		IntVec3 pos = Find.BuildingManager.TradeDropLocation();
		DropPodContentsInfo contents = new DropPodContentsInfo(tradePawn);
		tradePawn.ChangePawnTeamTo(TeamType.Colonist);
		DropPodUtility.MakeDropPodAt(pos, contents);
	}

	public override void TakeFromPlayer()
	{
		tradePawn.Destroy();
		foreach (Pawn colonistsAndPrisoner in Find.PawnManager.ColonistsAndPrisoners)
		{
			colonistsAndPrisoner.psychology.thoughts.GainThought(ThoughtType.KnowPrisonerSold);
		}
	}
}
