namespace Raiqub.Toolkit.DependencyInjection.Tests.Examples;

#pragma warning disable CS9113 // Parameter is unread.

[GenerateAutomaticInterface]
public sealed class DisposableService(Dependency1Service d1S, Dependency2Service d2S, Dependency3Service d3S)
    : IDisposable, IDisposableService, IInterface
{
    public static int DisposableCallCount { get; private set; }

    public void Dispose() => DisposableCallCount++;
    public static void Reset() => DisposableCallCount = 0;
}
