using System;
using System.Collections.Generic;

namespace Autabee.Utility
{
    public class ValidationResult
    {
        public ValidationResult(bool success = true, string unSuccessfulText = "", params object[] formatObjects)
        {
            this.Success = success;
            FailInfo = new List<(string, object[])>();
            if (!string.IsNullOrWhiteSpace(unSuccessfulText))
            {
                FailInfo.Add((unSuccessfulText, formatObjects));
            }

        }

        public void AddResult(ValidationResult validationResult)
        {
            if (validationResult.Success == false)
            {
                Success = false;
                FailInfo.AddRange(validationResult.FailInfo);
            }
        }
        public void AddResult(string failText, params object[] formatObjects)
        {
            if (!string.IsNullOrWhiteSpace(failText))
            {
                Success = false;
                FailInfo.Add((failText, formatObjects));
            }
        }
        public string FailString()
        {
            string failString = "";
            foreach (var fail in FailInfo)
            {
                failString += string.Format(fail.Item1, fail.Item2) + Environment.NewLine;
            }
            return failString;
        }

        public List<(string, object[])> FailInfo { get; }

        public bool Success { get; private set; }
    }

    public class ValidationResult<T> : ValidationResult
    {
        public ValidationResult(bool success = true, T Object = default, string unSuccesfullText = "", params object[] formatObjects) : base(success, unSuccesfullText, formatObjects)
        {
            this.Object = Object;
        }

        public T Object { get; }
    }

}