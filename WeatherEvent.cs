public abstract class WeatherEvent
{
	public abstract bool Expired { get; }

	public virtual SkyTarget OverrideSkyTarget => null;

	public virtual float OverrideSkyTargetLerpFactor => 1f;

	public abstract void FireEvent();

	public abstract void WeatherEventTick();

	public virtual void WeatherEventDraw()
	{
	}
}
