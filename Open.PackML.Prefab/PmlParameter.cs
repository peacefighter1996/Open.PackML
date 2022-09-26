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
        public int Name { get; set; }

        [TagEndUserTerm("Detailed Error Information")]
        public int Value { get; set; }

        [TagEndUserTerm("Unit of measure")]
        [TagFixedSize(5)]
        public byte[] Unit { get; set; }
    }
}
