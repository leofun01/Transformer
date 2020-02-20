using System;

namespace DotNetTransformer.Math.Set {
	public abstract class EditableFiniteSet<T> : FiniteSet<T>, IEditableSet<T, IFiniteSet<T>>
		where T : IEquatable<T>
	{
		public abstract bool Add(T item);
		public abstract bool Remove(T item);
		public abstract void UnionWith(IFiniteSet<T> other);
		public abstract void IntersectWith(IFiniteSet<T> other);
		public abstract void ExceptWith(IFiniteSet<T> other);
		public abstract void SymmetricExceptWith(IFiniteSet<T> other);
		public abstract void Clear();
	}
}
