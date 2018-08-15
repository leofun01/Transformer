using System;
using System.Collections.Generic;

namespace DotNetTransformer.Math.Set {
	public abstract partial class FiniteSet<T> : ISet<T>, IEnumerable<T>
		where T : IEquatable<T>
	{
		public static readonly FiniteSet<T> Empty = new EmptySet();
		private sealed class EmptySet : FiniteSet<T>
		{
			public EmptySet() { }

			public override int Count { get { return 0; } }
			public override bool Contains(T item) { return false; }
			public override IEnumerator<T> GetEnumerator() { yield break; }
			public override bool IsSubsetOf(ISet<T> other) { return true; }
		}
	}
}
