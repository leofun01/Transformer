//	FlipRotate16D.cs
//	
//	Based on :
//		Math
//			Abstract algebra
//				Group theory
//					Finite group
//					Symmetry group
//					Non-commutative group (non-abelian group)
//	
//	Author   : leofun01

using System;
using System.Collections.Generic;
using System.Diagnostics;
using CultureInfo = System.Globalization.CultureInfo;
using DotNetTransformer.Extensions;
using DotNetTransformer.Math.Group;
using DotNetTransformer.Math.Permutation;
using DotNetTransformer.Math.Set;

namespace DotNetTransformer.Math.Transform {
	using T = FlipRotate16D;
	using P = PermutationInt64;

	[Serializable]
	[DebuggerDisplay("{ToString()}, CycleLength = {CycleLength}")]
	public struct FlipRotate16D : IFlipRotate<T, P>
	{
		public readonly P Permutation;
		private readonly short _vertex;
		public FlipRotate16D(P permutation, int vertex) {
			Permutation = permutation;
			_vertex = (short)vertex;
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
			long x = dimFrom ^ dimTo;
			P p = new P((x << (dimFrom << 2)) ^ (x << (dimTo << 2)));
			return new T(p, 1 << dimTo);
		}

		private static IDictionary<byte, IFiniteSet<T>> _reflections;
		private static IDictionary<byte, IFiniteGroup<T>> _rotations;
		private static IDictionary<byte, IFiniteGroup<T>> _allValues;

		public static IFiniteSet<T> GetReflections(int dimensions) {
			if(dimensions == 0) return FiniteSet<T>.Empty;
			return GetValues<IFiniteSet<T>>(
				dimensions, ref _reflections,
				dim => new ReflectionsSet<T, P>(dim, (P p, int v) => new T(p, v))
			);
		}
		public static IFiniteGroup<T> GetRotations(int dimensions) {
			if(dimensions == 0) dimensions = 1;
			return GetValues<IFiniteGroup<T>>(
				dimensions, ref _rotations,
				dim => new RotationsGroup<T, P>(dim, (P p, int v) => new T(p, v))
			);
		}
		public static IFiniteGroup<T> GetAllValues(int dimensions) {
			if(dimensions == 0) return GetRotations(1);
			return GetValues<IFiniteGroup<T>>(
				dimensions, ref _allValues,
				dim => new FlipRotateGroup<T, P>(dim, (P p, int v) => new T(p, v))
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

		private const byte _dimCount = 16;

		public bool IsReflection { get { return !IsRotation; } }
		public bool IsRotation {
			get {
				int v = _vertex;
				for(int i = 1; i < _dimCount; i <<= 1)
					v ^= v >> i;
				return ((Permutation.SwapsCount ^ v) & 1) == 0;
			}
		}

		P IFlipRotate<T, P>.Permutation { get { return Permutation; } }
		public int Vertex { get { return _vertex & 0xFFFF; } }

		public int CycleLength {
			get {
				int c = Permutation.CycleLength;
				return GroupExtension.Times<T>(this, c).Equals(None) ? c : (c << 1);
			}
		}
		public T InverseElement {
			get {
				P p = -Permutation;
				return new T(p, _vertex.GetPrev<P>(p));
			}
		}
		public T Add(T other) {
			P p = Permutation;
			return new T(p + other.Permutation,
				_vertex ^ other._vertex.GetPrev<P>(p)
			);
		}
		public T Subtract(T other) {
			P p = Permutation - other.Permutation;
			return new T(p,
				_vertex ^ other._vertex.GetPrev<P>(p)
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
		public bool Equals(T o) {
			return Permutation == o.Permutation && _vertex == o._vertex;
		}
		public override string ToString() {
			return string.Format(
				CultureInfo.InvariantCulture,
				"P:{0} V:{1:X4}", Permutation, Vertex
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

		public static implicit operator T(P o) { return new T(o, 0); }
	}
}
