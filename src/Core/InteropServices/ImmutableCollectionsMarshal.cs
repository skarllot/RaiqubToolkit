using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace Raiqub.Toolkit.Core.InteropServices;

/// <summary>
/// An unsafe class that provides a set of methods to access the underlying data representations of immutable collections.
/// </summary>
public static class ImmutableCollectionsMarshal
{
    /// <summary>
    /// Gets an <see cref="ImmutableArray{T}"/> value wrapping the input <typeparamref name="T"/> array.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input array.</typeparam>
    /// <param name="array">The input array to wrap in the returned <see cref="ImmutableArray{T}"/> value.</param>
    /// <returns>An <see cref="ImmutableArray{T}"/> value wrapping <paramref name="array"/>.</returns>
    /// <remarks>
    /// <para>
    /// When using this method, callers should take extra care to ensure that they're the sole owners of the input
    /// array, and that it won't be modified once the returned <see cref="ImmutableArray{T}"/> value starts being
    /// used. Doing so might cause undefined behavior in code paths which don't expect the contents of a given
    /// <see cref="ImmutableArray{T}"/> values to change after its creation.
    /// </para>
    /// <para>
    /// If <paramref name="array"/> is <see langword="null"/>, the returned <see cref="ImmutableArray{T}"/> value
    /// will be uninitialized (ie. its <see cref="ImmutableArray{T}.IsDefault"/> property will be <see langword="true"/>).
    /// </para>
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ImmutableArray<T> AsImmutableArray<T>(T[]? array)
    {
        return System.Runtime.InteropServices.ImmutableCollectionsMarshal.AsImmutableArray(array);
    }

    /// <summary>
    /// Gets the underlying <typeparamref name="T"/> array for an input <see cref="ImmutableArray{T}"/> value.
    /// </summary>
    /// <typeparam name="T">The type of elements in the input <see cref="ImmutableArray{T}"/> value.</typeparam>
    /// <param name="array">The input <see cref="ImmutableArray{T}"/> value to get the underlying <typeparamref name="T"/> array from.</param>
    /// <returns>The underlying <typeparamref name="T"/> array for <paramref name="array"/>, if present.</returns>
    /// <remarks>
    /// <para>
    /// When using this method, callers should make sure to not pass the resulting underlying array to methods that
    /// might mutate it. Doing so might cause undefined behavior in code paths using <paramref name="array"/> which
    /// don't expect the contents of the <see cref="ImmutableArray{T}"/> value to change.
    /// </para>
    /// <para>
    /// If <paramref name="array"/> is uninitialized (ie. its <see cref="ImmutableArray{T}.IsDefault"/> property is
    /// <see langword="true"/>), the resulting <typeparamref name="T"/> array will be <see langword="null"/>.
    /// </para>
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T[]? AsArray<T>(ImmutableArray<T> array)
    {
        return System.Runtime.InteropServices.ImmutableCollectionsMarshal.AsArray(array);
    }

    /// <summary>
    /// Get a <see cref="Span{T}"/> view over a <see cref="ImmutableArray{T}.Builder"/>'s data.
    /// Items should not be added or removed from the <see cref="ImmutableArray{T}.Builder"/> while the <see cref="Span{T}"/> is in use.
    /// </summary>
    /// <param name="builder">The immutable array builder to get the data view over.</param>
    /// <typeparam name="T">The type of the elements in the builder.</typeparam>
    public static Span<T> AsSpan<T>(ImmutableArray<T>.Builder? builder)
    {
        if (builder is null)
        {
            return default;
        }

        var accessor = Unsafe.As<ImmutableArray<T>.Builder, ImmutableArrayBuilderAccessor<T>>(ref builder);
        return new Span<T>(accessor.Elements, 0, accessor.Count);
    }

    /// <summary>
    /// Sets the count of the <see cref="ImmutableArray{T}.Builder"/> to the specified value.
    /// </summary>
    /// <param name="builder">The immutable array builder to set the count of.</param>
    /// <param name="count">The value to set the builder's count to.</param>
    /// <typeparam name="T">The type of the elements in the builder.</typeparam>
    /// <exception cref="NullReferenceException"><paramref name="builder"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is negative.</exception>
    /// <remarks>When increasing the count, uninitialized data is being exposed.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SetCount<T>(ImmutableArray<T>.Builder builder, int count)
    {
        builder.Count = count;
    }

    private sealed class ImmutableArrayBuilderAccessor<T>
    {
        public T[] Elements;
        public int Count;

        public ImmutableArrayBuilderAccessor(T[] elements, int count)
        {
            Elements = elements;
            Count = count;
        }
    }
}
