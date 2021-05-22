using System;
using System.Linq;
using Xunit;
using FluentAssertions;
using FsCheck;
using FsCheck.Xunit;
using CSharpx;

public class ResultSpecs
{
    static Random _random = new CryptoRandom();

    [Fact]
    public void Shoud_build_Success()
    {
        var outcome = CSharpx.Result.Success;

        outcome.Tag.Should().Be(CSharpx.ResultType.Success);
    }

    [Property(Arbitrary = new[] { typeof(ArbitraryListOfStrings) })]
    public void Shoud_build_Failure(string[] values)
    {
        values.ForEach(value =>
        {
            if (value == null) return;  // Skip null values

            var outcome = CSharpx.Result.Failure(value);

            outcome.Tag.Should().Be(CSharpx.ResultType.Failure);
            outcome._error.Should().Be(value);
        });
    }
}