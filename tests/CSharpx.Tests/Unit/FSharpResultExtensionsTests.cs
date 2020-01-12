using System;
using Xunit;
using FluentAssertions;
using FsCheck;
using FsCheck.Xunit;
using CSharpx.Tests.Fakes;
using Microsoft.FSharp.Core;
using CSharpx.FSharp;

namespace CSharpx.Tests.Unit
{
    public class FSharpResultExtensionsTests
    {
        [Property(Arbitrary = new[] { typeof(ArbitraryIntegers) })]
        public void Should_match_a_result(int value)
        {
            int? expected = null;
            var sut = FSharpResult<int, string>.NewOk(value);

            sut.Match(
                matched => expected = value,
                _ => { throw new InvalidOperationException(); }
            );

            expected.Should().Be(value);
        }

        [Property(Arbitrary = new[] { typeof(ArbitraryIntegers) })]
        public void Should_match_an_error(int value)
        {
            string error = null;
            var sut = FSharpResult<int, string>.NewError("bad result");

            sut.Match(
                _ => { throw new InvalidOperationException(); },
                message => { error = message; }
            );

            error.Should().Be("bad result");
        }

        [Property(Arbitrary = new[] { typeof(ArbitraryIntegers) })]
        public void Should_map_a_value(int value)
        {
            var sut = FSharpResult<int, string>.NewOk(value);

            Func<int, double> func = x => x / 0.5;
            var mapped = sut.Map(func);

            mapped.IsOk.Should().BeTrue();
            mapped.ResultValue.Should().Be(func(value));
        }

        [Property(Arbitrary = new[] { typeof(ArbitraryIntegers) })]
        public void Should_keep_error_when_mapping_a_value_of_fail(int value)
        {
            var sut = FSharpResult<int, string>.NewError("bad result");

            var mapped = sut.Map(x => x / 0.5);

            mapped.IsOk.Should().BeFalse();
            mapped.ResultValue.Should().Be(default(double));
        }

        [Property(Arbitrary = new[] { typeof(ArbitraryIntegers) })]
        public void Should_bind_a_value(int value)
        {
            var sut = FSharpResult<int, string>.NewOk(value);

            Func<int, FSharpResult<double, string>> func =
                x => FSharpResult<double, string>.NewOk(x / 0.5);
            var binded = sut.Bind(func);

            binded.IsOk.Should().BeTrue();
            binded.ResultValue.Should().Be(func(value).ResultValue);
        }

        [Property(Arbitrary = new[] { typeof(ArbitraryIntegers) })]
        public void Should_keep_error_when_binding_a_value_of_fail(int value)
        {
            var sut = FSharpResult<int, string>.NewError("bad result");

            var binded = sut.Bind(x => FSharpResult<double, string>.NewOk(x / 0.5));

            binded.IsOk.Should().BeFalse();
            binded.ResultValue.Should().Be(default(double));
        }

        [Property(Arbitrary = new[] { typeof(ArbitraryIntegers) })]
        public void Should_return_a_value(int value)
        {
            var sut = FSharpResult<int, string>.NewOk(value);

            var result = sut.ReturnOrFail();

            result.Should().Be(value);
        }

        [Fact]
        public void Should_throws_exception_on_a_fail()
        {
            var sut = FSharpResult<int, string>.NewError("bad result");

            Action action = () => sut.ReturnOrFail();

            action.Should().ThrowExactly<Exception>()
                .WithMessage("bad result");
        }
    }
}