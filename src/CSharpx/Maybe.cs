﻿//Use project level define(s) when referencing with Paket.
//#define CSX_MAYBE_INTERNAL // Uncomment this to set visibility to internal.
//#define CSX_REM_EITHER_FUNC // Uncomment this to remove dependency to Either.cs.

using System;

namespace CSharpx
{
#if !CSX_MAYBE_INTERNAL
    public
#endif
    enum MaybeType { Just, Nothing }

#if !CSX_MAYBE_INTERNAL
    public
#endif
    abstract class Maybe<T>
    {
        private readonly MaybeType tag;

        protected Maybe(MaybeType tag)
        {
            this.tag = tag;
        }

        public MaybeType Tag
        {
            get { return tag; }
        }

        public bool MatchNothing()
        {
            return Tag == MaybeType.Nothing;
        }

        public bool MatchJust(out T value)
        {
            value = Tag == MaybeType.Just
                ? ((Just<T>)this).Value
                : default(T);
            return Tag == MaybeType.Just;
        }
    }

#if !CSX_MAYBE_INTERNAL
    public
#endif
    sealed class Nothing<T> : Maybe<T>
    {
        internal Nothing() : base(MaybeType.Nothing) { }
    }

#if !CSX_MAYBE_INTERNAL
    public
#endif
    sealed class Just<T> : Maybe<T>
    {
        private readonly T value;

        internal Just(T value)
            : base(MaybeType.Just)
        {
            this.value = value;
        }

        public T Value
        {
            get { return value; }
        }
    }

#if !CSX_MAYBE_INTERNAL
    public
#endif
    static class Maybe
    {
        public static Maybe<T> Nothing<T>()
        {
            return new Nothing<T>();
        }

        public static Just<T> Just<T>(T value)
        {
            return new Just<T>(value);
        }

        public static Maybe<Tuple<T1, T2>> Merge<T1, T2>(Maybe<T1> first, Maybe<T2> second)
        {
            T1 value1;
            T2 value2;
            if (first.MatchJust(out value1) && second.MatchJust(out value2))
            {
                return Maybe.Just(Tuple.Create(value1, value2));
            }
            return Maybe.Nothing<Tuple<T1, T2>>();
        }

#if !CSX_REM_EITHER_FUNC
        /// <summary>
        /// Maps Choice 1Of2 to Some value, otherwise None.
        /// </summary>
        public static Maybe<T1> OfEither<T1, T2>(Either<T1, T2> either)
        {
            if (either.Tag == Either2Type.Either1Of2)
            {
                return Maybe.Just(((Either1Of2<T1, T2>)either).Value);
            }
            return Maybe.Nothing<T1>();
        }
#endif
    }

#if !CSX_MAYBE_INTERNAL
    public
#endif
    static class MaybeExtensions
    {
        public static Maybe<T> ToMaybe<T>(this T value)
        {
            return Equals(value, default(T)) ? Maybe.Nothing<T>() : Maybe.Just(value);
        }

        public static Maybe<T2> Bind<T1, T2>(this Maybe<T1> maybe, Func<T1, Maybe<T2>> func)
        {
            T1 value1;
            return maybe.MatchJust(out value1)
                ? func(value1)
                : Maybe.Nothing<T2>();
        }

        public static Maybe<T2> Map<T1, T2>(this Maybe<T1> maybe, Func<T1, T2> func)
        {
            T1 value1;
            return maybe.MatchJust(out value1)
                ? Maybe.Just(func(value1))
                : Maybe.Nothing<T2>();
        }

        public static T2 MapMaybeOrDefault<T1, T2>(this Maybe<T1> maybe, Func<T1, T2> func, T2 noneValue)
        {
            T1 value1;
            return maybe.MatchJust(out value1)
                ? func(value1)
                : noneValue;
        }

        public static void Do<T>(this Maybe<T> maybe, Action<T> action)
        {
            T value;
            if (maybe.MatchJust(out value))
            {
                action(value);
            }
        }

        public static Maybe<TResult> Select<TSource, TResult>(
            this Maybe<TSource> maybe, Func<TSource, TResult> selector)
        {
            return maybe.Map(selector);
        }

        public static Maybe<TResult> SelectMany<TSource, TValue, TResult>(
            this Maybe<TSource> maybe,
            Func<TSource, Maybe<TValue>> valueSelector,
            Func<TSource, TValue, TResult> resultSelector)
        {
            return maybe.Bind(
                sourceValue => valueSelector(sourceValue)
                    .Map(
                        resultValue => resultSelector(sourceValue, resultValue)));
        }

        public static T FromJust<T>(this Maybe<T> maybe)
        {
            T value;
            if (maybe.MatchJust(out value))
            {
                return value;
            }
            return default(T);
        }

        public static T FromJustOrFail<T>(this Maybe<T> maybe, Exception exceptionToThrow = null)
        {
            T value;
            if (maybe.MatchJust(out value))
            {
                return value;
            }
            throw exceptionToThrow ?? new ArgumentException("Value empty.");
        }

        public static bool IsNothing<T>(this Maybe<T> maybe)
        {
            return maybe.Tag == MaybeType.Nothing;
        }

        public static bool IsJust<T>(this Maybe<T> maybe)
        {
            return maybe.Tag == MaybeType.Just;
        }

        public static void Match<T>(this Maybe<T> maybe, Action<T> ifJust, Action ifNothing)
        {
            T value;
            if (maybe.MatchJust(out value))
            {
                ifJust(value);
                return;
            }
            ifNothing();
        }

        public static void Match<T1, T2>(this Maybe<Tuple<T1, T2>> maybe, Action<T1, T2> ifJust, Action ifNothing)
        {
            T1 value1;
            T2 value2;
            if (maybe.MatchJust(out value1, out value2))
            {
                ifJust(value1, value2);
                return;
            }
            ifNothing();
        }

        public static bool MatchJust<T1, T2>(this Maybe<Tuple<T1, T2>> maybe, out T1 value1, out T2 value2)
        {
            Tuple<T1, T2> value;
            if (maybe.MatchJust(out value))
            {
                value1 = value.Item1;
                value2 = value.Item2;
                return true;
            }
            value1 = default(T1);
            value2 = default(T2);
            return false;
        }

        public static void Do<T1, T2>(this Maybe<Tuple<T1, T2>> maybe, Action<T1, T2> action)
        {
            T1 value1;
            T2 value2;
            if (maybe.MatchJust(out value1, out value2))
            {
                action(value1, value2);
            }
        }
    }
}