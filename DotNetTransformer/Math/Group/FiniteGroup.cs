using DotNetTransformer.Math.Set;

namespace DotNetTransformer.Math.Group {
	public abstract class FiniteGroup<T> : FiniteSet<T>, IGroup<T>
		where T : IFiniteGroupElement<T>
	{
		public abstract T IdentityElement { get; }
	}
}
