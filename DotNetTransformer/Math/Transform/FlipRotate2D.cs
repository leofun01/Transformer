//	FlipRotate2D.cs
//	
//	Based on :
//		Math
//			Abstract algebra
//				Group theory
//					Discrete group
//					Finite group
//					Dihedral group { D_4 D_8 ?, C_2, C_4, SO(2) }
//					Symmetry group
//					Isometry group
//					Orthogonal group
//					Non-commutative group (non-abelian group)
//	
//	Author   : leofun01

using System;
using System.Collections.Generic;
using System.Diagnostics;
using StringBuilder = System.Text.StringBuilder;
using RotateFlipType = System.Drawing.RotateFlipType;
using DotNetTransformer.Extensions;
using DotNetTransformer.Math.Group;
using DotNetTransformer.Math.Permutation;
using DotNetTransformer.Math.Set;

namespace DotNetTransformer.Math.Transform {
	using T = FlipRotate2D;
	using P = PermutationByte;

	[Serializable]
	[DebuggerDisplay("{ToString()}, CycleLength = {CycleLength}")]
	public struct FlipRotate2D : IFlipRotate<T, P>
	{
		public readonly byte Value;
		private FlipRotate2D(byte value) { Value = value; }
		private FlipRotate2D(int value) { Value = (byte)value; }
		private FlipRotate2D(byte permutation, int vertex) {
			Value = (byte)(0x65471320 >> ((vertex ^ permutation) << 2) & 7);
		}
		public FlipRotate2D(P permutation, int vertex) {
			if((permutation._value & -6) != 0)
				throw new ArgumentException(
					"Parameter \"permutation\" has invalid value."
				);
			this = new T(permutation._value, vertex);
		}

		/// <summary>
		/// "NO": No changes.
		/// <para> 0 1  -->  0 1 </para>
		/// <para> 3 2  -->  3 2 </para>
		/// </summary>
		public static T None { get { return new T(); } }
		/// <summary>
		/// "HT": 180 degree rotation.
		/// <para> 0 1  -->  2 3 </para>
		/// <para> 3 2  -->  1 0 </para>
		/// </summary>
		public static T HalfTurn { get { return new T(1); } }
		/// <summary>
		/// "FX": Horizontal flip. Reflection across y-axis.
		/// <para> 0 1  -->  1 0 </para>
		/// <para> 3 2  -->  2 3 </para>
		/// </summary>
		public static T FlipX { get { return new T(2); } }
		/// <summary>
		/// "FY": Vertical flip. Reflection across x-axis.
		/// <para> 0 1  -->  3 2 </para>
		/// <para> 3 2  -->  0 1 </para>
		/// </summary>
		public static T FlipY { get { return new T(3); } }
		/// <summary>
		/// "PD": Reflection across primary diagonal line.
		/// <para> 0 1  -->  0 3 </para>
		/// <para> 3 2  -->  1 2 </para>
		/// </summary>
		public static T ReflectOverPrimaryDiagonal { get { return new T(4); } }
		/// <summary>
		/// "SD": Reflection across secondary diagonal line.
		/// <para> 0 1  -->  2 1 </para>
		/// <para> 3 2  -->  3 0 </para>
		/// </summary>
		public static T ReflectOverSecondaryDiagonal { get { return new T(5); } }
		/// <summary>
		/// "RC": 90 degree clockwise rotation.
		/// <para> 0 1  -->  3 0 </para>
		/// <para> 3 2  -->  2 1 </para>
		/// </summary>
		public static T RotateClockwise { get { return new T(6); } }
		/// <summary>
		/// "RN": 90 degree counter clockwise rotation.
		/// <para> 0 1  -->  1 2 </para>
		/// <para> 3 2  -->  0 3 </para>
		/// </summary>
		public static T RotateCounterClockwise { get { return new T(7); } }

