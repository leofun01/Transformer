//	PermutationInt64.cs
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
using System.Diagnostics;
using StringBuilder = System.Text.StringBuilder;
using CultureInfo = System.Globalization.CultureInfo;
using DotNetTransformer.Extensions;
using DotNetTransformer.Math.Group;
using DotNetTransformer.Math.Set;

namespace DotNetTransformer.Math.Permutation {
	using P = PermutationInt64;

	[Serializable]
	[DebuggerDisplay("{ToString()}, CycleLength = {CycleLength}")]
	public struct PermutationInt64 : IPermutation<P>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly long _value;
		internal PermutationInt64(long value) { _value = value; }
		public PermutationInt64(params byte[] array) : this((IEnumerable<byte>)array) { }
		public PermutationInt64(IEnumerable<byte> collection) {
			if(ReferenceEquals(collection, null))
				throw new ArgumentNullException();
			IEnumerator<byte> e = collection.GetEnumerator();
			byte count = 0, digit;
			_value = 0L;
			short digitFlag = 0;
			while(e.MoveNext()) {
				if(count >= _count)
					_throwArray(string.Format(
						"Collection size ({2}) is out of range ({0}, {1}).",
						0, _count + 1, count
					));
				digit = e.Current;
				if(digit >= _count)
					_throwArray(string.Format(
						"Value \"{2}\" is out of range ({0}, {1}).",
						0, _count, digit
					));
				if((1 << digit & digitFlag) != 0)
					_throwArray(string.Format(
						"Value \"{0}\" is duplicated.",
						digit
					));
				digitFlag |= (short)(1 << digit);
				_value |= (long)(digit ^ count) << (count << _s);
				++count;
			}
			digit = 0;
			while(((short)(1 << digit) & digitFlag) != 0)
				++digit;
			if(((short)(-1 << digit) & digitFlag) != 0)
				_throwArray(string.Format(
					"Value \"{0}\" is not found.",
					digit
				));
		}

