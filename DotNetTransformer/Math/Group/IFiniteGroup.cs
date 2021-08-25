//	IFiniteGroup.cs
//	
//	Based on :
//		Math
//			Abstract algebra
//				Group theory
//					Finite group
//	
//	Author   : leofun01

using DotNetTransformer.Math.Set;

namespace DotNetTransformer.Math.Group {
	public interface IFiniteGroup<T> : IFiniteSet<T>, IGroup<T>
		where T : IFiniteGroupElement<T>, new()
	{
		// bool IsCyclic { get; }
	}
}
