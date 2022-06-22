public static class Game
{
	public static EditMode editMode;

	public static bool InEditMode => editMode != EditMode.Off;

	public static GameMode GMode
	{
		get
		{
			if (Find.GameRoot != null)
			{
				return GameMode.Gameplay;
			}
			return GameMode.Menus;
		}
	}
}
