//	EmptySet.cs
//	
//	Based on :
//		Math
//			Set theory
//	
//	Author   : leofun01

using System;
using System.Collections.Generic;

namespace DotNetTransformer.Math.Set {
	public abstract partial class FiniteSet<T>
		where T : IEquatable<T>
	{
		public static readonly IFiniteSet<T> Empty = new EmptySet();
		private sealed class EmptySet : FiniteSet<T>
		{
			public EmptySet() { }

			// public override bool IsEmpty { get { return true; } }
			public override long Count { get { return 0L; } }
			public override bool Contains(T item) { return false; }
			public override IEnumerator<T> GetEnumerator() { yield break; }
			public override bool Equals(IFiniteSet<T> other) {
				return !ReferenceEquals(other, null) && other.Count == 0L;
			}
			public override int GetHashCode() { return 0; }
			public override bool IsSubsetOf(ISet<T> other) { return !ReferenceEquals(other, null); }
			public override bool IsSubsetOf(IFiniteSet<T> other) { return !ReferenceEquals(other, null); }
			public override bool IsSupersetOf(IFiniteSet<T> other) { return Equals(other); }
		}
	}
}
