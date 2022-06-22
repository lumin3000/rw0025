using System;
using System.Linq;
using NPack;

namespace URandom
{
	public class DiceRoll
	{
		public enum DiceType
		{
			D2 = 2,
			D3 = 3,
			D4 = 4,
			D6 = 6,
			D8 = 8,
			D10 = 10,
			D12 = 12,
			D20 = 20,
			D30 = 30,
			D100 = 100
		}

		private int[] _result;

		private DiceType _dice_type = DiceType.D6;

		private int _size = 1;

		public int[] result => _result;

		public DiceType type => _dice_type;

		public int size => _size;

		public DiceRoll(int size, DiceType type, ref MersenneTwister _rand)
		{
			if (size < 1)
			{
				throw new ArgumentOutOfRangeException("Number of dices shlud be > 0");
			}
			init(size, type, ref _rand);
		}

		private void init(int size, DiceType type, ref MersenneTwister _rand)
		{
			_result = new int[size];
			_dice_type = type;
			_size = size;
			for (int i = 0; i < _size; i++)
			{
				_result[i] = _rand.Next(1, (int)type);
			}
		}

		public string TypeToString()
		{
			return _size + _dice_type.ToString();
		}

		public string RollToString()
		{
			string text = string.Empty;
			for (int i = 0; i < _size; i++)
			{
				text += _result[i];
				if (i != _size - 1)
				{
					text += ", ";
				}
			}
			return text;
		}

		public int Sum()
		{
			return _result.Sum();
		}
	}
}
