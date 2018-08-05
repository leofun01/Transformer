using System.Collections.Generic;
using FlipRotate2d = DotNetTransformer.Math.Group.FlipRotate2d;
using RotateFlipType = System.Drawing.RotateFlipType;

namespace DotNetTransformer.Extensions {
	public static class EnumerableExtension
	{
		private delegate T Func<T>(T l, T r);
		private static T CollectAll<T>(this IEnumerable<T> collection, Func<T> func) where T : struct {
			T result = default(T);
			if(collection != null)
				foreach(T item in collection)
					result = func(result, item);
			return result;
		}
		public static FlipRotate2d AddAll(this IEnumerable<FlipRotate2d> collection) {
			return CollectAll<FlipRotate2d>(collection, (l, r) => l.Add(r));
		}
		public static FlipRotate2d ComposeAll(this IEnumerable<FlipRotate2d> collection) {
			return CollectAll<FlipRotate2d>(collection, (l, r) => l.Compose(r));
		}
		public static RotateFlipType AddAll(this IEnumerable<RotateFlipType> collection) {
			return CollectAll<RotateFlipType>(collection, (l, r) => l.Add(r));
		}
		public static RotateFlipType ComposeAll(this IEnumerable<RotateFlipType> collection) {
			return CollectAll<RotateFlipType>(collection, (l, r) => l.Compose(r));
		}
	}
}
