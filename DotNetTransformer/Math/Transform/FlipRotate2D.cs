using System;
using System.Collections.Generic;
using System.Diagnostics;
using StringBuilder = System.Text.StringBuilder;
using RotateFlipType = System.Drawing.RotateFlipType;
using DotNetTransformer.Math.Group;
using DotNetTransformer.Math.Permutation;

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

		private const byte _count = 8;
		private static readonly string[] _names;
		public static readonly FiniteGroup<T> AllValues;

		/// <summary>
		/// "NO": No changes.
		/// <para> 0 1  -->  0 1 </para>
		/// <para> 3 2  -->  3 2 </para>
		/// </summary>
		public static readonly T None;
		/// <summary>
		/// "HT": 180 degree rotation.
		/// <para> 0 1  -->  2 3 </para>
		/// <para> 3 2  -->  1 0 </para>
		/// </summary>
		public static readonly T HalfTurn;
		/// <summary>
		/// "FX": Horizontal flip. Reflection across y-axis.
		/// <para> 0 1  -->  1 0 </para>
		/// <para> 3 2  -->  2 3 </para>
		/// </summary>
		public static readonly T FlipX;
		/// <summary>
		/// "FY": Vertical flip. Reflection across x-axis.
		/// <para> 0 1  -->  3 2 </para>
		/// <para> 3 2  -->  0 1 </para>
		/// </summary>
		public static readonly T FlipY;
		/// <summary>
		/// "PD": Reflection across primary diagonal line.
		/// <para> 0 1  -->  0 3 </para>
		/// <para> 3 2  -->  1 2 </para>
		/// </summary>
		public static readonly T ReflectOverPrimaryDiagonal;
		/// <summary>
		/// "SD": Reflection across secondary diagonal line.
		/// <para> 0 1  -->  2 1 </para>
		/// <para> 3 2  -->  3 0 </para>
		/// </summary>
		public static readonly T ReflectOverSecondaryDiagonal;
		/// <summary>
		/// "RC": 90 degree clockwise rotation.
		/// <para> 0 1  -->  3 0 </para>
		/// <para> 3 2  -->  2 1 </para>
		/// </summary>
		public static readonly T RotateClockwise;
		/// <summary>
		/// "RN": 90 degree counter clockwise rotation.
		/// <para> 0 1  -->  1 2 </para>
		/// <para> 3 2  -->  0 3 </para>
		/// </summary>
		public static readonly T RotateCounterClockwise;

		static FlipRotate2D() {
			_names = new string[_count] { "NO", "HT", "FX", "FY", "PD", "SD", "RC", "RN" };
			None                         = new T(0);	// = FromString("NO");	// 000
			HalfTurn                     = new T(1);	// = FromString("HT");	// 001
			FlipX                        = new T(2);	// = FromString("FX");	// 010
			FlipY                        = new T(3);	// = FromString("FY");	// 011
			ReflectOverPrimaryDiagonal   = new T(4);	// = FromString("PD");	// 100
			ReflectOverSecondaryDiagonal = new T(5);	// = FromString("SD");	// 101
			RotateClockwise              = new T(6);	// = FromString("RC");	// 110
			RotateCounterClockwise       = new T(7);	// = FromString("RN");	// 111
			AllValues = new DihedralGroupD4();
		}

		private sealed class DihedralGroupD4 : FiniteGroup<T>
		{
			public DihedralGroupD4() { }

			public override T IdentityElement { get { return None; } }
			public override int Count { get { return _count; } }
			public override bool Contains(T item) { return true; }
			public override IEnumerator<T> GetEnumerator() {
				for(byte i = 0; i < _count; ++i)
					yield return new T(i);
			}
			public override int GetHashCode() { return _count; }
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

		P IFlipRotate<T, P>.Permutation { get { return new P((byte)((Value >> 2) * 5)); } }
		int IFlipRotate<T, P>.Vertex { get { return 0x6C9C >> (Value << 1) & 3; } }

		/// <summary>
		/// The order of a cyclic group that can be generated by this element.
		/// </summary>
		public int CycleLength {
			get {
				return 0x44222221 >> (Value << 2) & 7;
				/*//
				return 1 << (0xA554 >> (Value << 1) & 3);
				return 1 << ((Value + 3 - (Value >> 2)) >> 2);
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
				//*/
			}
		}
		public T Add(T other) {
			return new T(Value >> 1 & (other.Value >> 2) ^ other.Value ^ Value);
			/*//
			return new T((other.Value >> 1 & Value) >> 1 ^ other.Value ^ Value);
			//*/
		}
		public T Subtract(T other) {
			return new T((other.Value ^ Value) >> 1 & (other.Value >> 2) ^ other.Value ^ Value);
			/*//
			return new T((other.Value >> 1 & (other.Value ^ Value)) >> 1 ^ other.Value ^ Value);
			//*/
		}
		public T Times(int count) {
			return new T((count & 1) * Value ^ ((Value >> 1 & Value & count) >> 1));
			/*//
			return new T((count & 1) * Value ^ ((Value + 2 >> 3) & (count >> 1)));
			return new T((count & 1) * Value ^ ((0xC0 >> Value) & (count >> 1) & 1));
			//*/
		}

		public override int GetHashCode() { return Value; }
		public override bool Equals(object o) { return o is T && Equals((T)o); }
		public bool Equals(T o) { return Value == o.Value; }
		public override string ToString() { return _names[Value]; }
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

		public static explicit operator T(int o) { return FromInt32(o); }
		public static implicit operator T(RotateFlipType o) { return FromRotateFlipType(o); }
		public static implicit operator RotateFlipType(T o) { return o.ToRotateFlipType(); }
	}
}
