using System;

namespace NPack
{
	public class MersenneTwister : Random
	{
		private const int N = 624;

		private const int M = 397;

		private const uint MatrixA = 2567483615u;

		private const uint UpperMask = 2147483648u;

		private const uint LowerMask = 2147483647u;

		private const uint TemperingMaskB = 2636928640u;

		private const uint TemperingMaskC = 4022730752u;

		private const double FiftyThreeBitsOf1s = 9007199254740991.0;

		private const double Inverse53BitsOf1s = 1.1102230246251568E-16;

		private const double OnePlus53BitsOf1s = 9007199254740992.0;

		private const double InverseOnePlus53BitsOf1s = 1.1102230246251565E-16;

		private short _mti;

		private readonly uint[] _mt = new uint[624];

		private static readonly uint[] _mag01 = new uint[2] { 0u, 2567483615u };

		public MersenneTwister(int seed)
		{
			Init((uint)seed);
		}

		public MersenneTwister()
			: this(new Random().Next())
		{
		}

		public MersenneTwister(int[] initKey)
		{
			if (initKey == null)
			{
				throw new ArgumentNullException("initKey");
			}
			uint[] array = new uint[initKey.Length];
			for (int i = 0; i < initKey.Length; i++)
			{
				array[i] = (uint)initKey[i];
			}
			init(array);
		}

		public virtual uint NextUInt32()
		{
			return GenerateUInt32();
		}

		public virtual uint NextUInt32(uint maxValue)
		{
			return (uint)((double)GenerateUInt32() / (4294967295.0 / (double)maxValue));
		}

		public virtual uint NextUInt32(uint minValue, uint maxValue)
		{
			if (minValue >= maxValue)
			{
				throw new ArgumentOutOfRangeException();
			}
			return (uint)((double)GenerateUInt32() / (4294967295.0 / (double)(maxValue - minValue)) + (double)minValue);
		}

		public override int Next()
		{
			return Next(int.MaxValue);
		}

		public override int Next(int maxValue)
		{
			if (maxValue < 1)
			{
				if (maxValue < 0)
				{
					throw new ArgumentOutOfRangeException();
				}
				return 0;
			}
			return (int)(NextDouble() * (double)(maxValue + 1));
		}

		public override int Next(int minValue, int maxValue)
		{
			if (maxValue < minValue)
			{
				throw new ArgumentOutOfRangeException();
			}
			if (maxValue == minValue)
			{
				return minValue;
			}
			return Next(maxValue - minValue) + minValue;
		}

		public override void NextBytes(byte[] buffer)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException();
			}
			int num = buffer.Length;
			for (int i = 0; i < num; i++)
			{
				buffer[i] = (byte)Next(256);
			}
		}

		public override double NextDouble()
		{
			return compute53BitRandom(0.0, 1.1102230246251565E-16);
		}

		public double NextDouble(bool includeOne)
		{
			return (!includeOne) ? NextDouble() : compute53BitRandom(0.0, 1.1102230246251568E-16);
		}

		public double NextDoublePositive()
		{
			return compute53BitRandom(0.5, 1.1102230246251568E-16);
		}

		public float NextSingle()
		{
			return (float)NextDouble();
		}

		public float NextSingle(bool includeOne)
		{
			return (float)NextDouble(includeOne);
		}

		public float NextSinglePositive()
		{
			return (float)NextDoublePositive();
		}

		protected uint GenerateUInt32()
		{
			uint num2;
			if (_mti >= 624)
			{
				short num;
				for (num = 0; num < 227; num = (short)(num + 1))
				{
					num2 = (_mt[num] & 0x80000000u) | (_mt[num + 1] & 0x7FFFFFFFu);
					_mt[num] = _mt[num + 397] ^ (num2 >> 1) ^ _mag01[num2 & 1];
				}
				while (num < 623)
				{
					num2 = (_mt[num] & 0x80000000u) | (_mt[num + 1] & 0x7FFFFFFFu);
					_mt[num] = _mt[num + -227] ^ (num2 >> 1) ^ _mag01[num2 & 1];
					num = (short)(num + 1);
				}
				num2 = (_mt[623] & 0x80000000u) | (_mt[0] & 0x7FFFFFFFu);
				_mt[623] = _mt[396] ^ (num2 >> 1) ^ _mag01[num2 & 1];
				_mti = 0;
			}
			num2 = _mt[_mti++];
			num2 ^= temperingShiftU(num2);
			num2 ^= temperingShiftS(num2) & 0x9D2C5680u;
			num2 ^= temperingShiftT(num2) & 0xEFC60000u;
			return num2 ^ temperingShiftL(num2);
		}

		private static uint temperingShiftU(uint y)
		{
			return y >> 11;
		}

		private static uint temperingShiftS(uint y)
		{
			return y << 7;
		}

		private static uint temperingShiftT(uint y)
		{
			return y << 15;
		}

		private static uint temperingShiftL(uint y)
		{
			return y >> 18;
		}

		private void Init(uint seed)
		{
			_mt[0] = seed & 0xFFFFFFFFu;
			for (_mti = 1; _mti < 624; _mti++)
			{
				_mt[_mti] = (uint)(1812433253 * (_mt[_mti - 1] ^ (_mt[_mti - 1] >> 30)) + _mti);
				_mt[_mti] &= uint.MaxValue;
			}
		}

		private void init(uint[] key)
		{
			Init(19650218u);
			int num = key.Length;
			int num2 = 1;
			int num3 = 0;
			for (int num4 = ((624 <= num) ? num : 624); num4 > 0; num4--)
			{
				_mt[num2] = (uint)((_mt[num2] ^ ((_mt[num2 - 1] ^ (_mt[num2 - 1] >> 30)) * 1664525)) + key[num3] + num3);
				_mt[num2] &= uint.MaxValue;
				num2++;
				num3++;
				if (num2 >= 624)
				{
					_mt[0] = _mt[623];
					num2 = 1;
				}
				if (num3 >= num)
				{
					num3 = 0;
				}
			}
			for (int num4 = 623; num4 > 0; num4--)
			{
				_mt[num2] = (uint)((_mt[num2] ^ ((_mt[num2 - 1] ^ (_mt[num2 - 1] >> 30)) * 1566083941)) - num2);
				_mt[num2] &= uint.MaxValue;
				num2++;
				if (num2 >= 624)
				{
					_mt[0] = _mt[623];
					num2 = 1;
				}
			}
			_mt[0] = 2147483648u;
		}

		private double compute53BitRandom(double translate, double scale)
		{
			ulong num = (ulong)GenerateUInt32() >> 5;
			ulong num2 = (ulong)GenerateUInt32() >> 6;
			return ((double)num * 67108864.0 + (double)num2 + translate) * scale;
		}
	}
}
