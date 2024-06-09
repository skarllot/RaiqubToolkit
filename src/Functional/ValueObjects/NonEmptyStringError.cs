namespace Raiqub.Toolkit.Functional.ValueObjects;

/// <summary>
/// Specifies the errors that can occur when creating a <see cref="NonEmptyString"/>.
/// </summary>
public enum NonEmptyStringError
{
    /// <summary>Indicates that the input string was null.</summary>
    Null = 1,

    /// <summary>Indicates that the input string was empty or whitespace.</summary>
    Empty,
}
