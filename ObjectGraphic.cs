using UnityEngine;

public class ObjectGraphic
{
	private Material[] mats = new Material[4];

	public Material MatFront => mats[2];

	public Material MatSide => mats[1];

	public Material MatBack => mats[0];

	public ObjectGraphic(string filePathStart)
	{
		mats = new Material[3];
		mats[0] = MaterialPool.MatFrom(filePathStart + "_back", reportFailure: false);
		mats[1] = MaterialPool.MatFrom(filePathStart + "_side", reportFailure: false);
		mats[2] = MaterialPool.MatFrom(filePathStart + "_front", reportFailure: false);
		if (mats[0] == null || mats[0] == MatsSimple.BadMaterial)
		{
			mats[0] = mats[2];
		}
		if (mats[2] == null || mats[2] == MatsSimple.BadMaterial)
		{
			mats[2] = mats[0];
		}
		if (mats[1] == null || mats[1] == MatsSimple.BadMaterial)
		{
			mats[1] = mats[0];
		}
		for (int i = 0; i < 3; i++)
		{
			if (mats[i] == null)
			{
				Debug.LogError("Object missing materials. Loaded from " + filePathStart);
			}
		}
	}

	public Material MatAt(IntRot rot)
	{
		int num = rot.AsInt;
		if (num == 3 && mats.Length == 3)
		{
			num = 1;
		}
		return mats[num];
	}
}
