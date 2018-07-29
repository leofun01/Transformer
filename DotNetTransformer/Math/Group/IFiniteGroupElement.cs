//	IFiniteGroupElement.cs
//	
//	Based on :
//		Math
//			Abstract algebra
//				Group theory
//					Finite group
//	
//	Author   : leofun01
//	Created  : 2018-06-24
//	Modified : 2018-07-02

namespace DotNetTransformer.Math.Group {
	public interface IFiniteGroupElement<T> : IGroupElement<T>
		where T : IFiniteGroupElement<T>
	{
		int CycleLength { get; }
	}
}
