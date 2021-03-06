namespace Open.PackML
{
    public class ValidationResult
    {
        public ValidationResult(bool success, string unSuccesfullText = "")
        {
            this.success = success;
            this.unSuccesfullText = unSuccesfullText;
        }

        public bool success { get; private set; }
        public string unSuccesfullText { get; private set; }
        public void AddResult(ValidationResult validationResult)
        {
            if (validationResult.success == false)
            {
                success = false;
                unSuccesfullText += validationResult.unSuccesfullText;
            }
        }
    }
    public class ValidationResult<T>
    {
        public ValidationResult(T Object, bool success, string unSuccesfullText = "")
        {
            this.success = success;
            this.unSuccesfullText = unSuccesfullText;
            this.Object = Object;
        }

        public bool success { get; }
        public string unSuccesfullText { get; }
        public T Object { get; }
    }

}