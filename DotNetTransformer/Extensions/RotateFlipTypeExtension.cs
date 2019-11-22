using System.Collections.Generic;
using T = System.Drawing.RotateFlipType;

namespace DotNetTransformer.Extensions {
	public static class RotateFlipTypeExtension
	{
		/// <summary><return>
		/// <para>true for
		///		<para>RotateNoneFlipX,</para>
		///		<para>Rotate180FlipY,</para>
		///		<para>RotateNoneFlipY,</para>
		///		<para>Rotate180FlipX,</para>
		///		<para>Rotate90FlipX,</para>
		///		<para>Rotate270FlipY,</para>
		///		<para>Rotate90FlipY,</para>
		///		<para>Rotate270FlipX;</para>
		/// </para>
		/// <para>false for
		///		<para>RotateNoneFlipNone,</para>
		///		<para>Rotate180FlipXY,</para>
		///		<para>RotateNoneFlipXY,</para>
		///		<para>Rotate180FlipNone,</para>
		///		<para>Rotate90FlipNone,</para>
		///		<para>Rotate270FlipXY,</para>
		///		<para>Rotate270FlipNone,</para>
		///		<para>Rotate90FlipXY.</para>
		/// </para>
		/// </return></summary>
		public static bool IsReflection(this T _this) {
			return (_this & T.RotateNoneFlipX) == T.RotateNoneFlipX;
		}
		/// <summary><return>
		/// <para>true for
		///		<para>RotateNoneFlipX,</para>
		///		<para>Rotate180FlipY,</para>
		///		<para>RotateNoneFlipY,</para>
		///		<para>Rotate180FlipX;</para>
		/// </para>
		/// <para>false for
		///		<para>RotateNoneFlipNone,</para>
		///		<para>Rotate180FlipXY,</para>
		///		<para>RotateNoneFlipXY,</para>
		///		<para>Rotate180FlipNone,</para>
		///		<para>Rotate90FlipX,</para>
		///		<para>Rotate270FlipY,</para>
		///		<para>Rotate90FlipY,</para>
		///		<para>Rotate270FlipX,</para>
		///		<para>Rotate90FlipNone,</para>
		///		<para>Rotate270FlipXY,</para>
		///		<para>Rotate270FlipNone,</para>
		///		<para>Rotate90FlipXY.</para>
		/// </para>
		/// </return></summary>
		public static bool IsAxisReflection(this T _this) {
			return (_this & T.Rotate90FlipX) == T.RotateNoneFlipX;
		}
		/// <summary><return>
		/// <para>true for
		///		<para>Rotate90FlipX,</para>
		///		<para>Rotate270FlipY,</para>
		///		<para>Rotate90FlipY,</para>
		///		<para>Rotate270FlipX;</para>
		/// </para>
		/// <para>false for
		///		<para>RotateNoneFlipNone,</para>
		///		<para>Rotate180FlipXY,</para>
		///		<para>RotateNoneFlipXY,</para>
		///		<para>Rotate180FlipNone,</para>
		///		<para>RotateNoneFlipX,</para>
		///		<para>Rotate180FlipY,</para>
		///		<para>RotateNoneFlipY,</para>
		///		<para>Rotate180FlipX,</para>
		///		<para>Rotate90FlipNone,</para>
		///		<para>Rotate270FlipXY,</para>
		///		<para>Rotate270FlipNone,</para>
		///		<para>Rotate90FlipXY.</para>
		/// </para>
		/// </return></summary>
		public static bool IsDiagonalReflection(this T _this) {
			return (_this & T.Rotate90FlipX) == T.Rotate90FlipX;
		}
		/// <summary><return>
		/// <para>true for
		///		<para>RotateNoneFlipNone,</para>
		///		<para>Rotate180FlipXY,</para>
		///		<para>RotateNoneFlipXY,</para>
		///		<para>Rotate180FlipNone,</para>
		///		<para>Rotate90FlipNone,</para>
		///		<para>Rotate270FlipXY,</para>
		///		<para>Rotate270FlipNone,</para>
		///		<para>Rotate90FlipXY;</para>
		/// </para>
		/// <para>false for
		///		<para>RotateNoneFlipX,</para>
		///		<para>Rotate180FlipY,</para>
		///		<para>RotateNoneFlipY,</para>
		///		<para>Rotate180FlipX,</para>
		///		<para>Rotate90FlipX,</para>
		///		<para>Rotate270FlipY,</para>
		///		<para>Rotate90FlipY,</para>
		///		<para>Rotate270FlipX.</para>
		/// </para>
		/// </return></summary>
		public static bool IsRotation(this T _this) {
			return (_this & T.RotateNoneFlipX) == T.RotateNoneFlipNone;
		}
		/// <summary><return>
		/// <para>true for
		///		<para>RotateNoneFlipNone,</para>
		///		<para>Rotate180FlipXY,</para>
		///		<para>RotateNoneFlipXY,</para>
		///		<para>Rotate180FlipNone;</para>
		/// </para>
		/// <para>false for
		///		<para>RotateNoneFlipX,</para>
		///		<para>Rotate180FlipY,</para>
		///		<para>RotateNoneFlipY,</para>
		///		<para>Rotate180FlipX,</para>
		///		<para>Rotate90FlipX,</para>
		///		<para>Rotate270FlipY,</para>
		///		<para>Rotate90FlipY,</para>
		///		<para>Rotate270FlipX,</para>
		///		<para>Rotate90FlipNone,</para>
		///		<para>Rotate270FlipXY,</para>
		///		<para>Rotate270FlipNone,</para>
		///		<para>Rotate90FlipXY.</para>
		/// </para>
		/// </return></summary>
		public static bool IsStraightAngleRotation(this T _this) {
			return (_this & T.Rotate90FlipX) == T.RotateNoneFlipNone;
		}
		/// <summary><return>
		/// <para>true for
		///		<para>Rotate90FlipNone,</para>
		///		<para>Rotate270FlipXY,</para>
		///		<para>Rotate270FlipNone,</para>
		///		<para>Rotate90FlipXY;</para>
		/// </para>
		/// <para>false for
		///		<para>RotateNoneFlipNone,</para>
		///		<para>Rotate180FlipXY,</para>
		///		<para>RotateNoneFlipXY,</para>
		///		<para>Rotate180FlipNone,</para>
		///		<para>RotateNoneFlipX,</para>
		///		<para>Rotate180FlipY,</para>
		///		<para>RotateNoneFlipY,</para>
		///		<para>Rotate180FlipX,</para>
		///		<para>Rotate90FlipX,</para>
		///		<para>Rotate270FlipY,</para>
		///		<para>Rotate90FlipY,</para>
		///		<para>Rotate270FlipX.</para>
		/// </para>
		/// </return></summary>
		public static bool IsRightAngleRotation(this T _this) {
			return (_this & T.Rotate90FlipX) == T.Rotate90FlipNone;
		}
		/// <summary><return>
		/// <para>true for
		///		<para>Rotate90FlipX,</para>
		///		<para>Rotate270FlipY,</para>
		///		<para>Rotate90FlipY,</para>
		///		<para>Rotate270FlipX,</para>
		///		<para>Rotate90FlipNone,</para>
		///		<para>Rotate270FlipXY,</para>
		///		<para>Rotate270FlipNone,</para>
		///		<para>Rotate90FlipXY;</para>
		/// </para>
		/// <para>false for
		///		<para>RotateNoneFlipNone,</para>
		///		<para>Rotate180FlipXY,</para>
		///		<para>RotateNoneFlipXY,</para>
		///		<para>Rotate180FlipNone,</para>
		///		<para>RotateNoneFlipX,</para>
		///		<para>Rotate180FlipY,</para>
		///		<para>RotateNoneFlipY,</para>
		///		<para>Rotate180FlipX.</para>
		/// </para>
		/// </return></summary>
		public static bool IsSwapDimensions(this T _this) {
			return (_this & T.Rotate90FlipNone) == T.Rotate90FlipNone;
		}

