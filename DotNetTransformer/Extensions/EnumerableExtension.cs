using System;
using System.Collections.Generic;

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

		public static T CollectAll<T>(this IEnumerable<T> collection, Func<T> func) {
			T result = default(T);
			foreach(T item in collection)
				result = func(result, item);
			return result;
		}

		public static IEnumerable<T> GetRange<T>(this T start, Predicate<T> match, Generator<T> next) {
			T t = start;
			do {
				yield return t;
				t = next(t);
			} while(match(t));
		}
		public static IEnumerable<T> GetRange<T>(this T start, T stop, Generator<T> next) {
			return GetRange<T>(start, t => !t.Equals(stop), next);
		}
	}
}
