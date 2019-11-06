using System.Collections.Generic;
using DotNetTransformer.Extensions;

namespace DotNetTransformer.Math.Permutation {
	public static class PermutationExtension
	{
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
		public static void ApplyNextPermutation<T>(this T[] a, int maxLength, Order<T> match) {
			int length = a.GetLength(0);
			if(maxLength > length) maxLength = length;
			int n = 0, i;
			while(++n < maxLength && match(a[n - 1], a[n])) ;
			if(n < maxLength) {
				for(i = 0; match(a[i], a[n]); ++i) ;
				T t = a[n];
				a[n] = a[i];
				a[i] = t;
			}
			for(i = 0; i < --n; ++i) {
				T t = a[n];
				a[n] = a[i];
				a[i] = t;
			}
		}
		public static void ApplyNextPermutation(this int[] a, int maxLength) {
			ApplyNextPermutation<int>(a, maxLength, (int l, int r) => l >= r);
		}
		public static void ApplyPreviousPermutation(this int[] a, int maxLength) {
			ApplyNextPermutation<int>(a, maxLength, (int l, int r) => l <= r);
		}
	}
}
