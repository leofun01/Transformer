using DotNetTransformer.Math.Group;
using DotNetTransformer.Math.Permutation;

namespace DotNetTransformer.Math.Transform {
	public static class FlipRotateExtension
	{
		public static int GetCycleLength<T, P>(this T _this)
			where T : IFlipRotate<T, P>, new()
			where P : IPermutation<P>, new()
		{
			int c = _this.Permutation.CycleLength;
			return GroupExtension.Times<T>(_this, c).Equals(new T()) ? c : (c << 1);
		}
	}
}
