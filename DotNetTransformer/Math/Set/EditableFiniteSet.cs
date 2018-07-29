//	EditableFiniteSet.cs
//	
//	Based on :
//		Math
//			Set theory
//				Finite set
//	
//	Created  : 2018-07-02
//	Modified : 2018-07-16

using System;

namespace DotNetTransformer.Math.Set {
	public abstract class EditableFiniteSet<T> : FiniteSet<T>, IEditableSet<T, FiniteSet<T>>
		where T : IEquatable<T>
	{
		public abstract bool Add(T item);
		public abstract bool Remove(T item);
		public abstract void UnionWith(FiniteSet<T> other);
		public abstract void IntersectWith(FiniteSet<T> other);
		public abstract void ExceptWith(FiniteSet<T> other);
		public abstract void SymmetricExceptWith(FiniteSet<T> other);
		public abstract void Clear();
	}
}
