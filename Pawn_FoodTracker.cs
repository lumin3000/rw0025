public class Pawn_FoodTracker : Saveable
{
	private Pawn pawn;

	private StatusLevel_Food pieceFood;

	public StatusLevel_Food Food => pieceFood;

	public float NutritionWanted => 100f - pieceFood.curLevel;

	public Pawn_FoodTracker(Pawn pawn)
	{
		this.pawn = pawn;
		pieceFood = new StatusLevel_Food(pawn);
	}

	public void ExposeData()
	{
		Scribe.LookSaveable(ref pieceFood, "PieceFood", pawn);
	}

	public void FoodTick()
	{
		pieceFood.StatusLevelTick();
	}
}
