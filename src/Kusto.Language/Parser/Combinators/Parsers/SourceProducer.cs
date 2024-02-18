using System;
using System.Collections.Generic;

namespace Kusto.Language.Parsing
{
    using Utils;

    /// <summary>
    /// A function that converts one or more input elements into a single output element.
    /// </summary>
    /// <typeparam name="TInput">The type of the input elements.</typeparam>
    /// <typeparam name="TOutput">The type of the produced output.</typeparam>
    /// <param name="source">The source of the input elements.</param>
    /// <param name="start">The starting offset of the first input element.</param>
    /// <param name="length">The number of input elements successfully scanned.</param>
    public delegate TOutput SourceProducer<TInput, TOutput>(Source<TInput> source, int start, int length);
}