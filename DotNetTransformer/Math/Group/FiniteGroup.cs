using DotNetTransformer.Math.Set;

namespace DotNetTransformer.Math.Group {
	public abstract class FiniteGroup<T> : FiniteSet<T>, IFiniteGroup<T>
		where T : IFiniteGroupElement<T>, new()
	{
		public virtual T IdentityElement { get { return new T(); } }
	}
}
