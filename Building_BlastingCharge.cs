using System.Collections.Generic;
using UnityEngine;

public class Building_BlastingCharge : Building
{
	public override IEnumerable<Command> GetCommandOptions()
	{
		yield return new Command_Action
		{
			icon = Res.LoadTexture("UI/Commands/Detonate"),
			tipDef = new TooltipDef("Start the countdown to detonation."),
			hotKey = KeyCode.V,
			action = Command_Detonate
		};
	}

	private void Command_Detonate()
	{
		GetComp<CompExplosive>().StartWick();
	}
}
