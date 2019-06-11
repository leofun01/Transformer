//	PermutationByte.cs
//	
//	Based on :
//		Math
//			Abstract algebra
//				Group theory
//					Finite group
//					Permutation group
//	
//	Author   : leofun01

using System;
using System.Collections;
using System.Collections.Generic;
using StringBuilder = System.Text.StringBuilder;

namespace DotNetTransformer.Math.Group.Permutation {
	[Serializable]
	public struct PermutationByte : IPermutation<PermutationByte>
	{
		private readonly byte _value;
		private PermutationByte(byte value) { _value = value; }
		private PermutationByte(short value) {
			value = (short)(((short)(value >> 2) | value) & 0x0F0F);
			_value = (byte)((short)(value >> 4) | value);
		}
		public PermutationByte(params byte[] array) {
			if(ReferenceEquals(array, null))
				throw new ArgumentNullException();
			int count = array.GetLength(0);
			if(count > _count)
				_throwArray("Array length is out of range (0, 16).");
			_value = 0;
			if(count < 1) return;
			byte startIndex = 0;
			for(byte digit = 0; digit < _count; ++digit) {
				byte i = 0;
				while(i < count && array[i] != digit) ++i;
				if(i == count) {
					if(startIndex >= digit || i > digit)
						_throwArray(string.Concat("Value \'", digit, "\' is not found."));
					else {
						_value ^= (byte)(((1 << (digit << _s)) - 1) & _mix);
						return;
					}
				}
				else {
					_value |= (byte)(digit << (i << _s));
					if(startIndex < i) startIndex = i;
				}
			}
			_value ^= _mix;
		}

		private const byte _mix = 0xE4, _mask = 3;
		private const byte _count = 4, _len = 8, _s = 1;

		public byte Value { get { return (byte)(_value ^ _mix); } }
		public int this[int index] {
			get {
				return Value >> (index << _s) & _mask;
			}
		}
		public PermutationByte InverseElement {
			get {
				byte t = Value, r = 0;
				byte i = 0;
				do
					r |= (byte)(i << ((t >> (i << _s) & _mask) << _s));
				while(++i < _count);
				return new PermutationByte((byte)(r ^ _mix));
			}
		}
		public int CycleLength {
			get {
				byte t = Value;
				byte digitFlag = 0, multFlag = 0;
				for(byte i = 0; i < _count; ++i) {
					if((1 << i & digitFlag) != 0) continue;
					byte digit = i, cLen = 0;
					do {
						++cLen;
						digitFlag |= (byte)(1 << digit);
						digit = (byte)(t >> (digit << _s) & _mask);
					} while((1 << digit & digitFlag) == 0);
					multFlag |= (byte)(1 << --cLen);
				}
				if(multFlag == 1) return 1;
				return ((multFlag >> 2) & 3) + 2;
			}
		}
		public PermutationByte Add(PermutationByte other) {
			byte t = Value, o = other.Value, r = 0;
			byte i = 0;
			do {
				r |= (byte)((t >> ((o >> i & _mask) << _s) & _mask) << i);
				i += 1 << _s;
			} while(i < _len);
			return new PermutationByte((byte)(r ^ _mix));
		}
		public PermutationByte Subtract(PermutationByte other) {
			byte t = Value, o = other.Value, r = 0;
			byte i = 0;
			do {
				r |= (byte)((t >> i & _mask) << ((o >> i & _mask) << _s));
				i += 1 << _s;
			} while(i < _len);
			return new PermutationByte((byte)(r ^ _mix));
		}
		public PermutationByte Times(int count) {
			int c = CycleLength;
			count = (count % c + c) % c;
			PermutationByte t = this;
			PermutationByte r = (count & 1) != 0 ? t : new PermutationByte();
			while((count >>= 1) != 0) {
				t = t.Add(t);
				if((count & 1) != 0)
					r = r.Add(t);
			}
			return r;
		}

		public List<PermutationByte> GetCycles(Predicate<PermutationByte> match) {
			List<PermutationByte> list = new List<PermutationByte>(_count);
			byte t = Value;
			byte digitFlag = 0;
			for(byte i = 0; i < _count; ++i) {
				if((1 << i & digitFlag) != 0) continue;
				byte digit = i;
				byte value = 0;
				do {
					value |= (byte)(_mask << (digit << _s) & _value);
					digitFlag |= (byte)(1 << digit);
					digit = (byte)(t >> (digit << _s) & _mask);
				} while((1 << digit & digitFlag) == 0);
				PermutationByte p = new PermutationByte(value);
				if(match(p)) list.Add(p);
			}
			return list;
		}
		public int GetCyclesCount(Predicate<int> match) {
			byte t = Value;
			byte digitFlag = 0;
			int count = 0;
			for(byte i = 0; i < _count; ++i) {
				if((1 << i & digitFlag) != 0) continue;
				byte digit = i, cLen = 0;
				do {
					++cLen;
					digitFlag |= (byte)(1 << digit);
					digit = (byte)(t >> (digit << _s) & _mask);
				} while((1 << digit & digitFlag) == 0);
				if(match(cLen)) ++count;
			}
			return count;
		}

