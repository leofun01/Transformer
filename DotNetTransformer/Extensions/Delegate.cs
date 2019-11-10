namespace DotNetTransformer.Extensions {
	public delegate bool Order<in T>(T l, T r);
	public delegate T Func<T>(T l, T r);
	public delegate TOut Func<in TIn, out TOut>(TIn l, TIn r);
}
