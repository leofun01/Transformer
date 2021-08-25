using System;

namespace DotNetTransformer.Math.Transform {
	[Flags]
	public enum SetType : byte
	{
		None = 0,
		Rotations = 1,
		Reflections = 2,
		Both = 3,
		Other = 16
	}
}
