using Xunit;
using FluentAssertions;

namespace CSharpx.Tests.Unit
{
    public class StringExtensionsTests
    {
        [Theory]
        [InlineData("hello", true)]
        [InlineData("0123456789", false)]
        [InlineData("hello01234", false)]
        [InlineData("hello.tests", false)]
        [InlineData("hello tests", false)]
        public void Should_detect_letter_characters(string value, bool expected)
        {
            value.IsAlpha().Should().Be(expected);
        }

        [Theory]
        [InlineData("hello", true)]
        [InlineData("0123456789", true)]
        [InlineData("hello01234", true)]
        [InlineData("hello.tests", false)]
        [InlineData("hello tests", false)]
        public void Should_detect_alphanumeric_characters(string value, bool expected)
        {
            value.IsAlphanumeric().Should().Be(expected);
        }

        [Theory]
        [InlineData("hello01234", false)]
        [InlineData("hello.tests", false)]
        [InlineData("hello tests", true)]
        [InlineData("hello\ntests", true)]
        [InlineData("hello\ttests", true)]
        public void Should_detect_whitespace_characters(string value, bool expected)
        {
            value.ContainsWhiteSpace().Should().Be(expected);
        }

        [Theory]
        [InlineData("hello tests@", "hello tests")]
        [InlineData("hello\ttests@", "hello tests")]
        public void Should_sanitize_strings_normalizing_white_spaces(string value, string expected)
        {
            value.Sanitize().Should().Be(expected);
        }

        [Theory]
        [InlineData("hello\ntests@", "hello\ntests")]
        [InlineData("hello\ttests@", "hello\ttests")]
        public void Should_sanitize_strings_without_normalizing_white_spaces(string value, string expected)
        {
            value.Sanitize(normalizeWhiteSpace: false).Should().Be(expected);
        }

        [Theory]
        [InlineData("hello this is a test", new object[] {'!', "!!", 10}, "hello ! this !! is 10 a test")]
        public void Should_intersperse_values(string value, object[] values, string expected)
        {
            value.Intersperse(values).Should().Be(expected);
        }
    }
}