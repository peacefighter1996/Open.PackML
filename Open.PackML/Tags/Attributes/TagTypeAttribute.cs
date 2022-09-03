using System;
using System.Linq;

namespace Open.PackML.Tags.Attributes
{
    [AttributeUsage(AttributeTargets.Class |
                       AttributeTargets.Struct)
]
    public class TagTypeAttribute : Attribute
    {
        public TagType type;

        public TagTypeAttribute(TagType type)
        {
            this.type = type;
        }
    }
}
