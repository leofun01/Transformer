//	FiniteGroupExtension.cs
//	
//	Based on :
//		Math
//			Abstract algebra
//				Group theory
//					Finite group
//	
//	Author   : leofun01

using System;
using System.Collections.Generic;

namespace DotNetTransformer.Math.Group {
	public static class FiniteGroupExtension
	{
		private class InternalGroup<T> : FiniteGroup<T>
			where T : IFiniteGroupElement<T>, new()
		{
			private readonly T _ident;
			private readonly List<T> _list;

			public InternalGroup(IEnumerable<T> collection) {
				_ident = new T();
				_list = new List<T>();
				_list.Add(_ident);
				foreach(T item in collection)
					if(!_list.Contains(item))
						_list.Add(item);
				T outer, inner, new_e;
				int count, outer_i = 0, inner_i;
				do {
					for(count = _list.Count; outer_i < count; ++outer_i) {
						outer = _list[outer_i];
						for(inner_i = 0; inner_i < count; ++inner_i) {
							inner = _list[inner_i];
							if(!_list.Contains(new_e = outer.Add(inner))) _list.Add(new_e);
							if(!_list.Contains(new_e = inner.Add(outer))) _list.Add(new_e);
						}
					}
				} while(count < _list.Count);
			}

			public override T IdentityElement { get { return _ident; } }
			public override int Count { get { return _list.Count; } }
			public override bool Contains(T item) {
				return _list.Contains(item);
			}
			public override IEnumerator<T> GetEnumerator() {
				return _list.GetEnumerator();
			}
		}

		public static FiniteGroup<T> CreateGroup<T>(this IEnumerable<T> collection)
			where T : IFiniteGroupElement<T>, new()
		{
			return new InternalGroup<T>(collection);
		}
		public static bool IsGeneratingSetOf<T>(this IEnumerable<T> collection, FiniteGroup<T> group)
			where T : IFiniteGroupElement<T>, new()
		{
			return !ReferenceEquals(collection, null)
				&& !ReferenceEquals(group, null)
				&& group.IsSubsetOf(CreateGroup<T>(collection));
		}

		public static T Times<T>(this T t, int count)
			where T : IFiniteGroupElement<T>, new()
		{
			int c = t.CycleLength;
			count = (count % c + c) % c;
			T r = (count & 1) != 0 ? t : new T();
			while((count >>= 1) != 0) {
				t = t.Add(t);
				if((count & 1) != 0)
					r = r.Add(t);
			}
			return r;
		}
	}
}
