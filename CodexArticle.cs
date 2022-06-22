using System.Collections.Generic;

public class CodexArticle
{
	public string title;

	public List<CodexContent> contentList = new List<CodexContent>();

	public CodexArticle(string title)
	{
		this.title = title;
	}
}
