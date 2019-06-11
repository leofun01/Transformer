//	EnumerableExtension.cs
//	
//	Based on :
//		.Net
//			System.Collections.Generic.IEnumerable<T>
//	
//	Author   : leofun01

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

		public delegate T Func<T>(T l, T r);
		public static T CollectAll<T>(this IEnumerable<T> collection, Func<T> func) {
			T result = default(T);
			foreach(T item in collection)
				result = func(result, item);
			return result;
		}
	}
}
