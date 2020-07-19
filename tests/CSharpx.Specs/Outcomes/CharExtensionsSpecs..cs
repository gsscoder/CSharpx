using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using FluentAssertions;
using CSharpx;

public class CharExtensionsSpecs
{
    [Theory]
    [InlineData('f', 0, "", "")]
    [InlineData('f', 1, "", "f")]
    [InlineData('f', 5, " ", "f f f f f")]
    public void Should_replicate(char value, int count, string separator, string expected)
    {
        var outcome = value.Replicate(count, separator);
        
        outcome.Should().Be(expected);
    }
}