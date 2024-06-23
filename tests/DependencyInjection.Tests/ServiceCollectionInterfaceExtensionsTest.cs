﻿using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Raiqub.Toolkit.DependencyInjection.Tests.Examples;

namespace Raiqub.Toolkit.DependencyInjection.Tests;

public class ServiceCollectionInterfaceExtensionsTest
{
    [Fact]
    public void TryAddSingletonWithInterfacesOfTShouldRegisterInterfaces()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection
            .AddSingleton<Dependency1Service>()
            .AddSingleton<Dependency2Service>()
            .AddSingleton<Dependency3Service>();

        serviceCollection.TryAddSingletonWithInterfaces<NonDisposableService>();
        serviceCollection.TryAddSingletonWithInterfaces<DisposableService>();
        serviceCollection.TryAddSingletonWithInterfaces<AsyncDisposableService>();
        serviceCollection.TryAddSingletonWithInterfaces<BothDisposableService>();

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

        nds1.Should().BeSameAs(nds2).And.BeSameAs(nds3);
        ds1.Should().BeSameAs(ds2).And.BeSameAs(ds3);
        ads1.Should().BeSameAs(ads2).And.BeSameAs(ads3);
        bds1.Should().BeSameAs(bds2).And.BeSameAs(bds3);

        collection1.Should().HaveCount(1);
        collection2.Should().HaveCount(1);

        collection1[0].Should().BeOfType<NonDisposableService>();
        collection2[0].Should().BeOfType<NonDisposableService>();

        collection1[0].Should().BeSameAs(nds1);
        collection2[0].Should().BeSameAs(nds1);
    }
}
