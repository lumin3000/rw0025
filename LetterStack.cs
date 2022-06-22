using System.Collections.Generic;
using UnityEngine;

public class LetterStack : Saveable
{
	private const float LettersBottomY = 350f;

	private const float LetterSpacing = 12f;

	private List<Letter> letters = new List<Letter>();

	public void ExposeData()
	{
		Scribe.LookList(ref letters, "Letters");
	}

	public void ReceiveLetter(Letter let)
	{
		GenSound.PlaySoundOnCamera("Interface/LetterArrive", 0.3f);
		letters.Add(let);
		let.arrivalTime = Time.time;
	}

	public void RemoveLetter(Letter let)
	{
		letters.Remove(let);
	}

	public void LettersOnGUI()
	{
		float num = (float)Screen.height - 350f - 30f;
		foreach (Letter item in letters.ListFullCopy())
		{
			item.DrawButtonAt(new Vector2((float)Screen.width - 38f - 12f, num));
			num -= 42f;
		}
	}
}
