using System;
using System.Collections;
using System.Collections.Generic;
using StringBuilder = System.Text.StringBuilder;

namespace DotNetTransformer.Math.Group.Permutation {
	[Serializable]
	public struct PermutationInt32 : IPermutation<PermutationInt32>
	{
		private readonly int _value;
		private PermutationInt32(int value) { _value = value; }
		public PermutationInt32(params byte[] array) {
			if(ReferenceEquals(array, null))
				throw new ArgumentNullException();
			int count = array.GetLength(0);
			if(count > _count)
				_throwArray("Array length is out of range (0, 8).");
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
						_value ^= ((1 << (digit << _s)) - 1) & _mix;
						return;
					}
				}
				else {
					_value |= (int)digit << (i << _s);
					if(startIndex < i) startIndex = i;
				}
			}
			_value ^= _mix;
		}

		private const int _mix = 0x76543210, _mask = 7;
		private const byte _count = 8, _len = 32, _s = 2;

		public int Value { get { return _value ^ _mix; } }
		public int this[int index] {
			get {
				return Value >> (index << _s) & _mask;
			}
		}
		public PermutationInt32 InverseElement {
			get {
				int t = Value, r = 0;
				byte i = 0;
				do
					r |= i << ((t >> (i << _s) & _mask) << _s);
				while(++i < _count);
				return new PermutationInt32(r ^ _mix);
			}
		}
		public int CycleLength {
			get {
				int t = Value;
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
				if((multFlag & -0x20) != 0) return ((multFlag >> 6) & 3) + 6;
				int r = 1;
				if((multFlag & 0x0A) != 0) r *= 2;
				if((multFlag & 0x04) != 0) r *= 3;
				if((multFlag & 0x08) != 0) r *= 2;
				if((multFlag & 0x10) != 0) r *= 5;
				return r;
			}
		}
		public PermutationInt32 Add(PermutationInt32 other) {
			int t = Value, o = other.Value, r = 0;
			byte i = 0;
			do {
				r |= (t >> ((o >> i & _mask) << _s) & _mask) << i;
				i += 1 << _s;
			} while(i < _len);
			return new PermutationInt32(r ^ _mix);
		}
		public PermutationInt32 Subtract(PermutationInt32 other) {
			int t = Value, o = other.Value, r = 0;
			byte i = 0;
			do {
				r |= (t >> i & _mask) << ((o >> i & _mask) << _s);
				i += 1 << _s;
			} while(i < _len);
			return new PermutationInt32(r ^ _mix);
		}
		public PermutationInt32 Times(int count) {
			int c = CycleLength;
			count = (count % c + c) % c;
			PermutationInt32 t = this;
			PermutationInt32 r = (count & 1) != 0 ? t : new PermutationInt32();
			while((count >>= 1) != 0) {
				t = t.Add(t);
				if((count & 1) != 0)
					r = r.Add(t);
			}
			return r;
		}

		public List<PermutationInt32> GetCycles(Predicate<PermutationInt32> match) {
			List<PermutationInt32> list = new List<PermutationInt32>(_count);
			int t = Value;
			byte digitFlag = 0;
			for(byte i = 0; i < _count; ++i) {
				if((1 << i & digitFlag) != 0) continue;
				byte digit = i;
				int value = 0;
				do {
					value |= _mask << (digit << _s) & _value;
					digitFlag |= (byte)(1 << digit);
					digit = (byte)(t >> (digit << _s) & _mask);
				} while((1 << digit & digitFlag) == 0);
				PermutationInt32 p = new PermutationInt32(value);
				if(match(p)) list.Add(p);
			}
			return list;
		}
		public int GetCyclesCount(Predicate<int> match) {
			int t = Value;
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
			return o is PermutationInt32 && Equals((PermutationInt32)o);
		}
		public bool Equals(PermutationInt32 o) { return _value == o._value; }
		public override string ToString() {
			return _toString(_count);
		}
		public string ToString(byte minLength) {
			if(minLength > _count) minLength = _count;
			int t = Value;
			byte i = _count;
			if(minLength > 0) --minLength;
			while(--i > minLength && (t >> (i << _s) & _mask) == i) ;
			if(minLength < i) minLength = i;
			return _toString(++minLength);
		}
		private string _toString(byte length) {
			int t = Value;
			byte i = 0;
			StringBuilder sb = new StringBuilder(length, length);
			length <<= _s;
			do {
				sb.Append((char)(t >> i & _mask | '0'));
				i += 1 << _s;
			} while(i < length);
			return sb.ToString();
		}
		public IEnumerator<int> GetEnumerator() {
			int t = Value;
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
		public static PermutationInt32 FromString(string s) {
			if(ReferenceEquals(s, null)) throw new ArgumentNullException();
			if(s.Length > _count)
				_throwString("String length is out of range (0, 8).");
			if(s.Length < 1) return new PermutationInt32();
			int value = 0;
			byte startIndex = 0;
			for(byte digit = 0; digit < _count; ++digit) {
				byte i = 0;
				char c;
				do {
					c = s[i];
					if((-_count & c) != '0')
						_throwString(string.Concat("\'", c, "\' is not a digit from [0-7]."));
				} while((c & _mask) != digit && ++i < s.Length);
				if(i == s.Length)
					if(startIndex >= digit || i > digit)
						_throwString(string.Concat("Digit \'", (char)(digit | '0'), "\' is not found."));
					else return new PermutationInt32(((1 << (digit << _s)) - 1) & _mix ^ value);
				else {
					value |= (int)digit << (i << _s);
					if(startIndex < i) startIndex = i;
				}
			}
			return new PermutationInt32(_mix ^ value);
		}
		public static PermutationInt32 FromInt32(int value) {
			if((value & -0x77777778) != 0)
				_throwInt32("Some digits is out of range [0-7].");
			byte startIndex = 0;
			for(byte digit = 0; digit < _count; ++digit) {
				byte i = 0;
				while(i < _count && (value >> (i << _s) & _mask) != digit) ++i;
				if(i == _count)
					if(startIndex >= digit || (value & (-1 << (digit << _s))) != 0)
						_throwInt32(string.Concat("Digit \'", (char)(digit | '0'), "\' is not found."));
					else return new PermutationInt32(((1 << (digit << _s)) - 1) & _mix ^ value);
				else if(startIndex < i) startIndex = i;
			}
			return new PermutationInt32(_mix ^ value);
		}
		private static void _throwString(string message) {
			throw new ArgumentException(message
				+ " Use unique digits from [0-7], like \"01234567\".");
		}
		private static void _throwInt32(string message) {
			throw new ArgumentException(message
				+ " Use hexadecimal format and unique digits from [0-7], like 0x76543210.");
		}
		private static void _throwArray(string message) {
			throw new ArgumentException(message
				+ " Use unique values from range (0, 8)");
		}

		public static bool operator ==(PermutationInt32 l, PermutationInt32 r) { return l.Equals(r); }
		public static bool operator !=(PermutationInt32 l, PermutationInt32 r) { return !l.Equals(r); }

		public static PermutationInt32 operator +(PermutationInt32 o) { return o; }
		public static PermutationInt32 operator -(PermutationInt32 o) { return o.InverseElement; }
		public static PermutationInt32 operator +(PermutationInt32 l, PermutationInt32 r) { return l.Add(r); }
		public static PermutationInt32 operator -(PermutationInt32 l, PermutationInt32 r) { return l.Subtract(r); }
		public static PermutationInt32 operator *(PermutationInt32 l, int r) { return l.Times(r); }
		public static PermutationInt32 operator *(int l, PermutationInt32 r) { return r.Times(l); }

		public static implicit operator PermutationInt32(string o) { return FromString(o); }
		public static implicit operator PermutationInt32(int o) { return FromInt32(o); }
		[CLSCompliant(false)]
		public static implicit operator PermutationInt32(uint o) { return FromInt32((int)o); }
	}
}
