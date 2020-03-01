using DotNetTransformer.Math.Set;

namespace DotNetTransformer.Math.Group {
	public interface IFiniteGroup<T> : IFiniteSet<T>, IGroup<T>
		where T : IFiniteGroupElement<T>, new()
	{ }
}
