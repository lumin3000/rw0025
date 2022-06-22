using System.Collections.Generic;

public class ThingDrawManager
{
	private List<Thing> drawableThingList;

	public ThingDrawManager()
	{
		drawableThingList = new List<Thing>();
	}

	public void RegisterDrawable(Thing t)
	{
		drawableThingList.Add(t);
	}

	public void DeRegisterDrawable(Thing t)
	{
		drawableThingList.Remove(t);
	}

	public void DrawDynamicThings()
	{
		if (!DebugSettings.drawThingsDynamic)
		{
			return;
		}
		FogGrid fogGrid = Find.FogGrid;
		IntRect currentViewRect = Find.CameraMap.CurrentViewRect;
		foreach (Thing drawableThing in drawableThingList)
		{
			if (currentViewRect.Contains(drawableThing.Position) && (drawableThing.def.seeThroughFog || !fogGrid.IsFogged(drawableThing.Position)))
			{
				drawableThing.Draw();
			}
		}
		if (Find.GameRoot != null)
		{
			MapEdgeClipDrawer.DrawClippers();
		}
	}
}
