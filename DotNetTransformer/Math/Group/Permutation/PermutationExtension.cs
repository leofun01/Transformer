using System.Collections.Generic;

namespace DotNetTransformer.Math.Group.Permutation {
	public static class PermutationExtension {
		public static List<T> GetCyclesAll<T>(this IPermutation<T> _this)
			where T : IPermutation<T>
		{
			return _this.GetCycles(p => true);
		}
		public static List<T> GetCyclesNonTrivial<T>(this IPermutation<T> _this)
			where T : IPermutation<T>
		{
			return _this.GetCycles(p => p.CycleLength > 1);
		}
		public static int GetCyclesCountAll<T>(this IPermutation<T> _this)
			where T : IPermutation<T>
		{
			return _this.GetCyclesCount(i => true);
		}
		public static int GetCyclesCountNonTrivial<T>(this IPermutation<T> _this)
			where T : IPermutation<T>
		{
			return _this.GetCyclesCount(i => i > 1);
		}
	}
}
