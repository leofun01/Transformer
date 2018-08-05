using DotNetTransformer.Math.Set;

namespace DotNetTransformer.Math.Group {
	public interface IGroup<T> : ISet<T>
		where T : IGroupElement<T>
	{
		T IdentityElement { get; }
	}
}
