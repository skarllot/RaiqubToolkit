using System.Collections.Immutable;
using FluentAssertions;
using Raiqub.Toolkit.Core.InteropServices;

namespace Raiqub.Toolkit.Core.Tests.InteropServices;

public class ImmutableCollectionsMarshalTest
{
    [Fact]
    public void AsImmutableArrayShouldWrapInputArrayWhenInputIsNotNull()
    {
        // Arrange
        int[] array = [1, 2, 3];

        // Act
        var immutableArray = ImmutableCollectionsMarshal.AsImmutableArray(array);

        // Assert
        immutableArray.IsDefault.Should().BeFalse();
        immutableArray.Should().HaveCount(array.Length);
        immutableArray.Should().Equal(1, 2, 3);
    }

    [Fact]
    public void AsArrayShouldReturnUnderlyingArrayWhenInputIsInitialized()
    {
        // Arrange
        var array = ImmutableArray.Create([1, 2, 3]);

        // Act
        var resultArray = ImmutableCollectionsMarshal.AsArray(array);

        // Assert
        Assert.NotNull(resultArray);
        resultArray.Should().HaveCount(array.Length);
        resultArray.Should().Equal(1, 2, 3);
    }

    [Fact]
    public void AsSpanShouldReturnEmptySpanWhenBuilderIsNull()
    {
        // Arrange
        ImmutableArray<int>.Builder? builder = null;

        // Act
        var span = ImmutableCollectionsMarshal.AsSpan(builder);

        // Assert
        span.IsEmpty.Should().BeTrue();
    }

    [Fact]
    public void AsSpanShouldReturnSpanViewWithCorrectLengthAndData()
    {
        // Arrange
        var builder = ImmutableArray.CreateBuilder<int>();
        builder.Add(10);
        builder.Add(20);
        builder.Add(30);

        // Act
        var span = ImmutableCollectionsMarshal.AsSpan(builder);

        // Assert
        span.Length.Should().Be(builder.Count);
        span[0].Should().Be(builder[0]);
        span[1].Should().Be(builder[1]);
        span[2].Should().Be(builder[2]);
    }

    [Fact]
    public void AsSpanShouldReturnSpanViewWithCorrectDataAfterAddingItems()
    {
        // Arrange
        var builder = ImmutableArray.CreateBuilder<int>();
        builder.Add(100);

        // Act
        var span = ImmutableCollectionsMarshal.AsSpan(builder);
        builder.Add(200); // Add more items after creating the span

        // Assert
        span[0].Should().Be(100); // Ensure span still reflects initial state
    }

    [Fact]
    public void AsSpanShouldReturnSpanThatWritesDirectlyToBuilder()
    {
        // Arrange
        var builder = ImmutableArray.CreateBuilder<int>();

        // Act
        builder.Count = 3;
        var span = ImmutableCollectionsMarshal.AsSpan(builder);
        span[0] = 100;
        span[1] = 123;
        span[2] = 349;

        // Assert
        builder.Should().Equal(100, 123, 349);
    }

    [Fact]
    public void SetCountShouldResizeToSpecifiedCapacity()
    {
        var builder = ImmutableArray.CreateBuilder<int>(0);

        ImmutableCollectionsMarshal.SetCount(builder, 5);
        var span = ImmutableCollectionsMarshal.AsSpan(builder);
        span[0] = 100;
        span[1] = 123;
        span[2] = 349;
        span[3] = 483;
        span[4] = 550;

        builder.Should().Equal(100, 123, 349, 483, 550);
    }
}
