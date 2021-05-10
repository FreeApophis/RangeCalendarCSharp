using System;
using System.Collections.Generic;
using System.Linq;

namespace Calendar
{
    public delegate T Reader<in TEnvironment, out T>(TEnvironment environment);

    public static partial class ReaderExtensions
    {
        // SelectMany: (Reader<TEnvironment, TSource>, TSource -> Reader<TEnvironment, TSelector>, (TSource, TSelector) -> TResult) -> Reader<TEnvironment, TResult>
        public static Reader<TEnvironment, TResult> SelectMany<TEnvironment, TSource, TSelector, TResult>(
            this Reader<TEnvironment, TSource> source,
            Func<TSource, Reader<TEnvironment, TSelector>> selector,
            Func<TSource, TSelector, TResult> resultSelector) =>
                environment =>
                {
                    TSource value = source(environment);
                    return resultSelector(value, selector(value)(environment));
                };

        // Wrap: TSource -> Reader<TEnvironment, TSource>
        public static Reader<TEnvironment, TSource> Reader<TEnvironment, TSource>(this TSource value) =>
            environment => value;

        // Select: (Reader<TEnvironment, TSource>, TSource -> TResult) -> Reader<TEnvironment, TResult>
        public static Reader<TEnvironment, TResult> Select<TEnvironment, TSource, TResult>(
            this Reader<TEnvironment, TSource> source, Func<TSource, TResult> selector) =>
                source.SelectMany(value => selector(value).Reader<TEnvironment, TResult>(), (value, result) => result);

        // Flip the inner (Reader) and outer (Sequence) monad => Apply Environment on each element
        public static Reader<TEnvironment, IEnumerable<TElement>> FlipMonad<TEnvironment, TElement>(this IEnumerable<Reader<TEnvironment, TElement>> sequence)
            => environment
                => from element in sequence
                   select element(environment);
    }
}
