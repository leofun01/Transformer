//	FiniteGroupExtension.cs
//	
//	Based on :
//		Math
//			Abstract algebra
//				Group theory
//					Finite group
//	
//	Author   : leofun01
//	Created  : 2018-06-24
//	Modified : 2018-06-24

using System;
using System.Collections.Generic;

namespace DotNetTransformer.Math.Group {
	public static class FiniteGroupExtension
	{
		private class InternalGroup<T> : FiniteGroup<T>
			where T : IFiniteGroupElement<T>
		{
			private readonly T _ident;
			private readonly List<T> _list;

			public InternalGroup(IEnumerable<T> list) {
				_list = new List<T>(list);
				if(_list.Count < 1)
					throw new ArgumentException("Parameter \"list\" is empty. Group cannot be empty.", "list");
				_ident = _list[0].Add(_list[0].InverseElement);
				int count, outer_i = 0, inner_i;
				T outer, inner, new_e;
				do {
					for(count = _list.Count; outer_i < count; ++outer_i) {
						outer = _list[outer_i];
						if(!outer.Add(outer.InverseElement).Equals(_ident) ||
							!outer.InverseElement.Add(outer).Equals(_ident))
							throw new ArgumentException("Group cannot contain more than one identity element.", "list");
						for(inner_i = 0; inner_i < count; ++inner_i) {
							inner = _list[inner_i];
							new_e = outer.Add(inner);
							if(!_list.Contains(new_e)) _list.Add(new_e);
							new_e = inner.Add(outer);
							if(!_list.Contains(new_e)) _list.Add(new_e);
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

		public static FiniteGroup<T> CreateGroup<T>(this IEnumerable<T> list)
			where T : IFiniteGroupElement<T>
		{
			return new InternalGroup<T>(list);
		}
		public static bool IsGeneratingSetOf<T>(this IEnumerable<T> list, FiniteGroup<T> group)
			where T : IFiniteGroupElement<T>
		{
			return group.IsSubsetOf(CreateGroup<T>(list));
		}
	}
}
