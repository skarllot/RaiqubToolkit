namespace Raiqub.Toolkit.DependencyInjection.Tests.Examples;

public sealed class Dependency2Service : IAsyncDisposable
{
    public int DisposableCallCount { get; private set; }

    public ValueTask DisposeAsync()
    {
        DisposableCallCount++;
        return ValueTask.CompletedTask;
    }
}
