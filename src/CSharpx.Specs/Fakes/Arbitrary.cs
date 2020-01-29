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
            "one", "two", "three", null, "four", "five", "six", null, "seven", "eight", "nine", null, "ten"})
            .ToArbitrary();
}