using System;
using System.Collections.Generic;
using StringBuilder = System.Text.StringBuilder;
using RotateFlipType = System.Drawing.RotateFlipType;

namespace DotNetTransformer.Math.Group.Transform2D {
	[Serializable]
	public struct FlipRotate2D : IFiniteGroupElement<FlipRotate2D>
	{
		public readonly byte Value;
		private FlipRotate2D(byte value) { Value = value; }
		private FlipRotate2D(int value) { Value = (byte)value; }

		private const byte _count = 8;
		private static readonly string[] _names;
		public static readonly FiniteGroup<FlipRotate2D> AllValues;

		/// <summary>
		/// "NO": No changes.
		/// <para> 0 1  -->  0 1 </para>
		/// <para> 3 2  -->  3 2 </para>
		/// </summary>
		public static readonly FlipRotate2D None;
		/// <summary>
		/// "HT": 180 degree rotation.
		/// <para> 0 1  -->  2 3 </para>
		/// <para> 3 2  -->  1 0 </para>
		/// </summary>
		public static readonly FlipRotate2D HalfTurn;
		/// <summary>
		/// "FX": Horizontal flip. Reflection across y-axis.
		/// <para> 0 1  -->  1 0 </para>
		/// <para> 3 2  -->  2 3 </para>
		/// </summary>
		public static readonly FlipRotate2D FlipX;
		/// <summary>
		/// "FY": Vertical flip. Reflection across x-axis.
		/// <para> 0 1  -->  3 2 </para>
		/// <para> 3 2  -->  0 1 </para>
		/// </summary>
		public static readonly FlipRotate2D FlipY;
		/// <summary>
		/// "PD": Reflection across primary diagonal line.
		/// <para> 0 1  -->  0 3 </para>
		/// <para> 3 2  -->  1 2 </para>
		/// </summary>
		public static readonly FlipRotate2D ReflectOverPrimaryDiagonal;
		/// <summary>
		/// "SD": Reflection across secondary diagonal line.
		/// <para> 0 1  -->  2 1 </para>
		/// <para> 3 2  -->  3 0 </para>
		/// </summary>
		public static readonly FlipRotate2D ReflectOverSecondaryDiagonal;
		/// <summary>
		/// "RC": 90 degree clockwise rotation.
		/// <para> 0 1  -->  3 0 </para>
		/// <para> 3 2  -->  2 1 </para>
		/// </summary>
		public static readonly FlipRotate2D RotateClockwise;
		/// <summary>
		/// "RN": 90 degree counter clockwise rotation.
		/// <para> 0 1  -->  1 2 </para>
		/// <para> 3 2  -->  0 3 </para>
		/// </summary>
		public static readonly FlipRotate2D RotateCounterClockwise;

		static FlipRotate2D() {
			_names = new string[_count] { "NO", "HT", "FX", "FY", "PD", "SD", "RC", "RN" };
			None                         = new FlipRotate2D(0);	// = FromString("NO");	// 000
			HalfTurn                     = new FlipRotate2D(1);	// = FromString("HT");	// 001
			FlipX                        = new FlipRotate2D(2);	// = FromString("FX");	// 010
			FlipY                        = new FlipRotate2D(3);	// = FromString("FY");	// 011
			ReflectOverPrimaryDiagonal   = new FlipRotate2D(4);	// = FromString("PD");	// 100
			ReflectOverSecondaryDiagonal = new FlipRotate2D(5);	// = FromString("SD");	// 101
			RotateClockwise              = new FlipRotate2D(6);	// = FromString("RC");	// 110
			RotateCounterClockwise       = new FlipRotate2D(7);	// = FromString("RN");	// 111
			AllValues = new DihedralGroupD4();
		}

		private sealed class DihedralGroupD4 : FiniteGroup<FlipRotate2D>
		{
			public DihedralGroupD4() { }

