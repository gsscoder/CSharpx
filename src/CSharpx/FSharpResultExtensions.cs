//#define CSX_FSHARP_DISABLED // Uncomment or define at build time to remove all F# related types.
//#define CSX_FSHARP_RESEXT_INTERNAL // Uncomment or define at build time to set FSharpResultExtensions accessibility to internal.
#if !CSX_FSHARP_DISABLED
using System;
using Microsoft.FSharp.Core;

namespace CSharpx.FSharp
{
#if !CSX_FSHARP_INTERNAL
    public
#endif
    static class FSharpResultExtensions
    {
        /// <summary>
        /// Allows pattern matching on Results.
        /// </summary>
        public static void Match<T, TError>(this FSharpResult<T, TError> result,
            Action<T> onOk,
            Action<TError> onError)
        {
            if (onOk == null) throw new ArgumentNullException(nameof(onOk));
            if (onError == null) throw new ArgumentNullException(nameof(onError));

            if (result.IsOk) {
                onOk(result.ResultValue);
                return;
            }
            onError(result.ErrorValue);
        }

        /// <summary>
        /// Allows pattern matching on Results.
        /// </summary>
        public static TResult Either<T, TError, TResult>(this FSharpResult<T, TError> result,
            Func<T, TResult> onOk,
            Func<TError, TResult> onError)
        {
            if (onOk == null) throw new ArgumentNullException(nameof(onOk));
            if (onError == null) throw new ArgumentNullException(nameof(onError));

            return Trail.Either(onOk, onError, result);
        }

        static class Trail
        {
            // Takes a result and maps it with okFunc if it is a success, otherwise it maps it with errorFunc.
            public static TResult Either<T, TError, TResult>(
                Func<T, TResult> okFunc,
                Func<TError, TResult> errorFunc,
                FSharpResult<T, TError> result)
            {
                if (result.IsOk) {
                    return okFunc(result.ResultValue);
                }
                return errorFunc(result.ErrorValue);
            }
        }
    }
}
#endif