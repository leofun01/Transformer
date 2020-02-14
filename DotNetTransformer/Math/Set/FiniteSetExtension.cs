using System;
using System.Collections.Generic;

namespace DotNetTransformer.Math.Set {
	public static class FiniteSetExtension
	{
		private abstract class InternalBase<T, TEnum> : FiniteSet<T>
			where T     : IEquatable<T>
			where TEnum : IEnumerable<T>
		{
			protected readonly TEnum _collection;

			protected internal InternalBase(TEnum collection) {
				_collection = collection;
			}

			public override abstract bool Contains(T item);
			public sealed override IEnumerator<T> GetEnumerator() {
				return _collection.GetEnumerator();
			}
			public sealed override bool Equals(FiniteSet<T> other) {
				return IsMatch<FiniteSet<T>>(other, base.Equals);
			}
			public sealed override int GetHashCode() {
				return _collection.GetHashCode();
			}
			public sealed override bool IsSubsetOf(ISet<T> other) {
				return IsMatch<ISet<T>>(other, base.IsSubsetOf);
			}
			public sealed override bool IsSubsetOf(FiniteSet<T> other) {
				return IsMatch<FiniteSet<T>>(other, base.IsSubsetOf);
			}
			public sealed override bool IsSupersetOf(FiniteSet<T> other) {
				return IsMatch<FiniteSet<T>>(other, base.IsSupersetOf);
			}
			private bool IsMatch<TSet>(TSet other, Predicate<TSet> match)
				where TSet : ISet<T>
			{
				InternalBase<T, TEnum> o = other as InternalBase<T, TEnum>;
				return ReferenceEquals(_collection, other)
					|| !ReferenceEquals(o, null) && ReferenceEquals(_collection, o._collection)
					|| match(other);
			}
		}
		private sealed class InternalCollection<T> : InternalBase<T, ICollection<T>>
			where T : IEquatable<T>
		{
			internal InternalCollection(ICollection<T> collection) : base(collection) { }

			public sealed override long Count { get { return _collection.Count; } }
			public sealed override bool Contains(T item) {
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

			public sealed override long Count { get { return _count; } }
			public sealed override bool Contains(T item) {
				return _contains(item);
			}
		}

		internal static FiniteSet<T> ToFiniteSet<T>(this ICollection<T> collection)
			where T : IEquatable<T>
		{
			return ReferenceEquals(collection, null) ?
				null : new InternalCollection<T>(collection);
		}
		internal static FiniteSet<T> ToFiniteSet<T>(this IEnumerable<T> collection,
			long count, Predicate<T> contains
		)
			where T : IEquatable<T>
		{
			return ReferenceEquals(collection, null) ?
				null : new InternalEnumerable<T>(collection, count, contains);
		}
	}
}
