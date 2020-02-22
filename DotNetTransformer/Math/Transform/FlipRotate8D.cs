using System;
using System.Collections.Generic;
using System.Diagnostics;
using CultureInfo = System.Globalization.CultureInfo;
using DotNetTransformer.Extensions;
using DotNetTransformer.Math.Group;
using DotNetTransformer.Math.Permutation;

namespace DotNetTransformer.Math.Transform {
	using T = FlipRotate8D;
	using P = PermutationInt32;

	[Serializable]
	[DebuggerDisplay("{ToString()}, CycleLength = {CycleLength}")]
	public struct FlipRotate8D : IFlipRotate<T, P>
	{
		private readonly int _value;
		private FlipRotate8D(int value) { _value = value; }
		public FlipRotate8D(P permutation, int vertex) {
			vertex &= 0xFF;
			vertex |= vertex << 0x09;
			vertex |= vertex << 0x12;
			_value = vertex & _vert | permutation._value;
		}

		public static T None { get { return new T(); } }

		public static T GetFlip(int dimension) {
			if((dimension & -_dimCount) != 0)
				throw new ArgumentOutOfRangeException("dimension");
			return new T(new P(), 1 << dimension);
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
			P p = new P((x << (dimFrom << 2)) ^ (x << (dimTo << 2)));
			return new T(p, 1 << dimTo);
		}

		public static IEnumerable<T> GetReflections(int dimensions) {
			if(dimensions < 0 || dimensions > _dimCount)
				throw new ArgumentOutOfRangeException(
				);
			return FlipRotateExtension.GetValues<T, P>(
				dimensions, (p, v) => new T(p, v),
				p => p.SwapsCount & 1 ^ 1, v => v + 2
			);
		}
		public static FiniteGroup<T> GetRotations(int dimensions) {
			return GetValues(dimensions,
				c => dimensions > 0 ? c >> 1 : c,
				p => p.SwapsCount & 1, v => v + 2
			);
		}
		public static FiniteGroup<T> GetValues(int dimensions) {
			return GetValues(dimensions, c => c, _ => 0, v => v + 1);
		}
		private static FiniteGroup<T> GetValues(int dimensions,
			Generator<int> count,
			Converter<P, int> startVertex,
			Generator<int> nextVertex
		) {
			if(dimensions < 0 || dimensions > _dimCount)
				throw new ArgumentOutOfRangeException(
				);
			int f = 1, i = 1;
			while(i < dimensions) f *= ++i;
			return new InternalGroup(
				FlipRotateExtension.GetValues<T, P>(
					dimensions, (p, v) => new T(p, v),
					startVertex, nextVertex
				),
				count(f << dimensions)
			);
		}

		private sealed class InternalGroup : FiniteGroup<T>
		{
			private IEnumerable<T> _collection;
			private int _count;

			public InternalGroup(IEnumerable<T> collection, int count) {
				_collection = collection;
				_count = count;
			}

			public override T IdentityElement { get { return None; } }
			public override long Count { get { return _count; } }
			public override IEnumerator<T> GetEnumerator() {
				return _collection.GetEnumerator();
			}
			public override int GetHashCode() { return _count; }
		}

		private const byte _dimCount = 8;
		private const int _perm = 0x77777777, _vert = _perm ^ -1;

		public bool IsReflection { get { return !IsRotation; } }
		public bool IsRotation {
			get {
				int v = _value >> 3;
				for(int i = 1; i < _dimCount; i <<= 1)
					v ^= v >> (i << 2);
				return ((Permutation.SwapsCount ^ v) & 1) == 0;
			}
		}

		public P Permutation { get { return new P(_value & _perm); } }
		public int Vertex {
			get {
				int v = _value;
				v = (v >> 0x12 & 0x2222) | (v & 0x8888);
				return (v >> 0x09 | v) & 0xFF;
			}
		}

		public int CycleLength {
			get {
				int c = Permutation.CycleLength;
				return GroupExtension.Times<T>(this, c).Equals(None) ? c : (c << 1);
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
				"P:{0} V:{1:X2}", Permutation, Vertex
			);
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
