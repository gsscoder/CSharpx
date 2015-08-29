# CSharpx
Functional programming and other utilities for C# (_work in progress_).

Every source file contains conditional compilation symbols to change type visibility from public to internal
and set other preferences.

Source files are conceived to be manually added to other projects or by setting a reference via [Paket](http://fsprojects.github.io/Paket/). See these [paket.dependencies](https://github.com/gsscoder/commandline/blob/master/paket.dependencies) and [paket.references](https://github.com/gsscoder/commandline/blob/master/src/CommandLine/paket.references) as example.

## Maybe.cs
- C# native implementation of F# `'T option` / Haskell `Data.Maybe a` type.

## Either.cs
- **This type will be ASAP strongly refactored, please don't use it untile this message disappears**.
- C# native implementation of F# `Choice` discriminated unions.
- Import only required `Either<T1 ... T7>` type.

## EnumerableExtensions.cs
- Most useful extension methods from code.google.com/p/morelinq/.

## Unit.cs
- Equivalent to F# `unit`.

## Identity.cs
- Identity monadic type.
