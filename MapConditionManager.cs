using System.Collections.Generic;
using UnityEngine;

public class MapConditionManager : Saveable
{
	private const float ConditionUIHeight = 26f;

	private List<MapCondition> activeConditions = new List<MapCondition>();

	public float WeatherLerpFactor => (ActiveCondition(MapConditionType.Eclipse) as MapCondition_Eclipse)?.SkyLerpStrength ?? 0f;

	public SkyTarget WeatherLerpTarget => new SkyTarget(0f, 0f, SkyColors.NightEdgeClear);

	public float UIHeight => (float)activeConditions.Count * 26f;

	public void RegisterCondition(MapCondition cond)
	{
		activeConditions.Add(cond);
	}

	public void ExposeData()
	{
		Scribe.LookList(ref activeConditions, "ActiveConditions");
	}

	public void MapConditionManagerTick()
	{
		foreach (MapCondition activeCondition in activeConditions)
		{
			activeCondition.MapConditionTick();
		}
		activeConditions.RemoveAll((MapCondition cond) => cond.Expired);
	}

	public bool ConditionIsActive(MapConditionType condType)
	{
		return ActiveCondition(condType) != null;
	}

	public MapCondition ActiveCondition(MapConditionType condType)
	{
		foreach (MapCondition activeCondition in activeConditions)
		{
			if (condType == activeCondition.conditionType)
			{
				return activeCondition;
			}
		}
		return null;
	}

	public void DoConditionsUI(Rect rect)
	{
		GUI.skin.label.alignment = TextAnchor.MiddleRight;
		GenUI.SetFontSmall();
		GUI.BeginGroup(rect);
		Rect position = new Rect(0f, 0f, rect.width, 26f);
		position.width -= 15f;
		foreach (MapCondition activeCondition in activeConditions)
		{
			GUI.Label(position, activeCondition.Label);
			position.y += 26f;
		}
		GUI.EndGroup();
		TooltipHandler.TipRegion(rect, "Special conditions affecting the area right now.");
		GUI.skin.label.alignment = TextAnchor.UpperLeft;
	}
}
