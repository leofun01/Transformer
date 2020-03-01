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

		public static ISet<T> Inverse<T>(this ISet<T> _this)
			where T : IEquatable<T>
		{
			return new InternalSet<T>(e => !_this.Contains(e));
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

		public static bool IsSubsetOf<T, S>(this S _this, ISuperSet<T, S> other)
			where T : IEquatable<T>
			where S : ISet<T>
		{
			return !ReferenceEquals(other, null) && other.IsSupersetOf(_this);
		}
		public static bool IsSupersetOf<T, S>(this S _this, ISubSet<T, S> other)
			where T : IEquatable<T>
			where S : ISet<T>
		{
			return !ReferenceEquals(other, null) && other.IsSubsetOf(_this);
		}

		public static bool EqualsSubsets<T, S>(this S _this, S other)
			where T : IEquatable<T>
			where S : ISubSet<T, S>
		{
			return ReferenceEquals(_this, other) || (
				other.IsSupersetOf<T, S>(_this)
				&& _this.IsSupersetOf<T, S>(other)
			);
		}
		public static bool EqualsSupersets<T, S>(this S _this, S other)
			where T : IEquatable<T>
			where S : ISuperSet<T, S>
		{
			return ReferenceEquals(_this, other) || (
				other.IsSubsetOf<T, S>(_this)
				&& _this.IsSubsetOf<T, S>(other)
			);
		}
		public static bool Equals<T, U, S>(this U _this, S other)
			where T : IEquatable<T>
			where U : ISubSet<T, S>, ISuperSet<T, S>
			where S : ISet<T>
		{
			return ReferenceEquals(_this, other) || (
				other.IsSupersetOf<T, S>(_this)
				&& _this.IsSupersetOf(other)
			);
		}
	}
}
