using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FoodProperties
{
	public float nutrition;

	public FoodCategory category;

	public int baseEatTicks = 500;

	public ThoughtType eatenThoughtType;

	public string eatSoundFolderName = "Interaction/Food/StartEat";

	public List<AudioClip> eatSoundList = new List<AudioClip>();

	public FoodProperties(FoodCategory category, float nutrition)
	{
		if (category == FoodCategory.NoFood)
		{
			Debug.LogError("You can't define a food with category NoFood");
		}
		this.category = category;
		this.nutrition = nutrition;
	}

	public void PostLoad()
	{
		if (eatSoundFolderName != string.Empty)
		{
			eatSoundList = Res.LoadSoundsInFolder(eatSoundFolderName).ToList();
		}
	}
}
