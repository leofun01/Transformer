namespace DotNetTransformer.Extensions {
	public delegate T Func<T>(T l, T r);
	public delegate bool Comparison<T>(T l, T r);
}
