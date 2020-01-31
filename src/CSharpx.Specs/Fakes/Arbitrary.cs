using Microsoft.FSharp.Collections;
using FsCheck;

static class ArbitraryIntegers
{
    public static Arbitrary<int> IntegerGenerator() => Gen.Choose(-30, 30).ToArbitrary();
}

static class ArbitraryListOfIntegers
{
    public static Arbitrary<FSharpList<int>> IntegerListGenerator() => Gen.ListOf(30,
        Gen.Choose(-30, 30)).ToArbitrary();
}

static class ArbitraryListOfStrings
{
    public static Arbitrary<string[]> StringListGenerator() => Gen.Shuffle(new [] {
            string.Empty, "one", "two", "three", null, "four", string.Empty, "five", "six", null,
            "seven", "eight", "nine", null, "ten", string.Empty})
            .ToArbitrary();
}