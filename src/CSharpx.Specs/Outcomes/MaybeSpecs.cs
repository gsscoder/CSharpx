using Xunit;
using FluentAssertions;
using FsCheck;
using FsCheck.Xunit;
using CSharpx;

public class MaybeSpecs
{
    [Property(Arbitrary = new[] { typeof(ArbitraryIntegers) })]
    public void Shoud_build_a_Just(int value)
    {
        var outcome = Maybe.Just(value);

        outcome.Should().NotBeNull()
            .And.BeOfType<Just<int>>();
        outcome.FromJust().Should().Be(value);
    }

    [Fact]
    public void Shoud_build_a_Nothing()
    {
        var outcome = Maybe.Nothing<int>();

        outcome.Should().NotBeNull()
            .And.BeOfType<Nothing<int>>();
        outcome.Tag.Should().Be(MaybeType.Nothing);
    }
}