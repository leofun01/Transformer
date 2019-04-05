//	FlipRotate2d.cs
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
using StringBuilder = System.Text.StringBuilder;
using RotateFlipType = System.Drawing.RotateFlipType;

namespace DotNetTransformer.Math.Group {
	[Serializable]
	public struct FlipRotate2d : IFiniteGroupElement<FlipRotate2d>
	{
		public readonly byte Value;
		private FlipRotate2d(byte value) { Value = value; }
		private FlipRotate2d(int value) { Value = (byte)value; }

		private const byte _count = 8;
		private static readonly string[] _names;
		public static readonly FiniteGroup<FlipRotate2d> AllValues;

		/// <summary>
		/// "NO": No changes.
		/// <para> 0 1  -->  0 1 </para>
		/// <para> 3 2  -->  3 2 </para>
		/// </summary>
		public static readonly FlipRotate2d None;
		/// <summary>
		/// "HT": 180 degree rotation.
		/// <para> 0 1  -->  2 3 </para>
		/// <para> 3 2  -->  1 0 </para>
		/// </summary>
		public static readonly FlipRotate2d HalfTurn;
		/// <summary>
		/// "FX": Horizontal flip. Reflection across y-axis.
		/// <para> 0 1  -->  1 0 </para>
		/// <para> 3 2  -->  2 3 </para>
		/// </summary>
		public static readonly FlipRotate2d FlipX;
		/// <summary>
		/// "FY": Vertical flip. Reflection across x-axis.
		/// <para> 0 1  -->  3 2 </para>
		/// <para> 3 2  -->  0 1 </para>
		/// </summary>
		public static readonly FlipRotate2d FlipY;
		/// <summary>
		/// "PD": Reflection across primary diagonal line.
		/// <para> 0 1  -->  0 3 </para>
		/// <para> 3 2  -->  1 2 </para>
		/// </summary>
		public static readonly FlipRotate2d ReflectOverPrimaryDiagonal;
		/// <summary>
		/// "SD": Reflection across secondary diagonal line.
		/// <para> 0 1  -->  2 1 </para>
		/// <para> 3 2  -->  3 0 </para>
		/// </summary>
		public static readonly FlipRotate2d ReflectOverSecondaryDiagonal;
		/// <summary>
		/// "RC": 90 degree clockwise rotation.
		/// <para> 0 1  -->  3 0 </para>
		/// <para> 3 2  -->  2 1 </para>
		/// </summary>
		public static readonly FlipRotate2d RotateClockwise;
		/// <summary>
		/// "RN": 90 degree counter clockwise rotation.
		/// <para> 0 1  -->  1 2 </para>
		/// <para> 3 2  -->  0 3 </para>
		/// </summary>
		public static readonly FlipRotate2d RotateCounterClockwise;

		static FlipRotate2d() {
			_names = new string[_count] { "NO", "HT", "FX", "FY", "PD", "SD", "RC", "RN" };
			None                         = new FlipRotate2d(0);	// = FromString("NO");	// 000
			HalfTurn                     = new FlipRotate2d(1);	// = FromString("HT");	// 001
			FlipX                        = new FlipRotate2d(2);	// = FromString("FX");	// 010
			FlipY                        = new FlipRotate2d(3);	// = FromString("FY");	// 011
			ReflectOverPrimaryDiagonal   = new FlipRotate2d(4);	// = FromString("PD");	// 100
			ReflectOverSecondaryDiagonal = new FlipRotate2d(5);	// = FromString("SD");	// 101
			RotateClockwise              = new FlipRotate2d(6);	// = FromString("RC");	// 110
			RotateCounterClockwise       = new FlipRotate2d(7);	// = FromString("RN");	// 111
			AllValues = new DihedralGroup();
		}

		private sealed class DihedralGroup : FiniteGroup<FlipRotate2d>
		{
			public DihedralGroup() { }

			public override FlipRotate2d IdentityElement { get { return None; } }
			// public override bool IsCyclic { get { return false; } }
			public override int Count { get { return _count; } }
			public override bool Contains(FlipRotate2d item) { return true; }
			public override IEnumerator<FlipRotate2d> GetEnumerator() {
				for(byte i = 0; i < _count; ++i)
					yield return new FlipRotate2d(i);
			}
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

