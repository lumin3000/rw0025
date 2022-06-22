public class AcceptanceReport
{
	public string reasonText;

	public bool accepted;

	public static AcceptanceReport WasAccepted
	{
		get
		{
			AcceptanceReport acceptanceReport = new AcceptanceReport(string.Empty);
			acceptanceReport.accepted = true;
			return acceptanceReport;
		}
	}

	public static AcceptanceReport WasRejected
	{
		get
		{
			AcceptanceReport acceptanceReport = new AcceptanceReport(string.Empty);
			acceptanceReport.accepted = false;
			return acceptanceReport;
		}
	}

	public AcceptanceReport(string ReasonText)
	{
		accepted = false;
		reasonText = ReasonText;
	}

	public static implicit operator AcceptanceReport(bool value)
	{
		if (value)
		{
			return WasAccepted;
		}
		return WasRejected;
	}

	public static implicit operator AcceptanceReport(string value)
	{
		return new AcceptanceReport(value);
	}
}
