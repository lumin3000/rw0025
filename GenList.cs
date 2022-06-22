using System;
using System.Collections.Generic;
using UnityEngine;

public static class GenList
{
	public static void Shuffle<T>(this IList<T> list)
	{
		int num = list.Count;
		while (num > 1)
		{
			num--;
			int index = UnityEngine.Random.Range(0, num + 1);
			T value = list[index];
			list[index] = list[num];
			list[num] = value;
		}
	}

	public static void InsertionSort<T>(this IList<T> list, Comparison<T> comparison)
	{
		int count = list.Count;
		for (int i = 1; i < count; i++)
		{
			T val = list[i];
			int num = i - 1;
			while (num >= 0 && comparison(list[num], val) > 0)
			{
				list[num + 1] = list[num];
				num--;
			}
			list[num + 1] = val;
		}
	}

	public static List<T> ListFullCopy<T>(this List<T> srcList)
	{
		List<T> list = new List<T>();
		list.Capacity = srcList.Capacity;
		foreach (T src in srcList)
		{
			list.Add(src);
		}
		return list;
	}

	public static Dictionary<T, A> DictFullCopy<T, A>(this Dictionary<T, A> srcDict)
	{
		Dictionary<T, A> dictionary = new Dictionary<T, A>();
		foreach (KeyValuePair<T, A> item in srcDict)
		{
			dictionary.Add(item.Key, item.Value);
		}
		return dictionary;
	}

	public static HashSet<T> HashSetFullCopy<T>(this HashSet<T> srcHasher)
	{
		HashSet<T> hashSet = new HashSet<T>();
		foreach (T item in srcHasher)
		{
			hashSet.Add(item);
		}
		return hashSet;
	}

	public static T RandomElement<T>(this List<T> SrcList)
	{
		if (SrcList.Count == 0)
		{
			Debug.LogWarning("Getting random element from empty list.");
			return default(T);
		}
		return SrcList[UnityEngine.Random.Range(0, SrcList.Count)];
	}
}
