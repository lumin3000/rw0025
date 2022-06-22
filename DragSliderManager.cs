using UnityEngine;

public static class DragSliderManager
{
	private static bool Dragging;

	private static float RootX;

	private static DragSliderCallback draggingUpdateMethod;

	private static DragSliderCallback completedMethod;

	public static bool DragSlider(Rect SlRect, DragSliderCallback newDraggingUpdateMethod, DragSliderCallback newCompletedMethod)
	{
		if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && SlRect.Contains(Event.current.mousePosition))
		{
			StartDragSliding(newDraggingUpdateMethod, newCompletedMethod);
			return true;
		}
		return false;
	}

	private static void StartDragSliding(DragSliderCallback newDraggingUpdateMethod, DragSliderCallback newCompletedMethod)
	{
		Dragging = true;
		draggingUpdateMethod = newDraggingUpdateMethod;
		completedMethod = newCompletedMethod;
		RootX = Input.mousePosition.x;
	}

	private static float CurMouseOffset()
	{
		return Input.mousePosition.x - RootX;
	}

	public static void DragSlidersOnGUI()
	{
		if (Dragging && Event.current.type == EventType.MouseUp && Event.current.button == 0)
		{
			Dragging = false;
			if (completedMethod != null)
			{
				completedMethod(CurMouseOffset());
			}
		}
	}

	public static void DragSlidersUpdate()
	{
		if (Dragging && draggingUpdateMethod != null)
		{
			draggingUpdateMethod(CurMouseOffset());
		}
	}
}
