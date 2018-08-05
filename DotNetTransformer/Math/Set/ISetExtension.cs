using System;

namespace DotNetTransformer.Math.Set {
	public static class ISetExtension {
		private sealed class InternalSet<T> : ISet<T>
			where T : IEquatable<T>
		{
			private readonly Predicate<T> _contains;

			public InternalSet(Predicate<T> contains) { _contains = contains; }

			public bool Contains(T item) { return _contains(item); }
		}

		public static ISet<T> Union<T>(this ISet<T> _this, ISet<T> other)
			where T : IEquatable<T>
		{
			return ReferenceEquals(_this, other) ? _this :
				new InternalSet<T>(e => _this.Contains(e) || other.Contains(e));
		}
		public static ISet<T> Intersect<T>(this ISet<T> _this, ISet<T> other)
			where T : IEquatable<T>
		{
			return ReferenceEquals(_this, other) ? _this :
				new InternalSet<T>(e => _this.Contains(e) && other.Contains(e));
		}
		public static ISet<T> Except<T>(this ISet<T> _this, ISet<T> other)
			where T : IEquatable<T>
		{
			return ReferenceEquals(_this, other) ? (ISet<T>)FiniteSet<T>.Empty :
				new InternalSet<T>(e => _this.Contains(e) && !other.Contains(e));
		}
		public static ISet<T> SymmetricExcept<T>(this ISet<T> _this, ISet<T> other)
			where T : IEquatable<T>
		{
			return ReferenceEquals(_this, other) ? (ISet<T>)FiniteSet<T>.Empty :
				new InternalSet<T>(e => _this.Contains(e) ^ other.Contains(e));
		}

		public static bool IsSubsetOf<T, TSet>(this TSet _this, ISuperSet<T, TSet> other)
			where T : IEquatable<T>
			where TSet : ISet<T>
		{
			return other.IsSupersetOf(_this);
		}
		public static bool IsSupersetOf<T, TSet>(this TSet _this, ISubSet<T, TSet> other)
			where T : IEquatable<T>
			where TSet : ISet<T>
		{
			return other.IsSubsetOf(_this);
		}
	}
}
