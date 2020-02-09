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
			public override bool Equals(FiniteSet<T> other) {
				return CollectionsEquals(other) || base.Equals(other);
			}
			public override bool IsSubsetOf(ISet<T> other) {
				return CollectionsEquals(other) || base.IsSubsetOf(other);
			}
			public override bool IsSubsetOf(FiniteSet<T> other) {
				return CollectionsEquals(other) || base.IsSubsetOf(other);
			}
			public override bool IsSupersetOf(FiniteSet<T> other) {
				return CollectionsEquals(other) || base.IsSupersetOf(other);
			}
			private bool CollectionsEquals(object other) {
				InternalSet<T> o = other as InternalSet<T>;
				return !ReferenceEquals(o, null)
					&& _collection.Equals(o._collection);
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
