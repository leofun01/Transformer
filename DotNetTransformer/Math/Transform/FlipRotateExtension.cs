using System.Collections.Generic;
using DotNetTransformer.Extensions;
using DotNetTransformer.Math.Group;
using DotNetTransformer.Math.Permutation;

namespace DotNetTransformer.Math.Transform {
	public static class FlipRotateExtension
	{
		public static IEnumerable<T> GetAllValues<T, P>(int dim, Constructor<P, int, T> ctor)
			where T : IFlipRotate<T, P>, new()
			where P : IPermutation<P>, new()
		{
			int c = 1 << dim;
			P i = new P();
			foreach(P p in i.GetRange<P>(i, dim))
				for(int v = 0; v < c; ++v)
					yield return ctor(p, v);
		}
		public static int GetCycleLength<T, P>(this T _this)
			where T : IFlipRotate<T, P>, new()
			where P : IPermutation<P>, new()
		{
			int c = _this.Permutation.CycleLength;
			return GroupExtension.Times<T>(_this, c).Equals(new T()) ? c : (c << 1);
		}
	}
}
