using Xunit;
using FluentAssertions;

namespace CSharpx.Tests.Unit
{
    public class StringExtensionsTests
    {
        [Fact]
        public void Should_detect_alphanumeric_characters()
        {
            "hello".IsAlphanumeric().Should().BeTrue();
            "0123456789".IsAlphanumeric().Should().BeTrue();
            "hello01234".IsAlphanumeric().Should().BeTrue();
            "hello.tests".IsAlphanumeric().Should().BeFalse();
            "hello tests".IsAlphanumeric().Should().BeFalse();
        }

        [Fact]
        public void Should_detect_whitespace_characters()
        {
            "hello01234".ContainsWhiteSpace().Should().BeFalse();
            "hello.tests".ContainsWhiteSpace().Should().BeFalse();
            "hello tests".ContainsWhiteSpace().Should().BeTrue();
            "hello\ntests".ContainsWhiteSpace().Should().BeTrue();
            "hello\ttests".ContainsWhiteSpace().Should().BeTrue();
        }

        [Fact]
        public void Should_sanitize_strings_normalizing_white_spaces()
        {
            "hello tests@".Sanitize().Should().Be("hello tests");
            "hello\ttests@".Sanitize().Should().Be("hello tests");
        }

        [Fact]
        public void Should_sanitize_strings_without_normalizing_white_spaces()
        {
            "hello\ntests@".Sanitize(normalizeWhiteSpace: false).Should().Be("hello\ntests");
            "hello\ttests@".Sanitize(normalizeWhiteSpace: false).Should().Be("hello\ttests");
        }

        [Fact]
        public void Should_intersperse_values()
        {
            "hello this is a test".Intersperse('!', "!!", 10).Should().Be("hello ! this !! is 10 a test");
        }
    }
}