using UnityEngine;

public class Building_AirTank : Building_AirNetworkable
{
	public Building_AirTank()
	{
		StoredAirMax = 400f;
	}

	public override void Draw()
	{
		base.Draw();
		DrawAirBar(new Vector2(0f, 0f), 3f);
	}
}
