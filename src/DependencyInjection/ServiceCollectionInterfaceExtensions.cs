using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Raiqub.Toolkit.DependencyInjection;

/// <summary><see cref="IServiceCollection"/> extensions for registering services and its interfaces.</summary>
public static class ServiceCollectionInterfaceExtensions
{
    /// <summary>
    /// Adds the specified <typeparamref name="TService"/> and its implemented interfaces using the specified
    /// <paramref name="lifetime"/> to the collection if them hasn't already been registered.
    /// </summary>
    /// <typeparam name="TService">The type of the service to add.</typeparam>
    /// <param name="collection">The <see cref="IServiceCollection"/>.</param>
    /// <param name="lifetime">The lifetime of the service.</param>
    public static void TryAddWithInterfaces<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        TService>(
        this IServiceCollection collection,
        ServiceLifetime lifetime)
        where TService : class
    {
        collection.TryAdd(ServiceDescriptor.Describe(typeof(TService), typeof(TService), lifetime));

        if (lifetime is ServiceLifetime.Singleton or ServiceLifetime.Scoped)
        {
            TryAddRedirect<TService>(collection);
        }
        else
        {
            TryAddCopy(collection, typeof(TService));
        }
    }

    /// <summary>
    /// Adds the specified <typeparamref name="TService"/> and its implemented interfaces as a
    /// <see cref="ServiceLifetime.Singleton"/> service to the collection if them hasn't already been registered.
    /// </summary>
    /// <typeparam name="TService">The type of the service to add.</typeparam>
    /// <param name="collection">The <see cref="IServiceCollection"/>.</param>
    /// <param name="instance">The instance of the service to add.</param>
    public static void TryAddSingletonWithInterfaces<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        TService>(
        this IServiceCollection collection,
        TService instance)
        where TService : class
    {
        collection.TryAddSingleton(instance);

        TryAddRedirect<TService>(collection);
    }

    /// <summary>
    /// Adds the specified <typeparamref name="TService"/> and its implemented interfaces as a
    /// <see cref="ServiceLifetime.Singleton"/> service to the collection if them hasn't already been registered.
    /// </summary>
    /// <typeparam name="TService">The type of the service to add.</typeparam>
    /// <param name="collection">The <see cref="IServiceCollection"/>.</param>
    public static void TryAddSingletonWithInterfaces<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        TService>(
        this IServiceCollection collection)
        where TService : class
    {
        TryAddWithInterfaces<TService>(collection, ServiceLifetime.Singleton);
    }

    /// <summary>
    /// Adds the specified <typeparamref name="TService"/> and its implemented interfaces as a
    /// <see cref="ServiceLifetime.Scoped"/> service to the collection if them hasn't already been registered.
    /// </summary>
    /// <typeparam name="TService">The type of the service to add.</typeparam>
    /// <param name="collection">The <see cref="IServiceCollection"/>.</param>
    public static void TryAddScopedWithInterfaces<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        TService>(
        this IServiceCollection collection)
        where TService : class
    {
        TryAddWithInterfaces<TService>(collection, ServiceLifetime.Scoped);
    }

    /// <summary>
    /// Adds the specified <typeparamref name="TService"/> and its implemented interfaces as a
    /// <see cref="ServiceLifetime.Transient"/> service to the collection if them hasn't already been registered.
    /// </summary>
    /// <typeparam name="TService">The type of the service to add.</typeparam>
    /// <param name="collection">The <see cref="IServiceCollection"/>.</param>
    public static void TryAddTransientWithInterfaces<
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        TService>(
        this IServiceCollection collection)
        where TService : class
    {
        TryAddWithInterfaces<TService>(collection, ServiceLifetime.Transient);
    }

    /// <summary>
    /// Adds the specified <paramref name="implementationType"/> and its implemented interfaces using the specified
    /// <paramref name="lifetime"/> to the collection if them hasn't already been registered.
    /// </summary>
    /// <param name="collection">The <see cref="IServiceCollection"/>.</param>
    /// <param name="implementationType">The implementation type of the service.</param>
    /// <param name="lifetime">The lifetime of the service.</param>
    public static void TryAddWithInterfaces(
        this IServiceCollection collection,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        Type implementationType,
        ServiceLifetime lifetime)
    {
        collection.TryAdd(ServiceDescriptor.Describe(implementationType, implementationType, lifetime));

        if (lifetime is ServiceLifetime.Singleton or ServiceLifetime.Scoped)
        {
            TryAddRedirect(collection, implementationType);
        }
        else
        {
            TryAddCopy(collection, implementationType);
        }
    }

    /// <summary>
    /// Adds the specified <paramref name="implementationType"/> and its implemented interfaces as a
    /// <see cref="ServiceLifetime.Singleton"/> service to the collection if them hasn't already been registered.
    /// </summary>
    /// <param name="collection">The <see cref="IServiceCollection"/>.</param>
    /// <param name="implementationType">The implementation type of the service.</param>
    public static void TryAddSingletonWithInterfaces(
        this IServiceCollection collection,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        Type implementationType)
    {
        TryAddWithInterfaces(collection, implementationType, ServiceLifetime.Singleton);
    }

    /// <summary>
    /// Adds the specified <paramref name="implementationType"/> and its implemented interfaces as a
    /// <see cref="ServiceLifetime.Scoped"/> service to the collection if them hasn't already been registered.
    /// </summary>
    /// <param name="collection">The <see cref="IServiceCollection"/>.</param>
    /// <param name="implementationType">The implementation type of the service.</param>
    public static void TryAddScopedWithInterfaces(
        this IServiceCollection collection,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        Type implementationType)
    {
        TryAddWithInterfaces(collection, implementationType, ServiceLifetime.Scoped);
    }

    /// <summary>
    /// Adds the specified <paramref name="implementationType"/> and its implemented interfaces as a
    /// <see cref="ServiceLifetime.Transient"/> service to the collection if them hasn't already been registered.
    /// </summary>
    /// <param name="collection">The <see cref="IServiceCollection"/>.</param>
    /// <param name="implementationType">The implementation type of the service.</param>
    public static void TryAddTransientWithInterfaces(
        this IServiceCollection collection,
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
        Type implementationType)
    {
        TryAddWithInterfaces(collection, implementationType, ServiceLifetime.Transient);
    }

    private static void TryAddCopy(IServiceCollection collection, Type implementationType)
    {
        foreach (var item in GetInterfaces(implementationType))
        {
            collection.TryAddTransient(item, implementationType);
        }
    }

    private static void TryAddRedirect<TService>(IServiceCollection collection)
    {
        var factory = static (IServiceProvider sp) => sp.GetRequiredService(typeof(TService));

        foreach (var item in GetInterfaces(typeof(TService)))
        {
            collection.TryAddTransient(item, factory);
        }
    }

    private static void TryAddRedirect(IServiceCollection collection, Type implementationType)
    {
        var factory = (IServiceProvider sp) => sp.GetRequiredService(implementationType);

        foreach (var item in GetInterfaces(implementationType))
        {
            collection.TryAddTransient(item, factory);
        }
    }

    private static IEnumerable<Type> GetInterfaces(Type type) =>
        type.GetInterfaces().Where(static x => x != typeof(IDisposable) && x != typeof(IAsyncDisposable));
}
