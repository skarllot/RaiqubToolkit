using AutomaticInterface;

namespace Raiqub.Toolkit.DependencyInjection.Tests.Examples;

#pragma warning disable CS9113 // Parameter is unread.

[GenerateAutomaticInterface]
public class NonDisposableService(Dependency1Service d1S, Dependency2Service d2S, Dependency3Service d3S)
    : INonDisposableService, IInterface;
