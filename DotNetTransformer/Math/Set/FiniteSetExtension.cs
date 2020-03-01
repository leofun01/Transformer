using System;
using System.Collections.Generic;
using DotNetTransformer.Math.Group;

namespace DotNetTransformer.Math.Set {
	public static class FiniteSetExtension
	{
		private abstract class InternalBase<T, E> : FiniteSet<T>
			where T : IEquatable<T>
			where E : IEnumerable<T>
		{
			protected readonly E _collection;

			protected internal InternalBase(E collection) {
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
				InternalBase<T, E> o = other as InternalBase<T, E>;
				return ReferenceEquals(_collection, other)
					|| !ReferenceEquals(o, null) && ReferenceEquals(_collection, o._collection)
					|| match(other);
			}
		}
		private sealed class InternalGroup<T> : InternalBase<T, IFiniteGroup<T>>
			where T : IFiniteGroupElement<T>, new()
		{
			internal InternalGroup(IFiniteGroup<T> collection) : base(collection) { }

			public override sealed long Count { get { return _collection.Count; } }
			public override sealed bool Contains(T item) {
				return _collection.Contains(item);
			}
		}
		private sealed class InternalCollection<T> : InternalBase<T, ICollection<T>>
			where T : IEquatable<T>
		{
			internal InternalCollection(ICollection<T> collection) : base(collection) { }

			public override sealed long Count { get { return _collection.Count; } }
			public override sealed bool Contains(T item) {
				return _collection.Contains(item);
			}
		}
		private sealed class InternalEnumerable<T> : InternalBase<T, IEnumerable<T>>
			where T : IEquatable<T>
		{
			private readonly long _count;
			private readonly Predicate<T> _contains;

			internal InternalEnumerable(IEnumerable<T> collection,
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

		internal static IFiniteSet<T> ToFiniteSet<T>(this IFiniteGroup<T> collection)
			where T : IFiniteGroupElement<T>, new()
		{
			return ReferenceEquals(collection, null) ?
				null : new InternalGroup<T>(collection);
		}
		internal static IFiniteSet<T> ToFiniteSet<T>(this ICollection<T> collection)
			where T : IEquatable<T>
		{
			return ReferenceEquals(collection, null) ?
				null : new InternalCollection<T>(collection);
		}
		internal static IFiniteSet<T> ToFiniteSet<T>(this IEnumerable<T> collection,
			long count, Predicate<T> contains
		)
			where T : IEquatable<T>
		{
			return ReferenceEquals(collection, null) ?
				null : new InternalEnumerable<T>(collection, count, contains);
		}
	}
}
