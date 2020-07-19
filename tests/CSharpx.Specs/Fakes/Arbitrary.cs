using Microsoft.FSharp.Collections;
using FsCheck;

static class ArbitraryNegativeIntegers
{
    public static Arbitrary<int> IntegerGenerator() => Gen.Choose(-60, -30).ToArbitrary();
}

static class ArbitraryIntegers
{
    public static Arbitrary<int> IntegerGenerator() => Gen.Choose(-30, 30).ToArbitrary();
}

static class ArbitraryPositiveIntegers
{
    public static Arbitrary<int> IntegerGenerator() => Gen.Choose(1, 60).ToArbitrary();
}

static class ArbitraryListOfIntegers
{
    public static Arbitrary<FSharpList<int>> IntegerListGenerator() => Gen.ListOf(30,
        Gen.Choose(-30, 30)).ToArbitrary();
}

static class ArbitraryListOfStrings
{
    public static Arbitrary<string[]> StringListGenerator() => Gen.Shuffle(new [] {
            string.Empty, "one", "1", "two", "2", "three", "3", null, "four", "4", string.Empty, "five",
            "5", "six", "6", null, "seven", "7", "eight", "8", "nine", "9", null, "ten", "10", string.Empty})
            .ToArbitrary();
}