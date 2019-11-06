namespace DotNetTransformer.Extensions {
	public delegate T Func<T>(T l, T r);
	public delegate bool Order<T>(T l, T r);
}
