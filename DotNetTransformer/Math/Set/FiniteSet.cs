//	FiniteSet.cs
//	
//	Based on :
//		Math
//			Set theory
//				Finite set
//	
//	Author   : leofun01

using System;
using System.Collections;
using System.Collections.Generic;

namespace DotNetTransformer.Math.Set {
	public abstract partial class FiniteSet<T> : ISet<T>, IEnumerable<T>
		//, ISubSet<T, FiniteSet<T>>, ISubSet<T, FiniteSet<T>, FiniteSet<T>>
		, ISubSet<T, ISet<T>>//, ISubSet<T, ISet<T>, ISet<T>>
		//, ISuperSet<T, FiniteSet<T>>, ISuperSet<T, FiniteSet<T>, FiniteSet<T>>
		, ISuperSet<T, ISubSet<T, FiniteSet<T>>>
		where T : IEquatable<T>
	{
		//public bool IsCountable { get { return true; } }
		//public bool IsFinite { get { return true; } }
		//public virtual bool IsEmpty { get { return !GetEnumerator().MoveNext(); } }
		public abstract int Count { get; }
		public virtual bool Contains(T item) {
			return Exist(e => e.Equals(item));
		}
		public bool Exist(Predicate<T> match) {
			foreach(T item in this)
				if(match(item))
					return true;
			return false;
		}
		public abstract IEnumerator<T> GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}
		public virtual bool IsSubsetOf(ISet<T> other) {
			return !Exist(e => !other.Contains(e));
		}
		public bool IsSupersetOf(ISubSet<T, FiniteSet<T>> other) {
			return other.IsSubsetOf(this);
		}
	}
}
