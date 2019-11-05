using System.Collections.Generic;
using DotNetTransformer.Extensions;

namespace DotNetTransformer.Math.Group {
	public static class GroupExtension
	{
		public static T Compose<T>(this T _this, T other)
			where T : IGroupElement<T>, new()
		{
			return other.Add(_this);
		}
		public static T Subtract<T>(this T _this, T other)
			where T : IGroupElement<T>, new()
		{
			return _this.Add(other.InverseElement);
		}
		public static T InverseAdd<T>(this T t, T o)
			where T : IGroupElement<T>, new()
		{
			return o.InverseElement.Add(t);
		}
		public static T Conjugate<T>(this T t, T o)
			where T : IGroupElement<T>, new()
		{
			return o.InverseElement.Add(t).Add(o);
		}
		public static T GetCommutatorWith<T>(this T t, T o)
			where T : IGroupElement<T>, new()
		{
			return o.Add(t).InverseElement.Add(t.Add(o));
		}

		public static T AddAll<T>(this IEnumerable<T> collection)
			where T : IGroupElement<T>, new()
		{
			return collection.CollectAll<T>((l, r) => l.Add(r));
		}
		public static T ComposeAll<T>(this IEnumerable<T> collection)
			where T : IGroupElement<T>, new()
		{
			return collection.CollectAll<T>(Compose<T>);
		}
	}
}
