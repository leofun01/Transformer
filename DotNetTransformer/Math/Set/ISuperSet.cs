using System;

namespace DotNetTransformer.Math.Set {
	public interface ISuperSet<in T, in TSet> : ISet<T>
		where T    : IEquatable<T>
		where TSet : ISet<T>
	{
		bool IsSupersetOf(TSet other);
	}
}
