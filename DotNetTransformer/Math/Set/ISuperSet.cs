//	ISuperSet.cs
//	
//	Based on :
//		Math
//			Set theory
//	
//	Author   : leofun01
//	Created  : 2018-07-15
//	Modified : 2018-07-15

using System;

namespace DotNetTransformer.Math.Set {
	// T    is contravariant
	// TSet is contravariant
	public interface ISuperSet<in T, in TSet> : ISet<T>
		where T    : IEquatable<T>
		where TSet : ISet<T>
	{
		bool IsSupersetOf(TSet other);
	}
	/*//
	public interface ISuperSet<in T, in TSubSet, in TSuperSet> : ISet<T>, ISuperSet<T, TSubSet>
		where T : IEquatable<T>
		where TSubSet : ISubSet<T, TSubSet, TSuperSet>
		where TSuperSet : ISuperSet<T, TSubSet, TSuperSet>
	{ }
	//*/
}
