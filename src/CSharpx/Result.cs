
//#define CSX_RESULT_INTERNAL // Uncomment or define at build time to set accessibility to internal.

using System;

namespace CSharpx
{

#if !CSX_RESULT_INTERNAL
    public
#endif    
    enum ResultType
    {
        Success,
        Failure
    }

#if !CSX_RESULT_INTERNAL
    public
#endif    
    struct Result
    {
#if DEBUG
        internal
#endif        
        readonly string _error;

        internal Result(string error)
        {
            if (error == null) throw new ArgumentNullException(error);
            Tag = ResultType.Failure;
            _error = error;
        }

        public ResultType Tag { get; private set; }

#region Basic Match Methods
        public bool MatchFailure(out string error)
        {
            error = Tag == ResultType.Failure ? _error : default;
            return Tag == ResultType.Failure;
        }

        public bool MatchSuccess() => Tag == ResultType.Success;
#endregion

#region Value Case Constructors
        public static Result Failure(string error) => new Result(error);

        public static Result Success => new Result();
#endregion
    }
}