		public static T GetInverseElement(this T _this) {
			byte t = (byte)_this;
			return (T)(0x76541230 >> (t << 2) & 7);
			/*//
			return (T)(((t >> 2 ^ 1) & t) << 1 ^ t);
			//*/
		}
		public static int GetCycleLength(this T _this) {
			return 0x22224241 >> ((byte)_this << 2) & 7;
			/*//
			return 1 << (0x5598 >> ((byte)_this << 1) & 3);
			byte t = (byte)_this;
			return 1 << (20 >> t & 3 | (t >> 2));
			//*/
		}
		public static T Add(this T _this, T other) {
			byte t = (byte)_this, o = (byte)other;
			return (T)(((t >> 2 ^ t) & o & 1) << 1 ^ t ^ o);
		}
		public static T Compose(this T _this, T other) {
			byte t = (byte)_this, o = (byte)other;
			return (T)(((o >> 2 ^ o) & t & 1) << 1 ^ t ^ o);
		}
		public static T Subtract(this T _this, T other) {
			byte t = (byte)_this, o = (byte)other;
			return (T)((((t ^ o) >> 2 ^ t ^ 1) & o & 1) << 1 ^ t ^ o);
		}
		public static T Times(this T _this, int count) {
			byte t = (byte)_this;
			return (T)((count & 1) * t ^ ((t >> 1 ^ 2) & (t << 1) & count));
		}

		public static T AddAll(this IEnumerable<T> collection) {
			return collection.CollectAll<T>(Add);
		}
		public static T ComposeAll(this IEnumerable<T> collection) {
			return collection.CollectAll<T>(Compose);
		}
	}
}
