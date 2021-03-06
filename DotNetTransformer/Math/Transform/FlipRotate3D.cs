using System;
using System.Collections.Generic;
using System.Diagnostics;
using CultureInfo = System.Globalization.CultureInfo;
using DotNetTransformer.Extensions;
using DotNetTransformer.Math.Group;
using DotNetTransformer.Math.Permutation;
using DotNetTransformer.Math.Set;

namespace DotNetTransformer.Math.Transform {
	using T = FlipRotate3D;
	using P = PermutationByte;

	[Serializable]
	[DebuggerDisplay("{ToString()}, CycleLength = {CycleLength}")]
	public struct FlipRotate3D : IFlipRotate<T, P>
	{
		private readonly byte _value;
		private FlipRotate3D(byte value) { _value = value; }
		private FlipRotate3D(byte permutation, int vertex) {
			int v = permutation & 0x33;
			v = (v >> 2 | v) & _perm;
			_value = (byte)((vertex << _s | v) & 0x7F);
		}
		public FlipRotate3D(P permutation, int vertex) {
			if(permutation[3] != 3)
				throw new ArgumentException(
					"Parameter \"permutation\" has invalid value."
				);
			this = new T(permutation._value, vertex);
		}

		public static T None { get { return new T(); } }

		public static T FlipX     { get { return new T(0x10); } }
		public static T FlipY     { get { return new T(0x20); } }
		public static T FlipZ     { get { return new T(0x40); } }
		public static T RotateXY  { get { return new T(0x21); } }
		public static T RotateYZ  { get { return new T(0x4C); } }
		public static T RotateZX  { get { return new T(0x1A); } }
		public static T RotateXYZ { get { return new T(0x0E); } }

		public static T GetFlip(int dimension) {
			if(dimension < 0 || dimension >= _dimCount)
				throw new ArgumentOutOfRangeException("dimension");
			return new T((byte)(1 << dimension << _s));
		}
		public static T GetRotate(int dimFrom, int dimTo) {
			if(dimFrom < 0 || dimFrom >= _dimCount)
				throw new ArgumentOutOfRangeException("dimFrom");
			if(dimTo < 0 || dimTo >= _dimCount)
				throw new ArgumentOutOfRangeException("dimTo");
			if(dimFrom == dimTo)
				throw new ArgumentException(
				);
			int x = dimFrom ^ dimTo;
			P p = new P((byte)((x << (dimFrom << 1)) ^ (x << (dimTo << 1))));
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

		private const byte _dimCount = 3;
		private const short _s = 4, _perm = (-1 << _s) ^ -1;

		public bool IsReflection { get { return !IsRotation; } }
		public bool IsRotation {
			get {
				int v = Vertex;
				for(int i = 1; i < _dimCount; i <<= 1)
					v ^= v >> i;
				return ((Permutation.SwapsCount ^ v) & 1) == 0;
			}
		}

		public P Permutation {
			get {
				int v = _value & _perm;
				v ^= v << 2;
				return new P((byte)v);
			}
		}
		public int Vertex { get { return _value >> _s; } }

		public int CycleLength {
			get {
				int c = Permutation.CycleLength;
				return GroupExtension.Times<T>(this, c).Equals(None) ? c : (c << 1);
			}
		}
		public T InverseElement {
			get {
				P p = -Permutation;
				return new T(p._value, Vertex.GetPrev<P>(p, _dimCount));
			}
		}
		public T Add(T other) {
			P p = Permutation;
			return new T((p + other.Permutation)._value,
				Vertex ^ other.Vertex.GetPrev<P>(p, _dimCount)
			);
		}
		public T Subtract(T other) {
			P p = Permutation - other.Permutation;
			return new T(p._value,
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
				"P:{0} V:{1:X1}", Permutation.ToString(_dimCount), Vertex
			);
		}
		public PermutationInt32 ToPermutationInt32() {
			P p = Permutation;
			int v = Vertex;
			const int b = 0x11111111;
			for(byte i = 0, l = 4; i < _dimCount; ++i, l <<= 1)
				v ^= (((-1 << l) ^ -1) & (b << p[i]) ^ v) << l;
			return PermutationInt32.FromInt32Internal(v);
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
