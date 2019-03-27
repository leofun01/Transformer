//	RotateFlipTypeExtension.cs
//	
//	Based on :
//		.Net
//			System.Drawing.RotateFlipType
//	
//	Author   : leofun01

using RotateFlipType = System.Drawing.RotateFlipType;

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
		public static bool IsReflection(this RotateFlipType _this) {
			return (_this & RotateFlipType.RotateNoneFlipX) == RotateFlipType.RotateNoneFlipX;
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
		public static bool IsAxisReflection(this RotateFlipType _this) {
			return (_this & RotateFlipType.Rotate90FlipX) == RotateFlipType.RotateNoneFlipX;
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
		public static bool IsDiagonalReflection(this RotateFlipType _this) {
			return (_this & RotateFlipType.Rotate90FlipX) == RotateFlipType.Rotate90FlipX;
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
		public static bool IsRotation(this RotateFlipType _this) {
			return (_this & RotateFlipType.RotateNoneFlipX) == RotateFlipType.RotateNoneFlipNone;
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
		public static bool IsStraightAngleRotation(this RotateFlipType _this) {
			return (_this & RotateFlipType.Rotate90FlipX) == RotateFlipType.RotateNoneFlipNone;
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
		public static bool IsRightAngleRotation(this RotateFlipType _this) {
			return (_this & RotateFlipType.Rotate90FlipX) == RotateFlipType.Rotate90FlipNone;
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
		public static bool IsSwapDimensions(this RotateFlipType _this) {
			return (_this & RotateFlipType.Rotate90FlipNone) == RotateFlipType.Rotate90FlipNone;
		}

		public static RotateFlipType GetInverseElement(this RotateFlipType _this) {
			byte t = (byte)_this;
			return (RotateFlipType)(0x76541230 >> (t << 2) & 7);
			//return (RotateFlipType)(((t >> 2 ^ 1) & t) << 1 ^ t);
			// return IsRightAngleRotation(_this) ? _this ^ RotateFlipType.RotateNoneFlipXY : _this;
			// return Add(_this, Add(_this, _this));
			// return Add(Add(_this, _this), _this);
			// return Compose(_this, Compose(_this, _this));
			// return Compose(Compose(_this, _this), _this);
		}
		public static int GetCycleLength(this RotateFlipType _this) {
			return 0x22224241 >> ((byte)_this << 2) & 7;
			//return 1 << (0x5598 >> ((byte)_this << 1) & 3);
			//byte t = (byte)_this;
			//return 1 << (20 >> t & 3 | (t >> 2));
		}
		public static RotateFlipType Add(this RotateFlipType _this, RotateFlipType other) {
			byte t = (byte)_this, o = (byte)other;
			return (RotateFlipType)(((t >> 2 ^ t) & o & 1) << 1 ^ t ^ o);
			// return Compose(other, _this);
		}
		public static RotateFlipType Compose(this RotateFlipType _this, RotateFlipType other) {
			byte t = (byte)_this, o = (byte)other;
			return (RotateFlipType)(((o >> 2 ^ o) & t & 1) << 1 ^ t ^ o);
			// return Add(other, _this);
		}
		public static RotateFlipType Times(this RotateFlipType _this, int count) {
			byte t = (byte)_this;
			return (RotateFlipType)((t >> 1 ^ 2) & (t << 1) & count ^ ((count & 1) * t));
		}
	}
}
