//	IFiniteSet.cs
//	
//	Based on :
//		Math
//			Set theory
//				Finite set
//	
//	Author   : leofun01

using System;
using System.Collections.Generic;

namespace DotNetTransformer.Math.Set {
	public interface IFiniteSet<T> : ISet<T>, IEnumerable<T>
		, IEquatable<IFiniteSet<T>>
		, ISubSet<T, ISet<T>>
		, ISubSet<T, IFiniteSet<T>>
		, ISuperSet<T, IFiniteSet<T>>
		// , ISubSet<T, IFiniteSet<T>, IFiniteSet<T>>
		// , ISuperSet<T, IFiniteSet<T>, IFiniteSet<T>>
		where T : IEquatable<T>
	{
		long Count { get; }
	}
}
