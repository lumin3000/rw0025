using System.Collections.Generic;

public class FastPriorityQueue<T>
{
	protected List<T> innerList = new List<T>();

	protected IComparer<T> comparer;

	public int Count => innerList.Count;

	public FastPriorityQueue()
	{
		comparer = Comparer<T>.Default;
	}

	public FastPriorityQueue(IComparer<T> comparer)
	{
		this.comparer = comparer;
	}

	public void Push(T item)
	{
		int num = innerList.Count;
		innerList.Add(item);
		while (num != 0)
		{
			int num2 = (num - 1) / 2;
			if (CompareElements(num, num2) < 0)
			{
				SwitchElements(num, num2);
				num = num2;
				continue;
			}
			break;
		}
	}

	public T Pop()
	{
		T result = innerList[0];
		int num = 0;
		innerList[0] = innerList[innerList.Count - 1];
		innerList.RemoveAt(innerList.Count - 1);
		while (true)
		{
			int num2 = num;
			int num3 = 2 * num + 1;
			int num4 = 2 * num + 2;
			if (innerList.Count > num3 && CompareElements(num, num3) > 0)
			{
				num = num3;
			}
			if (innerList.Count > num4 && CompareElements(num, num4) > 0)
			{
				num = num4;
			}
			if (num == num2)
			{
				break;
			}
			SwitchElements(num, num2);
		}
		return result;
	}

	public void Update(int i)
	{
		int num = i;
		while (num != 0)
		{
			int num2 = (num - 1) / 2;
			if (CompareElements(num, num2) < 0)
			{
				SwitchElements(num, num2);
				num = num2;
				continue;
			}
			break;
		}
		if (num < i)
		{
			return;
		}
		while (true)
		{
			int num3 = num;
			int num4 = 2 * num + 1;
			int num2 = 2 * num + 2;
			if (innerList.Count > num4 && CompareElements(num, num4) > 0)
			{
				num = num4;
			}
			if (innerList.Count > num2 && CompareElements(num, num2) > 0)
			{
				num = num2;
			}
			if (num == num3)
			{
				break;
			}
			SwitchElements(num, num3);
		}
	}

	public T Peek()
	{
		if (innerList.Count > 0)
		{
			return innerList[0];
		}
		return default(T);
	}

	public void Clear()
	{
		innerList.Clear();
	}

	public void RemoveLocation(T item)
	{
		int num = -1;
		for (int i = 0; i < innerList.Count; i++)
		{
			if (comparer.Compare(innerList[i], item) == 0)
			{
				num = i;
			}
		}
		if (num != -1)
		{
			innerList.RemoveAt(num);
		}
	}

	protected void SwitchElements(int i, int j)
	{
		T value = innerList[i];
		innerList[i] = innerList[j];
		innerList[j] = value;
	}

	protected int CompareElements(int i, int j)
	{
		return comparer.Compare(innerList[i], innerList[j]);
	}
}
