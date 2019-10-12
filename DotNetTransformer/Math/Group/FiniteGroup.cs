//	FiniteGroup.cs
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
	public abstract class FiniteGroup<T> : FiniteSet<T>, IGroup<T>
		where T : IFiniteGroupElement<T>, new()
	{
		public virtual T IdentityElement { get { return new T(); } }
		// public virtual bool IsCyclic { get { return Exist(e => e.CycleLength == Count); } }
		// public override bool IsEmpty { get { return false; } }
	}
}
