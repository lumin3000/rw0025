using System.Collections.Generic;

public abstract class TutorItem
{
	protected List<TutorSignal> showSignalsList = new List<TutorSignal>();

	public virtual bool Completed => false;

	public abstract void TutorItemOnGUI();

	public bool ShowOnSignal(TutorSignal signal)
	{
		if (showSignalsList.Contains(signal))
		{
			return true;
		}
		return false;
	}
}
