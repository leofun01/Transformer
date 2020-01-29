namespace DotNetTransformer.Extensions {
	public delegate T Generator<T>(T arg);

	public delegate bool Order<in T>(T l, T r);

	public delegate T Func<T>(T l, T r);
	public delegate TOut Func<in TIn, out TOut>(TIn l, TIn r);

	public delegate TOut Constructor<in TIn0, in TIn1, out TOut>(TIn0 arg0, TIn1 arg1);
}
