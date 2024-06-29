using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Raiqub.Toolkit.DependencyInjection.Tests.Examples;

namespace Raiqub.Toolkit.DependencyInjection.Tests.ServiceCollectionInterfaceExtensionsTests;

[Collection(ServicesTestGroup.Name)]
public class TryAddTransientWithInterfacesOfT
{
    public TryAddTransientWithInterfacesOfT()
    {
        DisposableService.Reset();
        AsyncDisposableService.Reset();
        BothDisposableService.Reset();
        BothDisposableService.Reset();
    }

    [Fact]
    public void ShouldRegisterInterfaces()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection
            .AddSingleton<Dependency1Service>()
            .AddSingleton<Dependency2Service>()
            .AddSingleton<Dependency3Service>();

        serviceCollection.TryAddTransientWithInterfaces<NonDisposableService>();
        serviceCollection.TryAddTransientWithInterfaces<DisposableService>();
        serviceCollection.TryAddTransientWithInterfaces<AsyncDisposableService>();
        serviceCollection.TryAddTransientWithInterfaces<BothDisposableService>();

        var serviceProvider = serviceCollection.BuildServiceProvider();

        var nds1 = serviceProvider.GetRequiredService<INonDisposableService>();
        var ds1 = serviceProvider.GetRequiredService<IDisposableService>();
        var ads1 = serviceProvider.GetRequiredService<IAsyncDisposableService>();
        var bds1 = serviceProvider.GetRequiredService<IBothDisposableService>();
        var collection1 = serviceProvider.GetServices<IInterface>().ToList();
        var nds2 = serviceProvider.GetRequiredService<INonDisposableService>();
        var ds2 = serviceProvider.GetRequiredService<IDisposableService>();
        var ads2 = serviceProvider.GetRequiredService<IAsyncDisposableService>();
        var bds2 = serviceProvider.GetRequiredService<IBothDisposableService>();
        var collection2 = serviceProvider.GetServices<IInterface>().ToList();
        var nds3 = serviceProvider.GetRequiredService<NonDisposableService>();
        var ds3 = serviceProvider.GetRequiredService<DisposableService>();
        var ads3 = serviceProvider.GetRequiredService<AsyncDisposableService>();
        var bds3 = serviceProvider.GetRequiredService<BothDisposableService>();

        nds1.Should().NotBeSameAs(nds2).And.NotBeSameAs(nds3);
        ds1.Should().NotBeSameAs(ds2).And.NotBeSameAs(ds3);
        ads1.Should().NotBeSameAs(ads2).And.NotBeSameAs(ads3);
        bds1.Should().NotBeSameAs(bds2).And.NotBeSameAs(bds3);

        collection1.Should().HaveCount(1);
        collection2.Should().HaveCount(1);

        collection1[0].Should().BeOfType<NonDisposableService>();
        collection2[0].Should().BeOfType<NonDisposableService>();

        collection1[0].Should().NotBeSameAs(nds1);
        collection2[0].Should().NotBeSameAs(nds1);
    }

    [Fact]
    public async Task ShouldDisposeInstances()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection
            .AddSingleton<Dependency1Service>()
            .AddSingleton<Dependency2Service>()
            .AddSingleton<Dependency3Service>();

        serviceCollection.TryAddTransientWithInterfaces<NonDisposableService>();
        serviceCollection.TryAddTransientWithInterfaces<DisposableService>();
        serviceCollection.TryAddTransientWithInterfaces<AsyncDisposableService>();
        serviceCollection.TryAddTransientWithInterfaces<BothDisposableService>();

        var serviceProvider = serviceCollection.BuildServiceProvider();

        var nds1 = serviceProvider.GetRequiredService<INonDisposableService>();
        var ds1 = serviceProvider.GetRequiredService<IDisposableService>();
        var ads1 = serviceProvider.GetRequiredService<IAsyncDisposableService>();
        var bds1 = serviceProvider.GetRequiredService<IBothDisposableService>();
        var nds2 = serviceProvider.GetRequiredService<INonDisposableService>();
        var ds2 = serviceProvider.GetRequiredService<IDisposableService>();
        var ads2 = serviceProvider.GetRequiredService<IAsyncDisposableService>();
        var bds2 = serviceProvider.GetRequiredService<IBothDisposableService>();

        await serviceProvider.DisposeAsync();

        Assert.NotNull(nds1);
        Assert.NotNull(ds1);
        Assert.NotNull(ads1);
        Assert.NotNull(bds1);
        Assert.NotNull(nds2);
        Assert.NotNull(ds2);
        Assert.NotNull(ads2);
        Assert.NotNull(bds2);

        DisposableService.DisposableCallCount.Should().Be(2);
        AsyncDisposableService.DisposableCallCount.Should().Be(2);
        BothDisposableService.DisposableCallCount.Should().Be(0);
        BothDisposableService.AsyncDisposableCallCount.Should().Be(2);
    }
}
