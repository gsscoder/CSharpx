using System;
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
        public void Should_bind_a_value(int value)
        {
            var sut = FSharpResult<int, string>.NewOk(value);

            Func<int, FSharpResult<double, string>> func =
                x => FSharpResult<double, string>.NewOk(x / 0.5);
            var mapped = sut.Bind(func);

            mapped.IsOk.Should().BeTrue();
            mapped.ResultValue.Should().Be(func(value).ResultValue);
        }
    }
}