using System;
using System.Collections.Generic;
using RotateFlipType = System.Drawing.RotateFlipType;
using FlipRotate2D = DotNetTransformer.Math.Group.FlipRotate2D;

namespace DotNetTransformer.Extensions {
	public static class EnumerableExtension
	{
		public static bool Exist<T>(this IEnumerable<T> collection, Predicate<T> match) {
			foreach(T item in collection)
				if(match(item))
					return true;
			return false;
		}
		public static bool All<T>(this IEnumerable<T> collection, Predicate<T> match) {
			return !collection.Exist<T>(e => !match(e));
		}

		private delegate T Func<T>(T l, T r);
		private static T CollectAll<T>(this IEnumerable<T> collection, Func<T> func) where T : struct {
			T result = default(T);
			foreach(T item in collection)
				result = func(result, item);
			return result;
		}
		public static FlipRotate2D AddAll(this IEnumerable<FlipRotate2D> collection) {
			return CollectAll<FlipRotate2D>(collection, (l, r) => l.Add(r));
		}
		public static FlipRotate2D ComposeAll(this IEnumerable<FlipRotate2D> collection) {
			return CollectAll<FlipRotate2D>(collection, (l, r) => l.Compose(r));
		}
		public static RotateFlipType AddAll(this IEnumerable<RotateFlipType> collection) {
			return CollectAll<RotateFlipType>(collection, (l, r) => l.Add(r));
		}
		public static RotateFlipType ComposeAll(this IEnumerable<RotateFlipType> collection) {
			return CollectAll<RotateFlipType>(collection, (l, r) => l.Compose(r));
		}
	}
}
