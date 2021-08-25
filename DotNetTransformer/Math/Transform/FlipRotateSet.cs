//	FlipRotateSet.cs
//	
//	Based on :
//		Math
//			Set theory
//			Abstract algebra
//				Group theory
//					Finite group
//					Symmetry group
//	
//	Author   : leofun01

using System.Collections.Generic;
using DotNetTransformer.Extensions;
using DotNetTransformer.Math.Permutation;
using DotNetTransformer.Math.Set;

namespace DotNetTransformer.Math.Transform {
	internal abstract class FlipRotateSet<T, P> : FiniteSet<T>
		where T : IFlipRotate<T, P>, new()
		where P : IPermutation<P>, new()
	{
		protected readonly byte _dim;
		protected readonly Constructor<T, P, int> _ctor;
		protected FlipRotateSet(byte dimensions, Constructor<T, P, int> ctor) {
			_dim = dimensions;
			_ctor = ctor;
		}

		protected bool IsRotational(int swaps, int vertex) {
			for(int i = 1; i < _dim; i <<= 1)
				vertex ^= vertex >> i;
			return ((swaps ^ vertex) & 1) == 0;
		}

		public abstract SetType Type { get; }
		public override long Count {
			get {
				long c = 1L;
				for(byte i = 1; i < _dim; c *= ++i) ;
				return c << _dim;
			}
		}
		public override bool Contains(T item) {
			return item.Vertex >> _dim == 0 &&
				item.Permutation.ReducibleTo(_dim);
		}
		public override IEnumerator<T> GetEnumerator() {
			int c = 1 << _dim;
			P i = new P();
			foreach(P p in i.GetRange<P>(i, _dim))
				for(int v = 0; v < c; ++v)
					yield return _ctor(p, v);
		}
		public override bool Equals(IFiniteSet<T> other) {
			FlipRotateSet<T, P> o = other as FlipRotateSet<T, P>;
			return ReferenceEquals(this, other) || (
				!ReferenceEquals(o, null)
				&& _dim == o._dim
				&& Type == o.Type
			) || base.Equals(other);
		}
		public override int GetHashCode() {
			long c = Count;
			return (int)(c >> 32 ^ c) ^ (2 >> _dim & 1);
		}
		public override bool IsSubsetOf(IFiniteSet<T> other) {
			FlipRotateSet<T, P> o = other as FlipRotateSet<T, P>;
			return ReferenceEquals(this, other) || (
				!ReferenceEquals(o, null)
				&& _dim <= o._dim
				&& (Type & o.Type) == Type
			) || base.IsSubsetOf(other);
		}
		public override bool IsSupersetOf(IFiniteSet<T> other) {
			FlipRotateSet<T, P> o = other as FlipRotateSet<T, P>;
			return ReferenceEquals(this, other) || (
				!ReferenceEquals(o, null)
				&& _dim >= o._dim
				&& (Type & o.Type) == o.Type
			) || base.IsSupersetOf(other);
		}
	}
}
