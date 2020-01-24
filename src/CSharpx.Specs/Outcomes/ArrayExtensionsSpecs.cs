using Xunit;
using FluentAssertions;
using CSharpx;

public class AttayExtensionsSpecs
{
    [Theory]
    [InlineData(new object[] { 7, 3, 1, 2, 5, 4, 0, 6, 9, 8 },
        new object[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 })]
    [InlineData(new object[] { "b", "e", "g", "a", "h", "j", "l", "c", "d", "f" },
        new object[] { "a", "b", "c", "d", "e", "f", "g", "h", "l", "j" })]
    public void Should_sort_arrays(object[] value, object[] expected)
    {
        value.Sort().Should().BeEquivalentTo(expected);
    }
}