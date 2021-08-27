using System;
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

		public static byte GetNext<T>(this byte v, T p)
			where T : IPermutation<T>, new()
		{
			return (byte)GetNext<T>(v, p, 8);
		}
		public static byte GetPrev<T>(this byte v, T p)
			where T : IPermutation<T>, new()
		{
			return (byte)GetPrev<T>(v, p, 8);
		}
		public static short GetNext<T>(this short v, T p)
			where T : IPermutation<T>, new()
		{
			return (short)GetNext<T>(v, p, 16);
		}
		public static short GetPrev<T>(this short v, T p)
			where T : IPermutation<T>, new()
		{
			return (short)GetPrev<T>(v, p, 16);
		}
		public static int GetNext<T>(this int v, T p)
			where T : IPermutation<T>, new()
		{
			return GetNext<T>(v, p, 32);
		}
		public static int GetPrev<T>(this int v, T p)
			where T : IPermutation<T>, new()
		{
			return GetPrev<T>(v, p, 32);
		}
		public static int GetNext<T>(this int v, T p, byte size)
			where T : IPermutation<T>, new()
		{
			int r = 0;
			for(byte i = 0; i < size; ++i)
				r |= (v >> p[i] & 1) << i;
			return r;
		}
		public static int GetPrev<T>(this int v, T p, byte size)
			where T : IPermutation<T>, new()
		{
			int r = 0;
			for(byte i = 0; i < size; ++i)
				r |= (v >> i & 1) << p[i];
			return r;
		}

		public static void ApplyNextPermutation<T>(this T[] a, IEnumerable<int> indexes, Order<T> match) {
			if(a == null || indexes == null) return;
			if(match == null) throw new ArgumentNullException();
			IEnumerator<int> ie = indexes.GetEnumerator();
			bool moved = ie.MoveNext();
			if(!moved) return;
			int n = ie.Current, p = n;
			Stack<int> stack = new Stack<int>();
			do
				stack.Push(n);
			while((moved = ie.MoveNext()) && match(a[p = n], a[n = ie.Current]));
			if(moved) {
				T t = a[n];
				ie = indexes.GetEnumerator();
				while(ie.MoveNext() && match(a[p = ie.Current], t)) ;
				a[n] = a[p];
				a[p] = t;
			}
			ie = indexes.GetEnumerator();
			while(ie.MoveNext() && (n = ie.Current) < (p = stack.Pop())) {
				T t = a[n];
				a[n] = a[p];
				a[p] = t;
			}
			stack.Clear();
		}
		public static void ApplyNextPermutation<T>(this T[] a, IEnumerable<int> indexes)
			where T : IComparable<T>
		{
			ApplyNextPermutation<T>(a, indexes, (T l, T r) => l != null && l.CompareTo(r) >= 0);
		}
		public static void ApplyPrevPermutation<T>(this T[] a, IEnumerable<int> indexes)
			where T : IComparable<T>
		{
			ApplyNextPermutation<T>(a, indexes, (T l, T r) => l != null && l.CompareTo(r) <= 0);
		}

		public static void ApplyNextPermutation<T>(this T[] a, int maxLength, int indexMask, Order<T> match) {
			int length = a.GetLength(0);
			if(length > maxLength) length = maxLength;
			indexMask &= (1 << length) - 1;
			ApplyNextPermutation<T>(a,
				new Collections.EnumerableConverter<int, int>(
					EnumerableExtension.GetRange<int>(indexMask, 0, (int v) => v >> 1),
					(int v) => {
						int s = 0;
						while((indexMask >> s) != v) ++s;
						return s;
					},
					(int v) => (v & 1) != 0
				), match
			);
		}
		public static void ApplyNextPermutation<T>(this T[] a, int maxLength, int indexMask)
			where T : IComparable<T>
		{
			ApplyNextPermutation<T>(a, maxLength, indexMask, (T l, T r) => l != null && l.CompareTo(r) >= 0);
		}
		public static void ApplyPrevPermutation<T>(this T[] a, int maxLength, int indexMask)
			where T : IComparable<T>
		{
			ApplyNextPermutation<T>(a, maxLength, indexMask, (T l, T r) => l != null && l.CompareTo(r) <= 0);
		}

		public static void ApplyNextPermutation<T>(this T[] a, int maxLength, Order<T> match) {
			if(a == null) return;
			if(match == null) throw new ArgumentNullException();
			int length = a.GetLength(0);
			if(length > maxLength) length = maxLength;
			int n = 0, p = 0;
			while(++n < length && match(a[n - 1], a[n])) ;
			if(n < length) {
				T t = a[n];
				while(match(a[p], t)) ++p;
				a[n] = a[p];
				a[p] = t;
			}
			for(p = 0; p < --n; ++p) {
				T t = a[n];
				a[n] = a[p];
				a[p] = t;
			}
		}
		public static void ApplyNextPermutation<T>(this T[] a, int maxLength)
			where T : IComparable<T>
		{
			ApplyNextPermutation<T>(a, maxLength, (T l, T r) => l != null && l.CompareTo(r) >= 0);
		}
		public static void ApplyPrevPermutation<T>(this T[] a, int maxLength)
			where T : IComparable<T>
		{
			ApplyNextPermutation<T>(a, maxLength, (T l, T r) => l != null && l.CompareTo(r) <= 0);
		}

		public static IEnumerable<T> GetRange<T>(this T start, T stop, int maxLength)
			where T : IPermutation<T>, new()
		{
			return start.GetRange<T>(stop, p => p.GetNextPermutation(maxLength));
		}
	}
}
