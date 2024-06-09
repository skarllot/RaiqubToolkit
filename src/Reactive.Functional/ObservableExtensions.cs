using System.Reactive.Linq;
using FxKit;

namespace Raiqub.Toolkit.Reactive.Functional;

/// <summary><see cref="IObservable{T}"/> extensions.</summary>
public static class ObservableExtensions
{
    /// <summary>
    /// Filters the successful results from an observable sequence of <see cref="Result"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of the value in the result.</typeparam>
    /// <typeparam name="TError">The type of the error in the result.</typeparam>
    /// <param name="source">The input observable sequence of results.</param>
    /// <returns>An observable sequence containing the successful values.</returns>
    public static IObservable<TResult> Successes<TResult, TError>(this IObservable<Result<TResult, TError>> source)
        where TResult : notnull
        where TError : notnull
    {
        return source.Where(x => x.IsOk).Select(x => x.Unwrap());
    }

    /// <summary>
    /// Filters the error results from an observable sequence of <see cref="Result{TOk,TErr}"/>.
    /// </summary>
    /// <typeparam name="TResult">The type of the value in the result.</typeparam>
    /// <typeparam name="TError">The type of the error in the result.</typeparam>
    /// <param name="source">The input observable sequence of results.</param>
    /// <returns>An observable sequence containing the error values.</returns>
    public static IObservable<TError> Failures<TResult, TError>(this IObservable<Result<TResult, TError>> source)
        where TResult : notnull
        where TError : notnull
    {
        return source.Where(x => !x.IsOk).Select(x => x.UnwrapErr());
    }
}
