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

    [Property(Arbitrary = new[] { typeof(ArbitraryListOfStrings) })]
    public void Error_with_same_string_and_exception_are_equal(string[] values)
    {
        values.ForEach(value =>
        {
            if (value == null) return;  // Skip null values

            var outcome = new Error(
                $"custom message {value}", new[] {new Exception(message: $"exception message {value}")}).Equals(
                          new Error(
                $"custom message {value}", new[] {new Exception(message: $"exception message {value}")}));

            outcome.Should().BeTrue();
        });
    }

    [Fact]
    public void Shoud_build_Success()
    {
        var outcome = CSharpx.Result.Success;

        outcome.Tag.Should().Be(CSharpx.ResultType.Success);
    }

    [Property(Arbitrary = new[] { typeof(ArbitraryListOfStrings) })]
    public void Shoud_build_Failure_with_string(string[] values)
    {
        values.ForEach(value =>
        {
            if (value == null) return;  // Skip null values

            var outcome = CSharpx.Result.Failure(value);

            outcome.Tag.Should().Be(CSharpx.ResultType.Failure);
            outcome._error.Should().Be(new Error(value, Enumerable.Empty<Exception>()));
        });
    }

    [Property(Arbitrary = new[] { typeof(ArbitraryListOfStrings) })]
    public void Shoud_build_Failure_with_string_and_exception(string[] values)
    {
        values.ForEach(value =>
        {
            if (value == null) return;  // Skip null values

            var outcome = CSharpx.Result.Failure($"custom message {value}",
                new Exception(message: $"exception message {value}"));

            outcome.Tag.Should().Be(CSharpx.ResultType.Failure);
            outcome._error.Should().Be(
                new Error($"custom message {value}", new[] {new Exception(message: $"exception message {value}")}));
        });
    }    
}