using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using DotNetTransformer.Extensions;

namespace DotNetTransformer {
	[Serializable]
	public sealed class Array1dTransformer<T> : IEquatable<Array1dTransformer<T>>, ICloneable
	{
		private readonly T[] _array;
		private bool _flip;

		public Array1dTransformer(T[] array) {
			if(ReferenceEquals(array, null))
				throw new ArgumentNullException("array");
			_array = array;
		}
		private Array1dTransformer(T[] array, bool flip) {
			_array = array;
			_flip = flip;
		}

		#region System.Array members
		public int Length { get { return _array.Length; } }
		[ComVisible(false)]
		public long LongLength { get { return _array.LongLength; } }
		public int Rank { get { return _array.Rank; } }

		public int GetLength(int dimension) {
			return _array.GetLength(dimension);
		}
		[ComVisible(false)]
		public long GetLongLength(int dimension) {
			return _array.GetLongLength(dimension);
		}
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public int GetLowerBound(int dimension) {
			return _array.GetLowerBound(dimension);
		}
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		public int GetUpperBound(int dimension) {
			return _array.GetUpperBound(dimension);
		}
		#endregion // System.Array members

		public T this[int i] {
			get {
				return _array[_flip ? _array.GetUpperBound(0) - i : i];
			}
			set {
				_array[_flip ? _array.GetUpperBound(0) - i : i] = value;
			}
		}
		public void Apply(bool flip) {
			_flip ^= flip;
		}
		public Array1dTransformer<T> Transform(bool flip) {
			return new Array1dTransformer<T>(_array, _flip ^ flip);
		}
		public Array1dTransformer<T> Clone() { return (Array1dTransformer<T>)MemberwiseClone(); }
		object ICloneable.Clone() { return Clone(); }
		public override bool Equals(object o) {
			return Equals(o as Array1dTransformer<T>);
		}
		public bool Equals(Array1dTransformer<T> o) {
			return !ReferenceEquals(o, null) && ReferenceEquals(_array, o._array) && (_flip == o._flip);
		}
		public override int GetHashCode() {
			return (_flip ? -1 : 0) ^ _array.GetHashCode();
		}
		public T[] ToArray() {
			return _array.Transform<T>(_flip);
		}

		public static bool operator ==(Array1dTransformer<T> l, Array1dTransformer<T> r) { return l.Equals(r); }
		public static bool operator !=(Array1dTransformer<T> l, Array1dTransformer<T> r) { return !l.Equals(r); }

		public static explicit operator T[](Array1dTransformer<T> o) { return o.ToArray(); }
	}
}
