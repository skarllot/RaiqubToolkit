using FxKit;
using FxKit.Parsers;
using Generator.Equals;
using static FxKit.Prelude;

namespace Raiqub.Toolkit.Functional.ValueObjects;

/// <summary>
/// Represents a non-empty string that cannot be null or whitespace.
/// </summary>
[Equatable]
public sealed partial class NonEmptyString
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NonEmptyString"/> class with the specified value.
    /// </summary>
    /// <param name="value">The non-empty string value. Must not be null or whitespace.</param>
    private NonEmptyString(string value) => Value = value;

    /// <summary>
    /// Implicitly converts a <see cref="NonEmptyString"/> to a <see cref="string"/>.
    /// </summary>
    /// <param name="nonEmptyString">The <see cref="NonEmptyString"/> instance.</param>
    /// <returns>The underlying string value.</returns>
    public static implicit operator string(NonEmptyString nonEmptyString) => nonEmptyString.Value;

    /// <summary>Gets the underlying string value.</summary>
    [StringEquality(StringComparison.Ordinal)]
    public string Value { get; }

    /// <summary>
    /// Creates a new instance of <see cref="NonEmptyString"/> from the specified string value.
    /// </summary>
    /// <param name="value">The string value to create the <see cref="NonEmptyString"/> from.</param>
    /// <returns>
    /// A <see cref="Result{TOk,TErr}"/> containing the <see cref="NonEmptyString"/> if successful,
    /// or a <see cref="NonEmptyStringError"/> if the input string is null or whitespace.
    /// </returns>
    public static Result<NonEmptyString, NonEmptyStringError> Create(string value)
    {
        return from v in Optional(value).OkOr(NonEmptyStringError.Null)
            from parsed in StringParser.NonNullOrWhiteSpace(v).OkOr(NonEmptyStringError.Empty)
            select new NonEmptyString(parsed.Trim());
    }

    /// <inheritdoc />
    public override string ToString() => Value;
}