			public override FlipRotate2D IdentityElement { get { return None; } }
			public override int Count { get { return _count; } }
			public override bool Contains(FlipRotate2D item) { return true; }
			public override IEnumerator<FlipRotate2D> GetEnumerator() {
				for(byte i = 0; i < _count; ++i)
					yield return new FlipRotate2D(i);
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

		public FlipRotate2D InverseElement {
			get {
				return new FlipRotate2D(Value + 2 >> 3 ^ Value);
				/*//
				return new FlipRotate2D(0xC0 >> Value & 1 ^ Value);
				return new FlipRotate2D(0x67543210 >> (Value << 2) & 7);
				return new FlipRotate2D((Value >> 1) & (Value >> 2) ^ Value);
				return new FlipRotate2D(8 >> (Value >> 1) & 1 ^ Value);
				return new FlipRotate2D(0x40 >> (Value & 6) & 3 ^ Value);
				//*/
			}
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
				//*/
			}
		}
		public FlipRotate2D Add(FlipRotate2D other) {
			return new FlipRotate2D(Value >> 1 & (other.Value >> 2) ^ other.Value ^ Value);
			// return new FlipRotate2D((other.Value >> 1 & Value) >> 1 ^ other.Value ^ Value);
		}
		public FlipRotate2D Subtract(FlipRotate2D other) {
			return new FlipRotate2D((other.Value ^ Value) >> 1 & (other.Value >> 2) ^ other.Value ^ Value);
			// return new FlipRotate2D((other.Value >> 1 & (other.Value ^ Value)) >> 1 ^ other.Value ^ Value);
		}
		public FlipRotate2D Times(int count) {
			return new FlipRotate2D((count & 1) * Value ^ ((Value >> 1 & Value & count) >> 1));
			// return new FlipRotate2D((count & 1) * Value ^ ((0xC0 >> Value) & (count >> 1) & 1));
		}

		public override int GetHashCode() { return Value; }
		public override bool Equals(object o) { return o is FlipRotate2D && Equals((FlipRotate2D)o); }
		public bool Equals(FlipRotate2D o) { return Value == o.Value; }
		public override string ToString() { return _names[Value]; }
		public RotateFlipType ToRotateFlipType() {
			return (RotateFlipType)(0x31756420 >> (Value << 2) & 7);
			// return (RotateFlipType)((Value << 1 & 6) ^ (Value >> 2) ^ (Value & 4));
		}

		/// <exception cref="ArgumentException">
		/// Invalid <paramref name="name"/>.
		/// </exception>
		public static FlipRotate2D FromString(string name) {
			int index = Array.IndexOf<string>(_names, name);
			if(index >= 0) return new FlipRotate2D(index);
			StringBuilder sb = new StringBuilder("Acceptable values : ");
			sb.Append(_names[0]);
			for(int i = 1; i < _count; ++i) {
				sb.Append(", ");
				sb.Append(_names[i]);
			}
			sb.Append(".");
			throw new ArgumentException(sb.ToString(), "name");
		}
		public static FlipRotate2D FromInt32(int value) { return new FlipRotate2D(value & 7); }
		public static FlipRotate2D FromRotateFlipType(RotateFlipType value) {
			return new FlipRotate2D(0x53427160 >> ((byte)value << 2) & 7);
			/*//
			byte v = (byte)value;
			return new FlipRotate2D((v << 2 & 4) ^ (v << 1 & 2) ^ (v >> 1));
			//*/
		}

		public static bool operator ==(FlipRotate2D l, FlipRotate2D r) { return l.Equals(r); }
		public static bool operator !=(FlipRotate2D l, FlipRotate2D r) { return !l.Equals(r); }

		public static FlipRotate2D operator +(FlipRotate2D o) { return o; }
		public static FlipRotate2D operator -(FlipRotate2D o) { return o.InverseElement; }
		public static FlipRotate2D operator +(FlipRotate2D l, FlipRotate2D r) { return l.Add(r); }
		public static FlipRotate2D operator -(FlipRotate2D l, FlipRotate2D r) { return l.Subtract(r); }
		public static FlipRotate2D operator *(FlipRotate2D l, int r) { return l.Times(r); }
		public static FlipRotate2D operator *(int l, FlipRotate2D r) { return r.Times(l); }

		public static explicit operator FlipRotate2D(int o) { return FromInt32(o); }
		public static implicit operator FlipRotate2D(RotateFlipType o) { return FromRotateFlipType(o); }
		public static implicit operator RotateFlipType(FlipRotate2D o) { return o.ToRotateFlipType(); }
	}
}
