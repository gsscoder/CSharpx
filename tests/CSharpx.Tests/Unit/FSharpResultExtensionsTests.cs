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
        public void Should_match_result(int value)
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
        public void Should_match_result_with_Either(int value)
        {
            var sut = FSharpResult<int, string>.NewOk(value);

            var expected = sut.Either(
                _ => true,
                _ => false
            );

            expected.Should().BeTrue();
        }

        [Property(Arbitrary = new[] { typeof(ArbitraryIntegers) })]
        public void Should_match_error(int value)
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
        public void Should_match_error_with_Either(int value)
        {
            var sut = FSharpResult<int, string>.NewError("bad result");

            var error = sut.Either(
                _ => true,
                _ => false
            );

            error.Should().BeFalse();
        }
    }
}