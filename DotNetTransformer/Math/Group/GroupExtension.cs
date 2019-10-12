//	GroupExtension.cs
//	
//	Based on :
//		Math
//			Abstract algebra
//				Group theory
//					Group
//	
//	Author   : leofun01

using System.Collections.Generic;
using DotNetTransformer.Extensions;

namespace DotNetTransformer.Math.Group {
	public static class GroupExtension
	{
		public static T Compose<T>(this T _this, T other)
			where T : IGroupElement<T>, new()
		{
			return other.Add(_this);
			// return _this.InverseElement.Add(other.InverseElement).InverseElement;
			// return other.Subtract<T>(_this.InverseElement);
			// return _this.InverseElement.Subtract<T>(other).InverseElement;
		}
		public static T Subtract<T>(this T _this, T other)
			where T : IGroupElement<T>, new()
		{
			return _this.Add(other.InverseElement);
			// return other.Add(_this.InverseElement).InverseElement;
			// return other.InverseElement.Compose<T>(_this);
			// return _this.InverseElement.Compose<T>(other).InverseElement;
		}

		public static T AddAll<T>(this IEnumerable<T> collection)
			where T : IGroupElement<T>, new()
		{
			return collection.CollectAll<T>((l, r) => l.Add(r));
			// return collection.CollectAll<T>((l, r) => r.Compose<T>(l));
		}
		public static T ComposeAll<T>(this IEnumerable<T> collection)
			where T : IGroupElement<T>, new()
		{
			return collection.CollectAll<T>((l, r) => Compose<T>(l, r));
			// return collection.CollectAll<T>((l, r) => r.Add(l));
		}
	}
}
