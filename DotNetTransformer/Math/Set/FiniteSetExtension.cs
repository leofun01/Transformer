using System;
using System.Collections.Generic;

namespace DotNetTransformer.Math.Set {
	public static class FiniteSetExtension
	{
		private sealed class InternalSet<T> : FiniteSet<T>
			where T : IEquatable<T>
		{
			private readonly ICollection<T> _collection;

			internal InternalSet(ICollection<T> collection) {
				_collection = collection;
			}

			public override long Count { get { return _collection.Count; } }
			public override bool Contains(T item) {
				return _collection.Contains(item);
			}
			public override IEnumerator<T> GetEnumerator() {
				return _collection.GetEnumerator();
			}
		}

		internal static FiniteSet<T> ToFiniteSet<T>(this ICollection<T> collection)
			where T : IEquatable<T>
		{
			return ReferenceEquals(collection, null) ?
				null : new InternalSet<T>(collection);
		}
	}
}
