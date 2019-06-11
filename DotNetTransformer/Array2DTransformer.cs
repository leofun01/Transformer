//	Array2DTransformer.cs
//	
//	Based on :
//		.Net
//			System.Array
//	
//	Author   : leofun01

using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using DotNetTransformer.Extensions;
using FlipRotate2D = DotNetTransformer.Math.Group.Transform2D.FlipRotate2D;

namespace DotNetTransformer {
	[Serializable]
	public class Array2DTransformer<T> : IEquatable<Array2DTransformer<T>>, ICloneable
	{
		private readonly T[,] _array;
		private FlipRotate2D _transformation;

		public Array2DTransformer(T[,] array) {
			if(ReferenceEquals(array, null))
				throw new ArgumentNullException("array");
			_array = array;
		}

		#region System.Array members
		public int Length { get { return _array.Length; } }
		[ComVisible(false)]
		public long LongLength { get { return _array.LongLength; } }
		public int Rank { get { return _array.Rank; } }

		public int GetLength(int dimension) {
			if((dimension & -2) != 0)
				throw new IndexOutOfRangeException();
			return _array.GetLength(_transformation.Value >> 2 ^ dimension);
		}
		[ComVisible(false)]
		public long GetLongLength(int dimension) {
			if((dimension & -2) != 0)
				throw new IndexOutOfRangeException();
			return _array.GetLongLength(_transformation.Value >> 2 ^ dimension);
		}
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public int GetLowerBound(int dimension) {
			if((dimension & -2) != 0)
				throw new IndexOutOfRangeException();
			return _array.GetLowerBound(_transformation.Value >> 2 ^ dimension);
		}
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public int GetUpperBound(int dimension) {
			if((dimension & -2) != 0)
				throw new IndexOutOfRangeException();
			return _array.GetUpperBound(_transformation.Value >> 2 ^ dimension);
		}
		#endregion // System.Array members

		public virtual T this[int x, int y] {
			get {
				int[] i = _getIndexes(x, y);
				return _array[i[0], i[1]];
				//return (T)_array.GetValue(i);
			}
			set {
				int[] i = _getIndexes(x, y);
				_array[i[0], i[1]] = value;
				//_array.SetValue(value, i);
			}
		}
		private int[] _getIndexes(int x, int y) {
				byte t = _transformation.Value;
				int dim = t >> 2;
				int[] i = new int[2];
				i[dim] = ((t >> 1 ^ t) & 1) == 0 ? x : _array.GetUpperBound(dim) - x;
				i[dim ^ 1] = (t & 1) == 0 ? y : _array.GetUpperBound(dim ^ 1) - y;
				return i;
		}
		public virtual void Apply(FlipRotate2D transformation) {
			_transformation += transformation;
		}
		public Array2DTransformer<T> Transform(FlipRotate2D transformation) {
			Array2DTransformer<T> o = Clone();
			o.Apply(transformation);
			return o;
		}
		public Array2DTransformer<T> Clone() { return (Array2DTransformer<T>)MemberwiseClone(); }
		object ICloneable.Clone() { return Clone(); }
		public override bool Equals(object o) {
			return Equals(o as Array2DTransformer<T>);
		}
		public virtual bool Equals(Array2DTransformer<T> o) {
			return !ReferenceEquals(o, null) && ReferenceEquals(_array, o._array) && _transformation == o._transformation;
		}
		public override int GetHashCode() {
			return 0x11111111 * _transformation.Value ^ _array.GetHashCode();
		}
		public virtual T[,] ToArray() {
			return _array.Transform<T>(_transformation);
			/*//
			byte t = _transformation.Value;
			int dim = t >> 2;
			int w = _array.GetLength(dim);
			int h = _array.GetLength(dim ^ 1);
			int dx = (t >> 1 ^ t) & 1, dy = t & 1;
			int x_init = dx * (w - 1); dx = 1 - (dx << 1);
			int y_init = dy * (h - 1); dy = 1 - (dy << 1);
			T[,] result = new T[w, h];
			int[] i = new int[2];
			i[dim ^ 1] = x_init;
			for(int rx = 0; rx < w; ++rx, i[dim ^ 1] += dx) {
				i[dim] = y_init;
				for(int ry = 0; ry < h; ++ry, i[dim] += dy)
					result[rx, ry] = _array[i[1], i[0]];
			}
			return result;
			//*/
		}

		public static bool operator ==(Array2DTransformer<T> l, Array2DTransformer<T> r) { return l.Equals(r); }
		public static bool operator !=(Array2DTransformer<T> l, Array2DTransformer<T> r) { return !l.Equals(r); }

		public static explicit operator T[,](Array2DTransformer<T> o) { return o.ToArray(); }
	}
}
