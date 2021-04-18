using System.Collections.Generic;
using DotNetTransformer.Extensions;
using DotNetTransformer.Math.Permutation;

namespace DotNetTransformer.Math.Transform {
	internal sealed class ReflectionsSet<T, P> : FlipRotateSet<T, P>
		where T : IFlipRotate<T, P>, new()
		where P : IPermutation<P>, new()
	{
		public ReflectionsSet(byte dimensions, Constructor<T, P, int> ctor) : base(dimensions, ctor) { }

		public override SetType Type { get { return SetType.Reflections; } }
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
						yield return _ctor(p, v);
			}
		}
	}
}
