using System;

namespace DotNetTransformer.Math.Set {
	public interface ISuperSet<in T, in S> : ISet<T>
		where T : IEquatable<T>
		where S : ISet<T>
	{
		bool IsSupersetOf(S other);
	}
}
