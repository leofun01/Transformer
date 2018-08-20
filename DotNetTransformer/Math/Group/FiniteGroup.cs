using DotNetTransformer.Math.Set;

namespace DotNetTransformer.Math.Group {
	public abstract class FiniteGroup<T> : FiniteSet<T>, IGroup<T>
		where T : IFiniteGroupElement<T>
	{
		public abstract T IdentityElement { get; }
		//public virtual bool IsCyclic { get { return Exist(e => e.CycleLength == Count); } }
		//public override bool IsEmpty { get { return false; } }
	}
}
