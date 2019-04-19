//	IFiniteGroupElement.cs
//	
//	Based on :
//		Math
//			Abstract algebra
//				Group theory
//					Finite group
//	
//	Author   : leofun01

namespace DotNetTransformer.Math.Group {
	public interface IFiniteGroupElement<T> : IGroupElement<T>
		where T : IFiniteGroupElement<T>
	{
		int CycleLength { get; }
		T Subtract(T other);
	}
}
