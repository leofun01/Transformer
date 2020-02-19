using System;
using System.Collections.Generic;

namespace DotNetTransformer.Math.Set {
	public interface IFiniteSet<T, S> : ISet<T>, IEnumerable<T>
		, IEquatable<S>
		, ISubSet<T, S>
		, ISuperSet<T, S>
		where T : IEquatable<T>
		where S : IFiniteSet<T, S>
	{
		long Count { get; }
		bool IsSubsetOf(ISet<T> other);
	}
}
