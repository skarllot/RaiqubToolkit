namespace Raiqub.Toolkit.DependencyInjection.Tests.Examples;

public sealed class Dependency3Service : IDisposable, IAsyncDisposable
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
