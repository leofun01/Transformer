using System;
using System.Collections.Generic;
using System.Diagnostics;
using CultureInfo = System.Globalization.CultureInfo;
using DotNetTransformer.Math.Group;
using DotNetTransformer.Math.Permutation;

namespace DotNetTransformer.Math.Transform {
	using T = FlipRotate4D;
	using P = PermutationByte;

	[Serializable]
	[DebuggerDisplay("{ToString()}, CycleLength = {CycleLength}")]
	public struct FlipRotate4D : IFlipRotate<T, P>
	{
		private readonly short _value;
		private FlipRotate4D(short value) { _value = value; }
		public FlipRotate4D(P permutation, int vertex) {
			_value = (short)((vertex << _s | permutation._value) & 0x0FFF);
		}

		public static T None { get { return new T(); } }

		public static T GetFlip(int dimension) {
			if((dimension & -_dimCount) != 0)
				throw new ArgumentOutOfRangeException("dimension");
			return new T((short)(1 << dimension << _s));
		}
		public static T GetRotate(int dimFrom, int dimTo) {
			if((dimFrom & -_dimCount) != 0)
				throw new ArgumentOutOfRangeException("dimFrom");
			if((dimTo & -_dimCount) != 0)
				throw new ArgumentOutOfRangeException("dimTo");
			if(dimFrom == dimTo)
				throw new ArgumentException(
				);
			int x = dimFrom ^ dimTo;
			P p = new P((byte)((x << (dimFrom << 1)) ^ (x << (dimTo << 1))));
			return new T(p, 1 << dimTo);
		}

		public static readonly FiniteGroup<T> AllValues;

		static FlipRotate4D() {
			AllValues = new InternalGroup();
		}

		private sealed class InternalGroup : FiniteGroup<T>
		{
			public InternalGroup() { }

			public override T IdentityElement { get { return None; } }
			public override int Count { get { return 384; } }
			public override bool Contains(T item) { return true; }
			public override IEnumerator<T> GetEnumerator() {
				P none = new P(), p = none;
				const byte count = 1 << _dimCount;
				do {
					for(byte i = 0; i < count; ++i)
						yield return new T(p, i);
					p = p.GetNextPermutation(_dimCount);
				} while(p != none);
			}
			public override int GetHashCode() { return Count; }
		}

		private const byte _dimCount = 4;
		private const short _s = 8, _perm = (1 << _s) - 1;

		public bool IsReflection { get { return !IsRotation; } }
		public bool IsRotation {
			get {
				int v = Vertex;
				for(int i = 1; i < _dimCount; i <<= 1)
					v ^= v >> i;
				return ((Permutation.SwapsCount ^ v) & 1) == 0;
			}
		}

		public P Permutation { get { return new P((byte)(_value & _perm)); } }
		public int Vertex { get { return _value >> _s; } }

		public int CycleLength {
			get {
				return this.GetCycleLength<T, P>();
			}
		}
		public T InverseElement {
			get {
				P p = -Permutation;
				return new T(p, p.GetNextVertex<P>(Vertex));
			}
		}
		public T Add(T other) {
			P p = Permutation;
			return new T(p + other.Permutation,
				p.GetNextVertex<P>(other.Vertex) ^ Vertex
			);
		}
		public T Subtract(T other) {
			P p = Permutation - other.Permutation;
			return new T(p,
				p.GetNextVertex<P>(other.Vertex) ^ Vertex
			);
		}
		public T Times(int count) {
			return FiniteGroupExtension.Times<T>(this, count);
		}

		public override int GetHashCode() { return _value; }
		public override bool Equals(object o) {
			return o is T && Equals((T)o);
		}
		public bool Equals(T o) { return _value == o._value; }
		public override string ToString() {
			return string.Format(
				CultureInfo.InvariantCulture,
				"P:{0} V:{1:X1}", Permutation, Vertex
			);
		}
		public PermutationInt64 ToPermutationInt64() {
			P p = Permutation;
			long v = Vertex;
			const long b = 0x1111111111111111L;
			for(byte i = 0, l = 4; i < _dimCount; ++i, l <<= 1)
				v ^= ((1L << l) - 1L & (b << p[i]) ^ v) << l;
			return new PermutationInt64(v ^ -0x123456789ABCDF0L);
		}

		///	<exception cref="ArgumentException">
		///		<exception cref="ArgumentNullException">
		///			Invalid <paramref name="s"/>.
		///		</exception>
		///	</exception>
		public static T FromString(string s) {
			if(ReferenceEquals(s, null)) throw new ArgumentNullException();
			string[] ss = s.Trim().Split(
				(" ").ToCharArray(),
				StringSplitOptions.RemoveEmptyEntries
			);
			int len = ss.GetLength(0);
			if(len != 2) throw new ArgumentException();
			Dictionary<string, string> dict = new Dictionary<string, string>();
			for(int j = 0; j < len; ++j) {
				int i = ss[j].IndexOf(':');
				dict.Add(ss[j].Substring(0, i), ss[j].Substring(i + 1));
			}
			return new T(
				P.FromString(dict["P"]),
				int.Parse(dict["V"]
					, System.Globalization.NumberStyles.HexNumber
					, CultureInfo.InvariantCulture
				)
			);
		}

		public static bool operator ==(T l, T r) { return l.Equals(r); }
		public static bool operator !=(T l, T r) { return !l.Equals(r); }

		public static T operator +(T o) { return o; }
		public static T operator -(T o) { return o.InverseElement; }
		public static T operator +(T l, T r) { return l.Add(r); }
		public static T operator -(T l, T r) { return l.Subtract(r); }
		public static T operator *(T l, int r) { return l.Times(r); }
		public static T operator *(int l, T r) { return r.Times(l); }

		public static implicit operator T(P o) { return new T(o._value); }
	}
}
