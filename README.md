# CSharpx

Functional programming and other utilities for C#, following *don't reinvent the wheel* philosophy.

Every source file contains conditional compilation symbols to change type visibility from public to internal
and set other preferences.

Source files are conceived to be manually added to other projects or by setting a reference via [Paket](http://fsprojects.github.io/Paket/).

## Maybe.cs

- C# native implementation of F# `'T option` / Haskell `data Maybe a = Just a | Nothing` type.

## Either.cs

- C# native implementation of Haskell `data Either a b = Left a | Right b` type.
- Similar also to F# `Choice<'T, 'U>`.
- Like in Haskell the convention is to let `Right` case hold the value and `Left` keep track of error or similar data.
- If you want a better implementation of this kind of types, please check my **C#** port of [Chessie](https://github.com/fsprojects/Chessie),
named [RailwaySharp](https://github.com/gsscoder/railwaysharp).

## EnumerableExtensions.cs

- Most useful extension methods from [MoreLINQ](https://code.google.com/p/morelinq/).
- With other useful methods too.

## Unit.cs

- Equivalent to F# `unit`.

## Identity.cs

- Identity monadic type.
- The use of this monad can be easly replaced by direct function application.

## SetOnce.cs
- Types to allow setting of a value only once.
- Included thread-safe implementation.

## Helpers.cs
- Mainly general purpose extension methods.
- Contains `System.String` extensions to generate test data.