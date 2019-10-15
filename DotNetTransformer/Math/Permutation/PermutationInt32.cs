using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using StringBuilder = System.Text.StringBuilder;
using CultureInfo = System.Globalization.CultureInfo;
using DotNetTransformer.Math.Group;

namespace DotNetTransformer.Math.Permutation {
	[Serializable]
	[DebuggerDisplay("{ToString()}, CycleLength = {CycleLength}")]
	public struct PermutationInt32 : IPermutation<PermutationInt32>
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private readonly int _value;
		private PermutationInt32(int value) { _value = value; }
		public PermutationInt32(params byte[] array) : this((IEnumerable<byte>)array) { }
		public PermutationInt32(IEnumerable<byte> collection) {
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
				_value |= (int)(digit ^ count) << (count << _s);
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

		private const int _mix = 0x76543210, _mask = 7;
		private const byte _count = 8, _len = 32, _s = 2;
		private const string _charPattern = "[0-7]";

		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public int Value { get { return _value ^ _mix; } }
		public int this[int index] {
			get {
				return Value >> (index << _s) & _mask;
			}
		}
		public int CycleLength {
			get {
				byte multFlag = 0;
				int t = Value;
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
				if(multFlag == 1) return 1;
				if((multFlag & -0x20) != 0) return (multFlag >> 6 & 3) + 6;
				int r = 1;
				if((multFlag & 0x0A) != 0) r *= 2;
				if((multFlag & 0x04) != 0) r *= 3;
				if((multFlag & 0x08) != 0) r *= 2;
				if((multFlag & 0x10) != 0) r *= 5;
				return r;
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
			return this.Times<PermutationInt32>(count);
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
			int count = 0;
			int t = Value;
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
			StringBuilder sb = new StringBuilder(length, length);
			length <<= _s;
			int t = Value;
			byte i = 0;
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
		public static PermutationInt32 FromString(string s) {
			if(ReferenceEquals(s, null)) throw new ArgumentNullException();
			if(s.Length > _count)
				_throwString(string.Format(
					"String length ({2}) is out of range ({0}, {1}).",
					0, _count + 1, s.Length
				));
			if(s.Length < 1) return new PermutationInt32();
			int value = 0;
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
				_throwInt32(string.Format(
					"Some digits is out of {0}.",
					_charPattern
				));
			byte startIndex = 0;
			for(byte digit = 0; digit < _count; ++digit) {
				byte i = 0;
				while(i < _count && (value >> (i << _s) & _mask) != digit) ++i;
				if(i == _count)
					if(startIndex >= digit || (value & (-1 << (digit << _s))) != 0)
						_throwInt32(string.Format(
							"Digit \'{0}\' is not found.",
							(char)(digit | '0')
						));
					else return new PermutationInt32(((1 << (digit << _s)) - 1) & _mix ^ value);
				else if(startIndex < i) startIndex = i;
			}
			return new PermutationInt32(_mix ^ value);
		}
		[DebuggerStepThrough]
		private static void _throwString(string message) {
			throw new ArgumentException(string.Concat(message,
				" Use unique digits from ",
				_charPattern,
				". Example: \"01234567\"."
			));
		}
		[DebuggerStepThrough]
		private static void _throwInt32(string message) {
			throw new ArgumentException(string.Concat(message,
				" Use hexadecimal format and unique digits from ",
				_charPattern,
				". Example: 0x76543210."
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
