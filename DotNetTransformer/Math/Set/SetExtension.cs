using System;

namespace DotNetTransformer.Math.Set {
	public static class SetExtension {
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
			where T    : IEquatable<T>
			where TSet : ISet<T>
		{
			return !ReferenceEquals(other, null) && other.IsSupersetOf(_this);
		}
		public static bool IsSupersetOf<T, TSet>(this TSet _this, ISubSet<T, TSet> other)
			where T    : IEquatable<T>
			where TSet : ISet<T>
		{
			return !ReferenceEquals(other, null) && other.IsSubsetOf(_this);
		}

		public static bool EqualsSubsets<T, TSet>(this TSet _this, TSet other)
			where T    : IEquatable<T>
			where TSet : ISubSet<T, TSet>
		{
			return ReferenceEquals(_this, other) || (
				other.IsSupersetOf<T, TSet>(_this)
				&& _this.IsSupersetOf<T, TSet>(other)
			);
		}
		public static bool EqualsSupersets<T, TSet>(this TSet _this, TSet other)
			where T    : IEquatable<T>
			where TSet : ISuperSet<T, TSet>
		{
			return ReferenceEquals(_this, other) || (
				other.IsSubsetOf<T, TSet>(_this)
				&& _this.IsSubsetOf<T, TSet>(other)
			);
		}
		public static bool Equals<T, TSS, TSet>(this TSS _this, TSet other)
			where T    : IEquatable<T>
			where TSS  : ISubSet<T, TSet>, ISuperSet<T, TSet>
			where TSet : ISet<T>
		{
			return ReferenceEquals(_this, other) || (
				other.IsSupersetOf<T, TSet>(_this)
				&& _this.IsSupersetOf(other)
			);
		}
	}
}
