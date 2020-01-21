# <img src="/assets/icon.png" height="30px" alt="CSharpx Logo"> CSharpx

Functional programming and other utilities for C#, following *don't reinvent the wheel* philosophy.

Every source file contains conditional compilation symbols to change type visibility from public to internal
and set other preferences.

If you can't customize it enough using compiler directives, please tell me or fork it and do it in your way.

Project that includes **CSharpx** as source:
- [PickAll](https://github.com/gsscoder/pickall)
- [Tagger](https://github.com/gsscoder/tagger)
- [Command Line Parser Library](https://github.com/commandlineparser/commandline)

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

## Targets

- .NET Standard 2.0
- .NET Framework 4.6.1

## Install via NuGet

If you prefer, you can install it via NuGet:
```sh
$ dotnet add package CSharpx --version 1.9.3-beta
```
The latest stable version is [1.4.0](https://www.nuget.org/packages/CSharpx/1.4.0).

## [IEnumerableExtensions](https://github.com/gsscoder/CSharpx/blob/master/src/CSharpx/IEnumerableExtensions.cs)

- Most useful extension methods from [MoreLINQ](https://code.google.com/p/morelinq/).
- With other useful methods too.
```CSharp
var sequence = new int[] {0, 1, 2, 3, 4}.Intersperse(5);
// will result in {0, 5, 1, 5, 2, 5, 3, 5, 4}
var element = sequence.Choice();
// will choose a random element
```

## [Maybe](https://github.com/gsscoder/CSharpx/blob/master/src/CSharpx/Maybe.cs)

- Encapsulates an optional value that can contain a value or being empty.
- C# native implementation of F# `'T option` / Haskell `data Maybe a = Just a | Nothing` type.
```csharp
var greet = true;
var value = greet ? Maybe.Return("world") : Maybe.Nothing<string>();
value.Match(
    who => Console.WriteLine($"hello {who}!"),
    () => Environment.Exit(1));
```

## [Either](https://github.com/gsscoder/CSharpx/blob/master/src/CSharpx/Either.cs)

- Represents a value that can contain either a value or an error.
- C# native implementation of Haskell `data Either a b = Left a | Right b` type.
- Similar also to F# `Choice<'T, 'U>`.
- Like in Haskell the convention is to let `Right` case hold the value and `Left` keep track of error or similar data.
- If you want a more complete implementation of this kind of types, please check my **C#** port of [Chessie](https://github.com/fsprojects/Chessie),
named [RailwaySharp](https://github.com/gsscoder/railwaysharp).

## [FSharpResultExtensions](https://github.com/gsscoder/CSharpx/blob/master/src/CSharpx/FSharpResultExtensions.cs)

- Supplies convenient extension methods to consume `FSharpResult<T, TError>` in simple and functional way from **C#**.
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

## [SetOnce](https://github.com/gsscoder/CSharpx/blob/master/src/CSharpx/SetOnce.cs)

- Types to allow setting of a value only once.
- Included thread-safe implementation.
```csharp
class Server
{
    private SetOnce<int> _portNumber = new SetOnce<int>();
    public int PortNumber
    {
        get { return _portNumber.Value; }
        set { _portNumber.Value = value; }
    }
    // ...
}

var server = new Server();
server.PortNumber = 6060;
server.PortNumber = 8080; // will throw InvalidOperationException
```

## [ArrayExtensions](https://github.com/gsscoder/CSharpx/blob/master/src/CSharpx/ArrayExtensions.cs)

- Few array helper methods.
```csharp
    var array = new int[] {2, 0, 4, 1, 3};
    var sorted = array.Sort(); // sort as pure function
    // will result in: {0, 1, 2, 3, 4}
```

## [StringExtensions](https://github.com/gsscoder/CSharpx/blob/master/src/CSharpx/StringExtensions.cs)

- General purpose string manipulation extensions.
```csharp
Console.WriteLine(
    "I want to change a word".ApplyAt(4, word => word.Mangle()));
// will print something like: 'I want to change &a word'

Console.WriteLine(
    "\t[hello\nREADME@\t".Sanitize(normalizeWhiteSpace: true));
// will print: ' hello README '
```

## [Unit](https://github.com/gsscoder/CSharpx/blob/master/src/CSharpx/Unit.cs)

- `Unit` is similar to `void` but, since it's a *real* type, it allows the use functions without a result in a computation (*functional style*). It's essentially **F#** `unit` and **Haskell** `()`.
```csharp
// prints each word and returns 0 to the shell
static int Main(string[] args)
{
    var sentence = "this is a sentence";;
    return (from _ in
            from word in sentence.Split()
            select PrintLine(word)
            select 0).Distinct().Single();
}

static Unit PrintLine(string value)
{
    Console.WriteLine(value);
    return Unit.Default;
}
```

## Latest Changes

- Ported to .NET Core.
- Compiles for .NET Framework 4.6.1.
- Added `FSharpResultExtensions`.
- Added `Return` extension method to `Maybe` type.
- `SafeSetOnce` renamed to `BlockingSetOnce`.

## Related Projects

Projects conceived to be included as source:
- [RailwaySharp](https://github.com/gsscoder/railwaysharp)
- [Cataloq](https://github.com/gsscoder/cataloq)

## Icon

[Sharp](https://thenounproject.com/search/?q=sharp&i=1808600) icon designed by Alex Burte from [The Noun Project](https://thenounproject.com/).