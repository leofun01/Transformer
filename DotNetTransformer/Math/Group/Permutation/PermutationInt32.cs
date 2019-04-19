//	PermutationInt32.cs
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
	public struct PermutationInt32 : IFiniteGroupElement<PermutationInt32>
		, IEnumerable<byte>
	{
		private readonly int _value;
		private PermutationInt32(int value) { _value = value; }

		private const int _mix = 0x76543210;
		private const byte _count = 8, _len = 32;

		public int Value { get { return _value ^ _mix; } }
		public byte this[int index] {
			get {
				return (byte)(Value >> (index << 2) & 7);
			}
		}
		public PermutationInt32 InverseElement {
			get {
				int t = Value, r = 0, i = 0;
				do
					r |= i << ((t >> (i << 2) & 7) << 2);
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
					byte digit = i, mult = 0;
					do {
						++mult;
						digitFlag |= (byte)(1 << digit);
						digit = (byte)(t >> (digit << 2) & 7);
					} while((1 << digit & digitFlag) == 0);
					multFlag |= (byte)(1 << --mult);
				}
				if((multFlag & 0xE0) != 0) return (multFlag >> 6) + 6;
				byte r = 1;
				if((multFlag & 0x0A) != 0) r *= 2;
				if((multFlag & 0x04) != 0) r *= 3;
				if((multFlag & 0x08) != 0) r *= 2;
				if((multFlag & 0x10) != 0) r *= 5;
				return r;
			}
		}
		public PermutationInt32 Add(PermutationInt32 other) {
			int t = Value, o = other.Value, r = 0, i = 0;
			do {
				r |= (t >> ((o >> i & 7) << 2) & 7) << i;
				i += 4;
			} while(i < _len);
			return new PermutationInt32(r ^ _mix);
		}
		public PermutationInt32 Subtract(PermutationInt32 other) {
			int t = Value, o = other.Value, r = 0, i = 0;
			do {
				r |= (t >> i & 7) << ((o >> i & 7) << 2);
				i += 4;
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
			int t = Value, i = _count;
			if(minLength > 0) --minLength;
			while(--i > minLength && (t >> (i << 2) & 7) == i) ;
			if(++minLength < ++i) minLength = (byte)i;
			return _toString(minLength);
		}
		private string _toString(byte length) {
			int t = Value, i = 0;
			StringBuilder sb = new StringBuilder(length, length);
			length <<= 2;
			do {
				sb.Append(t >> i & 7);
				i += 4;
			} while(i < length);
			return sb.ToString();
		}
		public IEnumerator<byte> GetEnumerator() {
			int t = Value, i = 0;
			do {
				yield return (byte)(t >> i & 7);
				i += 4;
			} while(i < _len);
		}
		IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

		/// <exception cref="ArgumentException">
		/// Invalid <paramref name="s"/>.
		/// </exception>
		public static PermutationInt32 FromString(string s) {
			if(s.Length > _count) _throwString();
			if(string.IsNullOrEmpty(s))
				return new PermutationInt32();
			int startIndex = -1, value = 0;
			for(int digit = 0; digit < _count; ++digit) {
				int i = 0;
				do
					if((-_count & s[i]) != '0') _throwString();
				while((s[i] & 7) != digit && ++i < s.Length);
				if(i == s.Length)
					if(startIndex >= digit || s.Length > digit) _throwString();
					else return new PermutationInt32(((1 << (digit << 2)) - 1) & _mix ^ value);
				else {
					value |= digit << (i << 2);
					if(startIndex < i) startIndex = i;
				}
			}
			return new PermutationInt32(_mix ^ value);
		}
		private static void _throwString() {
			throw new ArgumentException("Bad string. Use unique digits from [0-7], like \"01234567\".");
		}
		public static PermutationInt32 FromInt32(int value) {
			if((value & -0x77777778) != 0) _throwInt32();
			int startIndex = -1;
			for(int digit = 0; digit < _count; ++digit) {
				int i = -1;
				while(++i < _count && (value >> (i << 2) & 7) != digit) ;
				if(i == _count)
					if(startIndex >= digit || (value & (-1 << (digit << 2))) != 0) _throwInt32();
					else return new PermutationInt32(((1 << (digit << 2)) - 1) & _mix ^ value);
				else if(startIndex < i) startIndex = i;
			}
			return new PermutationInt32(_mix ^ value);
		}
		private static void _throwInt32() {
			throw new ArgumentException("Bad value. Use hexadecimal format and unique digits from [0-7], like 0x76543210.");
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
	}
}
