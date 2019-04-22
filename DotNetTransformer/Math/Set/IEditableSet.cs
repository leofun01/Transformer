//	IEditableSet.cs
//	
//	Based on :
//		Math
//			Set theory

using System;

namespace DotNetTransformer.Math.Set {
	public interface IEditableSet<in T, in TSet> : ISet<T>
		where T    : IEquatable<T>
		where TSet : ISet<T>
	{
		// bool Add(T item);
		// bool Remove(T item);
		void UnionWith(TSet other);
		void IntersectWith(TSet other);
		void ExceptWith(TSet other);
		void SymmetricExceptWith(TSet other);
		void Clear();
	}
}
