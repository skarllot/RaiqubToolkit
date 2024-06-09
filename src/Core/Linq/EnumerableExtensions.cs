using JetBrains.Annotations;

namespace Raiqub.Toolkit.Core.Linq;

/// <summary><see cref="IEnumerable{T}"/> extensions.</summary>
public static class EnumerableExtensions
{
    /// <summary>
    /// Applies an accumulator function over a sequence. If the sequence is empty, the default value is returned.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">A sequence to aggregate over.</param>
    /// <param name="func">An accumulator function to apply to each element.</param>
    /// <param name="defaultValue">The default value to return if the sequence is empty.</param>
    /// <returns>The final accumulator value, or <paramref name="defaultValue"/> if the source sequence is empty.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="func"/> is null.</exception>
    public static TSource AggregateOrDefault<TSource>(
        [InstantHandle] this IEnumerable<TSource> source,
        [InstantHandle] Func<TSource, TSource, TSource> func,
        TSource defaultValue)
    {
        ArgumentNullException.ThrowIfNull(func);

        using var e = source.GetEnumerator();
        if (!e.MoveNext())
        {
            return defaultValue;
        }

        var result = e.Current;
        while (e.MoveNext())
        {
            result = func(result, e.Current);
        }

        return result;
    }

    /// <summary>
    /// Applies an accumulator function over a sequence. If the sequence is empty, the default value is returned.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">A sequence to aggregate over.</param>
    /// <param name="func">An accumulator function to apply to each element.</param>
    /// <param name="defaultValue">The function used to generate the default value to return if the sequence is empty.</param>
    /// <returns>The final accumulator value, or the result of <paramref name="defaultValue"/> if the source sequence is empty.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="func"/> is null.</exception>
    public static TSource AggregateOrDefault<TSource>(
        [InstantHandle] this IEnumerable<TSource> source,
        [InstantHandle] Func<TSource, TSource, TSource> func,
        [InstantHandle] Func<TSource> defaultValue)
    {
        using var e = source.GetEnumerator();
        if (!e.MoveNext())
        {
            return defaultValue();
        }

        var result = e.Current;
        while (e.MoveNext())
        {
            result = func(result, e.Current);
        }

        return result;
    }
}
