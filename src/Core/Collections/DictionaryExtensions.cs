using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace Raiqub.Toolkit.Core.Collections;

/// <summary><see cref="Dictionary{TKey,TValue}"/> extensions.</summary>
[SuppressMessage("Design", "MA0016:Prefer using collection abstraction instead of implementation")]
public static class DictionaryExtensions
{
    /// <summary>
    /// Adds a key/value pair to the <see cref="Dictionary{TKey,TValue}"/> if the key does not already exist.
    /// </summary>
    /// <param name="source">The dictionary to get or add an element from.</param>
    /// <param name="key">The key of the element to add.</param>
    /// <param name="valueFactory">The function used to generate a value for the key</param>
    /// <exception cref="ArgumentNullException"><paramref name="key"/> is a <see langword="null"/> reference.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="valueFactory"/> is a <see langword="null"/> reference.</exception>
    /// <exception cref="OverflowException">The dictionary contains too many elements.</exception>
    /// <returns>
    /// The value for the key. This will be either the existing value for the key if the key is already in the
    /// dictionary, or the new value for the key as returned by <paramref name="valueFactory"/>
    /// if the key was not in the dictionary.
    /// </returns>
    public static TValue GetOrAdd<TKey, TValue>(
        this Dictionary<TKey, TValue> source,
        TKey key,
        [InstantHandle] Func<TKey, TValue> valueFactory) where TKey : notnull
    {
        ArgumentNullException.ThrowIfNull(valueFactory);

        ref var existingValue = ref CollectionsMarshal.GetValueRefOrAddDefault(source, key, out var exists);

        if (!exists)
            existingValue = valueFactory(key);

        return existingValue!;
    }

    /// <summary>
    /// Adds a key/value pair to the <see cref="Dictionary{TKey,TValue}"/> if the key does not already exist.
    /// </summary>
    /// <param name="source">The dictionary to get or add an element from.</param>
    /// <param name="key">The key of the element to add.</param>
    /// <param name="valueFactory">The function used to generate a value for the key</param>
    /// <param name="factoryArgument">An argument value to pass into <paramref name="valueFactory"/>.</param>
    /// <exception cref="ArgumentNullException"><paramref name="key"/> is a <see langword="null"/> reference.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="valueFactory"/> is a <see langword="null"/> reference.</exception>
    /// <exception cref="OverflowException">The dictionary contains too many elements.</exception>
    /// <returns>
    /// The value for the key. This will be either the existing value for the key if the key is already in the
    /// dictionary, or the new value for the key as returned by <paramref name="valueFactory"/>
    /// if the key was not in the dictionary.
    /// </returns>
    public static TValue GetOrAdd<TKey, TValue, TArg>(
        this Dictionary<TKey, TValue> source,
        TKey key,
        [InstantHandle] Func<TKey, TArg, TValue> valueFactory,
        TArg factoryArgument) where TKey : notnull
    {
        ArgumentNullException.ThrowIfNull(valueFactory);

        ref var existingValue = ref CollectionsMarshal.GetValueRefOrAddDefault(source, key, out var exists);

        if (!exists)
            existingValue = valueFactory(key, factoryArgument);

        return existingValue!;
    }

    /// <summary>
    /// Adds a key/value pair to the <see cref="Dictionary{TKey,TValue}"/> if the key does not already exist.
    /// </summary>
    /// <param name="source">The dictionary to get or add an element from.</param>
    /// <param name="key">The key of the element to add.</param>
    /// <param name="value">the value to be added, if the key does not already exist.</param>
    /// <exception cref="ArgumentNullException"><paramref name="key"/> is a <see langword="null"/> reference.</exception>
    /// <exception cref="OverflowException">The dictionary contains too many elements.</exception>
    /// <returns>
    /// The value for the key. This will be either the existing value for the key if the key is already in the
    /// dictionary, or the new value if the key was not in the dictionary.
    /// </returns>
    public static TValue GetOrAdd<TKey, TValue>(
        this Dictionary<TKey, TValue> source,
        TKey key,
        TValue value) where TKey : notnull
    {
        ref var existingValue = ref CollectionsMarshal.GetValueRefOrAddDefault(source, key, out var exists);

        if (!exists)
            existingValue = value;

        return existingValue!;
    }

