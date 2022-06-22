using System;
using System.Collections.Generic;

public static class WeatherPartPool
{
	private static List<object> instances = new List<object>();

	public static WeatherOverlay GetInstanceOf(Type overlayType)
	{
		foreach (object instance in instances)
		{
			if (instance.GetType() == overlayType)
			{
				return (WeatherOverlay)instance;
			}
		}
		object obj = Activator.CreateInstance(overlayType);
		instances.Add(obj);
		return (WeatherOverlay)obj;
	}
}
