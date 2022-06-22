public interface Edible
{
	bool EdibleNow { get; }

	float CurNutrition { get; }

	void Notify_Eaten();
}
