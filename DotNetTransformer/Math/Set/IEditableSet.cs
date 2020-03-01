//	IEditableSet.cs
//	
//	Based on :
//		Math
//			Set theory

using System;

namespace DotNetTransformer.Math.Set {
	public interface IEditableSet<in T, in S> : ISet<T>
		where T : IEquatable<T>
		where S : ISet<T>
	{
		// bool Add(T item);
		// bool Remove(T item);
		void UnionWith(S other);
		void IntersectWith(S other);
		void ExceptWith(S other);
		void SymmetricExceptWith(S other);
		void Clear();
	}
}
