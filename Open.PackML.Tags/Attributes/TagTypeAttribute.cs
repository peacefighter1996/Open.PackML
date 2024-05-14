using System;
using System.Linq;

namespace Open.PackML.Tags.Attributes
{
    [AttributeUsage(
          AttributeTargets.Method
        | AttributeTargets.Property)
    ]
    public class TagTypeAttribute : Attribute
    {
        public TagType TagType { get; }

        public TagTypeAttribute(TagType type)
        {
            this.TagType = type;
        }
    }
}
