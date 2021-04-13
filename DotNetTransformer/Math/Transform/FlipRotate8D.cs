using System;
using System.Collections.Generic;
using System.Diagnostics;
using CultureInfo = System.Globalization.CultureInfo;
using DotNetTransformer.Extensions;
using DotNetTransformer.Math.Group;
using DotNetTransformer.Math.Permutation;
using DotNetTransformer.Math.Set;

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

		private static IDictionary<byte, IFiniteSet<T>> _reflections;
		private static IDictionary<byte, IFiniteGroup<T>> _rotations;
		private static IDictionary<byte, IFiniteGroup<T>> _allValues;

		public static IFiniteSet<T> GetReflections(int dimensions) {
			return GetValues<IFiniteSet<T>>(
				dimensions, ref _reflections,
				dim => new ReflectionsSet(dim)
			);
		}
		public static IFiniteGroup<T> GetRotations(int dimensions) {
			return GetValues<IFiniteGroup<T>>(
				dimensions, ref _rotations,
				dim => new RotationsGroup(dim)
			);
		}
		public static IFiniteGroup<T> GetAllValues(int dimensions) {
			return GetValues<IFiniteGroup<T>>(
				dimensions, ref _allValues,
				dim => new FlipRotateGroup(dim)
			);
		}
		private static S GetValues<S>(int dimensions,
			ref IDictionary<byte, S> collection,
			Converter<byte, S> ctor
		)
			where S : IFiniteSet<T>
		{
			if(dimensions < 0 || dimensions > _dimCount)
				throw new ArgumentOutOfRangeException(
				);
			byte dim = (byte)dimensions;
			if(ReferenceEquals(collection, null))
				collection = new SortedList<byte, S>(_dimCount + 1);
			if(collection.ContainsKey(dim))
				return collection[dim];
			else {
				S r = ctor(dim);
				collection.Add(dim, r);
				return r;
			}
		}

		private abstract class FlipRotateSet : FiniteSet<T>
		{
			protected readonly byte _dim;
			protected FlipRotateSet(byte dimensions) {
				_dim = dimensions;
			}
			protected bool IsRotational(int swaps, int vertex) {
				for(int i = 1; i < _dim; i <<= 1)
					vertex ^= vertex >> i;
				return ((swaps ^ vertex) & 1) == 0;
			}

			public override long Count {
				get {
					long c = 1L;
					for(byte i = 1; i < _dim; c *= ++i) ;
					return c << _dim;
				}
			}
			public override bool Contains(T item) {
				return (item._value & (_vert << (_dim << 2))) == 0 &&
					item.Permutation.ReducibleTo(_dim);
			}
			public override IEnumerator<T> GetEnumerator() {
				int c = 1 << _dim;
				P i = new P();
				foreach(P p in i.GetRange<P>(i, _dim))
					for(int v = 0; v < c; ++v)
						yield return new T(p, v);
			}
		}
		private class FlipRotateGroup : FlipRotateSet, IFiniteGroup<T>
		{
			public FlipRotateGroup(byte dimensions) : base(dimensions) { }

			public T IdentityElement { get { return None; } }
		}
		private sealed class ReflectionsSet : FlipRotateSet
		{
			public ReflectionsSet(byte dimensions) : base(dimensions) { }

			public override long Count {
				get {
					return base.Count >> 1;
				}
			}
			public override bool Contains(T item) {
				return base.Contains(item) && item.IsReflection;
			}
			public override IEnumerator<T> GetEnumerator() {
				int c = 1 << _dim;
				P i = new P();
				foreach(P p in i.GetRange<P>(i, _dim)) {
					int s = p.SwapsCount;
					for(int v = 0; v < c; ++v)
						if(!IsRotational(s, v))
							yield return new T(p, v);
				}
			}
		}
		private sealed class RotationsGroup : FlipRotateGroup
		{
			public RotationsGroup(byte dimensions) : base(dimensions) { }

			public override long Count {
				get {
					long c = base.Count;
					return c - (c >> 1);
				}
			}
			public override bool Contains(T item) {
				return base.Contains(item) && item.IsRotation;
			}
			public override IEnumerator<T> GetEnumerator() {
				int c = 1 << _dim;
				P i = new P();
				foreach(P p in i.GetRange<P>(i, _dim)) {
					int s = p.SwapsCount;
					for(int v = 0; v < c; ++v)
						if(IsRotational(s, v))
							yield return new T(p, v);
				}
			}
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
				return new T(p, Vertex.GetPrev<P>(p, _dimCount));
			}
		}
		public T Add(T other) {
			P p = Permutation;
			return new T(p + other.Permutation,
				Vertex ^ other.Vertex.GetPrev<P>(p, _dimCount)
			);
		}
		public T Subtract(T other) {
			P p = Permutation - other.Permutation;
			return new T(p,
				Vertex ^ other.Vertex.GetPrev<P>(p, _dimCount)
			);
		}
		public T Times(int count) {
			return FiniteGroupExtension.Times<T>(this, count);
		}

		public override int GetHashCode() {
			return Permutation.GetHashCode() ^ Vertex;
		}
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