    /// <summary>
    /// Adds a key/value pair to the <see cref="Dictionary{TKey,TValue}"/> if the key does not already
    /// exist, or updates a key/value pair in the <see cref="Dictionary{TKey,TValue}"/> if the key
    /// already exists.
    /// </summary>
    /// <param name="source">The dictionary to add or update an element to/from.</param>
    /// <param name="key">The key to be added or whose value should be updated</param>
    /// <param name="addValueFactory">The function used to generate a value for an absent key.</param>
    /// <param name="updateValueFactory">The function used to generate a new value for an existing key based on the key's existing value.</param>
    /// <exception cref="ArgumentNullException"><paramref name="key"/> is a <see langword="null"/> reference.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="addValueFactory"/> is a <see langword="null"/> reference.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="updateValueFactory"/> is a <see langword="null"/> reference.</exception>
    /// <exception cref="OverflowException">The dictionary contains too many elements.</exception>
    /// <returns>
    /// The new value for the key.  This will be either the result of <paramref name="addValueFactory"/>
    /// (if the key was absent) or the result of <paramref name="updateValueFactory"/> (if the key was present).
    /// </returns>
    public static TValue AddOrUpdate<TKey, TValue>(
        this Dictionary<TKey, TValue> source,
        TKey key,
        [InstantHandle] Func<TKey, TValue> addValueFactory,
        [InstantHandle] Func<TKey, TValue, TValue> updateValueFactory)
        where TKey : notnull
    {
        ArgumentNullException.ThrowIfNull(addValueFactory);
        ArgumentNullException.ThrowIfNull(updateValueFactory);

        ref var existingValue = ref CollectionsMarshal.GetValueRefOrAddDefault(source, key, out var exists);

        existingValue = exists
            ? updateValueFactory(key, existingValue!)
            : addValueFactory(key);

        return existingValue;
    }

    /// <summary>
    /// Adds a key/value pair to the <see cref="Dictionary{TKey,TValue}"/> if the key does not already
    /// exist, or updates a key/value pair in the <see cref="Dictionary{TKey,TValue}"/> if the key
    /// already exists.
    /// </summary>
    /// <param name="source">The dictionary to add or update an element to/from.</param>
    /// <param name="key">The key to be added or whose value should be updated</param>
    /// <param name="addValueFactory">The function used to generate a value for an absent key.</param>
    /// <param name="updateValueFactory">The function used to generate a new value for an existing key based on the key's existing value.</param>
    /// <param name="factoryArgument">An argument to pass into <paramref name="addValueFactory"/> and <paramref name="updateValueFactory"/>.</param>
    /// <exception cref="ArgumentNullException"><paramref name="key"/> is a <see langword="null"/> reference.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="addValueFactory"/> is a <see langword="null"/> reference.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="updateValueFactory"/> is a <see langword="null"/> reference.</exception>
    /// <exception cref="OverflowException">The dictionary contains too many elements.</exception>
    /// <returns>
    /// The new value for the key.  This will be either the result of <paramref name="addValueFactory"/>
    /// (if the key was absent) or the result of <paramref name="updateValueFactory"/> (if the key was present).
    /// </returns>
    public static TValue AddOrUpdate<TKey, TValue, TArg>(
        this Dictionary<TKey, TValue> source,
        TKey key,
        [InstantHandle] Func<TKey, TArg, TValue> addValueFactory,
        [InstantHandle] Func<TKey, TValue, TArg, TValue> updateValueFactory,
        TArg factoryArgument) where TKey : notnull
    {
        ArgumentNullException.ThrowIfNull(addValueFactory);
        ArgumentNullException.ThrowIfNull(updateValueFactory);

        ref var existingValue = ref CollectionsMarshal.GetValueRefOrAddDefault(source, key, out var exists);

        existingValue = exists
            ? updateValueFactory(key, existingValue!, factoryArgument)
            : addValueFactory(key, factoryArgument);

        return existingValue;
    }

    /// <summary>
    /// Adds a key/value pair to the <see cref="Dictionary{TKey,TValue}"/> if the key does not already
    /// exist, or updates a key/value pair in the <see cref="Dictionary{TKey,TValue}"/> if the key
    /// already exists.
    /// </summary>
    /// <param name="source">The dictionary to add or update an element to/from.</param>
    /// <param name="key">The key to be added or whose value should be updated</param>
    /// <param name="addValue">The value to be added for an absent key</param>
    /// <param name="updateValueFactory">The function used to generate a new value for an existing key based on the key's existing value.</param>
    /// <exception cref="ArgumentNullException"><paramref name="key"/> is a <see langword="null"/> reference.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="updateValueFactory"/> is a <see langword="null"/> reference.</exception>
    /// <exception cref="OverflowException">The dictionary contains too many elements.</exception>
    /// <returns>
    /// The new value for the key.  This will be either the value of <paramref name="addValue"/>
    /// (if the key was absent) or the result of <paramref name="updateValueFactory"/> (if the key was present).
    /// </returns>
    public static TValue AddOrUpdate<TKey, TValue>(
        this Dictionary<TKey, TValue> source,
        TKey key,
        TValue addValue,
        [InstantHandle] Func<TKey, TValue, TValue> updateValueFactory)
        where TKey : notnull
    {
        ArgumentNullException.ThrowIfNull(updateValueFactory);

        ref var existingValue = ref CollectionsMarshal.GetValueRefOrAddDefault(source, key, out var exists);

        existingValue = exists
            ? updateValueFactory(key, existingValue!)
            : addValue;

        return existingValue;
    }
}
