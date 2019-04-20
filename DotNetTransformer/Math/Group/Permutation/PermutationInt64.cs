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

		private const long _mix = -0x123456789ABCDF0L;
		private const byte _count = 16, _len = 64;

		public long Value { get { return _value ^ _mix; } }
		public byte this[int index] {
			get {
				return (byte)(Value >> (index << 2) & 0xFL);
			}
		}
		public PermutationInt64 InverseElement {
			get {
				long t = Value, r = 0L;
				byte i = 0;
				do
					r |= (long)i << (int)((t >> (i << 2) & 0xFL) << 2);
				while(++i < _count);
				return new PermutationInt64(r ^ _mix);
			}
		}
		public int CycleLength {
			get {
				throw new NotImplementedException();
			}
		}
		public PermutationInt64 Add(PermutationInt64 other) {
			long t = Value, o = other.Value, r = 0L;
			byte i = 0;
			do {
				r |= (t >> (int)((o >> i & 0xFL) << 2) & 0xFL) << i;
				i += 4;
			} while(i < _len);
			return new PermutationInt64(r ^ _mix);
		}
		public PermutationInt64 Subtract(PermutationInt64 other) {
			long t = Value, o = other.Value, r = 0L;
			byte i = 0;
			do {
				r |= (t >> i & 0xFL) << (int)((o >> i & 0xFL) << 2);
				i += 4;
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
			while(--i > minLength && (t >> (i << 2) & 0xFL) == i) ;
			if(minLength < i) minLength = i;
			return _toString(++minLength);
		}
		private string _toString(byte length) {
			long t = Value;
			byte i = 0, digit;
			StringBuilder sb = new StringBuilder(length, length);
			length <<= 2;
			do {
				digit = (byte)(t >> i & 0xFL);
				sb.Append((char)(digit + (digit < 10 ? '0' : '7')));
				i += 4;
			} while(i < length);
			return sb.ToString();
		}
		public IEnumerator<byte> GetEnumerator() {
			long t = Value;
			byte i = 0;
			do {
				yield return (byte)(t >> i & 0xFL);
				i += 4;
			} while(i < _len);
		}
		IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

		public static PermutationInt64 FromString(string s) {
			throw new NotImplementedException();
		}
		private static void _throwString() {
			throw new ArgumentException("Bad string. Use unique digits from [0-9A-Fa-f], like \"0123456789ABCDEF\".");
		}
		public static PermutationInt64 FromInt64(long value) {
			sbyte startIndex = -1;
			for(byte digit = 0; digit < _count; ++digit) {
				sbyte i = -1;
				while(++i < _count && (value >> (i << 2) & 0xFL) != digit) ;
				if(i == _count)
					if(startIndex >= digit || (value & (-1L << (digit << 2))) != 0L) _throwInt64();
					else return new PermutationInt64(((1L << (digit << 2)) - 1L) & _mix ^ value);
				else if(startIndex < i) startIndex = i;
			}
			return new PermutationInt64(_mix ^ value);
		}
		private static void _throwInt64() {
			throw new ArgumentException("Bad value. Use hexadecimal format and unique digits from [0-9A-Fa-f], like -0x123456789ABCDF0L.");
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
		public static implicit operator PermutationInt64(ulong o) { return FromInt64((long)o); }
	}
}
