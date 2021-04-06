using System;
using System.Collections.Generic;

namespace DotNetTransformer.Math.Set {
	public static class FiniteSetExtension
	{
		private abstract class BaseAdapter<T, E> : FiniteSet<T>
			where T : IEquatable<T>
			where E : IEnumerable<T>
		{
			protected readonly E _collection;

			protected internal BaseAdapter(E collection) {
				_collection = collection;
			}

			public override abstract bool Contains(T item);
			public override sealed IEnumerator<T> GetEnumerator() {
				return _collection.GetEnumerator();
			}
			public override sealed bool Equals(IFiniteSet<T> other) {
				return IsMatch<IFiniteSet<T>>(other, base.Equals);
			}
			public override sealed int GetHashCode() {
				return _collection.GetHashCode();
			}
			public override sealed bool IsSubsetOf(ISet<T> other) {
				return IsMatch<ISet<T>>(other, base.IsSubsetOf);
			}
			public override sealed bool IsSubsetOf(IFiniteSet<T> other) {
				return IsMatch<IFiniteSet<T>>(other, base.IsSubsetOf);
			}
			public override sealed bool IsSupersetOf(IFiniteSet<T> other) {
				return IsMatch<IFiniteSet<T>>(other, base.IsSupersetOf);
			}
			private bool IsMatch<S>(S other, Predicate<S> match)
				where S : ISet<T>
			{
				BaseAdapter<T, E> o = other as BaseAdapter<T, E>;
				return ReferenceEquals(_collection, other)
					|| !ReferenceEquals(o, null) && ReferenceEquals(_collection, o._collection)
					|| match(other);
			}
		}
		private sealed class CollectionAdapter<T> : BaseAdapter<T, ICollection<T>>
			where T : IEquatable<T>
		{
			internal CollectionAdapter(ICollection<T> collection) : base(collection) { }

			public override sealed long Count { get { return _collection.Count; } }
			public override sealed bool Contains(T item) {
				return _collection.Contains(item);
			}
		}
		private sealed class EnumerableAdapter<T> : BaseAdapter<T, IEnumerable<T>>
			where T : IEquatable<T>
		{
			private readonly long _count;
			private readonly Predicate<T> _contains;

			internal EnumerableAdapter(IEnumerable<T> collection,
				long count, Predicate<T> contains
			) : base(collection) {
				_count = count;
				_contains = contains;
			}

			public override sealed long Count { get { return _count; } }
			public override sealed bool Contains(T item) {
				return _contains(item);
			}
		}

		internal static IFiniteSet<T> ToFiniteSet<T>(this ICollection<T> collection)
			where T : IEquatable<T>
		{
			return ReferenceEquals(collection, null) ?
				null : new CollectionAdapter<T>(collection);
		}
		internal static IFiniteSet<T> ToFiniteSet<T>(this IEnumerable<T> collection,
			long count, Predicate<T> contains
		)
			where T : IEquatable<T>
		{
			return ReferenceEquals(collection, null) ?
				null : new EnumerableAdapter<T>(collection, count, contains);
		}
	}
}
