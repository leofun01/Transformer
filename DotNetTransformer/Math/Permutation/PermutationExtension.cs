using System.Collections.Generic;
using DotNetTransformer.Extensions;
using DotNetTransformer.Math.Set;

namespace DotNetTransformer.Math.Permutation {
	public static class PermutationExtension
	{
		public static IFiniteSet<T> GetCyclesAll<T>(this T _this)
			where T : IPermutation<T>, new()
		{
			return _this.GetCycles(p => true);
		}
		public static IFiniteSet<T> GetCyclesNonTrivial<T>(this T _this)
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

		public static short GetNext<T>(this short v, T p)
			where T : IPermutation<T>, new()
		{
			int r = 0;
			for(byte i = 0; i < 16; ++i)
				r |= (v >> p[i] & 1) << i;
			return (short)r;
		}
		public static short GetPrev<T>(this short v, T p)
			where T : IPermutation<T>, new()
		{
			int r = 0;
			for(byte i = 0; i < 16; ++i)
				r |= (v >> i & 1) << p[i];
			return (short)r;
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
			if(length > maxLength) length = maxLength;
			int n = 0, i;
			while(++n < length && match(a[n - 1], a[n])) ;
			if(n < length) {
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

		public static IEnumerable<T> GetRange<T>(this T start, T stop, int maxLength)
			where T : IPermutation<T>, new()
		{
			return start.GetRange<T>(stop, p => p.GetNextPermutation(maxLength));
		}
	}
}
