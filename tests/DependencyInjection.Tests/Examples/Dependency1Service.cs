namespace Raiqub.Toolkit.DependencyInjection.Tests.Examples;

public sealed class Dependency1Service : IDisposable
{
    public int DisposableCallCount { get; private set; }

    public void Dispose() => DisposableCallCount++;
}
