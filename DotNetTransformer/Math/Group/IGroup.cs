//	IGroup.cs
//	
//	Based on :
//		Math
//			Abstract algebra
//				Group theory
//	
//	Author   : leofun01

using DotNetTransformer.Math.Set;

namespace DotNetTransformer.Math.Group {
	public interface IGroup<T> : ISet<T>
		where T : IGroupElement<T>, new()
	{
		T IdentityElement { get; }
		// bool IsCyclic { get; }
	}
}