		private const long _mix = -0x123456789ABCDF0L, _mask = 0xFL;
		private const byte _s = 2, _count = 16, _len = _count << _s;
		private const string _charPattern = "[0-9A-Fa-f]";

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public long Value { get { return _value ^ _mix; } }
		public int this[int index] {
			get {
				if(index < 0 || index >= _count) return index;
				return (int)(Value >> (index << _s) & _mask);
			}
		}
		public int SwapsCount {
			get {
				int count = 0;
				ForAllCyclesDo(_ => { }, c => { count += --c; });
				return count;
			}
		}
		public int CycleLength {
			get {
				int multFlag = 0;
				ForAllCyclesDo(_ => { }, c => { multFlag |= 1 << --c; });
				if(multFlag == 1) return 1;
				if((multFlag & -0x2000) != 0) return (multFlag >> 14 & 3) + 14;
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
		public P InverseElement {
			get {
				long t = Value, r = 0L;
				byte i = 0;
				do
					r |= (long)i << ((int)(t >> (i << _s) & _mask) << _s);
				while(++i < _count);
				return new P(r ^ _mix);
			}
		}
		public P Add(P other) {
			long t = Value, o = other.Value, r = 0L;
			byte i = 0;
			do {
				r |= (t >> (int)((o >> i & _mask) << _s) & _mask) << i;
				i += 1 << _s;
			} while(i < _len);
			return new P(r ^ _mix);
		}
		public P Subtract(P other) {
			long t = Value, o = other.Value, r = 0L;
			byte i = 0;
			do {
				r |= (t >> i & _mask) << (int)((o >> i & _mask) << _s);
				i += 1 << _s;
			} while(i < _len);
			return new P(r ^ _mix);
		}
		public P Times(int count) {
			return FiniteGroupExtension.Times<P>(this, count);
		}

		public bool ReducibleTo(int length) {
			return (_value & (-1L << (length << _s))) == 0L;
		}

		public P GetNextPermutation(int maxLength, Order<int> match) {
			int[] a = ToArray();
			a.ApplyNextPermutation<int>(maxLength, match);
			long r = 0;
			for(int i = 0; i < _count; ++i)
				r |= (long)a[i] << (i << _s);
			return new P(r ^ _mix);
		}
		public P GetNextPermutation(int maxLength) {
			return GetNextPermutation(maxLength, (int l, int r) => l >= r);
		}
		public P GetPreviousPermutation(int maxLength) {
			return GetNextPermutation(maxLength, (int l, int r) => l <= r);
		}

		public void ForAllCyclesDo(Action<byte> digitAction, Action<byte> cycleAction) {
			long t = Value;
			short digitFlag = 0;
			for(byte i = 0; i < _count; ++i) {
				if((1 << i & digitFlag) != 0) continue;
				byte digit = i;
				byte cLen = 0;
				do {
					digitAction(digit);
					++cLen;
					digitFlag |= (short)(1 << digit);
					digit = (byte)(t >> (digit << _s) & _mask);
				} while((1 << digit & digitFlag) == 0);
				cycleAction(cLen);
			}
		}
		public IFiniteSet<P> GetCycles(Predicate<P> match) {
			List<P> list = new List<P>(_count);
			long value = 0, t = this._value;
			ForAllCyclesDo(
				d => { value |= _mask << (d << _s) & t; },
				_ => {
					P p = new P(value);
					if(match(p)) list.Add(p);
					value = 0;
				}
			);
			return list.ToFiniteSet<P>();
		}
		public int GetCyclesCount(Predicate<int> match) {
			int count = 0;
			ForAllCyclesDo(_ => { }, c => { if(match(c)) ++count; });
			return count;
		}

		public override int GetHashCode() { return (int)(_value >> 32 ^ _value); }
		public override bool Equals(object o) {
			return o is P && Equals((P)o);
		}
		public bool Equals(P o) { return _value == o._value; }
		public override string ToString() {
			return _toString(_count);
		}
		public string ToString(int minLength) {
			if(minLength > _count) minLength = _count;
			long t = Value;
			byte i = _count;
			if(minLength > 0) --minLength;
			while(--i > minLength && (t >> (i << _s) & _mask) == i) ;
			if(minLength < i) minLength = i;
			return _toString(++minLength);
		}
		private string _toString(int length) {
			StringBuilder sb = new StringBuilder(length, length);
			length <<= _s;
			long t = Value;
			byte i = 0, digit;
			do {
				digit = (byte)(t >> i & _mask);
				sb.Append((char)((digit < 10 ? '0' : '7') + digit));
				i += 1 << _s;
			} while(i < length);
			return sb.ToString();
		}
		public IEnumerator<int> GetEnumerator() {
			long t = Value;
			byte i = 0;
			do {
				yield return (int)(t >> i & _mask);
				i += 1 << _s;
			} while(i < _len);
		}
		IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
		public int[] ToArray() {
			long v = Value;
			int[] a = new int[_count];
			int i = 0;
			do {
				a[i] = (int)(v & _mask);
				v >>= 1 << _s;
			} while(++i < _count);
			return a;
		}

		///	<exception cref="ArgumentException">
		///		<exception cref="ArgumentNullException">
		///			Invalid <paramref name="s"/>.
		///		</exception>
		///	</exception>
		public static P FromString(string s) {
			if(ReferenceEquals(s, null)) throw new ArgumentNullException();
			if(s.Length > _count)
				_throwString(string.Format(
					"String length ({2}) is out of range ({0}, {1}).",
					0, _count + 1, s.Length
				));
			if(s.Length < 1) return new P();
			long value = 0L;
			byte startIndex = 0;
			for(byte digit = 0; digit < _count; ++digit) {
				byte i = 0;
				char c;
				do {
					c = s[i];
					if(c >= 'a' && c <= 'f') c &= '\xFFDF';
					if(c < '0' || c > 'F' || (c > '9' && c < 'A'))
						_throwString(string.Format(
							"\'{0}\' is not a digit from {1}.",
							c, _charPattern
						));
					if(c >= 'A') c -= '\x0007';
				} while((c & _mask) != digit && ++i < s.Length);
				if(i == s.Length)
					if(startIndex >= digit || i > digit)
						_throwString(string.Format(
							"Digit \'{0}\' is not found.",
							(char)((digit < 10 ? '0' : '7') + digit)
						));
					else return new P(((1L << (digit << _s)) - 1L) & _mix ^ value);
				else {
					value |= (long)digit << (i << _s);
					if(startIndex < i) startIndex = i;
				}
			}
			return new P(_mix ^ value);
		}
		internal static P FromInt64Internal(long value) {
			return new P(value ^ _mix);
		}
		public static P FromInt64(long value) {
			byte startIndex = 0;
			for(byte digit = 0; digit < _count; ++digit) {
				byte i = 0;
				while(i < _count && (value >> (i << _s) & _mask) != digit) ++i;
				if(i == _count)
					if(startIndex >= digit || (value & (-1L << (digit << _s))) != 0L)
						_throwInt64(string.Format(
							"Digit \'{0}\' is not found.",
							(char)((digit < 10 ? '0' : '7') + digit)
						));
					else return new P(((1L << (digit << _s)) - 1L) & _mix ^ value);
				else if(startIndex < i) startIndex = i;
			}
			return new P(_mix ^ value);
		}
		[DebuggerStepThrough]
		private static void _throwString(string message) {
			throw new ArgumentException(string.Concat(message,
				" Use unique digits from ",
				_charPattern,
				". Example: \"0123456789ABCDEF\"."
			));
		}
		[DebuggerStepThrough]
		private static void _throwInt64(string message) {
			throw new ArgumentException(string.Concat(message,
				" Use hexadecimal format and unique digits from ",
				_charPattern,
				". Example: -0x123456789ABCDF0L."
			));
		}
		[DebuggerStepThrough]
		private static void _throwArray(string message) {
			StringBuilder sb = new StringBuilder(message);
			sb.AppendFormat(
				" Use unique values from range ({0}, {1}).",
				0, _count
			);
			sb.Append(" Example: {");
			byte i = 0;
			sb.AppendFormat(
				CultureInfo.InvariantCulture,
				" {0}", i
			);
			while(++i < _count)
				sb.AppendFormat(
					CultureInfo.InvariantCulture,
					", {0}", i
				);
			sb.Append(" }.");
			throw new ArgumentException(sb.ToString());
		}

		public static bool operator ==(P l, P r) { return l.Equals(r); }
		public static bool operator !=(P l, P r) { return !l.Equals(r); }

		public static P operator +(P o) { return o; }
		public static P operator -(P o) { return o.InverseElement; }
		public static P operator +(P l, P r) { return l.Add(r); }
		public static P operator -(P l, P r) { return l.Subtract(r); }
		public static P operator *(P l, int r) { return l.Times(r); }
		public static P operator *(int l, P r) { return r.Times(l); }

		public static implicit operator P(string o) { return FromString(o); }
		public static implicit operator P(long o) { return FromInt64(o); }
		[CLSCompliant(false)]
		public static implicit operator P(ulong o) { return FromInt64((long)o); }
	}
}
