using System.Collections.Generic;
using System.Text;

public static class LogWrite
{
	public static string ListString<T>(IEnumerable<T> list)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("[");
		bool flag = false;
		foreach (T item in list)
		{
			stringBuilder.Append(item.ToString());
			stringBuilder.Append(" - ");
			flag = true;
		}
		if (flag)
		{
			stringBuilder.Remove(stringBuilder.Length - 3, 3);
		}
		stringBuilder.Append("]");
		return stringBuilder.ToString();
	}
}
