using System;
using System.Linq;

namespace Open.PackML.Tags.Attributes
{
    [AttributeUsage(
        AttributeTargets.Property
        | AttributeTargets.Method)
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
