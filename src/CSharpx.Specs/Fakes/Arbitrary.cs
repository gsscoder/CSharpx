using FsCheck;

static class ArbitraryIntegers
{
    public static Arbitrary<int> IntegerGenerator()
    {
        return Gen.Choose(-30, 30).ToArbitrary();
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