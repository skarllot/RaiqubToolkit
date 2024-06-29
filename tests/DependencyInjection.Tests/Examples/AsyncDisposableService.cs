namespace Raiqub.Toolkit.DependencyInjection.Tests.Examples;

#pragma warning disable CS9113 // Parameter is unread.

[GenerateAutomaticInterface]
public sealed class AsyncDisposableService(Dependency1Service d1S, Dependency2Service d2S, Dependency3Service d3S)
    : IAsyncDisposable, IAsyncDisposableService, IInterface
{
    public static int DisposableCallCount { get; private set; }

    public ValueTask DisposeAsync()
    {
        DisposableCallCount++;
        return ValueTask.CompletedTask;
    }

    public static void Reset() => DisposableCallCount = 0;
}
