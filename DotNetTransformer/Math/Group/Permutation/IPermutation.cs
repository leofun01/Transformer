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

using System.Collections.Generic;

namespace DotNetTransformer.Math.Group.Permutation {
	public interface IPermutation<T> : IFiniteGroupElement<T>
		, IEnumerable<byte>
		where T : IPermutation<T>
	{
		byte this[int index] { get; }
	}
}
