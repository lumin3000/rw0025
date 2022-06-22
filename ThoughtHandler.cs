using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ThoughtHandler : Saveable
{
	public Pawn pawn;

	protected List<Thought> ThoughtList = new List<Thought>();

	public IEnumerable<Thought> AllThoughts
	{
		get
		{
			foreach (Thought thought in ThoughtList)
			{
				yield return thought;
			}
		}
	}

	public IEnumerable<ThoughtType> ThoughtTypesPresent => AllThoughts.Select((Thought th) => th.thType).Distinct();

	public ThoughtHandler(Pawn pawn)
	{
		this.pawn = pawn;
	}

	public void ExposeData()
	{
		Scribe.LookList(ref ThoughtList, "ThoughtList");
	}

	public List<Thought> ThoughtGroupOfType(ThoughtType ThType)
	{
		return AllThoughts.Where((Thought tho) => tho.thType == ThType).ToList();
	}

	public float EffectOfThoughtGroup(ThoughtType ThType, ThoughtEffectType EfType)
	{
		float num = 0f;
		float num2 = 1f;
		float num3 = 0f;
		List<Thought> list = ThoughtGroupOfType(ThType);
		foreach (Thought item in list)
		{
			float num4 = item.BaseEffectOn(EfType) * num2;
			num += num4;
			num3 += item.effectMultiplier;
			num2 *= item.Def.stackedEffectMultiplier;
		}
		float num5 = num3 / (float)list.Count;
		return num * num5;
	}

	public void GainThought(ThoughtType ThType)
	{
		GainThought(new Thought(ThType));
	}

	public void GainThought(Thought newThought)
	{
		if (newThought == null)
		{
			Debug.LogWarning(string.Concat(pawn, " tried to gain null thought."));
			return;
		}
		Thought_Observation newThoughtObs = newThought as Thought_Observation;
		if (newThoughtObs != null)
		{
			Thought_Observation thought_Observation = (Thought_Observation)(from th in ThoughtList
				where th.thType == newThought.thType && th is Thought_Observation && (th as Thought_Observation).targetHash == newThoughtObs.targetHash
				orderby th.age descending
				select th).FirstOrDefault();
			if (thought_Observation != null)
			{
				thought_Observation.Renew();
			}
			else
			{
				ThoughtList.Add(newThought);
			}
			return;
		}
		int num = ThoughtList.Where((Thought th) => th.thType == newThought.thType).Count();
		if (num < newThought.thType.GetDefinition().stackLimit)
		{
			ThoughtList.Add(newThought);
			return;
		}
		(from th in ThoughtList
			where th.thType == newThought.thType
			orderby th.age descending
			select th).First()?.Renew();
	}

	public void ThoughtRecordTick()
	{
		foreach (ThoughtDefinition allThoughtDef in ThoughtDefinitions.AllThoughtDefs)
		{
			if (allThoughtDef.activeCondition != null && allThoughtDef.activeCondition(pawn))
			{
				GainThought(allThoughtDef.thoughtType);
			}
		}
		foreach (Thought thought in ThoughtList)
		{
			thought.ThoughtTick();
		}
		ThoughtList.RemoveAll((Thought th) => th.age > th.Def.duration);
	}

	public void RemoveThought(Thought t)
	{
		ThoughtList.Remove(t);
	}
}
