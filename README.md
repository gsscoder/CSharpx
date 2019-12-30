# <img src="/assets/icon.png" height="30px" alt="CSharpx Logo"> CSharpx

Functional programming and other utilities for C#, following *don't reinvent the wheel* philosophy.

Every source file contains conditional compilation symbols to change type visibility from public to internal
and set other preferences.

Source files are conceived to be manually added to other projects or by setting a reference via [Paket](http://fsprojects.github.io/Paket/).

If you can't customize it enough using compiler directives, please tell me or fork it and do it in your way. If you want a sample of project that includes **CSharpx** as source, see [PickAll](https://github.com/gsscoder/pickall).

## Install via NuGet

If you prefer, you can install it via NuGet:
```sh
dotnet add package CSharpx --version 1.2.3-rc2
```

## [EnumerableExtensions](https://github.com/gsscoder/CSharpx/blob/master/src/CSharpx/EnumerableExtensions.cs)

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

-- Represents a value that can contain either a value or an error.
- C# native implementation of Haskell `data Either a b = Left a | Right b` type.
- Similar also to F# `Choice<'T, 'U>`.
- Like in Haskell the convention is to let `Right` case hold the value and `Left` keep track of error or similar data.
- If you want a more complete implementation of this kind of types, please check my **C#** port of [Chessie](https://github.com/fsprojects/Chessie),
named [RailwaySharp](https://github.com/gsscoder/railwaysharp).

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

## Icon

[Sharp](https://thenounproject.com/search/?q=sharp&i=1808600) icon designed by Alex Burte from [The Noun Project](https://thenounproject.com/)