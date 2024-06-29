namespace Raiqub.Toolkit.DependencyInjection.Tests.Examples;

#pragma warning disable CS9113 // Parameter is unread.

[GenerateAutomaticInterface]
public sealed class BothDisposableService(Dependency1Service d1S, Dependency2Service d2S, Dependency3Service d3S)
    : IDisposable, IAsyncDisposable, IBothDisposableService, IInterface
{
    public static int DisposableCallCount { get; private set; }
    public static int AsyncDisposableCallCount { get; private set; }

    public void Dispose() => DisposableCallCount++;

    public ValueTask DisposeAsync()
    {
        AsyncDisposableCallCount++;
        return ValueTask.CompletedTask;
    }

    public static void Reset()
    {
        DisposableCallCount = 0;
        AsyncDisposableCallCount = 0;
    }
}
