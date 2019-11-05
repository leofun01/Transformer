using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using StringBuilder = System.Text.StringBuilder;
using CultureInfo = System.Globalization.CultureInfo;
using DotNetTransformer.Math.Group;

namespace DotNetTransformer.Math.Permutation {
	using P = PermutationByte;

	[Serializable]
	[DebuggerDisplay("{ToString()}, CycleLength = {CycleLength}")]
	public struct PermutationByte : IPermutation<P>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		internal readonly byte _value;
		internal PermutationByte(byte value) { _value = value; }
		internal PermutationByte(short value) {
			value = (short)(((short)(value >> 2) | value) & 0x0F0F);
			_value = (byte)((short)(value >> 4) | value);
		}
		public PermutationByte(params byte[] array) : this((IEnumerable<byte>)array) { }
		public PermutationByte(IEnumerable<byte> collection) {
			if(ReferenceEquals(collection, null))
				throw new ArgumentNullException();
			IEnumerator<byte> e = collection.GetEnumerator();
			byte count = 0, digit;
			_value = 0;
			byte digitFlag = 0;
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
				digitFlag |= (byte)(1 << digit);
				_value |= (byte)((digit ^ count) << (count << _s));
				++count;
			}
			digit = 0;
			while(((byte)(1 << digit) & digitFlag) != 0)
				++digit;
			if(((byte)((1 << digit) - 1 ^ -1) & digitFlag) != 0)
				_throwArray(string.Format(
					"Value \"{0}\" is not found.",
					digit
				));
		}

		private const byte _mix = 0xE4, _mask = 3;
		private const byte _s = 1, _count = 4, _len = _count << _s;
		private const string _charPattern = "[0-3]";

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public byte Value { get { return (byte)(_value ^ _mix); } }
		public int this[int index] {
			get {
				return Value >> (index << _s) & _mask;
			}
		}
		public int CycleLength {
			get {
				byte multFlag = 0;
				byte t = Value;
				byte digitFlag = 0;
				for(byte i = 0; i < _count; ++i) {
					if((1 << i & digitFlag) != 0) continue;
					byte digit = i;
					byte cLen = 0;
					do {
						++cLen;
						digitFlag |= (byte)(1 << digit);
						digit = (byte)(t >> (digit << _s) & _mask);
					} while((1 << digit & digitFlag) == 0);
					multFlag |= (byte)(1 << --cLen);
				}
				return (multFlag >> 1) + 1 - (multFlag >> 3);
			}
		}
		public P InverseElement {
			get {
				byte t = Value, r = 0;
				byte i = 0;
				do
					r |= (byte)(i << ((t >> (i << _s) & _mask) << _s));
				while(++i < _count);
				return new P((byte)(r ^ _mix));
			}
		}
		public P Add(P other) {
			byte t = Value, o = other.Value, r = 0;
			byte i = 0;
			do {
				r |= (byte)((t >> ((o >> i & _mask) << _s) & _mask) << i);
				i += 1 << _s;
			} while(i < _len);
			return new P((byte)(r ^ _mix));
		}
		public P Subtract(P other) {
			byte t = Value, o = other.Value, r = 0;
			byte i = 0;
			do {
				r |= (byte)((t >> i & _mask) << ((o >> i & _mask) << _s));
				i += 1 << _s;
			} while(i < _len);
			return new P((byte)(r ^ _mix));
		}
		public P Times(int count) {
			return this.Times<P>(count);
		}

		public P GetNextPermutation() {
		}
		public P GetPreviousPermutation() {
		}

		public List<P> GetCycles(Predicate<P> match) {
			List<P> list = new List<P>(_count);
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
				P p = new P(value);
				if(match(p)) list.Add(p);
			}
			return list;
		}
		public int GetCyclesCount(Predicate<int> match) {
			int count = 0;
			byte t = Value;
			byte digitFlag = 0;
			for(byte i = 0; i < _count; ++i) {
				if((1 << i & digitFlag) != 0) continue;
				byte digit = i;
				byte cLen = 0;
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
			return o is P && Equals((P)o);
		}
		public bool Equals(P o) { return _value == o._value; }
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
			StringBuilder sb = new StringBuilder(length, length);
			length <<= _s;
			byte t = Value;
			byte i = 0;
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
				yield return t >> i & _mask;
				i += 1 << _s;
			} while(i < _len);
		}
		IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }

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
			byte value = 0;
			byte startIndex = 0;
			for(byte digit = 0; digit < _count; ++digit) {
				byte i = 0;
				char c;
				do {
					c = s[i];
					if((-_count & c) != '0')
						_throwString(string.Format(
							"\'{0}\' is not a digit from {1}.",
							c, _charPattern
						));
				} while((c & _mask) != digit && ++i < s.Length);
				if(i == s.Length)
					if(startIndex >= digit || i > digit)
						_throwString(string.Format(
							"Digit \'{0}\' is not found.",
							(char)(digit | '0')
						));
					else return new P((byte)(((1 << (digit << _s)) - 1) & _mix ^ value));
				else {
					value |= (byte)(digit << (i << _s));
					if(startIndex < i) startIndex = i;
				}
			}
			return new P((byte)(_mix ^ value));
		}
		public static P FromByte(byte value) {
			byte startIndex = 0;
			for(byte digit = 0; digit < _count; ++digit) {
				byte i = 0;
				while(i < _count && (value >> (i << _s) & _mask) != digit) ++i;
				if(i == _count)
					if(startIndex >= digit || (value & (-1 << (digit << _s))) != 0)
						_throwByte(string.Format(
							"Digit \'{0}\' is not found.",
							(char)(digit | '0')
						));
					else return new P((byte)(((1 << (digit << _s)) - 1) & _mix ^ value));
				else if(startIndex < i) startIndex = i;
			}
			return new P((byte)(_mix ^ value));
		}
		public static P FromInt16(short value) {
			if((value & -0x3334) != 0)
				_throwInt16(string.Format(
					"Some digits is out of {0}.",
					_charPattern
				));
			byte startIndex = 0;
			for(byte digit = 0; digit < _count; ++digit) {
				byte i = 0;
				while(i < _count && (value >> (i << 2) & _mask) != digit) ++i;
				if(i == _count)
					if(startIndex >= digit || (value & (-1 << (digit << 2))) != 0)
						_throwInt16(string.Format(
							"Digit \'{0}\' is not found.",
							(char)(digit | '0')
						));
					else return new P((short)(((1 << (digit << 2)) - 1) & 0x3210 ^ value));
				else if(startIndex < i) startIndex = i;
			}
			return new P((short)(0x3210 ^ value));
		}
		[DebuggerStepThrough]
		private static void _throwString(string message) {
			throw new ArgumentException(string.Concat(message,
				" Use unique digits from ",
				_charPattern,
				". Example: \"0123\"."
			));
		}
		[DebuggerStepThrough]
		private static void _throwByte(string message) {
			throw new ArgumentException(string.Concat(message,
				" Use compressed data format and unique digits from ",
				_charPattern,
				", Example: 0xE4 or 0b_11_10_01_00."
			));
		}
		[DebuggerStepThrough]
		private static void _throwInt16(string message) {
			throw new ArgumentException(string.Concat(message,
				" Use hexadecimal format and unique digits from ",
				_charPattern,
				". Example: 0x3210."
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
		public static implicit operator P(byte o) { return FromByte(o); }
		public static implicit operator P(short o) { return FromInt16(o); }
		[CLSCompliant(false)]
		public static implicit operator P(sbyte o) { return FromByte((byte)o); }
		[CLSCompliant(false)]
		public static implicit operator P(ushort o) { return FromInt16((short)o); }
	}
}
