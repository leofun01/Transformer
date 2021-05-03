using System;
using System.Collections.Generic;
using DotNetTransformer.Math.Group;
using DotNetTransformer.Math.Set;

namespace DotNetTransformer.Math.Permutation {
	public interface IPermutation<T> : IFiniteGroupElement<T>
		, IEnumerable<int>
		, IComparable<T>
		where T : IPermutation<T>, new()
	{
		int this[int index] { get; }
		int SwapsCount { get; }

		bool ReducibleTo(int length);
		T Swap(int i, int j);

		T GetNextPermutation(int maxLength);
		T GetPrevPermutation(int maxLength);

		IFiniteSet<T> GetCycles(Predicate<T> match);
		int GetCyclesCount(Predicate<int> match);

		int[] ToArray();
	}
}
