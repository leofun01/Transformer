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
			where T : IFiniteGroupElement<T>
		{
			private readonly T _ident;
			private readonly List<T> _list;

			public InternalGroup(IEnumerable<T> collection) {
				_list = new List<T>(collection);
				if(_list.Count < 1)
					throw new ArgumentException("Parameter \"collection\" is empty.\r\nGroup cannot be empty.", "collection");
				T outer = _list[0], inner, new_e;
				_ident = outer.Add(outer.InverseElement);
				if(_ident.CycleLength != 1)
					throw new ArgumentOutOfRangeException("collection",
						"CycleLength of identity element must be equal to 1.");
				int count, outer_i = 0, inner_i;
				do {
					for(count = _list.Count; outer_i < count; ++outer_i) {
						outer = _list[outer_i];
						new_e = outer.InverseElement;
						CheckIdentity(outer, new_e);
						CheckIdentity(new_e, outer);
						for(inner_i = 0; inner_i < count; ++inner_i) {
							inner = _list[inner_i];
							if(!_list.Contains(new_e = outer.Add(inner))) _list.Add(new_e);
							if(!_list.Contains(new_e = inner.Add(outer))) _list.Add(new_e);
						}
					}
				} while(count < _list.Count);
			}
			private void CheckIdentity(T value, T inverse) {
				T ident = value.Add(inverse);
				if(!ident.Equals(_ident))
					throw new ArgumentException(
						string.Concat("{", value, "} + {", inverse, "} = {", ident,
							"}\r\nGroup cannot contain more than one identity element."), "collection");
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
			where T : IFiniteGroupElement<T>
		{
			return ReferenceEquals(collection, null) ? null : new InternalGroup<T>(collection);
		}
		public static bool IsGeneratingSetOf<T>(this IEnumerable<T> collection, FiniteGroup<T> group)
			where T : IFiniteGroupElement<T>
		{
			return !ReferenceEquals(group, null) && group.IsSubsetOf(CreateGroup<T>(collection));
		}
	}
}
