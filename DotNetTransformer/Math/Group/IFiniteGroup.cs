using DotNetTransformer.Math.Set;

namespace DotNetTransformer.Math.Group {
	public interface IFiniteGroup<T, G> : IFiniteSet<T, G>, IGroup<T>
		where T : IFiniteGroupElement<T>, new()
		where G : IFiniteGroup<T, G>
	{ }
}
