using FxKit;

namespace Raiqub.Toolkit.Functional;

/// <summary>Represents an existing non-null value of type <typeparamref name="T"/>.</summary>
/// <typeparam name="T">The type of the value. Must be a non-nullable type.</typeparam>
public readonly struct Some<T>
    where T : notnull
{
    /// <summary>The wrapped value.</summary>
    public readonly T Value;

    /// <summary>Initializes a new instance of the <see cref="Some{T}"/> struct with the specified value.</summary>
    /// <param name="value">The value to wrap. Must be non-null.</param>
    public Some(T value) => Value = value;

    /// <summary>
    /// Implicitly converts a <see cref="Some{T}"/> instance to an <see cref="Option{T}"/>.
    /// </summary>
    /// <param name="some">The <see cref="Some{T}"/> instance to convert.</param>
    /// <returns>An <see cref="Option{T}"/> containing the value of the <paramref name="some"/> instance.</returns>
    public static implicit operator Option<T>(Some<T> some) => Option<T>.Some(some.Value);

    /// <summary>Converts the wrapped value to an <see cref="Option{T}"/>.</summary>
    /// <returns>An <see cref="Option{T}"/> containing the wrapped value.</returns>
    public Option<T> ToOption() => Option<T>.Some(Value);

    /// <summary>Converts the wrapped value to a <see cref="Result{TOk,TErr}"/>.</summary>
    /// <typeparam name="TErr">The type of the error value. Must be a non-nullable type.</typeparam>
    /// <returns>A <see cref="Result{TOk,TErr}"/> representing a successful result containing the wrapped value.</returns>
    public Result<T, TErr> ToResult<TErr>() where TErr : notnull => Result<T, TErr>.Ok(Value);
}
