public abstract class Designator_Plants : Designator
{
	public Designator_Plants()
	{
	}

	public override void FinalizeDesignationSucceeded()
	{
		GenSound.PlaySoundOnCamera("Interface/DesignateMine", 0.15f);
	}

	public override void DesignatorUpdate()
	{
		GenUI.RenderMouseoverBracket();
	}
}
