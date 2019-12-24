using System;
using Xunit;
using FluentAssertions;
using FsCheck;
using FsCheck.Xunit;
using CSharpx.Tests.Fakes;

namespace CSharpx.Tests.Unit
{
    public class SetOnceTests
    {
        [Property(Arbitrary = new[] { typeof(ArbitraryStrings) })]
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
    }

    public class SafeSetOnceTests
    {
        [Property(Arbitrary = new[] { typeof(ArbitraryStrings) })]
        public void Should_set_a_value_once(string value)
        {
            var sut = new SafeSetOnce<string>();

            sut.Value = value;

            sut.Value.Should().Be(value);
        }

        [Fact]
        public void Setting_a_value_more_than_once_trhows_InvalidOperationException()
        {
            var sut = new SafeSetOnce<object>();
            sut.Value = new object();
            
            Action action = () => sut.Value = new object();

            action.Should().ThrowExactly<InvalidOperationException>()
                .WithMessage("Value can be set only once");
        }

        [Fact]
        public void Getting_a_value_from_an_unset_instance_trhows_InvalidOperationException()
        {
            var sut = new SafeSetOnce<object>();
            
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
            var sut = new SafeSetOnce<int>();

            sut.Value = value;

            ((int)sut.Value).Should().Be(value);
        }
    } 
}