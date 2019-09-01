using System;

namespace DotNetTransformer.Math.Group {
	public interface IGroupElement<T> : IEquatable<T>
		where T : IGroupElement<T>, new()
	{
		T InverseElement { get; }
		T Add(T other);
		T Times(int count);
	}
}
