using UnityEngine;

public static class MeshPool
{
	private const int MaxGridMeshSize = 15;

	public static readonly Mesh plane03;

	public static readonly Mesh plane05;

	public static readonly Mesh plane07;

	public static readonly Mesh plane08;

	public static readonly Mesh plane14;

	public static readonly Mesh plane14Flip;

	public static readonly Mesh plane10;

	public static readonly Mesh plane10Back;

	public static readonly Mesh plane12Back;

	public static readonly Mesh plane12BackFlip;

	public static readonly Mesh plane24Back;

	public static readonly Mesh plane24BackFlip;

	public static readonly Mesh plane17;

	public static readonly Mesh plane20;

	public static readonly Mesh plane30;

	public static readonly Mesh plane30Twist;

	public static readonly Mesh plane50;

	public static readonly Mesh[,] gridPlanes;

	public static readonly Mesh wholeMapPlane;

	public static readonly Mesh shadow0206;

	public static readonly Mesh shadow0306;

	public static readonly Mesh shadow0408;

	public static readonly Mesh shadow0604;

	public static readonly Mesh shadow0604short;

	public static readonly Mesh shadow0606;

	public static readonly Mesh shadow0608;

	public static readonly Mesh shadow1006;

	public static Mesh[] pies;

	public static Mesh circle;

	static MeshPool()
	{
		gridPlanes = new Mesh[15, 15];
		pies = new Mesh[361];
		plane03 = MeshMakerPlanes.NewPlaneMesh(0.33333f);
		plane05 = MeshMakerPlanes.NewPlaneMesh(0.5f);
		plane07 = MeshMakerPlanes.NewPlaneMesh(0.7f);
		plane08 = MeshMakerPlanes.NewPlaneMesh(0.8f);
		plane14 = MeshMakerPlanes.NewPlaneMesh(1.4f);
		plane14Flip = MeshMakerPlanes.NewPlaneMesh(1.4f, flipped: true);
		plane10 = MeshMakerPlanes.NewPlaneMesh(1f);
		plane10Back = MeshMakerPlanes.NewPlaneMesh(1f, flipped: false, backLift: true);
		plane12Back = MeshMakerPlanes.NewPlaneMesh(1.2f, flipped: false, backLift: true);
		plane12BackFlip = MeshMakerPlanes.NewPlaneMesh(1.2f, flipped: true, backLift: true);
		plane24Back = MeshMakerPlanes.NewPlaneMesh(2.4f, flipped: false, backLift: true);
		plane24BackFlip = MeshMakerPlanes.NewPlaneMesh(2.4f, flipped: true, backLift: true);
		plane17 = MeshMakerPlanes.NewPlaneMesh(1.7f);
		plane20 = MeshMakerPlanes.NewPlaneMesh(2f);
		plane30 = MeshMakerPlanes.NewPlaneMesh(3f);
		plane30Twist = MeshMakerPlanes.NewPlaneMesh(3f, flipped: false, backLift: false, twist: true);
		plane50 = MeshMakerPlanes.NewPlaneMesh(5f);
		for (int i = 0; i < 15; i++)
		{
			for (int j = 0; j < 15; j++)
			{
				gridPlanes[i, j] = MeshMakerPlanes.NewPlaneMesh(new Vector2(i, j), flipped: false, backLift: false, twist: false);
			}
		}
		wholeMapPlane = MeshMakerPlanes.NewWholeMapPlane();
		shadow0206 = MeshMakerPlanes.NewShadowMesh(0.2f, 0.6f);
		shadow0306 = MeshMakerPlanes.NewShadowMesh(0.3f, 0.6f);
		shadow0408 = MeshMakerPlanes.NewShadowMesh(0.4f, 0.8f);
		shadow0604 = MeshMakerPlanes.NewShadowMesh(0.6f, 0.4f);
		shadow0604short = MeshMakerPlanes.NewShadowMesh(0.6f, 0.2f, 0.4f);
		shadow0606 = MeshMakerPlanes.NewShadowMesh(0.6f, 0.6f);
		shadow0608 = MeshMakerPlanes.NewShadowMesh(0.6f, 0.8f);
		shadow1006 = MeshMakerPlanes.NewShadowMesh(1f, 0.6f);
		for (int k = 0; k < 361; k++)
		{
			pies[k] = MeshMakerCircles.MakePieMesh(k);
		}
		circle = MeshMakerCircles.MakeCircleMesh(1f);
	}
}
