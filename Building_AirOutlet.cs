using UnityEngine;

public class Building_AirOutlet : Building_AirNetworkable
{
	protected const float AirReleasePerTick = 0.05f;

	public override void Tick_ConsumeAir()
	{
		if (airNet == null)
		{
			return;
		}
		Room roomAt = Find.Grids.GetRoomAt(base.Position);
		if (roomAt != null && roomAt.airTight && roomAt.Air < roomAt.MaxAir && airNet.TotalAir > 0.05f)
		{
			if (roomAt.AirPressure < 0.95f && Random.value < 0.3f)
			{
				MoteMaker.ThrowAirPuffUp(this.TrueCenter(), AltitudeLayer.HighMote);
			}
			airNet.LoseAir(0.05f);
			roomAt.Air += 0.05f;
		}
	}
}
