using System;
using Xunit;
using FluentAssertions;
using FsCheck;
using FsCheck.Xunit;
using CSharpx;

public class EitherSpecs
{
    [Property(Arbitrary = new[] { typeof(ArbitraryListOfStrings) })]
    public void Shoud_build_Left(string[] values)
    {
        values.ForEach(value => {
            if (value == null) return; // Skip null values

            var outcome = Either.Left<string, int>(value);

            outcome.Should().NotBeNull()
                .And.BeOfType<Left<string, int>>();
            outcome.FromLeft().Should().Be(value);
        });
    }

    [Property(Arbitrary = new[] { typeof(ArbitraryIntegers) })]
    public void Shoud_build_Right(int value)
    {
        if (value == default(int)) return; // Skip default values

        var outcome = Either.Right<string, int>(value);

        outcome.Should().NotBeNull()
            .And.BeOfType<Right<string, int>>();
        outcome.FromRight().Should().Be(value);
    }

    [Fact]
    public void Trying_to_get_a_value_from_Left_with_FromLeftOrFail_raise_Exception_in_case_of_Right()
    {
        var sut = Either.Right<string, int>(new CryptoRandom().Next());

        Action action = () => sut.FromLeftOrFail();

        action.Should().ThrowExactly<Exception>()
            .WithMessage("The value is empty.");
    }

    [Fact]
    public void Trying_to_get_a_value_from_Right_with_FromRightOrFail_raise_Exception_in_case_of_Left()
    {
        var sut = Either.Left<string, int>("bad result");

        Action action = () => sut.FromRightOrFail();

        action.Should().ThrowExactly<Exception>()
            .WithMessage("The value is empty.");
    }
}