//	IPermutation.cs
//	
//	Based on :
//		Math
//			Abstract algebra
//				Group theory
//					Finite group
//					Permutation group
//	
//	Author   : leofun01

using System;
using System.Collections.Generic;
using DotNetTransformer.Math.Group;

namespace DotNetTransformer.Math.Permutation {
	public interface IPermutation<T> : IFiniteGroupElement<T>
		, IEnumerable<int>
		where T : IPermutation<T>, new()
	{
		int this[int index] { get; }

		List<T> GetCycles(Predicate<T> match);
		int GetCyclesCount(Predicate<int> match);
	}
}
