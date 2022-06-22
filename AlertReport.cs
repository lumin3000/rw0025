public struct AlertReport
{
	public bool active;

	public Thing culprit;

	public static AlertReport Active
	{
		get
		{
			AlertReport result = default(AlertReport);
			result.active = true;
			return result;
		}
	}

	public static AlertReport Inactive
	{
		get
		{
			AlertReport result = default(AlertReport);
			result.active = false;
			return result;
		}
	}

	public static AlertReport CulpritIs(Thing culp)
	{
		AlertReport result = default(AlertReport);
		result.active = culp != null;
		result.culprit = culp;
		return result;
	}

	public static implicit operator AlertReport(bool b)
	{
		AlertReport result = default(AlertReport);
		result.active = b;
		return result;
	}

	public static implicit operator AlertReport(Thing culprit)
	{
		return CulpritIs(culprit);
	}
}
