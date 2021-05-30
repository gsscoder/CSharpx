[![NuGet](https://img.shields.io/nuget/dt/csharpx.svg)](https://nuget.org/packages/csharpx)
[![NuGet](https://img.shields.io/nuget/v/csharpx.svg)](https://www.nuget.org/packages/csharpx)
[![NuGet](https://img.shields.io/nuget/vpre/csharpx.svg)](https://www.nuget.org/packages/csharpx)

# <img src="/assets/icon.png" height="30px" alt="CSharpx Logo"> CSharpx

Functional programming and other utilities for C#, following *don't reinvent the wheel* philosophy. This project was inspired by [Real-World Functional Programming](https://www.amazon.com/Real-World-Functional-Programming-Tomas-Petricek/dp/1933988924/ref=sr_1_1?keywords=Real-World+Functional+Programming&qid=1580118924&s=books&sr=1-1) and includes code from [MoreLINQ](https://github.com/morelinq/MoreLINQ).

Every source file contains conditional compilation symbols to change type visibility from public to internal
and set other preferences.

If you can't customize it enough using compiler directives, please tell me or fork it and do it in your way.

OSS project that includes **CSharpx**:

- [PickAll](https://github.com/gsscoder/pickall)
- [Tagger](https://github.com/gsscoder/tagger)
- [Integrity](https://github.com/gsscoder/integrity)
- [Command Line Parser Library](https://github.com/commandlineparser/commandline)

**CSharpx** is also used with success in various closed source project.

## Reference

It allows also source inclusion in other projects. Just one or more files in your project tree or reference it using [Paket](http://fsprojects.github.io/Paket/):

**paket.dependencies**
```
github gsscoder/csharpx src/csharpx/Maybe.cs 
```
**paket.references** (if you've a directory called `Internal`)
```
File:Maybe.cs Internal
```
- **Paket** will alter your `csproj` file adding a `Compile` item, so you need to set `EnableDefaultCompileItems` property to `false`. At this point, every other source file must be handled in the same way. For more detailed informations please read [Paket Documentation](https://fsprojects.github.io/Paket/github-dependencies.html).

Conditional compilation constants of previous version are semplified to one. `CSX_TYPES_INTERNAL` if set allows you to set visibility to internal.

Source code uses **C#** language version 8.0 features.

## Targets

- .NET Standard 2.0
- .NET Framework 4.7

## Install via NuGet

If you prefer, you can install it via NuGet:

```sh
$ dotnet add package CSharpx --version 2.8.0-preview.4
```

## Versions

- In `develop` branch source code may be different also from latest preview.
- The letest version on NuGet is [2.8.0-preview.4](https://www.nuget.org/packages/CSharpx/2.8.0-preview.4).
- The latest stable version on NuGet is [2.7.3](https://www.nuget.org/packages/CSharpx/2.7.3).

## [Maybe](https://github.com/gsscoder/CSharpx/blob/master/src/CSharpx/Maybe.cs)

- Encapsulates an optional value that can contain a value or being empty.
- C# native implementation of F# `'T option` / Haskell `data Maybe a = Just a | Nothing` type.

```csharp
var greet = true;
var value = greet ? "world".ToMaybe() : Maybe.Nothing<string>();
value.Match(
    who => Console.WriteLine($"hello {who}!"),
    () => Environment.Exit(1));
```

- Supports LINQ syntax:

```csharp
var result1 = Maybe.Just(30);
var result2 = Maybe.Just(10);
var result3 = Maybe.Just(2);

var sum = from r1 in result1
          from r2 in result2
          where r1 > 0
          select r1 - r2 into temp
          from r3 in result3
          select temp * r3;

var value = sum.FromJust(); // outcome: 40
```

- See [RailwaySharp](https://github.com/gsscoder/railwaysharp) for a complete railway-oriented programming library.

## [Either](https://github.com/gsscoder/CSharpx/blob/master/src/CSharpx/Either.cs)

- Represents a value that can contain either a value or an error.
- C# native implementation of Haskell `data Either a b = Left a | Right b` type.
- Similar also to F# `Choice<'T, 'U>`.
- Like in Haskell the convention is to let `Right` case hold the value and `Left` keep track of error or similar data.
- If you want a more complete implementation of this kind of types, please check my **C#** port of [Chessie](https://github.com/fsprojects/Chessie),
named [RailwaySharp](https://github.com/gsscoder/railwaysharp).

## [Result](https://github.com/gsscoder/CSharpx/blob/master/src/CSharpx/Result.cs)

- Represents a value that can be a success or a failure in form an error string.
- It's like a `bool` that uses an error string as `false`.

```csharp
Result ValidateArtifact(string path)
{
    Artifact artifact;
    try {
        artifact = ArtifactManager.Load(path);
    }
    catch (IOException e) {
        return Result.Failure($"Unable to load artifcat {path}:\n{e.Message}");
    }
    return artifact.CheckIntegrity() switch {
        Integrity.Healthy => Result.Success(),
        _                 => Result.Failure("Artifact integrity is compromised")
    };
}
```

## [FSharpResultExtensions](https://github.com/gsscoder/CSharpx/blob/master/src/CSharpx/FSharpResultExtensions.cs)

- Convenient extension methods to consume `FSharpResult<T, TError>` in simple and functional way from **C#**.

```csharp
// pattern match like
var result = Query.GetStockQuote("ORCL");
result.Match(
    quote => Console.WriteLine($"Price: {quote.Price}"),
    error => Console.WriteLine($"Trouble: {error}"));
// mapping
var result = Query.GetIndex(".DJI");
result.Map(
    quote => CurrencyConverter.Change(quote.Price, "$", "€"));
```

- Blog [post](https://gsscoder.github.io/consuming-fsharp-results-in-c/) about it.

## [StringExtensions](https://github.com/gsscoder/CSharpx/blob/master/src/CSharpx/Strings.cs#L53)

- General purpose string manipulation extensions.

```csharp
Console.WriteLine(
    "I want to change a word".ApplyAt(4, word => word.Mangle()));
// outcome like: 'I want to change &a word'

Console.WriteLine(
    "\t[hello\world@\t".Sanitize(normalizeWhiteSpace: true));
// outcome: ' hello world '
```

## [EnumerableExtensions](https://github.com/gsscoder/CSharpx/blob/master/src/CSharpx/EnumerableExtensions.cs)

- Most useful extension methods from [MoreLINQ](https://github.com/morelinq/MoreLINQ).
- Some of these reimplemnted (e.g. `Choose` using `Maybe`):
- **LINQ** `...OrDefault` implemented with `Maybe` type as return value.

```csharp
var numbers = new int[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9};
var evens = numbers.Choose(x => x % 2 == 0
                                ? Maybe.Just(x)
                                : Maybe.Nothing<int>());
// outcome: {0, 2, 4, 6, 8}
```

- With other useful methods too:

```CSharp
var sequence = new int[] {0, 1, 2, 3, 4}.Intersperse(5);
// outcome: {0, 5, 1, 5, 2, 5, 3, 5, 4}
var element = sequence.Choice();
// will choose a random element
var sequence = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }.ChunkBySize(3);
// outcome: { [0, 1, 2], [3, 4, 5], [6, 7, 8], [9, 10] }
var maybeFirst = new int[] {0, 1, 2}.FirstOrNothing(x => x == 1)
// outcome: Just(1)
```

- [Tests](https://github.com/gsscoder/CSharpx/blob/master/src/CSharpx.Specs/Outcomes/EnumerableExtensionsSpecs.cs) cover only new and modified extension methods.

## [Unit](https://github.com/gsscoder/CSharpx/blob/master/src/CSharpx/Unit.cs)

- `Unit` is similar to `void` but, since it's a *real* type. `void` is not, in fact you can't declare a variable of that type. `Unit` allows the use functions without a result in a computation (*functional style*). It's essentially **F#** `unit` and **Haskell** `Unit`.

```csharp
// prints each word and returns 0 to the shell
static int Main(string[] args)
{
    var sentence = "this is a sentence";;
    return (from _ in
            from word in sentence.Split()
            select Unit.Do(() => Console.WriteLine(word))
            select 0).Distinct().Single();
}
```

## Latest Changes

- `Maybe<T>` type implemented as a `struct`.
- `Either<TLeft, TRight>` type implemented as a `struct`.
- Code refactored for C# 8.0.
- Implemented `IEquatable<T>` for `Maybe<T>` type.

## Related Projects

- [RailwaySharp](https://github.com/gsscoder/railwaysharp)

## Icon

[Sharp](https://thenounproject.com/search/?q=sharp&i=1808600) icon designed by Alex Burte from [The Noun Project](https://thenounproject.com/)
