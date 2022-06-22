using UnityEngine;

public static class GenGameEnd
{
	public static void GameOverEveryoneDead()
	{
		EndGameWithMessage("Everyone is dead or gone. This story is over.\n\nPerhaps someone else will find a use for the ruins of this place.");
	}

	private static void EndGameWithMessage(string Msg)
	{
		DiaNode diaNode = new DiaNode(Msg);
		DiaOption diaOption = new DiaOption("Keep watching");
		diaOption.ResolveTree = true;
		diaNode.optionList.Add(diaOption);
		DiaOption diaOption2 = new DiaOption("Main Menu");
		diaOption2.ChosenCallback = delegate
		{
			Application.LoadLevel("Entry");
		};
		diaOption2.ResolveTree = true;
		diaNode.optionList.Add(diaOption2);
		DialogBoxHelper.InitDialogTree(diaNode);
	}
}
