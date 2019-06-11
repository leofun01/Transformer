using System;

namespace DotNetTransformer.Math.Set {
	public interface IEditableSet<in T, in TSet> : ISet<T>
		where T    : IEquatable<T>
		where TSet : ISet<T>
	{
		void UnionWith(TSet other);
		void IntersectWith(TSet other);
		void ExceptWith(TSet other);
		void SymmetricExceptWith(TSet other);
		void Clear();
	}
}
