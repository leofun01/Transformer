using System;
using System.Collections.Generic;
using DotNetTransformer.Extensions;
using DotNetTransformer.Math.Group;
using DotNetTransformer.Math.Permutation;

namespace DotNetTransformer.Math.Transform {
	public static class FlipRotateExtension
	{
		public static IEnumerable<T> GetValues<T, P>(int dimensions,
			Constructor<P, int, T> ctor
		)
			where T : IFlipRotate<T, P>, new()
			where P : IPermutation<P>, new()
		{
			return GetValues<T, P>(dimensions, ctor,
				_ => (0).GetRange<int>(1 << dimensions, v => v + 1)
			);
		}
		public static IEnumerable<T> GetValues<T, P>(int dimensions,
			Constructor<P, int, T> ctor,
			Converter<P, IEnumerable<int>> vertexes
		)
			where T : IFlipRotate<T, P>, new()
			where P : IPermutation<P>, new()
		{
			P i = new P();
			foreach(P p in i.GetRange<P>(i, dimensions))
				foreach(int v in vertexes(p))
					yield return ctor(p, v);
		}
	}
}
