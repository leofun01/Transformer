using System;

namespace DotNetTransformer.Math.Set {
	// T    is contravariant
	// TSet is contravariant
	public interface ISubSet<in T, in TSet> : ISet<T>
		where T    : IEquatable<T>
		where TSet : ISet<T>
	{
		bool IsSubsetOf(TSet other);
	}
	/*//
	public interface ISubSet<in T, in TSubSet, in TSuperSet> : ISet<T>, ISubSet<T, TSuperSet>
		where T : IEquatable<T>
		where TSubSet : ISubSet<T, TSubSet, TSuperSet>
		where TSuperSet : ISuperSet<T, TSubSet, TSuperSet>
	{ }
	//*/
}
