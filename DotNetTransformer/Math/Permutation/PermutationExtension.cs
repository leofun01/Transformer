using System.Collections.Generic;

namespace DotNetTransformer.Math.Permutation {
	public static class PermutationExtension {
		public static List<T> GetCyclesAll<T>(this T _this)
			where T : IPermutation<T>, new()
		{
			return _this.GetCycles(p => true);
		}
		public static List<T> GetCyclesNonTrivial<T>(this T _this)
			where T : IPermutation<T>, new()
		{
			return _this.GetCycles(p => p.CycleLength > 1);
		}
		public static int GetCyclesCountAll<T>(this T _this)
			where T : IPermutation<T>, new()
		{
			return _this.GetCyclesCount(i => true);
		}
		public static int GetCyclesCountNonTrivial<T>(this T _this)
			where T : IPermutation<T>, new()
		{
			return _this.GetCyclesCount(i => i > 1);
		}
		public static int GetNextVertex<T>(this T p, int v)
			where T : IPermutation<T>, new()
		{
			int r = 0;
			for(byte i = 0; v > 0; ++i) {
				r ^= (v & 1) << p[i];
				v >>= 1;
			}
			return r;
		}
	}
}
