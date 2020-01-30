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
            outcome.MatchLeft(out string left).Should().BeTrue();
            left.Should().Be(value);
        });
    }

    [Property(Arbitrary = new[] { typeof(ArbitraryIntegers) })]
    public void Shoud_build_Right(int value)
    {
        if (value == default(int)) return; // Skip default values

        var outcome = Either.Right<string, int>(value);

        outcome.Should().NotBeNull()
            .And.BeOfType<Right<string, int>>();
        outcome.MatchRight(out int right).Should().BeTrue();
        right.Should().Be(value);
    }
}