		public static T GetFlip(int dimension) {
			if((dimension & -_dimCount) != 0)
				throw new ArgumentOutOfRangeException("dimension");
			return new T(0, 1 << dimension);
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
			return new T(p._value, 1 << dimTo);
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
			protected bool IsRotational(int value) {
				return (0x96 >> value & 1) == 0;
			}
			protected const int _p = 0x56741320;

			public override long Count {
				get {
					return 1 << ((1 << _dim) - 1);
				}
			}
			public override bool Contains(T item) {
				return item.Vertex >> _dim == 0 &&
					item.Permutation.ReducibleTo(_dim);
			}
			public override IEnumerator<T> GetEnumerator() {
				int p = _p;
				byte c = (byte)Count;
				for(byte i = 0; i < c; ++i) {
					yield return new T(p & 7);
					p >>= 4;
				}
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
				int p = _p;
				byte c = (byte)base.Count;
				for(byte i = 0; i < c; ++i) {
					if(!IsRotational(i))
						yield return new T(p & 7);
					p >>= 4;
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
				int p = _p;
				byte c = (byte)base.Count;
				for(byte i = 0; i < c; ++i) {
					if(IsRotational(i))
						yield return new T(p & 7);
					p >>= 4;
				}
			}
		}

		private const byte _dimCount = 2;
		private const byte _count = 8;
		private static readonly string[] _names;

		static FlipRotate2D() {
			_names = new string[_count] { "NO", "HT", "FX", "FY", "PD", "SD", "RC", "RN" };
		}

		/// <summary><return>
		/// <para>true for "FX", "FY", "PD", "SD";</para>
		/// <para>false for "NO", "HT", "RC", "RN".</para>
		/// </return></summary>
		public bool IsReflection { get { return (Value + 2 & 4) == 4; } }
		/// <summary><return>
		/// <para>true for "FX", "FY";</para>
		/// <para>false for "NO", "HT", "PD", "SD", "RC", "RN".</para>
		/// </return></summary>
		public bool IsAxisReflection { get { return (Value & 6) == 2; } }
		/// <summary><return>
		/// <para>true for "PD", "SD";</para>
		/// <para>false for "NO", "HT", "FX", "FY", "RC", "RN".</para>
		/// </return></summary>
		public bool IsDiagonalReflection { get { return (Value & 6) == 4; } }
		/// <summary><return>
		/// <para>true for "NO", "HT", "RC", "RN";</para>
		/// <para>false for "FX", "FY", "PD", "SD".</para>
		/// </return></summary>
		public bool IsRotation { get { return (Value + 2 & 4) == 0; } }
		/// <summary><return>
		/// <para>true for "NO", "HT";</para>
		/// <para>false for "FX", "FY", "PD", "SD", "RC", "RN".</para>
		/// </return></summary>
		public bool IsStraightAngleRotation { get { return Value < 2; } }
		/// <summary><return>
		/// <para>true for "RC", "RN";</para>
		/// <para>false for "NO", "HT", "FX", "FY", "PD", "SD".</para>
		/// </return></summary>
		public bool IsRightAngleRotation { get { return Value > 5; } }
		/// <summary><return>
		/// <para>true for "PD", "SD", "RC", "RN";</para>
		/// <para>false for "NO", "HT", "FX", "FY".</para>
		/// </return></summary>
		public bool IsSwapDimensions { get { return Value > 3; } }

		public P Permutation {
			get { return new P((byte)((Value >> 2) * 5)); }
		}
		public int Vertex {
			get { return 0x6C9C >> (Value << 1) & 3; }
		}

		/// <summary>
		/// The order of a cyclic group that can be generated by this element.
		/// </summary>
		public int CycleLength {
			get {
				return 0x44222221 >> (Value << 2) & 7;
				/*//
				return 1 << (0xA554 >> (Value << 1) & 3);
				return 1 << ((Value + 3 - (Value >> 2)) >> 2);
				return 1 << (((Value * 15) >> 3) & 1) << ((Value >> 2) & (Value >> 1));
				//*/
			}
		}
		public T InverseElement {
			get {
				return new T(Value + 2 >> 3 ^ Value);
				/*//
				return new T(0xC0 >> Value & 1 ^ Value);
				return new T(0x67543210 >> (Value << 2) & 7);
				return new T((Value >> 1) & (Value >> 2) ^ Value);
				return new T(8 >> (Value >> 1) & 1 ^ Value);
				return new T(0x40 >> (Value & 6) & 3 ^ Value);
				return IsRightAngleRotation ? new T(Value ^ 1) : this;
				return this.Add(this.Add(this));
				return this.Add(this).Add(this);
				return this.Compose(this.Compose(this));
				return this.Compose(this).Compose(this);
				return None.Subtract(this);
				//*/
			}
		}
		public T Add(T other) {
			return new T(Value >> 1 & (other.Value >> 2) ^ other.Value ^ Value);
			/*//
			return new T((other.Value >> 1 & Value) >> 1 ^ other.Value ^ Value);
			return other.Compose(this);
			return InverseElement.Compose(other.InverseElement).InverseElement;
			return Subtract(other.InverseElement);
			return other.InverseElement.Subtract(this).InverseElement;
			//*/
		}
		public T Subtract(T other) {
			return new T((other.Value ^ Value) >> 1 & (other.Value >> 2) ^ other.Value ^ Value);
			/*//
			return new T((other.Value >> 1 & (other.Value ^ Value)) >> 1 ^ other.Value ^ Value);
			return Add(other.InverseElement);
			return other.Add(InverseElement).InverseElement;
			return other.InverseElement.Compose(this);
			return InverseElement.Compose(other).InverseElement;
			//*/
		}
		public T Times(int count) {
			return new T((count & 1) * Value ^ ((Value >> 1 & Value & count) >> 1));
			/*//
			return new T((count & 1) * Value ^ ((Value + 2 >> 3) & (count >> 1)));
			return new T((count & 1) * Value ^ ((0xC0 >> Value) & (count >> 1) & 1));
			return ((count & 1) == 1 ? this : None).Add((count & 2) == 2 && IsRightAngleRotation ? HalfTurn : None);
			//*/
		}

		public override int GetHashCode() { return Value; }
		public override bool Equals(object o) { return o is T && Equals((T)o); }
		public bool Equals(T o) { return Value == o.Value; }
		public override string ToString() { return _names[Value]; }
		public PermutationByte ToPermutationByte() {
			P p = Permutation;
			int v = Vertex;
			const int b = 0x55;
			v ^= ((b << p[0]) & 0x3 ^ v) << 2;
			v ^= ((b << p[1]) & 0xF ^ v) << 4;
			/*//
			for(byte i = 0, l = 2; i < _dimCount; ++i, l <<= 1)
				v ^= (((-1 << l) ^ -1) & (b << p[i]) ^ v) << l;
			//*/
			return PermutationByte.FromByteInternal((byte)v);
		}
		public RotateFlipType ToRotateFlipType() {
			return (RotateFlipType)(0x31756420 >> (Value << 2) & 7);
			/*//
			return (RotateFlipType)((Value << 1 & 6) ^ (Value >> 2) ^ (Value & 4));
			//*/
		}

		/// <exception cref="ArgumentException">
		/// Invalid <paramref name="name"/>.
		/// </exception>
		public static T FromString(string name) {
			int index = Array.IndexOf<string>(_names, name);
			if(index >= 0) return new T(index);
			StringBuilder sb = new StringBuilder("Acceptable values : ");
			sb.Append(_names[0]);
			for(int i = 1; i < _count; ++i) {
				sb.Append(", ");
				sb.Append(_names[i]);
			}
			sb.Append(".");
			throw new ArgumentException(sb.ToString(), "name");
		}
		public static T FromInt32(int value) { return new T(value & 7); }
		public static T FromRotateFlipType(RotateFlipType value) {
			return new T(0x53427160 >> ((byte)value << 2) & 7);
			/*//
			byte v = (byte)value;
			return new T((v << 2 & 4) ^ (v << 1 & 2) ^ (v >> 1));
			//*/
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
		public static explicit operator T(int o) { return FromInt32(o); }
		public static implicit operator T(RotateFlipType o) { return FromRotateFlipType(o); }
		public static implicit operator RotateFlipType(T o) { return o.ToRotateFlipType(); }
	}
}
