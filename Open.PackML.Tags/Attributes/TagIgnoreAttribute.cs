using System;
using System.Linq;

namespace Open.PackML.Tags.Attributes
{
    [AttributeUsage(
          AttributeTargets.Method
        | AttributeTargets.Property)
    ]
    public class TagIgnoreAttribute : Attribute { };
}
