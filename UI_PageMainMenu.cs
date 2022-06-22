using UnityEngine;

public class UI_PageMainMenu : UI_MenuPage
{
	private static readonly Texture2D BGPlanet = Res.LoadTexture("UI/HeroArt/BGPlanet");

	private static readonly Vector2 BGPlanetSize = new Vector2(2000f, 1208f);

	public override void PageOnGUI()
	{
		float num = Screen.width / 2;
		Vector2 bGPlanetSize = BGPlanetSize;
		float left = num - bGPlanetSize.x / 2f;
		Vector2 bGPlanetSize2 = BGPlanetSize;
		float x = bGPlanetSize2.x;
		Vector2 bGPlanetSize3 = BGPlanetSize;
		Rect position = new Rect(left, 0f, x, bGPlanetSize3.y);
		GUI.DrawTexture(position, BGPlanet);
		VersionControl.DrawVersionInCorner();
	}
}
