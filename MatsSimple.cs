using UnityEngine;

public static class MatsSimple
{
	public static readonly Material BadMaterial = GenRender.SolidColorMaterial(new Color(0.8235294f, 40f / 51f, 32f / 51f, 1f));

	public static readonly Material ClearMaterial = GenRender.SolidColorMaterial(new Color(0f, 0f, 0f, 0f));

	public static readonly Material WhiteMaterial = GenRender.SolidColorMaterial(Color.white);

	public static readonly Material TranslucentWhiteMaterial = GenRender.SolidColorMaterial(new Color(1f, 1f, 1f, 0.3f));
}
