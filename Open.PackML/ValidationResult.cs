namespace Open.PackML
{
    public class ValidationResult
    {
        public ValidationResult(bool success, string unSuccesfullText = "")
        {
            this.Success = success;
            this.unSuccesfullText = unSuccesfullText;
        }

        public bool Success { get; private set; }
        public string unSuccesfullText { get; private set; }
        public void AddResult(ValidationResult validationResult)
        {
            if (validationResult.Success == false)
            {
                Success = false;
                unSuccesfullText += validationResult.unSuccesfullText;
            }
        }
    }
    public class ValidationResult<T> : ValidationResult
    {
        public ValidationResult(T Object, bool success, string unSuccesfullText = ""): base(success, unSuccesfullText)
        {
            this.Object = Object;
        }
        public T Object { get; }
    }

}