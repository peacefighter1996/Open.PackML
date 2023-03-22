using System;
using System.Linq;

namespace Open.PackML.Tags.Attributes
{
    [AttributeUsage(
          AttributeTargets.Class
        | AttributeTargets.Struct
        | AttributeTargets.Property)
    ]
    public class TagEndUserTermAttribute : Attribute
    {
        public string EndUserTerm { get; }

        public TagEndUserTermAttribute(string endUserTerm)
        {
            this.EndUserTerm = endUserTerm;
        }
    }
}
