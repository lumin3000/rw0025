using System;
using System.Collections;
using System.Collections.Generic;
using NPack;

namespace URandom
{
	public sealed class ShuffleBagCollection<T> : IEnumerable, IEnumerable<T>
	{
		private Random m_generator;

		private List<T> m_data;

		private int m_cursor = -1;

		private T m_current = default(T);

		public T Current => m_current;

		public int Capacity => m_data.Capacity;

		public int Size => m_data.Count;

		public ShuffleBagCollection()
			: this(10, new MersenneTwister())
		{
		}

		public ShuffleBagCollection(MersenneTwister generator)
			: this(10, generator)
		{
		}

		public ShuffleBagCollection(int initialCapacity)
			: this(initialCapacity, new MersenneTwister())
		{
		}

		public ShuffleBagCollection(int initialCapacity, MersenneTwister generator)
		{
			if (initialCapacity < 0)
			{
				throw new ArgumentException("Capacity must be a positive integer.", "initialCapacity");
			}
			m_generator = generator;
			m_data = new List<T>(initialCapacity);
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			for (int i = 0; i <= Size; i++)
			{
				yield return Next();
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<T>)this).GetEnumerator();
		}

		public void Add(T item)
		{
			Add(item, 1);
		}

		public void Add(T item, int quantity)
		{
			if (quantity <= 0)
			{
				throw new ArgumentException("Quantity must be a positive integer.", "quantity");
			}
			for (int i = 0; i < quantity; i++)
			{
				m_data.Add(item);
			}
			m_cursor = m_data.Count - 1;
		}

		public T Next()
		{
			if (m_cursor < 1)
			{
				m_cursor = m_data.Count - 1;
				m_current = m_data[0];
				return m_current;
			}
			int index = m_generator.Next(m_cursor);
			m_current = m_data[index];
			m_data[index] = m_data[m_cursor];
			m_data[m_cursor] = m_current;
			m_cursor--;
			return m_current;
		}

		public void TrimExcess()
		{
			m_data.TrimExcess();
		}
	}
}
