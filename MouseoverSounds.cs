using UnityEngine;

public static class MouseoverSounds
{
	private static Rect? lastContainingRect = null;

	private static int silentUntilFrame = -1;

	private static int lastFrameFoundRect;

	public static readonly AudioClip SoundOptionMouseover = Res.LoadSound("Interface/OptionMouseover");

	public static readonly AudioClip SoundOptionMouseoverThump = Res.LoadSound("Interface/OptionMouseoverThump");

	public static void SilenceForNextFrame()
	{
		silentUntilFrame = Time.frameCount + 1;
	}

	public static void DoRegion(Rect r)
	{
		DoRegion(r, MouseoverSoundType.Normal);
	}

	public static void DoRegion(Rect r, MouseoverSoundType soundType)
	{
		if (Time.frameCount <= lastFrameFoundRect + 1)
		{
			return;
		}
		if (r.Contains(Event.current.mousePosition))
		{
			lastFrameFoundRect = Time.frameCount;
			if (!r.Equals(lastContainingRect))
			{
				lastContainingRect = new Rect(r);
				if (silentUntilFrame < Time.frameCount)
				{
					AudioClip clip = null;
					if (soundType == MouseoverSoundType.Normal)
					{
						clip = SoundOptionMouseover;
					}
					if (soundType == MouseoverSoundType.Thump)
					{
						clip = SoundOptionMouseoverThump;
					}
					GenSound.PlaySoundOnCamera(clip, 0.04f);
				}
			}
		}
		Rect? rect = lastContainingRect;
		if (rect.HasValue && lastContainingRect.Equals(r) && !lastContainingRect.Value.Contains(Event.current.mousePosition))
		{
			lastContainingRect = null;
		}
	}
}
