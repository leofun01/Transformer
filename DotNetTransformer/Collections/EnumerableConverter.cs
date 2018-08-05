using System;
using System.Collections;
using System.Collections.Generic;

namespace DotNetTransformer.Collections {
	public sealed class EnumerableConverter<TIn, TOut> : IEnumerable<TOut>
	{
		private IEnumerable<TIn> _collection;
		private Converter<TIn, TOut> _converter;
		private Predicate<TIn> _match;

		public EnumerableConverter(IEnumerable<TIn> collection, Converter<TIn, TOut> converter) : this(collection, converter, e => true) { }
		public EnumerableConverter(IEnumerable<TIn> collection, Converter<TIn, TOut> converter, Predicate<TIn> match) {
			if(collection == null) throw new ArgumentNullException("collection");
			if(converter == null) throw new ArgumentNullException("converter");
			if(match == null) throw new ArgumentNullException("match");
			_collection = collection;
			_converter = converter;
			_match = match;
		}

		public IEnumerator<TOut> GetEnumerator() {
			IEnumerator<TIn> e = _collection.GetEnumerator();
			while(e.MoveNext()) {
				if(_match(e.Current))
					yield return _converter(e.Current);
			}
		}
		IEnumerator IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}
	}
}
