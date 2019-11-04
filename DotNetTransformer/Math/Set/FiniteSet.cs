//	FiniteSet.cs
//	
//	Based on :
//		Math
//			Set theory
//				Finite set
//	
//	Author   : leofun01

using System;
using System.Collections;
using System.Collections.Generic;
using DotNetTransformer.Extensions;

namespace DotNetTransformer.Math.Set {
	public abstract partial class FiniteSet<T> : ISet<T>, IEnumerable<T>
		, IEquatable<FiniteSet<T>>
		, ISubSet<T, ISet<T>>
		, ISubSet<T, FiniteSet<T>>
		// , ISubSet<T, FiniteSet<T>, FiniteSet<T>>
		, ISuperSet<T, FiniteSet<T>>
		// , ISuperSet<T, FiniteSet<T>, FiniteSet<T>>
		where T : IEquatable<T>
	{
		// public bool IsCountable { get { return true; } }
		// public bool IsFinite { get { return true; } }
		// public virtual bool IsEmpty { get { return !GetEnumerator().MoveNext(); } }
		public abstract int Count { get; }
		public virtual bool Contains(T item) {
			return this.Exist<T>(e => e.Equals(item));
		}
		public abstract IEnumerator<T> GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}
		public virtual bool Equals(FiniteSet<T> other) {
			return ReferenceEquals(this, other) || (
				!ReferenceEquals(other, null)
				&& Count == other.Count
				&& !this.Exist<T>(e => !other.Contains(e))
				&& !other.Exist<T>(e => !Contains(e))
			);
		}
		public override sealed bool Equals(object obj) {
			return Equals(obj as FiniteSet<T>);
		}
		public override int GetHashCode() {
			int hash = Count;
			foreach(T item in this)
				hash ^= item.GetHashCode();
			return hash;
		}
		public virtual bool IsSubsetOf(ISet<T> other) {
			return ReferenceEquals(this, other) || (
				!ReferenceEquals(other, null)
				&& !this.Exist<T>(e => !other.Contains(e))
			);
		}
		public virtual bool IsSubsetOf(FiniteSet<T> other) {
			return ReferenceEquals(this, other) || (
				!ReferenceEquals(other, null)
				&& Count <= other.Count
				&& !this.Exist<T>(e => !other.Contains(e))
			);
		}
		public virtual bool IsSupersetOf(FiniteSet<T> other) {
			return ReferenceEquals(this, other) || (
				!ReferenceEquals(other, null)
				&& Count >= other.Count
				&& !other.Exist<T>(e => !Contains(e))
			);
		}

		public static bool operator ==(FiniteSet<T> l, FiniteSet<T> r) {
			return ReferenceEquals(l, r) || (
				!ReferenceEquals(l, null) &&
				l.Equals(r)
			);
		}
		public static bool operator !=(FiniteSet<T> l, FiniteSet<T> r) {
			return !(l == r);
		}
	}
}
