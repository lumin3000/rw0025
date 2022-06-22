using System.Collections.Generic;

public class UI_FeedbackFloaters
{
	protected List<FeedbackItem> FeedbackList = new List<FeedbackItem>();

	public void AddFeedback(FeedbackItem newFeedback)
	{
		FeedbackList.Add(newFeedback);
	}

	public void FeedbackUpdate()
	{
		foreach (FeedbackItem item in FeedbackList.ListFullCopy())
		{
			item.Update();
			if (item.TimeLeft <= 0f)
			{
				FeedbackList.Remove(item);
			}
		}
	}

	public void FeedbackOnGUI()
	{
		foreach (FeedbackItem feedback in FeedbackList)
		{
			feedback.FeedbackOnGUI();
		}
	}
}
