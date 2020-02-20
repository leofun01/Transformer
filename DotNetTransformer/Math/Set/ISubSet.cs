using System;

namespace DotNetTransformer.Math.Set {
	public interface ISubSet<in T, in S> : ISet<T>
		where T : IEquatable<T>
		where S : ISet<T>
	{
		bool IsSubsetOf(S other);
	}
}