		public FlipRotate2d InverseElement {
			get {
				return new FlipRotate2d(0x67543210 >> (Value << 2) & 7);
				// return new FlipRotate2d((Value >> 1) & (Value >> 2) ^ Value);
				// return Value > 5 ? new FlipRotate2d(Value ^ 1) : this;
				// return IsRightAngleRotation ? new FlipRotate2d(Value ^ 1) : this;
				// return this.Add(this.Add(this));
				// return this.Add(this).Add(this);
				// return this.Compose(this.Compose(this));
				// return this.Compose(this).Compose(this);
				// return None.Subtract(this);
			}
		}
		/// <summary>
		/// The order of a cyclic group that can be generated by this element.
		/// </summary>
		public int CycleLength {
			get {
				return 0x44222221 >> (Value << 2) & 7;
				// return 1 << (0xA554 >> (Value << 1) & 3);
				// return 1 << ((Value + 3 - (Value >> 2)) >> 2);
				// return 1 << (((Value * 15) >> 3) & 1) << ((Value >> 2) & (Value >> 1));
			}
		}
		public FlipRotate2d Add(FlipRotate2d other) {
			return new FlipRotate2d((Value >> 1) & (other.Value >> 2) ^ Value ^ other.Value);
			// return other.Compose(this);
			// return InverseElement.Compose(other.InverseElement).InverseElement;
			// return Subtract(other.InverseElement);
			// return other.InverseElement.Subtract(this).InverseElement;
		}
		public FlipRotate2d Compose(FlipRotate2d other) {
			return new FlipRotate2d((Value >> 2) & (other.Value >> 1) ^ Value ^ other.Value);
			// return other.Add(this);
			// return InverseElement.Add(other.InverseElement).InverseElement;
			// return other.Subtract(InverseElement);
			// return InverseElement.Subtract(other).InverseElement;
		}
		public FlipRotate2d Subtract(FlipRotate2d other) {
			return new FlipRotate2d((Value ^ other.Value) >> 1 & (other.Value >> 2) ^ Value ^ other.Value);
			// return new FlipRotate2d(((Value ^ other.Value) & (other.Value >> 1)) >> 1 ^ Value ^ other.Value);
			// return Add(other.InverseElement);
			// return other.Add(InverseElement).InverseElement;
			// return other.InverseElement.Compose(this);
			// return InverseElement.Compose(other).InverseElement;
		}
		public FlipRotate2d Times(int count) {
			return new FlipRotate2d((count & 1) * Value ^ ((Value >> 1 & Value & count) >> 1));
			// return new FlipRotate2d((count & 1) * Value ^ ((0xC0 >> Value) & (count >> 1) & 1));
			// return ((count & 1) == 1 ? this : None).Add((count & 2) == 2 && IsRightAngleRotation ? HalfTurn : None);
		}

		public override int GetHashCode() { return Value; }
		public override bool Equals(object o) { return o is FlipRotate2d && Equals((FlipRotate2d)o); }
		public bool Equals(FlipRotate2d o) { return Value == o.Value; }
		public override string ToString() { return _names[Value]; }
		public RotateFlipType ToRotateFlipType() {
			return (RotateFlipType)(0x31756420 >> (Value << 2) & 7);
			// return (RotateFlipType)((Value << 1 & 6) ^ (Value >> 2) ^ (Value & 4));
		}

		/// <exception cref="ArgumentException">
		/// Invalid <paramref name="name"/>.
		/// </exception>
		public static FlipRotate2d FromString(string name) {
			int index = Array.IndexOf<string>(_names, name);
			if(index >= 0) return new FlipRotate2d(index);
			StringBuilder sb = new StringBuilder("Acceptable values : ");
			sb.Append(_names[0]);
			for(int i = 1; i < _count; ++i) {
				sb.Append(", ");
				sb.Append(_names[i]);
			}
			sb.Append(".");
			throw new ArgumentException(sb.ToString(), "name");
		}
		public static FlipRotate2d FromInt(int value) { return new FlipRotate2d(value & 7); }
		public static FlipRotate2d FromRotateFlipType(RotateFlipType value) {
			return new FlipRotate2d(0x53427160 >> ((byte)value << 2) & 7);
			// byte v = (byte)value;
			// return new FlipRotate2d((v << 2 & 4) ^ (v << 1 & 2) ^ (v >> 1));
		}

		public static bool operator ==(FlipRotate2d l, FlipRotate2d r) { return l.Equals(r); }
		public static bool operator !=(FlipRotate2d l, FlipRotate2d r) { return !l.Equals(r); }

		public static FlipRotate2d operator +(FlipRotate2d o) { return o; }
		public static FlipRotate2d operator -(FlipRotate2d o) { return o.InverseElement; }
		public static FlipRotate2d operator +(FlipRotate2d l, FlipRotate2d r) { return l.Add(r); }
		public static FlipRotate2d operator -(FlipRotate2d l, FlipRotate2d r) { return l.Subtract(r); }
		public static FlipRotate2d operator *(FlipRotate2d l, int r) { return l.Times(r); }
		public static FlipRotate2d operator *(int l, FlipRotate2d r) { return r.Times(l); }

		public static explicit operator FlipRotate2d(int o) { return FromInt(o); }
		public static implicit operator FlipRotate2d(RotateFlipType o) { return FromRotateFlipType(o); }
		public static implicit operator RotateFlipType(FlipRotate2d o) { return o.ToRotateFlipType(); }
	}
}
