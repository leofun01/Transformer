using System.Collections.Generic;
using DotNetTransformer.Extensions;

namespace DotNetTransformer.Math.Group {
	public static class GroupExtension
	{
		public static T Compose<T>(this T _this, T other)
			where T : IGroupElement<T> {
			return other.Add(_this);
		}
		public static T Subtract<T>(this T _this, T other)
			where T : IGroupElement<T> {
			return _this.Add(other.InverseElement);
		}

		public static T AddAll<T>(this IEnumerable<T> collection)
			where T : IGroupElement<T> {
			return collection.CollectAll<T>((l, r) => l.Add(r));
		}
		public static T ComposeAll<T>(this IEnumerable<T> collection)
			where T : IGroupElement<T> {
			return collection.CollectAll<T>((l, r) => Compose<T>(l, r));
		}
	}
}