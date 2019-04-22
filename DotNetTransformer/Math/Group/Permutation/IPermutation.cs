using System.Collections.Generic;

namespace DotNetTransformer.Math.Group.Permutation {
	public interface IPermutation<T> : IFiniteGroupElement<T>
		, IEnumerable<byte>
		where T : IPermutation<T>
	{
		byte this[int index] { get; }
	}
}
