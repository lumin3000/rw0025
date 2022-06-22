using System.Collections.Generic;

public class CodexSection
{
	public string title;

	public List<CodexCategory> categoryList = new List<CodexCategory>();

	public CodexSection(string title)
	{
		this.title = title;
	}
}
