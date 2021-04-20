using DotNetTransformer.Extensions;
using DotNetTransformer.Math.Group;
using DotNetTransformer.Math.Permutation;

namespace DotNetTransformer.Math.Transform {
	internal class FlipRotateGroup<T, P> : FlipRotateSet<T, P>, IFiniteGroup<T>
		where T : IFlipRotate<T, P>, new()
		where P : IPermutation<P>, new()
	{
		public FlipRotateGroup(byte dimensions, Constructor<T, P, int> ctor) : base(dimensions, ctor) { }

		public T IdentityElement { get { return new T(); } }
		public override SetType Type { get { return SetType.Both; } }
	}
}
