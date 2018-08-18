//	ISet.cs
//	
//	Based on :
//		Math
//			Set theory
//	
//	Author   : leofun01

using System;

namespace DotNetTransformer.Math.Set {
	public interface ISet<in T>
		where T : IEquatable<T>
	{
		bool Contains(T item);
	}
}
