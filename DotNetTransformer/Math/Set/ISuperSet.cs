//	ISuperSet.cs
//	
//	Based on :
//		Math
//			Set theory
//	
//	Author   : leofun01

using System;

namespace DotNetTransformer.Math.Set {
	// T is contravariant
	// S is contravariant
	public interface ISuperSet<in T, in S> : ISet<T>
		where T : IEquatable<T>
		where S : ISet<T>
	{
		bool IsSupersetOf(S other);
	}
	/*//
	public interface ISuperSet<in T, in TSubSet, in TSuperSet> : ISet<T>, ISuperSet<T, TSubSet>
		where T : IEquatable<T>
		where TSubSet : ISubSet<T, TSubSet, TSuperSet>
		where TSuperSet : ISuperSet<T, TSubSet, TSuperSet>
	{ }
	//*/
}
