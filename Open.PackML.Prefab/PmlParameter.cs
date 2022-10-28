using Open.PackML.Tags.Attributes;
using System;
using System.Linq;

namespace Open.PackML.Prefab
{
    public class PmlParameter
    {
        [TagEndUserTerm("Parameter ID")]
        public int ID { get; set; }
        [TagEndUserTerm("Name of parameter")]
        public string Name { get; set; }

        [TagEndUserTerm("Value of parameter")]
        public object Value { get; set; }

        [TagEndUserTerm("Unit of measure")]
        [TagFixedSize(5)]
        public char[] Unit { get; set; }
    }
}
