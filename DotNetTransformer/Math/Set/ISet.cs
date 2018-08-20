using System;

namespace DotNetTransformer.Math.Set {
	// T is contravariant
	public interface ISet<in T>
		where T : IEquatable<T>
	{
		//bool IsCountable { get; }
		//bool IsFinite { get; }
		//bool IsEmpty { get; }
		bool Contains(T item);
	}
}
