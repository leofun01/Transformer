using System;
using System.Collections.Generic;
using DotNetTransformer.Math.Group;
using RotateFlipType = System.Drawing.RotateFlipType;

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

		public delegate T Func<T>(T l, T r);
		public static T CollectAll<T>(this IEnumerable<T> collection, Func<T> func) {
			T result = default(T);
			foreach(T item in collection)
				result = func(result, item);
			return result;
		}
		public static RotateFlipType AddAll(this IEnumerable<RotateFlipType> collection) {
			return CollectAll<RotateFlipType>(collection, (l, r) => l.Add(r));
		}
		public static RotateFlipType ComposeAll(this IEnumerable<RotateFlipType> collection) {
			return CollectAll<RotateFlipType>(collection, (l, r) => l.Compose(r));
		}
	}
}
