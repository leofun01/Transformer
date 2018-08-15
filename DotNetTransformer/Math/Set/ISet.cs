using System;

namespace DotNetTransformer.Math.Set {
	public interface ISet<in T>
		where T : IEquatable<T>
	{
		bool Contains(T item);
	}
}
