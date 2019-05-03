using System;
using System.Collections;
using System.Collections.Generic;
using StringBuilder = System.Text.StringBuilder;

namespace DotNetTransformer.Math.Group.Permutation {
	[Serializable]
	public struct PermutationInt64 : IPermutation<PermutationInt64>
	{
		private readonly long _value;
		private PermutationInt64(long value) { _value = value; }

		private const long _mix = -0x123456789ABCDF0L, _mask = 0xFL;
		private const byte _count = 16, _len = 64, _s = 2;

		public long Value { get { return _value ^ _mix; } }
		public int this[int index] {
			get {
				return (int)(Value >> (index << _s) & _mask);
			}
		}
		public PermutationInt64 InverseElement {
			get {
				long t = Value, r = 0L;
				byte i = 0;
				do
					r |= (long)i << (int)((t >> (i << _s) & _mask) << _s);
				while(++i < _count);
				return new PermutationInt64(r ^ _mix);
			}
		}
		public int CycleLength {
			get {
				long t = Value;
				short digitFlag = 0, multFlag = 0;
				for(byte i = 0; i < _count; ++i) {
					if((1 << i & digitFlag) != 0) continue;
					byte digit = i, mult = 0;
					do {
						++mult;
						digitFlag |= (short)(1 << digit);
						digit = (byte)(t >> (digit << _s) & _mask);
					} while((1 << digit & digitFlag) == 0);
					multFlag |= (short)(1 << --mult);
				}
				if(multFlag == 1) return 1;
				if((multFlag & -0x2000) != 0) return ((multFlag >> 14) & 3) + 14;
				int r = 1;
				if((multFlag & 0x0AAA) != 0) r *= 2;
				if((multFlag & 0x0924) != 0) r *= 3;
				if((multFlag & 0x0888) != 0) r *= 2;
				if((multFlag & 0x0210) != 0) r *= 5;
				if((multFlag & 0x0040) != 0) r *= 7;
				if((multFlag & 0x0080) != 0) r *= 2;
				if((multFlag & 0x0100) != 0) r *= 3;
				if((multFlag & 0x0400) != 0) r *= 11;
				if((multFlag & 0x1000) != 0) r *= 13;
				return r;
			}
		}
		public PermutationInt64 Add(PermutationInt64 other) {
			long t = Value, o = other.Value, r = 0L;
			byte i = 0;
			do {
				r |= (t >> (int)((o >> i & _mask) << _s) & _mask) << i;
				i += 1 << _s;
			} while(i < _len);
			return new PermutationInt64(r ^ _mix);
		}
		public PermutationInt64 Subtract(PermutationInt64 other) {
			long t = Value, o = other.Value, r = 0L;
			byte i = 0;
			do {
				r |= (t >> i & _mask) << (int)((o >> i & _mask) << _s);
				i += 1 << _s;
			} while(i < _len);
			return new PermutationInt64(r ^ _mix);
		}
		public PermutationInt64 Times(int count) {
			int c = CycleLength;
			count = (count % c + c) % c;
			PermutationInt64 t = this;
			PermutationInt64 r = (count & 1) != 0 ? t : new PermutationInt64();
			while((count >>= 1) != 0) {
				t = t.Add(t);
				if((count & 1) != 0)
					r = r.Add(t);
			}
			return r;
		}

