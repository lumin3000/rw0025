using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Res
{
	public static AudioClip LoadSound(string SoundPath)
	{
		AudioClip audioClip = (AudioClip)Resources.Load("Sounds/" + SoundPath);
		if (audioClip == null)
		{
			Debug.LogWarning("Could not load sound at " + SoundPath);
		}
		return audioClip;
	}

	public static IEnumerable<AudioClip> LoadSoundsInFolder(string folderPath)
	{
		object[] source = Resources.LoadAll("Sounds/" + folderPath, typeof(AudioClip));
		return source.Cast<AudioClip>();
	}

	public static Texture2D LoadTexture(string texPath)
	{
		return LoadTexture(texPath, reportFailure: true);
	}

	public static Texture2D LoadTexture(string texPath, bool reportFailure)
	{
		string text = "Textures/" + texPath;
		Texture2D texture2D = (Texture2D)Resources.Load(text);
		if (texture2D == null && reportFailure)
		{
			Debug.LogWarning("Could not load texture at " + text);
		}
		return texture2D;
	}
}
