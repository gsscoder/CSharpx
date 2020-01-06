using Xunit;
using FluentAssertions;
using FsCheck;
using FsCheck.Xunit;
using CSharpx.Tests.Fakes;
using Microsoft.FSharp.Core;

namespace CSharpx.Tests.Unit
{
    public class FSharpResultExtensionsTests
    {
        [Property(Arbitrary = new[] { typeof(ArbitraryIntegers) })]
        public void Should_match_result(int value)
        {
            var sut = FSharpResult<int, string>.NewOk(value);
            sut.Match(
                expected => expected.Should().Be(value),
                _ => {}
            );
        }

        [Property(Arbitrary = new[] { typeof(ArbitraryIntegers) })]
        public void Should_match_error(int value)
        {
            var sut = FSharpResult<int, string>.NewError("bad result");
            sut.Match(
                _ => {},
                error => { error.Should().Be("bad result"); }
            );
        }
    }
}