using System;

public class DialogBox_DebugWeatherChooser : DialogBox_DebugLister
{
	protected override void DoList()
	{
		foreach (Type item in typeof(Weather).AllSubclasses())
		{
			Type localType = item;
			AddOption(localType.Name, delegate
			{
				Weather newWeather = (Weather)Activator.CreateInstance(localType);
				Find.WeatherManager.TransitionTo(newWeather);
			});
		}
	}
}
