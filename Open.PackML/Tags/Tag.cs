using System;

namespace Open.PackML
{
    public abstract class Tag<T>
    {
        public Tag(string name, string endUserTerm, string description = null)
        {
            var error = string.Empty;
            var arguments = string.Empty;
            if (string.IsNullOrEmpty(name))
            {
                var argument = nameof(name);
                error += $"'{argument}' cannot be null or empty. ";
                arguments = argument + ',';
            }

            if (string.IsNullOrEmpty(endUserTerm))
            {
                var argument = nameof(endUserTerm);
                error += $"'{argument}' cannot be null or empty. ";
                arguments = argument + ',';
            }

            if(error != null)
            {
                throw new ArgumentException(error, arguments.Substring(0, arguments.Length - 1));
            }

            Name = name;
            EndUserTerm = endUserTerm;
            Description = description;
        }

        public Type DataType => typeof(T);
        public abstract TagType TagType { get; }
        public string Name { get; }
        public string EndUserTerm { get; }
        public string Description { get; }
    }
}
