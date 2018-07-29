//	IGroup.cs
//	
//	Based on :
//		Math
//			Abstract algebra
//				Group theory
//	
//	Author   : leofun01
//	Created  : 2018-06-22
//	Modified : 2018-07-02

using DotNetTransformer.Math.Set;

namespace DotNetTransformer.Math.Group {
	public interface IGroup<T> : ISet<T>
		where T : IGroupElement<T>
	{
		T IdentityElement { get; }
		//bool IsCyclic { get; }
	}
}
