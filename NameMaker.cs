using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class NameMaker
{
	private class NameBank
	{
		private const float UsedNameProportionLimit = 0.75f;

		public NameType ID;

		public List<string> NamesSexless = new List<string>();

		public List<string> NamesUnisex = new List<string>();

		public List<string> NamesMale = new List<string>();

		public List<string> NamesFemale = new List<string>();

		public HashSet<int> UsedNamesSexless = new HashSet<int>();

		public HashSet<int> UsedNamesUnisex = new HashSet<int>();

		public HashSet<int> UsedNamesMale = new HashSet<int>();

		public HashSet<int> UsedNamesFemale = new HashSet<int>();

		public NameBank(NameType ID)
		{
			this.ID = ID;
		}

		public string GetName(Gender Sex)
		{
			List<string> list = null;
			HashSet<int> hashSet = null;
			if (Sex == Gender.Sexless)
			{
				list = NamesSexless;
				hashSet = UsedNamesSexless;
			}
			else
			{
				float num = (float)NamesUnisex.Count / (float)NamesMale.Count;
				if (UnityEngine.Random.value < num)
				{
					list = NamesUnisex;
					hashSet = UsedNamesUnisex;
				}
				else
				{
					switch (Sex)
					{
					case Gender.Male:
						list = NamesMale;
						hashSet = UsedNamesMale;
						break;
					case Gender.Female:
						list = NamesFemale;
						hashSet = UsedNamesFemale;
						break;
					}
				}
			}
			if ((float)hashSet.Count > (float)list.Count * 0.75f)
			{
				Debug.LogWarning("Used too many character names. Allowing re-use...");
				hashSet.Clear();
			}
			int num2;
			do
			{
				num2 = UnityEngine.Random.Range(0, list.Count);
			}
			while (hashSet.Contains(num2));
			string result = list[num2];
			hashSet.Add(num2);
			return result;
		}
	}

	public const int MaxNameLength = 12;

	private static List<NameBank> NameBanks;

	static NameMaker()
	{
		NameBanks = new List<NameBank>();
		foreach (int value in Enum.GetValues(typeof(NameType)))
		{
			NameBanks.Add(new NameBank((NameType)value));
		}
		FillNameBanks();
		VerifyNameBanks();
	}

	private static NameBank BankOf(NameType BankID)
	{
		return NameBanks[(int)BankID];
	}

	public static string NewName(PawnKindDefinition kindDef, Gender gender)
	{
		NameType nameType = kindDef.RaceDef.nameType;
		if (nameType == NameType.NoName)
		{
			return kindDef.kindLabel;
		}
		return NewName(nameType, gender);
	}

	public static string NewName(NameType nType, Gender gender)
	{
		return BankOf(nType).GetName(gender);
	}

	private static void FillNameBanks()
	{
		BankOf(NameType.HumanStandard).NamesUnisex = GenFile.StringsFromFile("Namebanks/Names_Unisex").ToList();
		BankOf(NameType.HumanStandard).NamesMale = GenFile.StringsFromFile("Namebanks/Names_Male").Concat(GenFile.StringsFromFile("Namebanks/Names_Male_Japanese")).Concat(GenFile.StringsFromFile("Namebanks/Names_Male_Russian"))
			.ToList();
		BankOf(NameType.HumanStandard).NamesFemale = GenFile.StringsFromFile("Namebanks/Names_Female").Concat(GenFile.StringsFromFile("Namebanks/Names_Female_Japanese")).Concat(GenFile.StringsFromFile("Namebanks/Names_Female_Russian"))
			.ToList();
		BankOf(NameType.Robot).NamesSexless = GenFile.StringsFromFile("Namebanks/Names_Robot").ToList();
		BankOf(NameType.Trader).NamesSexless = GenFile.StringsFromFile("Namebanks/Names_Trader").ToList();
	}

	private static void VerifyNameBanks()
	{
		Action<List<string>> action = delegate(List<string> list)
		{
			List<string> list2 = (from x in list
				group x by x into g
				where g.Count() > 1
				select g.Key).ToList();
			foreach (string item in list2)
			{
				Debug.LogWarning("Duplicated name: " + item);
			}
			foreach (string item2 in list)
			{
				if (item2.Trim() != item2)
				{
					Debug.LogWarning("Trimmable whitespace on name: [" + item2 + "]");
				}
			}
		};
		foreach (NameBank nameBank in NameBanks)
		{
			action(nameBank.NamesUnisex);
			action(nameBank.NamesSexless);
			action(nameBank.NamesMale);
			action(nameBank.NamesFemale);
		}
	}
}
