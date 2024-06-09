using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Raiqub.Toolkit.Functional;

/// <summary>Provides utility methods for functional programming.</summary>
public static class Prelude
{
    /// <summary>Wraps a non-null value in an instance of <see cref="Some{T}"/>.</summary>
    /// <param name="value">The value to wrap. Must be non-null.</param>
    /// <typeparam name="T">The type of the value. Must be a non-nullable type.</typeparam>
    /// <returns>An instance of <see cref="Some{T}"/> containing the specified value.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is null.</exception>
    [DebuggerHidden]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Some<T> Given<T>(T value) where T : notnull
    {
        ArgumentNullException.ThrowIfNull(value);
        return new Some<T>(value);
    }
}
