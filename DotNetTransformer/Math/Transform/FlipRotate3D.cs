using System;
using System.Collections.Generic;
using System.Diagnostics;
using CultureInfo = System.Globalization.CultureInfo;
using DotNetTransformer.Math.Group;
using DotNetTransformer.Math.Permutation;

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

		private const short _s = 4, _perm = (1 << _s) - 1;

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
				return this.GetLengthTo<T>(None);
			}
		}
		public T InverseElement {
			get {
				P p = -Permutation;
				return new T(p._value, p.GetNextVertex<P>(Vertex));
			}
		}
		public T Add(T other) {
			P p = Permutation;
			return new T((p + other.Permutation)._value,
				p.GetNextVertex<P>(other.Vertex) ^ Vertex
			);
		}
		public T Subtract(T other) {
			P p = Permutation - other.Permutation;
			return new T(p._value,
				p.GetNextVertex<P>(other.Vertex) ^ Vertex
			);
		}
		public T Times(int count) {
			return this.Times<T>(count);
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
		public PermutationInt32 ToPermutationInt32() {
			P p = Permutation;
			int v = Vertex;
			const int b = 0x11111111;
			for(byte i = 0, l = 4; i < 3; ++i, l <<= 1)
				v ^= ((1 << l) - 1 & (b << p[i]) ^ v) << l;
			return new PermutationInt32(v ^ 0x76543210);
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
	}
}
