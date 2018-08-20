using RotateFlipType = System.Drawing.RotateFlipType;
using FlipRotate2d = DotNetTransformer.Math.Group.FlipRotate2d;

namespace DotNetTransformer.Extensions {
	public static class ArrayExtension
	{
		public static T[] Transform<T>(this T[] array, bool flip) {
			if(array == null) return null;
			int len = array.GetLength(0);
			int ai = flip ? len - 1 : 0, di = flip ? -1 : 1;
			T[] result = new T[len];
			for(int ri = 0; ri < len; ++ri, ai += di)
				result[ri] = array[ai];
			return result;
		}
		public static T[,] Transform<T>(this T[,] array, FlipRotate2d transformation) {
			if(array == null) return null;
			byte t = transformation.Value;
			int dim = t >> 2;
			int w = array.GetLength(dim);
			int h = array.GetLength(dim ^ 1);
			int dx = (t >> 1 ^ t) & 1, dy = t & 1;
			int ax_init = dx * (w - 1); dx = 1 - (dx << 1);
			int ay_init = dy * (h - 1); dy = 1 - (dy << 1);
			T[,] result = new T[w, h];
			int[] a = new int[2];
			a[dim ^ 1] = ax_init;
			for(int rx = 0; rx < w; ++rx, a[dim ^ 1] += dx) {
				a[dim] = ay_init;
				for(int ry = 0; ry < h; ++ry, a[dim] += dy)
					result[rx, ry] = array[a[1], a[0]];
			}
			return result;
		}
		public static T[,] Transform<T>(this T[,] array, RotateFlipType transformation) {
			return Transform<T>(array, FlipRotate2d.FromRotateFlipType(transformation));
		}
		/*/
		private static T[,] Transform<T>(T[,] array, int dimension, int dx, int dy) {
			if(array == null) return null;
			dimension &= 1;
			dx &= 1;
			dy &= 1;
			int w = array.GetLength(dimension);
			int h = array.GetLength(dimension ^ 1);
			int ax_init = dx * (w - 1);
			int ay_init = dy * (h - 1);
			dx = 1 - (dx << 1);
			dy = 1 - (dy << 1);
			T[,] result = new T[w, h];
			int[] a = new int[2];
			a[dimension ^ 1] = ax_init;
			for(int rx = 0; rx < w; ++rx, a[dimension ^ 1] += dx) {
				a[dimension] = ay_init;
				for(int ry = 0; ry < h; ++ry, a[dimension] += dy)
					result[rx, ry] = array[a[1], a[0]];
			}
			return result;
		}
		public static T[,] Transform<T>(this T[,] array, FlipRotate2d transformation) {
			int t = transformation.Value;
			return Transform(array, t >> 2, (t >> 1 ^ t), t);
		}
		public static T[,] Transform<T>(this T[,] array, RotateFlipType transformation) {
			int t = (int)transformation;
			return Transform(array, t, ((t >> 2) ^ (t >> 1) ^ t), t >> 1);
		}
		//*/
	}
}
