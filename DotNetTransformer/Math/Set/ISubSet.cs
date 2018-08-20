using System;

namespace DotNetTransformer.Math.Set {
	public interface ISubSet<in T, in TSet> : ISet<T>
		where T    : IEquatable<T>
		where TSet : ISet<T>
	{
		bool IsSubsetOf(TSet other);
	}
}
