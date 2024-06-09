using System.Diagnostics;
using System.Runtime.CompilerServices;
using FxKit;

namespace Raiqub.Toolkit.Functional;

/// <summary><see cref="Some{T}"/> extensions.</summary>
public static class Some
{
    /// <summary>
    /// Returns a new failure result if the predicate is false. Otherwise, returns the original result.
    /// </summary>
    /// <typeparam name="TOk">The type of the value in the result.</typeparam>
    /// <typeparam name="TErr">The type of the error in the result.</typeparam>
    /// <param name="source">The input <see cref="Result{TOk,TErr}"/>.</param>
    /// <param name="predicate">The predicate to be evaluated on the value.</param>
    /// <param name="error">The default error value to return if the check fails.</param>
    /// <returns>A <see cref="Result{TOk,TErr}"/> containing either the original value or the specified error.</returns>
    [DebuggerHidden]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TOk, TErr> Ensure<TOk, TErr>(
        this Some<TOk> source,
        Func<TOk, bool> predicate,
        TErr error)
        where TOk : notnull
        where TErr : notnull =>
        predicate(source.Value)
            ? Result<TOk, TErr>.Ok(source.Value)
            : Result<TOk, TErr>.Err(error);

    /// <summary>
    /// Maps the value using the given <paramref name="selector" />.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="selector"></param>
    /// <typeparam name="TSource"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    [DebuggerHidden]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Some<TResult> Map<TSource, TResult>(
        this Some<TSource> source,
        Func<TSource, TResult> selector)
        where TSource : notnull
        where TResult : notnull =>
        new(selector(source.Value));

    /// <inheritdoc cref="Map{T,U}" />
    [DebuggerHidden]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Some<TResult> Select<TSource, TResult>(
        this Some<TSource> source,
        Func<TSource, TResult> selector)
        where TSource : notnull
        where TResult : notnull =>
        Map(source, selector);

    /// <summary>
    /// Invokes the <paramref name="predicate" /> and if it returns <see langword="false"/>,
    /// then the result will be None; Some otherwise.
    /// </summary>
    /// <param name="source"></param>
    /// <param name="predicate"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    [DebuggerHidden]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Option<T> Where<T>(
        this Some<T> source,
        Func<T, bool> predicate)
        where T : notnull =>
        predicate(source.Value) ? Option<T>.Some(source.Value) : Option<T>.None;
}
