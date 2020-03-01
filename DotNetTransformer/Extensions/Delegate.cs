namespace DotNetTransformer.Extensions {
	public delegate T Generator<T>(T arg);

	public delegate bool Order<in T>(T l, T r);

	public delegate T Func<T>(T l, T r);
	public delegate TOut Func<out TOut, in TIn>(TIn l, TIn r);

	public delegate TOut Constructor<out TOut, in TIn0, in TIn1>(TIn0 arg0, TIn1 arg1);
}
