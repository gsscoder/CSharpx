using System;
using Xunit;
using FluentAssertions;
using FsCheck;
using FsCheck.Xunit;
using CSharpx;

public class SetOnceSpecs
{
    [Property(Arbitrary = new[] { typeof(ArbitraryListOfStrings) })]
    public void Should_set_a_value_once(string value)
    {
        var sut = new SetOnce<string>();

        sut.Value = value;

        sut.Value.Should().Be(value);
    }

    [Fact]
    public void Setting_a_value_more_than_once_trhows_InvalidOperationException()
    {
        var sut = new SetOnce<object>();
        sut.Value = new object();
        
        Action action = () => sut.Value = new object();

        action.Should().ThrowExactly<InvalidOperationException>()
            .WithMessage("Value can be set only once");
    }

    [Fact]
    public void Getting_a_value_from_an_unset_instance_trhows_InvalidOperationException()
    {
        var sut = new SetOnce<object>();
        
        Action action = () =>
            {
                var _ = sut.Value;
            };

        action.Should().ThrowExactly<InvalidOperationException>()
            .WithMessage("Value not set");
    }

    [Property(Arbitrary = new[] { typeof(ArbitraryIntegers) })]
    public void Should_cast_to_wrapped_value(int value)
    {
        var sut = new SetOnce<int>();

        sut.Value = value;

        ((int)sut.Value).Should().Be(value);
    }

    [Fact]
    public void If_value_is_unset_HasValue_should_be_false()
    {
        var sut = new SetOnce<string>();

        sut.HasValue.Should().BeFalse();
    }

    [Property(Arbitrary = new[] { typeof(ArbitraryListOfStrings) })]
    public void If_value_is_set_HasValue_should_be_true(string[] values)
    {
        values.ForEach(value => {
            var sut = new SetOnce<string>();

            sut.Value = value;

            sut.HasValue.Should().BeTrue();
        });
    }
}

public class BlockingSetOnceSpecs
{
    [Property(Arbitrary = new[] { typeof(ArbitraryListOfStrings) })]
    public void Should_set_a_value_once(string[] values)
    {
        values.ForEach(value => {
            var sut = new BlockingSetOnce<string>();

            sut.Value = value;

            sut.Value.Should().Be(value);
        });
    }

    [Fact]
    public void Setting_a_value_more_than_once_trhows_InvalidOperationException()
    {
        var sut = new BlockingSetOnce<object>();
        sut.Value = new object();
        
        Action action = () => sut.Value = new object();

        action.Should().ThrowExactly<InvalidOperationException>()
            .WithMessage("Value can be set only once");
    }

    [Fact]
    public void Getting_a_value_from_an_unset_instance_trhows_InvalidOperationException()
    {
        var sut = new BlockingSetOnce<object>();
        
        Action action = () =>
            {
                var _ = sut.Value;
            };

        action.Should().ThrowExactly<InvalidOperationException>()
            .WithMessage("Value not set");
    }

    [Property(Arbitrary = new[] { typeof(ArbitraryIntegers) })]
    public void Should_cast_to_wrapped_value(int value)
    {
        var sut = new BlockingSetOnce<int>();

        sut.Value = value;

        ((int)sut.Value).Should().Be(value);
    }

    [Fact]
    public void If_value_is_unset_HasValue_should_be_false()
    {
        var sut = new BlockingSetOnce<string>();

        sut.HasValue.Should().BeFalse();
    }

    [Property(Arbitrary = new[] { typeof(ArbitraryListOfStrings) })]
    public void If_value_is_set_HasValue_should_be_true(string[] values)
    {
        values.ForEach(value => {
            var sut = new BlockingSetOnce<string>();

            sut.Value = value;

            sut.HasValue.Should().BeTrue();
        });
    }
} 