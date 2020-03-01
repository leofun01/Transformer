using System;
using System.Collections.Generic;
using CultureInfo = System.Globalization.CultureInfo;
using TextWriter = System.IO.TextWriter;

using DotNetTransformer.Math.Permutation;
using DotNetTransformer.Math.Transform;

namespace DotNetTransformer {
	using T = FlipRotate16D;
	using P = PermutationInt64;
	using D = SortedDictionary<int, long>;

	public static class Program {
		public static void Main(string[] args) {
			byte dim = 0;
			string dimStr;
			if(args.GetLength(0) > 0) {
				dimStr = args[0];
			}
			else {
				Console.Write("Enter dimensions count [0..16] : ");
				dimStr = Console.ReadLine();
			}
			try {
				dim = byte.Parse(dimStr, CultureInfo.InvariantCulture);
				var dict = GetHyperCubeCycleCounts(dim);
				Console.WriteLine("\r\n{0}D group size : {1}\r\n", dim, GetValuesSum(dict));
				WriteDictionary(Console.Out, dict, "cLen", "count");
				Console.Write("\r\nRun is complete. ");
			}
			catch(Exception ex) {
				Console.WriteLine("Exception message : {0}", ex.Message);
			}
			Console.Write("\r\nPress enter to exit ... ");
			Console.ReadLine();
		}
		public static D GetHyperCubeCycleCounts(byte dim) {
			if(dim > 16) throw new ArgumentOutOfRangeException();
			D dict = new D();
			foreach(T t in T.GetAllValues(dim)) {
				int cLen = t.CycleLength;
				if(dict.ContainsKey(cLen)) ++dict[cLen];
				else dict.Add(cLen, 1L);
			}
			return dict;
		}
		public static long GetValuesSum(D dict) {
			long sum = 0L;
			foreach(var pair in dict)
				sum += pair.Value;
			return sum;
		}
		public static void WriteDictionary(
			TextWriter writer, D dict,
			string keysHeader,
			string valuesHeader
		) {
			int count = dict.Count;
			string[] keys = new string[count];
			string[] values = new string[count];
			int keyLength = keysHeader.Length;
			int valueLength = valuesHeader.Length;
			int i = 0;
			foreach(var pair in dict) {
				keys[i] = pair.Key.ToString();
				values[i] = pair.Value.ToString();
				if(keyLength < keys[i].Length)
					keyLength = keys[i].Length;
				if(valueLength < values[i].Length)
					valueLength = values[i].Length;
				++i;
			}
			string formatStr = string.Format("{{0,{0}}} : {{1,{1}}}", keyLength, valueLength);
			writer.WriteLine(formatStr, keysHeader, valuesHeader);
			for(i = 0; i < count; ++i) {
				writer.WriteLine(formatStr, keys[i], values[i]);
			}
		}
	}
}
