﻿using System;
using System.Linq;

namespace Open.PackML.Tags.Attributes
{
    [AttributeUsage(AttributeTargets.Class |
                       AttributeTargets.Struct)
]
    public class TagTypeAttribute : Attribute
    {
        public TagType Type { get; }

        public TagTypeAttribute(TagType type)
        {
            this.Type = type;
        }
    }
}