		public override int GetHashCode() { return _value; }
		public override bool Equals(object o) {
			return o is PermutationByte && Equals((PermutationByte)o);
		}
		public bool Equals(PermutationByte o) { return _value == o._value; }
		public override string ToString() {
			return _toString(_count);
		}
		public string ToString(byte minLength) {
			if(minLength > _count) minLength = _count;
			byte t = Value;
			byte i = _count;
			if(minLength > 0) --minLength;
			while(--i > minLength && (t >> (i << _s) & _mask) == i) ;
			if(minLength < i) minLength = i;
			return _toString(++minLength);
		}
		private string _toString(byte length) {
			byte t = Value;
			byte i = 0;
			StringBuilder sb = new StringBuilder(length, length);
			length <<= _s;
			do {
				sb.Append((char)(t >> i & _mask | '0'));
				i += 1 << _s;
			} while(i < length);
			return sb.ToString();
		}
		public short ToInt16() {
			short r = Value;
			r = (short)(((short)(r << 4) | r) & 0x0F0F);
			r = (short)(((short)(r << 2) | r) & 0x3333);
			return r;
		}
		public IEnumerator<int> GetEnumerator() {
			byte t = Value;
			byte i = 0;
			do {
				yield return (byte)(t >> i & _mask);
				i += 1 << _s;
			} while(i < _len);
		}
		IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

		///	<exception cref="ArgumentException">
		///		<exception cref="ArgumentNullException">
		///			Invalid <paramref name="s"/>.
		///		</exception>
		///	</exception>
		public static PermutationByte FromString(string s) {
			if(ReferenceEquals(s, null)) throw new ArgumentNullException();
			if(s.Length > _count)
				_throwString("String length is out of range (0, 4).");
			if(s.Length < 1) return new PermutationByte();
			byte value = 0;
			byte startIndex = 0;
			for(byte digit = 0; digit < _count; ++digit) {
				byte i = 0;
				char c;
				do {
					c = s[i];
					if((-_count & c) != '0')
						_throwString(string.Concat("\'", c, "\' is not a digit from [0-3]."));
				} while((c & _mask) != digit && ++i < s.Length);
				if(i == s.Length)
					if(startIndex >= digit || i > digit)
						_throwString(string.Concat("Digit \'", (char)(digit | '0'), "\' is not found."));
					else return new PermutationByte((byte)(((1 << (digit << _s)) - 1) & _mix ^ value));
				else {
					value |= (byte)(digit << (i << _s));
					if(startIndex < i) startIndex = i;
				}
			}
			return new PermutationByte((byte)(_mix ^ value));
		}
		public static PermutationByte FromByte(byte value) {
			byte startIndex = 0;
			for(byte digit = 0; digit < _count; ++digit) {
				byte i = 0;
				while(i < _count && (value >> (i << _s) & _mask) != digit) ++i;
				if(i == _count)
					if(startIndex >= digit || (value & (-1 << (digit << _s))) != 0)
						_throwByte(string.Concat("Digit \'", (char)(digit | '0'), "\' is not found."));
					else return new PermutationByte((byte)(((1 << (digit << _s)) - 1) & _mix ^ value));
				else if(startIndex < i) startIndex = i;
			}
			return new PermutationByte((byte)(_mix ^ value));
		}
		public static PermutationByte FromInt16(short value) {
			if((value & -0x3334) != 0)
				_throwInt16("Some digits is out of range [0-3].");
			byte startIndex = 0;
			for(byte digit = 0; digit < _count; ++digit) {
				byte i = 0;
				while(i < _count && (value >> (i << 2) & _mask) != digit) ++i;
				if(i == _count)
					if(startIndex >= digit || (value & (-1 << (digit << 2))) != 0)
						_throwInt16(string.Concat("Digit \'", (char)(digit | '0'), "\' is not found."));
					else return new PermutationByte((short)(((1 << (digit << 2)) - 1) & 0x3210 ^ value));
				else if(startIndex < i) startIndex = i;
			}
			return new PermutationByte((short)(0x3210 ^ value));
		}
		private static void _throwString(string message) {
			throw new ArgumentException(message
				+ " Use unique digits from [0-3], like \"0123\".");
		}
		private static void _throwByte(string message) {
			throw new ArgumentException(message
				+ " Use compressed data format and unique digits from [0-3], like 0xE4 or 0b_11_10_01_00.");
		}
		private static void _throwInt16(string message) {
			throw new ArgumentException(message
				+ " Use hexadecimal format and unique digits from [0-3], like 0x3210.");
		}
		private static void _throwArray(string message) {
			throw new ArgumentException(message
				+ " Use unique values from range (0, 4)");
		}

		public static bool operator ==(PermutationByte l, PermutationByte r) { return l.Equals(r); }
		public static bool operator !=(PermutationByte l, PermutationByte r) { return !l.Equals(r); }

		public static PermutationByte operator +(PermutationByte o) { return o; }
		public static PermutationByte operator -(PermutationByte o) { return o.InverseElement; }
		public static PermutationByte operator +(PermutationByte l, PermutationByte r) { return l.Add(r); }
		public static PermutationByte operator -(PermutationByte l, PermutationByte r) { return l.Subtract(r); }
		public static PermutationByte operator *(PermutationByte l, int r) { return l.Times(r); }
		public static PermutationByte operator *(int l, PermutationByte r) { return r.Times(l); }

		public static implicit operator PermutationByte(string o) { return FromString(o); }
		public static implicit operator PermutationByte(byte o) { return FromByte(o); }
		public static implicit operator PermutationByte(short o) { return FromInt16(o); }
		[CLSCompliant(false)]
		public static implicit operator PermutationByte(sbyte o) { return FromByte((byte)o); }
		[CLSCompliant(false)]
		public static implicit operator PermutationByte(ushort o) { return FromInt16((short)o); }
	}
}
