using System;
using System.Collections.Generic;
using CultureInfo = System.Globalization.CultureInfo;
using DotNetTransformer.Math.Permutation;
using DotNetTransformer.Math.Transform;

namespace DotNetTransformer {
	using T = FlipRotate16D;
	using P = PermutationInt64;

	public static class Program {
		public static void Main(string[] args) {
			byte dim = 0;
			if(args.GetLength(0) > 0) {
				try {
					dim = byte.Parse(args[0], CultureInfo.InvariantCulture);
					var dict = GetHyperCubeCycleCounts(dim);
				}
				catch(Exception ex) {
					Console.WriteLine("Exception message: {0}", ex.Message);
				}
			}
		}
		public static SortedDictionary<int, long> GetHyperCubeCycleCounts(byte dim) {
			if(dim > 16) throw new ArgumentOutOfRangeException();
			int pow2 = 1 << dim;
			P p = new P(), pNone = p;
			SortedDictionary<int, long> dict = new SortedDictionary<int, long>();
			do {
				for(int v = 0; v < pow2; ++v) {
					T t = new T(p, v);
					int cLen = t.CycleLength;
					if(dict.ContainsKey(cLen)) ++dict[cLen];
					else dict.Add(cLen, 1);
				}
				p = p.GetNextPermutation(dim);
			} while(p != pNone);
			return dict;
		}
	}
}
