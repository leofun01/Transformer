using System;
using System.Collections.Generic;

namespace DotNetTransformer.Math.Group.Permutation {
	public interface IPermutation<T> : IFiniteGroupElement<T>
		, IEnumerable<int>
		where T : IPermutation<T>, new()
	{
		int this[int index] { get; }

		List<T> GetCycles(Predicate<T> match);
		int GetCyclesCount(Predicate<int> match);
	}
}
