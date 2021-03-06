using DotNetTransformer.Math.Group;
using DotNetTransformer.Math.Permutation;

namespace DotNetTransformer.Math.Transform {
	public interface IFlipRotate<T, out P> : IFiniteGroupElement<T>
		where T : IFlipRotate<T, P>, new()
		where P : IPermutation<P>, new()
	{
		bool IsReflection { get; }
		bool IsRotation { get; }

		P Permutation { get; }
		int Vertex { get; }
	}
}
