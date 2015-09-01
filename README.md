# CSharpx
Functional programming and other utilities for C# (_pre-release_).

Every source file contains conditional compilation symbols to change type visibility from public to internal
and set other preferences.

Source files are conceived to be manually added to other projects or by setting a reference via [Paket](http://fsprojects.github.io/Paket/). See these [paket.dependencies](https://github.com/gsscoder/commandline/blob/master/paket.dependencies) and [paket.references](https://github.com/gsscoder/commandline/blob/master/src/CommandLine/paket.references) as example.

## Maybe.cs
- C# native implementation of F# `'T option` / Haskell `data Maybe a = Just a | Nothing` type.

## Either.cs
- C# native implementation of Haskell `data Either a b = Left a | Right b` type.
- Similar also to F# `Choice<'T, 'U>`.
- Like in Haskell the convention is to let `Right` case hold the value and `Left` keep track of error or similar data.
- If you want a better implementation of this kind of data, please check my **C#** port of [Chessie](https://github.com/fsprojects/Chessie),
named [RailwaySharp](https://github.com/gsscoder/railwaysharp).

## EnumerableExtensions.cs
- Most useful extension methods from code.google.com/p/morelinq/.
- With other useful methods too.

## Unit.cs
- Equivalent to F# `unit`.

## Identity.cs
- Identity monadic type.
- The use of this monad can be easly replaced by direct function application.

# Note
- Actually `Maybe.cs` and `EnumerableExtensions.cs` are the more mature implementations.
- I'm working for adding tests, time allowing.
- Please report any your thought.
