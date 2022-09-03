using System;

namespace Open.PackML
{
    public abstract class Tag<T>
    {
        public Tag(TagConfig tagConfig) : this(tagConfig.Name,tagConfig.EndUserTerm, tagConfig.Description)
        {
            
        }

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
    public class TagConfig
    {
        public string Name { get; set; }
        public string EndUserTerm { get; set; }
        public string Description { get; set; }
        public TagType TagType { get; set; }
        public Type DataType { get; set; }

        public override string ToString()
        {
            return String.Format("{0},{1},{2},{3},{4},{5}", TagType.ToString(), Name, EndUserTerm, Description, DataType.Name, DataType.Namespace);
        }

    }
}
