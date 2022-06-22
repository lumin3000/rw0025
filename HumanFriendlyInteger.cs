public static class HumanFriendlyInteger
{
	private static string[] ones = new string[10]
	{
		string.Empty,
		"one",
		"two",
		"three",
		"four",
		"five",
		"six",
		"seven",
		"eight",
		"nine"
	};

	private static string[] teens = new string[10] { "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };

	private static string[] tens = new string[8] { "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

	private static string[] thousandsGroups = new string[4]
	{
		string.Empty,
		" thousand",
		" million",
		" billion"
	};

	private static string FriendlyInteger(int n, string leftDigits, int thousands)
	{
		if (n == 0)
		{
			return leftDigits;
		}
		string text = leftDigits;
		if (text.Length > 0)
		{
			text += " ";
		}
		text = ((n < 10) ? (text + ones[n]) : ((n < 20) ? (text + teens[n - 10]) : ((n < 100) ? (text + FriendlyInteger(n % 10, tens[n / 10 - 2], 0)) : ((n >= 1000) ? (text + FriendlyInteger(n % 1000, FriendlyInteger(n / 1000, string.Empty, thousands + 1), 0)) : (text + FriendlyInteger(n % 100, ones[n / 100] + " Hundred", 0))))));
		return text + thousandsGroups[thousands];
	}

	public static string IntegerToWritten(int n)
	{
		if (n == 0)
		{
			return "zero";
		}
		if (n < 0)
		{
			return "negative " + IntegerToWritten(-n);
		}
		return FriendlyInteger(n, string.Empty, 0);
	}
}
