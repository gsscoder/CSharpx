
//requires: Unit.cs
//#define CSX_TYPES_INTERNAL // Uncomment or define at build time to set accessibility to internal.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CSharpx
{

#if !CSX_TYPES_INTERNAL
    public
#endif
    enum ResultType
    {
        Success,
        Failure
    }

#if !CSX_TYPES_INTERNAL
    public
#endif   
    struct Error : IEquatable<Error>
    {
        public string Message { get; private set; }
        public IEnumerable<Exception> Exceptions { get; private set; }

        internal Error(string message, IEnumerable<Exception> exceptions)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            if (exceptions == null) throw new ArgumentNullException(nameof(exceptions));

            Message = message;
            Exceptions = exceptions;
        }

        public override bool Equals(object other)
        {
            if (other == null) return false;
            if (other.GetType() != typeof(Error)) return false;
            var otherError = (Error)other;
            return otherError.Message.Equals(Message) &&
                   Enumerable.SequenceEqual(otherError.Exceptions, Exceptions, new ExceptionEqualityComparer());
        }

        public bool Equals(Error other) =>
            other.Message.Equals(Message) &&
            Enumerable.SequenceEqual(other.Exceptions, Exceptions, new ExceptionEqualityComparer());

        public static bool operator ==(Error left, Error right) => left.Equals(right);

        public static bool operator !=(Error left, Error right) => !left.Equals(right);

        override public int GetHashCode() =>
            Message.GetHashCode() + (from e in Exceptions select e.GetHashCode()).Sum();

        sealed class ExceptionEqualityComparer : IEqualityComparer<Exception>
        {
            public bool Equals(Exception first, Exception second)
            {
                if (first == null && second == null) return true;
                if (first == null || second == null) return false;
                if (first.GetType() != second.GetType()) return false;
                if (first.Message != second.Message) return false;
                if (first.InnerException != null) return Equals(first.InnerException, second.InnerException);
                return true;
            }

            public int GetHashCode(Exception e)
            {
                var hash = e.Message.GetHashCode();
                if (e.InnerException != null) hash ^= e.InnerException.Message.GetHashCode();
                return hash;
            }
        }
    }

#if !CSX_TYPES_INTERNAL
    public
#endif    
    struct Result : IEquatable<Result>
    {
#if DEBUG
        internal
#endif
        readonly Error _error;

        internal Result(Error error)
        {
            Tag = ResultType.Failure;
            _error = error;
        }

        public ResultType Tag { get; private set; }

        public override bool Equals(object other)
        {
            if (other == null) return false;
            var otherType = other.GetType();
            if (otherType != GetType()) return false;
            var otherTag = (ResultType)otherType.GetProperty(
                "Tag", BindingFlags.Public | BindingFlags.Instance).GetValue(other);
            if (otherTag != Tag) return false;
            if (otherTag == ResultType.Success && Tag == ResultType.Success) return true;
            var otherField = otherType.GetField("_error", BindingFlags.NonPublic | BindingFlags.Instance);
            return otherField.GetValue(other).Equals(_error);
        }

        public bool Equals(Result other) =>
            other.Tag != ResultType.Failure || _error.Equals(other._error);

        public static bool operator ==(Result left, Result right) => left.Equals(right);

        public static bool operator !=(Result left, Result right) => !left.Equals(right);

        public override int GetHashCode() =>
            Tag == ResultType.Success
                ? ToString().GetHashCode()
                : _error.GetHashCode();

        public override string ToString() =>
            Tag switch {
                ResultType.Success => "<Success>",
                _                  => new StringBuilder("Failure(")
                                      .Append(_error)
                                      .Append(")")
                                      .ToString()
            };

#region Basic Match Methods
        public bool MatchFailure(out Error error)
        {
            error = Tag == ResultType.Failure ? _error : default;
            return Tag == ResultType.Failure;
        }

        public bool MatchSuccess() => Tag == ResultType.Success;
#endregion

#region Value Case Constructors
        public static Result Failure(string error) => new Result(
            new Error(error, Enumerable.Empty<Exception>()));

        public static Result Failure(string error, Exception exception) => new Result(
            new Error(error, new[] {exception}));

       public static Result Failure(string error, params Exception[] exceptions) => new Result(
            new Error(error, exceptions));

        public static Result Failure(Exception exception) => new Result(
            new Error(string.Empty, new[] {exception}));

       public static Result Failure(params Exception[] exceptions) => new Result(
            new Error(string.Empty, exceptions));            

        public static Result Success => new Result();
#endregion
    }

#if !CSX_TYPES_INTERNAL
    public
#endif
    static class ResultExtensions
    {
        public static Unit Match(this Result result,
            Func<Unit> onSuccess, Func<Error, Unit> onFailure)
        {
            if (onSuccess == null) throw new ArgumentNullException(nameof(onSuccess));
            if (onFailure == null) throw new ArgumentNullException(nameof(onFailure));

            return result.MatchFailure(out Error error) switch {
                true => onFailure(error),
                _    => onSuccess() 
            };
        }

        public static Unit Match(this Result result,
            Func<Unit> onSuccess, Func<string, Unit> onFailure)
        {
            if (onSuccess == null) throw new ArgumentNullException(nameof(onSuccess));
            if (onFailure == null) throw new ArgumentNullException(nameof(onFailure));

            return result.MatchFailure(out Error error) switch {
                true => onFailure(error.Message),
                _    => onSuccess() 
            };
        }

        public static Unit Match(this Result result,
            Func<Unit> onSuccess, Func<Exception, Unit> onFailure)
        {
            if (onSuccess == null) throw new ArgumentNullException(nameof(onSuccess));
            if (onFailure == null) throw new ArgumentNullException(nameof(onFailure));

            return result.MatchFailure(out Error error) switch {
                true => onFailure(error.Exceptions.First()),
                _    => onSuccess() 
            };
        }        
    }
}