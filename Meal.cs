public class Meal : ThingWithComponents, Edible
{
	private int age;

	public bool EdibleNow => true;

	public float CurNutrition => def.food.nutrition;

	public override void ExposeData()
	{
		base.ExposeData();
		Scribe.LookField(ref age, "Age");
	}

	public void Notify_Eaten()
	{
		Destroy();
	}

	public override void TickRare()
	{
		age += 250;
		if (age >= def.meal_ticksBeforeSpoil)
		{
			Spoil();
		}
	}

	private void Spoil()
	{
		Destroy();
	}
}
