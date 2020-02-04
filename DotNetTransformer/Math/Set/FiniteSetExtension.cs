using System;
using System.Collections.Generic;

namespace DotNetTransformer.Math.Set {
	public static class FiniteSetExtension {
		private sealed class InternalSet<T> : FiniteSet<T>
			where T : IEquatable<T>
		{
			private readonly ICollection<T> _collection;

			public InternalSet(ICollection<T> collection) {
				_collection = collection;
			}

			public override int Count { get { return _collection.Count; } }
			public override IEnumerator<T> GetEnumerator() {
				return _collection.GetEnumerator();
			}
		}

		public static FiniteSet<T> ToFiniteSet<T>(this ICollection<T> collection)
			where T : IEquatable<T>
		{
			return ReferenceEquals(collection, null) ?
				null : new InternalSet<T>(collection);
		}
	}
}
