using Microsoft.FSharp.Collections;
using FsCheck;

static class ArbitraryIntegers
{
    public static Arbitrary<int> IntegerGenerator()
    {
        return Gen.Choose(-30, 30).ToArbitrary();
    }
}

static class ArbitraryListOfIntegers
{
    public static Arbitrary<FSharpList<int>> IntegerListGenerator()
    {
        return Gen.ListOf(30, Gen.Choose(-30, 30)).ToArbitrary();
    }
}

static class ArbitraryStrings
{
    public static Arbitrary<string[]> StringGenerator()
    {
        return Gen.Shuffle(new [] {
            "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten"})
            .ToArbitrary();
    }
}