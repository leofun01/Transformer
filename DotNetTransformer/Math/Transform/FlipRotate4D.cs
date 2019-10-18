using System;
using System.Diagnostics;
using DotNetTransformer.Math.Group;
using DotNetTransformer.Math.Permutation;

namespace DotNetTransformer.Math.Transform {
	using T = FlipRotate4D;
	using P = PermutationByte;

	[Serializable]
	[DebuggerDisplay("{ToString()}, CycleLength = {CycleLength}")]
	public struct FlipRotate4D : IFiniteGroupElement<T>
	{
		private readonly short _value;
		public FlipRotate4D(P permutation, int vertex) {
			vertex <<= _s;
			_value = (short)((vertex | permutation._value) & 0x0FFF);
		}

		public static T None { get { return new T(); } }

		private const short _s = 8, _perm = (1 << _s) - 1;

		public P Permutation { get { return new P((byte)(_value & _perm)); } }
		public int Vertex { get { return _value >> _s; } }

		public int CycleLength {
			get {
				int cLen = 1;
				T sum = this;
				while(sum != None) {
					sum += this;
					++cLen;
				}
				return cLen;
			}
		}
		public T InverseElement {
			get {
				P p = -Permutation;
				return new T(p, GetNextVertex(Vertex, p));
			}
		}
		public T Add(T other) {
			P p = Permutation;
			return new T(p + other.Permutation,
				GetNextVertex(other.Vertex, p) ^ Vertex
			);
		}
		public T Subtract(T other) {
			P p = Permutation - other.Permutation;
			return new T(p,
				GetNextVertex(other.Vertex, p) ^ Vertex
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

		private static int GetNextVertex(int v, P p) {
			int r = 0;
			for(byte i = 0; v != 0; ++i) {
				r ^= (v & 1) << p[i];
				v >>= 1;
			}
			return r;
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
