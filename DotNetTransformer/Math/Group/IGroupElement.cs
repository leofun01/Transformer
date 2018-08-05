//	IGroupElement.cs
//	
//	Based on :
//		Math
//			Abstract algebra
//				Group theory
//	
//	Author   : leofun01
//	Created  : 2018-06-22
//	Modified : 2018-07-02

using System;

namespace DotNetTransformer.Math.Group {
	public interface IGroupElement<T> : IEquatable<T>
		where T : IGroupElement<T>
	{
		T InverseElement { get; }
		T Add(T other);
		T Times(int count);
	}
}
