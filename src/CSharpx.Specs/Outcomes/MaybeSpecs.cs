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
        if (value == default(int)) return;// Skip default values

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

    [Property(Arbitrary = new[] { typeof(ArbitraryIntegers) })]
    public void Shoud_return_proper_maybe_with_a_value_type(int value)
    {
        var outcome = Maybe.Return(value);

         outcome.Should().NotBeNull();

        if (value == default(int)) {
            outcome.Should().BeOfType<Nothing<int>>();
        }
        else {
            outcome.Should().BeOfType<Just<int>>();
            outcome.FromJust().Should().Be(value);
        }
    }

    [Property(Arbitrary = new[] { typeof(ArbitraryListOfStrings) })]
    public void Shoud_return_proper_maybe_with_a_reference_type(string[] values)
    {
        values.ForEach(value =>
            {
                var outcome = Maybe.Return(value);

                outcome.Should().NotBeNull();

                if (value == default(string)) {
                    outcome.Should().BeOfType<Nothing<string>>();
                }
                else {
                    outcome.Should().BeOfType<Just<string>>();
                    outcome.FromJust().Should().Be(value);
                }
            });
    }
}