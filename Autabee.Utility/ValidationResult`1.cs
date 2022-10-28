using System;
using System.Collections.Generic;
using System.Linq;

namespace Autabee.Utility
{
#if(NET6_0_OR_GREATER)
    public record ValidationResult<T> : ValidationResult
#else
    public class ValidationResult<T> : ValidationResult
#endif
    {
        public ValidationResult(bool success = true, T Object = default, string unSuccesfullText = "", params object[] formatObjects) : base(success, unSuccesfullText, formatObjects)
        {
            this.Object = Object;
        }

        public ValidationResult(bool success = true, T Object = default, List<(string, object[])> failInfo = default) : base(success, failInfo)
        {
            this.Object = Object;
        }

        public ValidationResult(ValidationResult result) : base(result.Success, result.FailInfo)
        {
            Object = default;
        }
        public T Object { get; }
    }
}