		public override int GetHashCode() { return (int)(_value >> 32 ^ _value); }
		public override bool Equals(object o) {
			return o is PermutationInt64 && Equals((PermutationInt64)o);
		}
		public bool Equals(PermutationInt64 o) { return _value == o._value; }
		public override string ToString() {
			return _toString(_count);
		}
		public string ToString(byte minLength) {
			if(minLength > _count) minLength = _count;
			long t = Value;
			byte i = _count;
			if(minLength > 0) --minLength;
			while(--i > minLength && (t >> (i << _s) & _mask) == i) ;
			if(minLength < i) minLength = i;
			return _toString(++minLength);
		}
		private string _toString(byte length) {
			long t = Value;
			byte i = 0, digit;
			StringBuilder sb = new StringBuilder(length, length);
			length <<= _s;
			do {
				digit = (byte)(t >> i & _mask);
				sb.Append((char)(digit + (digit < 10 ? '0' : '7')));
				i += 1 << _s;
			} while(i < length);
			return sb.ToString();
		}
		public IEnumerator<int> GetEnumerator() {
			long t = Value;
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
		public static PermutationInt64 FromString(string s) {
			if(ReferenceEquals(s, null)) throw new ArgumentNullException();
			if(s.Length > _count)
				_throwString("String length is out of range (0, 16).");
			if(s.Length < 1) return new PermutationInt64();
			long value = 0L;
			byte startIndex = 0;
			for(byte digit = 0; digit < _count; ++digit) {
				byte i = 0;
				char c;
				do {
					c = s[i];
					if(c >= 'a' && c <= 'f') c &= '\xFFDF';
					if(c < '0' || c > 'F' || (c > '9' && c < 'A'))
						_throwString(string.Concat("\'", c, "\' is not a digit from [0-9A-Fa-f]."));
					if(c >= 'A') c -= '\x0007';
				} while((c & _mask) != digit && ++i < s.Length);
				if(i == s.Length)
					if(startIndex >= digit || s.Length > digit)
						_throwString(string.Concat("Digit \'", (char)(digit | '0'), "\' is not found."));
					else return new PermutationInt64(((1L << (digit << _s)) - 1L) & _mix ^ value);
				else {
					value |= (long)digit << (i << _s);
					if(startIndex < i) startIndex = i;
				}
			}
			return new PermutationInt64(_mix ^ value);
		}
		public static PermutationInt64 FromInt64(long value) {
			byte startIndex = 0;
			for(byte digit = 0; digit < _count; ++digit) {
				byte i = 0;
				while(i < _count && (value >> (i << _s) & _mask) != digit) ++i;
				if(i == _count)
					if(startIndex >= digit || (value & (-1L << (digit << _s))) != 0L)
						_throwInt64(string.Concat("Digit \'", (char)(digit | '0'), "\' is not found."));
					else return new PermutationInt64(((1L << (digit << _s)) - 1L) & _mix ^ value);
				else if(startIndex < i) startIndex = i;
			}
			return new PermutationInt64(_mix ^ value);
		}
		private static void _throwString(string message) {
			throw new ArgumentException(message
				+ " Use unique digits from [0-9A-Fa-f], like \"0123456789ABCDEF\".");
		}
		private static void _throwInt64(string message) {
			throw new ArgumentException(message
				+ " Use hexadecimal format and unique digits from [0-9A-Fa-f], like -0x123456789ABCDF0L.");
		}

		public static bool operator ==(PermutationInt64 l, PermutationInt64 r) { return l.Equals(r); }
		public static bool operator !=(PermutationInt64 l, PermutationInt64 r) { return !l.Equals(r); }

		public static PermutationInt64 operator +(PermutationInt64 o) { return o; }
		public static PermutationInt64 operator -(PermutationInt64 o) { return o.InverseElement; }
		public static PermutationInt64 operator +(PermutationInt64 l, PermutationInt64 r) { return l.Add(r); }
		public static PermutationInt64 operator -(PermutationInt64 l, PermutationInt64 r) { return l.Subtract(r); }
		public static PermutationInt64 operator *(PermutationInt64 l, int r) { return l.Times(r); }
		public static PermutationInt64 operator *(int l, PermutationInt64 r) { return r.Times(l); }

		public static implicit operator PermutationInt64(string o) { return FromString(o); }
		public static implicit operator PermutationInt64(long o) { return FromInt64(o); }
		[CLSCompliant(false)]
		public static implicit operator PermutationInt64(ulong o) { return FromInt64((long)o); }
	}
}
