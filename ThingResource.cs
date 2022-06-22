using UnityEngine;

public class ThingResource : ThingWithComponents, Edible
{
	public float CurNutrition => def.food.nutrition * (float)stackCount;

	public override string Label => def.label + " x" + stackCount;

	public bool EdibleNow => def.Edible;

	public override void SpawnSetup()
	{
		base.SpawnSetup();
		Building building = Find.Grids.ThingAt(base.Position, EntityType.Area_Stockpile) as Building;
		if (building != null)
		{
			Find.ResourceManager.Gain(def.eType, stackCount);
			Destroy();
		}
	}

	public override void DrawGUIOverlay()
	{
		if (Find.CameraMap.CurrentZoom == CameraZoomRange.Closest)
		{
			GenWorldUI.DrawThingLabelFor(this, stackCount.ToString(), new Color(1f, 1f, 1f, 0.75f));
		}
	}

	public void Notify_Eaten()
	{
		Destroy();
	}
}
