using UnityEngine;

public class UI_GlobalControls
{
	public const float Width = 134f;

	private UI_DialogButtons dialogButtons = new UI_DialogButtons();

	public void GlobalControlsOnGUI()
	{
		float num = (float)Screen.width - 134f;
		float num2 = Screen.height;
		Rect contentRect = new Rect(num, num2 - 170f, 134f, 170f);
		dialogButtons.DialogButtonsOnGUI(contentRect);
		num2 -= contentRect.height;
		num2 -= 4f;
		Vector2 timeButSize = UI_TimeControls.TimeButSize;
		float y = timeButSize.y;
		Rect timerRect = new Rect(num, num2 - y, 134f, y);
		UI_TimeControls.DoTimeControlsGUI(timerRect);
		num2 -= timerRect.height;
		num2 -= 4f;
		Rect dateRect = new Rect(num, num2 - 40f, 134f, 40f);
		UI_Date.DateOnGUI(dateRect);
		num2 -= dateRect.height;
		Rect rect = new Rect(num - 30f, num2 - 26f, 164f, 26f);
		Find.WeatherManager.DoWeatherGUI(rect);
		num2 -= rect.height;
		float uIHeight = Find.MapConditionManager.UIHeight;
		Rect rect2 = new Rect(num - 30f, num2 - uIHeight, 164f, uIHeight);
		Find.MapConditionManager.DoConditionsUI(rect2);
		num2 -= rect2.height;
	}
}
