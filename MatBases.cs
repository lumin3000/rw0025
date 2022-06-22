using UnityEngine;

public static class MatBases
{
	public static readonly Material Cutout;

	public static readonly Material Blueprint;

	public static readonly Material CutoutFlying;

	public static readonly Material PlantCutout;

	public static readonly Material Transparent;

	public static readonly Material Mote;

	public static readonly Material MotePostLight;

	public static readonly Material LocalGlow;

	public static readonly Material LightOverlay;

	public static readonly Material SunShadow;

	public static readonly Material SunShadowFade;

	public static readonly Material EdgeShadow;

	public static readonly Material TerrainHard;

	public static readonly Material TerrainFade;

	public static readonly Material IndoorMask;

	public static readonly Material SolidColor;

	public static readonly Material FogOfWar;

	public static readonly Material MetaOverlay;

	static MatBases()
	{
		Cutout = MatLoader.LoadMat("Map/Cutout");
		PlantCutout = MatLoader.LoadMat("Map/PlantCutout");
		Transparent = MatLoader.LoadMat("Map/Transparent");
		Mote = MatLoader.LoadMat("Map/Mote");
		LocalGlow = MatLoader.LoadMat("Map/LocalGlow");
		LightOverlay = MatLoader.LoadMat("Lighting/LightOverlay");
		SunShadow = MatLoader.LoadMat("Lighting/SunShadow");
		SunShadowFade = SunShadow;
		EdgeShadow = MatLoader.LoadMat("Lighting/EdgeShadow");
		TerrainHard = MatLoader.LoadMat("Terrain/TerrainHard");
		TerrainFade = MatLoader.LoadMat("Terrain/TerrainFade");
		IndoorMask = MatLoader.LoadMat("Misc/IndoorMask");
		SolidColor = MatLoader.LoadMat("Misc/SolidColor");
		FogOfWar = MatLoader.LoadMat("Misc/FogOfWar");
		MetaOverlay = MatLoader.LoadMat("Misc/MetaOverlay");
		CutoutFlying = new Material(Cutout);
		CutoutFlying.renderQueue = 3050;
		MotePostLight = MatLoader.LoadMat("Map/MotePostLight");
		Blueprint = new Material(Transparent);
		Blueprint.color = new Color(0.75f, 0.8f, 1f, 0.3f);
	}
}
