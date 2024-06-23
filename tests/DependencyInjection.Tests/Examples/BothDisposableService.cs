namespace Raiqub.Toolkit.DependencyInjection.Tests.Examples;

#pragma warning disable CS9113 // Parameter is unread.

[GenerateAutomaticInterface]
public sealed class BothDisposableService(Dependency1Service d1S, Dependency2Service d2S, Dependency3Service d3S)
    : IDisposable, IAsyncDisposable, IBothDisposableService, IInterface
{
    public int DisposableCallCount { get; private set; }
    public int AsyncDisposableCallCount { get; private set; }

    public void Dispose() => DisposableCallCount++;

    public ValueTask DisposeAsync()
    {
        AsyncDisposableCallCount++;
        return ValueTask.CompletedTask;
    }
}
